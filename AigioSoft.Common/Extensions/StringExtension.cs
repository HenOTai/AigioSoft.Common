using System;
using System.Globalization;
#if !(NET20 || NET35 || NET40)
using System.Linq;
#endif

// ReSharper disable once CheckNamespace
namespace AigioSoft.Common
{
    /// <summary>
    /// System.String 扩展
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// 尝试将日期和时间的指定字符串表示形式转换为其 System.DateTime 等效项。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime? GetDateTime(this string value)
        {
            DateTime temp;
            return DateTime.TryParse(value, out temp) ? temp as DateTime? : null;
        }

        /// <summary>
        /// 尝试将数字的字符串表示形式转换为它的等效 16 位有符号整数。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static short? GetInt16(this string value)
        {
            short temp;
            return short.TryParse(value, out temp) ? temp as short? : null;
        }

        /// <summary>
        /// 尝试将数字的字符串表示形式转换为它的等效 16 位无符号整数。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ushort? GetUInt16(this string value)
        {
            ushort temp;
            return ushort.TryParse(value, out temp) ? temp as ushort? : null;
        }

        /// <summary>
        /// 尝试将数字的字符串表示形式转换为它的等效 32 位有符号整数。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int? GetInt32(this string value)
        {
            int temp;
            return int.TryParse(value, out temp) ? temp as int? : null;
        }

        /// <summary>
        /// 尝试将数字的字符串表示形式转换为它的等效 32 位无符号整数。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static uint? GetUInt32(this string value)
        {
            uint temp;
            return uint.TryParse(value, out temp) ? temp as uint? : null;
        }

        /// <summary>
        /// 尝试将数字的字符串表示形式转换为它的等效 64 位有符号整数。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long? GetInt64(this string value)
        {
            long temp;
            return long.TryParse(value, out temp) ? temp as long? : null;
        }

        /// <summary>
        /// 尝试将数字的字符串表示形式转换为它的等效 64 位无符号整数。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ulong? GetUInt64(this string value)
        {
            ulong temp;
            return ulong.TryParse(value, out temp) ? temp as ulong? : null;
        }

        /// <summary>
        /// 尝试将数字的字符串表示形式转换为它的等效 byte 值 。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte? GetByte(this string value)
        {
            byte temp;
            return byte.TryParse(value, out temp) ? temp as byte? : null;
        }

        /// <summary>
        /// 16进制字符串转Bytes
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static byte[] HexStringToBytes(this string hex)
        {
            if (hex.Length == 0)
                return new byte[1];
            if (hex.Length % 2 == 1)
                hex = "0" + hex;
            byte[] numArray = new byte[hex.Length / 2];
            for (int index = 0; index < hex.Length / 2; ++index)
                numArray[index] = byte.Parse(hex.Substring(2 * index, 2), NumberStyles.AllowHexSpecifier);
            return numArray;
        }

#if !(NET20 || NET35 || NET40)
        /// <summary>
        /// 获取时间间隔字符串
        /// </summary>
        /// <param name="value">例（value=”1990/01/01 - 2000/01/01“，returns start=1990/01/01，end=2000/01/01）</param>
        /// <param name="separator">分隔符</param>
        /// <returns></returns>
        public static (DateTime? start, DateTime? end) GetTimeBucket(this string value, string separator = "-")
        {
            var splitArray = value.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            if (splitArray != null && splitArray.Any())
                return (splitArray.FirstOrDefault()?.Trim().GetDateTime(), splitArray.LastOrDefault()
                    ?.Trim()
                    .GetDateTime());
            return (null, null);
        }
#endif

#if NET35
        public static bool IsNullOrWhiteSpace(this string value) => string.IsNullOrEmpty(value) || value.Trim() == string.Empty;
#endif
    }
}