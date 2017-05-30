using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Newtonsoft.Json;
using System.ComponentModel;

namespace AigioSoft.Common.Helpers
{
    /// <summary>
    /// 验证码包
    /// </summary>
    public static class VerifyCodePackage
    {
        public const string FileExtension = ".asvcp";

        public const string Separator = "-EGAKCAPEDOCYFIREVTFOSOIGIA-";

        public static readonly Encoding DefaultEncoding = Encoding.UTF8;

        internal class Structure
        {
            // ReSharper disable once InconsistentNaming
            private static readonly (byte original, byte replace)[] JPEGSignature =
                new[] { ((byte)255, (byte)73), ((byte)216, (byte)77), ((byte)255, (byte)77) };
            // original FF D8 FF

            // ReSharper disable once InconsistentNaming
            private static readonly (byte original, byte replace)[] JIJFSignature =
                new[] { ((byte)74, (byte)70), ((byte)70, (byte)73), ((byte)73, (byte)76), ((byte)70, (byte)69) };
            // original 4A 46 49 46

            // ReSharper disable once InconsistentNaming
            private static readonly (byte original, byte replace)[] GZIPSignature =
                new[] { ((byte)31, (byte)65), ((byte)139, (byte)83), ((byte)8, (byte)86) };
            // original 1F 8B 08

            // ReSharper disable once InconsistentNaming
            private const int JFIFIndex = 6;

            /// <summary>
            /// JPEG文件头替换
            /// </summary>
            /// <param name="data"></param>
            internal static void JpegSignatureReplace(byte[] data)
            {
                var original = JPEGSignature.Select(x => x.original).ToArray();
                var replace = JPEGSignature.Select(x => x.replace).ToArray();
                if (data.Take(JPEGSignature.Length).SequenceEqual(original))
                    for (int i = 0; i < JPEGSignature.Length; i++)
                        data[i] = replace[i];
                original = JIJFSignature.Select(x => x.original).ToArray();
                replace = JIJFSignature.Select(x => x.replace).ToArray();
                if (data.Skip(JFIFIndex).Take(JIJFSignature.Length).SequenceEqual(original))
                    for (int i = JFIFIndex; i < JIJFSignature.Length + JFIFIndex; i++)
                        data[i] = replace[i - JFIFIndex];
            }

            /// <summary>
            /// GZip文件头替换
            /// </summary>
            /// <param name="stream"></param>
            internal static void GZipReplace(Stream stream)
            {
                var original = GZIPSignature.Select(x => x.original).ToArray();
                var replace = GZIPSignature.Select(x => x.replace).ToArray();
                stream.Seek(0, SeekOrigin.Begin);
                byte[] temp = new byte[GZIPSignature.Length];
                stream.Read(temp, 0, temp.Length);
                if (temp.SequenceEqual(original))
                    for (int i = 0; i < GZIPSignature.Length; i++)
                    {
                        stream.Seek(i, SeekOrigin.Begin);
                        stream.WriteByte(replace[i]);
                    }
            }

            /// <summary>
            /// 验证数据是否为验证码包类型
            /// </summary>
            /// <param name="stream"></param>
            /// <returns></returns>
            internal static bool Validation(Stream stream)
            {
                var original = GZIPSignature.Select(x => x.original).ToArray();
                var replace = GZIPSignature.Select(x => x.replace).ToArray();
                stream.Seek(0, SeekOrigin.Begin);
                byte[] temp = new byte[GZIPSignature.Length];
                stream.Read(temp, 0, temp.Length);
                var result = temp.SequenceEqual(replace);
                if (result)
                    for (int i = 0; i < GZIPSignature.Length; i++)
                    {
                        stream.Seek(i, SeekOrigin.Begin);
                        stream.WriteByte(original[i]);
                    }
                stream.Seek(0, SeekOrigin.Begin);
                return result;
            }

            /// <summary>
            /// 还原JPEG文件头
            /// </summary>
            /// <param name="stream"></param>
            internal static void RestoreImgStream(Stream stream)
            {
                var original = JPEGSignature.Select(x => x.original).ToArray();
                var replace = JPEGSignature.Select(x => x.replace).ToArray();
                byte[] temp = new byte[JPEGSignature.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(temp, 0, temp.Length);
                if (temp.SequenceEqual(replace))
                    for (int i = 0; i < JPEGSignature.Length; i++)
                    {
                        stream.Seek(i, SeekOrigin.Begin);
                        stream.WriteByte(original[i]);
                    }
                original = JIJFSignature.Select(x => x.original).ToArray();
                replace = JIJFSignature.Select(x => x.replace).ToArray();
                temp = new byte[JIJFSignature.Length];
                stream.Seek(JFIFIndex, SeekOrigin.Begin);
                stream.Read(temp, 0, temp.Length);
                if (temp.SequenceEqual(replace))
                    for (int i = 0; i < JIJFSignature.Length; i++)
                    {
                        stream.Seek(i + JFIFIndex, SeekOrigin.Begin);
                        stream.WriteByte(original[i]);
                    }
            }
        }

        public class FileHeaderData
        {
            public long PositionStart { get; set; }
            public long PositionEnd { get; set; }
            public string FileName { get; set; }
        }

        public enum UnpackEnum : byte
        {
            [Description("成功")]
            Success = 0,
            [Description("未找到分隔符")]
            NotFoundSeparator = 1,
            [Description("格式错误")]
            FormatNotCorrect = 2,
            [Description("异常")]
            Exception = 254,
            [Description("未知")]
            Unknown = 255,
        }

        public class UnpackResult
        {
            [JsonIgnore]
            internal static UnpackResult DefaultValue => new UnpackResult { Code = UnpackEnum.Unknown, Message = null };

            public UnpackEnum Code { get; set; }

            public string Message { get; set; }
        }

        /// <summary>
        /// 根据验证码图片生成包
        /// </summary>
        /// <param name="imageDataEnumerable">验证码图片组</param>
        /// <param name="destinationArchiveFileName">要生成的存档路径，指定为绝对路径。</param>
        public static void CreateFromBitmap(IEnumerable<(byte[] image, string fileName)> imageDataEnumerable, string destinationArchiveFileName)
        {
            if (!destinationArchiveFileName.EndsWith(FileExtension))
                destinationArchiveFileName += FileExtension;
            (byte[] image, string fileName)[] imageDataArr = imageDataEnumerable as (byte[] image, string fileName)[] ?? imageDataEnumerable?.ToArray();
            if (imageDataArr == null) return;
            var headers = new FileHeaderData[imageDataArr.Length];
            var separator = DefaultEncoding.GetBytes(Separator);
            using (var ms = new MemoryStream())
            using (var fs = File.Create(destinationArchiveFileName))
            using (GZipStream compressionStream = new GZipStream(fs, CompressionLevel.Optimal))
            {
                for (int i = 0; i < imageDataArr.Length; i++)
                {
                    var item = imageDataArr[i];
                    var start = ms.Position;
                    Structure.JpegSignatureReplace(item.image);
                    ms.Write(item.image, 0, item.image.Length);
                    var end = ms.Position;
                    headers[i] = new FileHeaderData
                    {
                        FileName = item.fileName,
                        PositionStart = start,
                        PositionEnd = end
                    };
                }
                var headersByte = DefaultEncoding.GetBytes(Convert.ToBase64String(DefaultEncoding.GetBytes(JsonConvert.SerializeObject(headers))));
                compressionStream.Write(headersByte, 0, headersByte.Length);
                compressionStream.Write(separator, 0, separator.Length);
                ms.Seek(0, SeekOrigin.Begin);
                ms.CopyTo(compressionStream);
            }
            using (var fs = new FileStream(destinationArchiveFileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                Structure.GZipReplace(fs);
        }

        /// <summary>
        /// 解压验证码图片包
        /// </summary>
        /// <param name="filePath">包所在路径</param>
        /// <param name="dirPath">解压到文件夹路径（如有文件名冲突将覆盖）</param>
        /// <returns></returns>
        public static UnpackResult UnpackPackage(string filePath, string dirPath)
        {
            if (!File.Exists(filePath))
                return new UnpackResult
                {
                    Message = "filePath is not exists.",
                    Code = UnpackEnum.Exception
                };
            var fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.Delete);
            File.Delete(filePath);
            return UnpackPackage(fs, dirPath);
        }

        /// <summary>
        /// 解压验证码图片包
        /// </summary>
        /// <param name="fileStream">验证码图片流</param>
        /// <param name="dirPath">解压到文件夹路径（如有文件名冲突将覆盖）</param>
        /// <returns></returns>
        public static UnpackResult UnpackPackage(Stream fileStream, string dirPath)
        {
            UnpackResult result = null;
            try
            {
                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);
                bool correctFormat = Structure.Validation(fileStream);
                if (correctFormat)
                {
                    using (var memoryStream = UnpackToMemoryStream(fileStream))
                    {
                        var headerData = GetHeaderData(memoryStream);
                        if (headerData.findSeparator && headerData.data.HasValue)
                        {
                            foreach (var item in headerData.data.Value.headers.OrderBy(x => x.PositionStart))
                            {
                                var imgPath = Path.Combine(dirPath, item.FileName + ".jpg");
                                if (File.Exists(imgPath))
                                    File.Delete(imgPath);
                                memoryStream.Seek(item.PositionStart + headerData.data.Value.length, SeekOrigin.Begin);
                                using (var fs = new FileStream(imgPath, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.Read))
                                {
                                    while (memoryStream.Position < item.PositionEnd + headerData.data.Value.length)
                                        fs.WriteByte((byte)memoryStream.ReadByte());
                                    Structure.RestoreImgStream(fs);
                                }
                                result = new UnpackResult
                                {
                                    Code = UnpackEnum.Success
                                };
                            }
                        }
                        else
                        {
                            result = new UnpackResult
                            {
                                Code = UnpackEnum.NotFoundSeparator
                            };
                        }
                    }
                }
                else
                {
                    result = new UnpackResult
                    {
                        Code = UnpackEnum.FormatNotCorrect
                    };
                }
                fileStream.Dispose();
            }
            catch (Exception ex)
            {
                result = new UnpackResult
                {
                    Message = ex.Message,
                    Code = UnpackEnum.Exception
                };
            }
            return result ?? UnpackResult.DefaultValue;
            ((FileHeaderData[] headers, int length)? data, bool findSeparator) GetHeaderData(MemoryStream memoryStream)
            {
                List<byte> headerBytes = new List<byte>();
                memoryStream.Seek(0, SeekOrigin.Begin);
                int readByte = memoryStream.ReadByte();
                var separatorData = DefaultEncoding.GetBytes(Separator);
                bool findSeparator = false;
                while (readByte != -1)
                {
                    headerBytes.Add((byte)readByte);
                    if (IsSeparator(headerBytes, separatorData))
                    {
                        findSeparator = true;
                        break;
                    }
                    readByte = memoryStream.ReadByte();
                }
                if (findSeparator)
                {
                    var headLength = headerBytes.Count;
                    var bytesHeader = headerBytes.Take(headLength - separatorData.Length).ToArray();
                    var textHeader = DefaultEncoding.GetString(Convert.FromBase64String(DefaultEncoding.GetString(bytesHeader)));
                    var jsonHeader = JsonConvert.DeserializeObject<FileHeaderData[]>(textHeader);
                    return ((jsonHeader, headLength), true);
                }
                return (null, false);
            }
            MemoryStream UnpackToMemoryStream(Stream stream)
            {
                var outMs = new MemoryStream();
                using (GZipStream deCompressionStream = new GZipStream(stream, CompressionMode.Decompress))
                {
                    deCompressionStream.CopyTo(outMs);
                    return outMs;
                }
            }
            bool IsSeparator(List<byte> bytes, byte[] separator)
            {
                if (bytes.Count > separator.Length)
                {
                    var endData = bytes.Skip(bytes.Count - separator.Length).ToArray();
                    return endData.SequenceEqual(separator);
                }
                return false;
            }
        }
    }
}
