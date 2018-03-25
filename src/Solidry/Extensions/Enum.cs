using System;
using System.ComponentModel;

namespace Solidry.Extensions
{
    public static class Enum
    {
        /// <summary>
        /// Get attribute of enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(this System.Enum value) where T : Attribute
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString());
            var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
            return (T)attributes[0];
        }

        /// <summary>
        /// Get description attribute of enum
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this System.Enum value)
        {
            return value.GetAttribute<DescriptionAttribute>().Description;
        }
    }
}