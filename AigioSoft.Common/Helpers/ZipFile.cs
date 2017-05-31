#if !(NET20 || NET35 || NET40)
using System.Linq;
using System.IO.Compression;
using System.IO;

namespace AigioSoft.Common.Helpers
{
    /// <summary>
    /// 操作ZIP文件
    /// </summary>
    public static class ZipFile
    {
        /// <summary>
        /// 创建 zip 存档，指定该存档包括是否递归文件夹，使用指定压缩级别。
        /// </summary>
        /// <param name="sourceDirectoryName">要存档的目录的路径，指定为相对路径或绝对路径。 相对路径是指相对于当前工作目录的路径</param>
        /// <param name="destinationArchiveFileName">要生成的存档路径，指定为相对路径或绝对路径。 相对路径是指相对于当前工作目录的路径。</param>
        /// <param name="recursiveDir">是否递归文件夹</param>
        /// <param name="compressionLevel">指示创建项时是否强调速度或压缩有效性的枚举值之一。</param>
        public static void CreateFromDirectory(string sourceDirectoryName, string destinationArchiveFileName = null,
            bool recursiveDir = false, CompressionLevel compressionLevel = CompressionLevel.Optimal)
        {
            using (var fs = new FileStream(sourceDirectoryName, FileMode.Create))
            using (ZipArchive archive = new ZipArchive(fs, ZipArchiveMode.Create))
                if (!string.IsNullOrWhiteSpace(destinationArchiveFileName))
                    AddDirs(archive, new DirectoryInfo(destinationArchiveFileName));
            void AddFiles(ZipArchive archive, params FileInfo[] fileinfos)
            {
                fileinfos = fileinfos.Where(x => x.Exists).ToArray();
                foreach (var file in fileinfos)
                {
                    ZipArchiveEntry readMeEntry = archive.CreateEntry(file.Name, compressionLevel);
                    using (var stream = readMeEntry.Open())
                    {
                        byte[] bytes = File.ReadAllBytes(file.FullName);
                        stream.Write(bytes, 0, bytes.Length);
                    }
                }
            }
            void AddDirs(ZipArchive archive, params DirectoryInfo[] dirinfos)
            {
                dirinfos = dirinfos.Where(x => x.Exists).ToArray();
                foreach (var dirinfo in dirinfos)
                {
                    var fileinfs = dirinfo.GetFiles();
                    AddFiles(archive, fileinfs);
                    if (recursiveDir)
                        AddDirs(archive, dirinfo.GetDirectories());
                }
            }
        }

        /// <summary>
        /// 将指定 zip 存档中的所有文件都解压缩到文件系统的一个目录下。
        /// </summary>
        /// <param name="sourceArchiveFileName">要解压缩存档的路径。</param>
        /// <param name="destinationDirectoryName">放置解压缩文件的目录的路径，指定为相对或绝对路径。 相对路径是指相对于当前工作目录的路径。</param>
        public static void ExtractToDirectory(string sourceArchiveFileName, string destinationDirectoryName)
            => System.IO.Compression.ZipFile.ExtractToDirectory(sourceArchiveFileName, destinationDirectoryName);

        /// <summary>
        /// 创建 zip 存档，该存档包括指定目录的文件和目录，使用指定压缩级别。
        /// </summary>
        /// <param name="sourceDirectoryName">要存档的目录的路径，指定为相对路径或绝对路径。 相对路径是指相对于当前工作目录的路径</param>
        /// <param name="destinationArchiveFileName">要生成的存档路径，指定为相对路径或绝对路径。 相对路径是指相对于当前工作目录的路径。</param>
        /// <param name="compressionLevel">指示创建项时是否强调速度或压缩有效性的枚举值之一。</param>
        public static void CreateFromDirectory(string sourceDirectoryName, string destinationArchiveFileName,
            CompressionLevel compressionLevel = CompressionLevel.Optimal)
            => System.IO.Compression.ZipFile.CreateFromDirectory(sourceDirectoryName, destinationArchiveFileName, compressionLevel, false);
    }
}
#endif