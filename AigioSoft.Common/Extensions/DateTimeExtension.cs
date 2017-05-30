using Newtonsoft.Json;
using System;

// ReSharper disable once CheckNamespace
namespace AigioSoft.Common
{
    /// <summary>
    /// System.DateTime 扩展
    /// </summary>
    public static class DateTimeExtension
    {
        public const string CompleteFormat = "yyyy-MM-dd HH:mm:ss.fffffff";
        public const string StandardFormat = "yyyy-MM-dd HH:mm:ss";
        public const string ConnectFormat = "yyyyMMddHHmmssfffffff";
        public const string DateFormat = "yyyy-MM-dd";
        public const string NoYearNoSecondFormat = "MM-dd HH:mm";

        /// <summary>
        /// 判断此时间是否在开始时间和结束时间中间
        /// </summary>
        /// <param name="dt">此时间</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="allowEqualStart">是否允许此时间等于开始时间</param>
        /// <param name="allowEqualEnd">是否允许此时间等于结束时间</param>
        /// <returns></returns>
        public static bool IsInBetween(this DateTime dt, DateTime start, DateTime end, bool allowEqualStart = true, bool allowEqualEnd = true)
        {
            return allowEqualEnd ? dt < end.AddDays(1) : dt < end && allowEqualStart ? dt >= start : dt > start;
        }

        /// <summary>
        /// 将时间类型格式化成 yyyy-MM-dd
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToDateFormat(this DateTime dt) => dt.ToString(DateFormat);

        /// <summary>
        /// 将时间类型格式化成 MM-dd HH:mm
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToNoYearNoSecondFormat(this DateTime dt) => dt.ToString(NoYearNoSecondFormat);

        /// <summary>
        /// 将时间类型格式化成 yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToStandardString(this DateTime dt) => dt.ToString(StandardFormat);

        /// <summary>
        /// 将时间类型格式化成 yyyyMMddHHmmssfffffff
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToConnectFormat(this DateTime dt) => dt.ToString(ConnectFormat);

        /// <summary>
        /// 将时间类型格式化成 yyyy-MM-dd HH:mm:ss.fffffff
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToCompleteFormat(this DateTime dt) => dt.ToString(CompleteFormat);

        /// <summary>
        /// Dates are written in the Microsoft JSON format, e.g. "\/Date(1198908717056)\/"
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToMicrosoftDateFormat(this DateTime dt)
        {
            return JsonConvert.SerializeObject(dt, new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
            }).Replace("\"", string.Empty).Replace("\\", string.Empty);
        }
    }
}