namespace StorageMonster.Util
{
    public static class StringUtils
    {
        public static bool IsNullOrEmptyOrWhiteSpace(this string st)
        {
            return st == null || (st.Trim().Length == 0);
        }
    }
}
