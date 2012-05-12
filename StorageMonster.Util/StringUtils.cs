using System;

namespace StorageMonster.Util
{
    public static class StringUtils
    {
        public static bool IsNullOrEmptyOrWhiteSpace(this string st)
        {
            return st == null || (st.Trim().Length == 0);
        }

        public static string ShortenString(this string @string, int maxLength)
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
    }
}
