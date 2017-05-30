using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace AigioSoft.Common
{
    /// <summary>
    /// System.Enum 扩展
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// 获取当前枚举描述
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDescriptionString(this Enum value)
        {
            return value.GetType()
                  .GetMember(value.ToString())
                  .Select(x => x.
                  GetCustomAttribute(typeof(DescriptionAttribute), false) as DescriptionAttribute)
                  .FirstOrDefault(x => x != null)?
                  .Description ?? value.ToString();
        }
    }
}