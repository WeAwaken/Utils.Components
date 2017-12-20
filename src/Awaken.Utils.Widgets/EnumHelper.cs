using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Awaken.Utils.Widgets
{
    public static class EnumHelper
    {
        public static string GetDescription<TEnum>(this TEnum theEnum) where TEnum : IComparable, IConvertible, IFormattable
        {
            var name = Enum.GetName(typeof(TEnum), Convert.ToInt32(theEnum));

            var attrs = typeof(TEnum)
                .GetField(name)
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .ToList();

            if (attrs != null && attrs.Any()) {
                return (attrs[0] as DescriptionAttribute).Description; 
            }

            return string.Empty;
        }
    }
}
