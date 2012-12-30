﻿using System;
using System.Linq;
using System.Text;

namespace StorageMonster.Utilities
{
    public static class StringExtensions
    {
        public static string Shorten(this string @string, int maxLength)
        {
            if (maxLength < 1)
                throw new ArgumentException("maxLength can not be less than 1");

            if (string.IsNullOrEmpty(@string)) return @string;

            if (@string.Length <= maxLength)
                return @string;

            if (maxLength <= 4)
                return @string.Substring(0,  maxLength);

            string chunk = @string.Substring(0, maxLength - 3);
            return chunk + "...";
        }

        public static string RemoveCharDuplicates(this string @string, params char[] charsToRemove)
        {
            if (@string == null)
                throw new ArgumentNullException("string");
            if(@string.Length == 0)
                return @string;

            if (charsToRemove == null || charsToRemove.Length == 0)
                return @string;
                        
            char prevChar = @string[0];
            StringBuilder builder = new StringBuilder().Append(prevChar);
            for (int i = 1; i < @string.Length; i++)
            {
                if (charsToRemove.Contains(@string[i]) && @string[i] == prevChar)
                    continue;
                builder.Append(@string[i]);
                prevChar = @string[i];
            }
            return builder.ToString();
        }
    }
}
