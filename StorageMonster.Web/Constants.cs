namespace StorageMonster.Web
{
    public static class Constants
    {
        public const string JsonContentType = "application/json";

        public const string StorageAccountIdFormKey = "storageAccountId";
        public const string StampFormKey = "stamp";

        public const string StorageAccountTitleViewDataKey = "storageAccountTitle";
        public const string MenuActivatorViewDataKey = "menu_activator";

        #region antiforgery salts
        public const string Salt_StorageAccount_Edit = "f8d6f50d-5c65-490d-87c1-a901644e0721";
        public const string Salt_StorageAccount_Delete = "f6221aae-2a6f-4559-b7de-b4b1886f3711";
        public const string Salt_StorageAccount_Add = "e290c087-1368-4a7d-a3a6-f2d51683be53";
        public const string Salt_StorageAccount_GetFolder = "c68040d9-fcb3-4b93-8fa4-cf355e9fb8eb";
        #endregion

#warning move to config
        public const string UserNameRegexp = "^[a-zA-Z1-9 ]{1,100}$";
        public const string StorageAccountNameRegexp = "^[a-zA-Z1-9 ]{1,100}$";
        public const string EmailRegexp = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
    }
}