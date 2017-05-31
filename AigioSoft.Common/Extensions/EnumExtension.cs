using System;
using System.ComponentModel;
using System.Linq;
#if !(NET20 || NET35 || NET40)
using System.Reflection;
#endif

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
#if !(NET20 || NET35 || NET40)
                  GetCustomAttribute(typeof(DescriptionAttribute), false) as DescriptionAttribute)
#else
                  GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[]).FirstOrDefault(x => x != null)?
#endif
                  .FirstOrDefault(x => x != null)?
                  .Description ?? value.ToString();
        }
    }
}