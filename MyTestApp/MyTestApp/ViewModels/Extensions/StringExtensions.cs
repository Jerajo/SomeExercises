using System;
using System.Collections.Generic;
using System.Text;

namespace MyTestApp.ViewModels.Extensions
{
    public static class StringExtensions
    {
        public static string Reverse(this string value)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                return string.Empty;

            char[] charArray = value.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}
