using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace GE.Warehouse.Core.Extension
{
    public static class EnumExtension
    {
        //public static List<KeyValuePair<string, string>> GetEnumList(Enum enumeration)
        //{
        //    string[] enumStrings = Enum.GetNames(enumeration.GetType());
        //    return enumStrings.Select(enumString => new KeyValuePair<string, string>(enumString, enumString.ToCamelCase())).ToList();
        //}

        public static string GetDescription(this Enum value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }

        /// <summary>
        /// Gets a list of key/value pairs for an enum, using the description attribute as value
        /// </summary>
        /// <param name="enumType"/>>typeof(your enum type)
        /// <returns>A list of KeyValuePairs with enum values and descriptions</returns>
        public static List<KeyValuePair<string, string>> GetValuesAndDescription(this Type enumType)
        {
            return (from Enum enumValue in Enum.GetValues(enumType)
                    select new KeyValuePair<string, string>(enumValue.ToString(), GetDescription(enumValue))).ToList();
        }

        /// <summary>
        /// Gets a list of key/value pairs for an enum, using the description attribute as value
        /// </summary>
        /// <param name="enumType"/>>typeof(your enum type)
        /// <returns>A list of KeyValuePairs with enum values and descriptions</returns>
        public static List<KeyValuePair<int, string>> GetIntAndDescription(this Type enumType)
        {
            return (from Enum enumValue in Enum.GetValues(enumType)
                    select new KeyValuePair<int, string>(Convert.ToInt32(enumValue), GetDescription(enumValue))).ToList();
        }
    }
}
