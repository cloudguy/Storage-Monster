﻿namespace StorageMonster.Web
{
    public static class Constants
    {
        public const string ForbiddenRequestLoggerName = "ForbiddenRequests";

        public const string JsonContentType = "application/json";
        public const string BinaryStreamContentType = "application/octet-stream";

        public const string RequestInfoHtmlClass = "request-info-summary";
        public const string RequestErrorHtmlClass = "request-error-summary";

        public const string StorageAccountIdFormKey = "storageAccountId";
        public const string StampFormKey = "stamp";
        public const string StorageHistoryFormKey = "nav_history";

        public const string StorageAccountTitleViewDataKey = "storageAccountTitle";
        public const string MenuActivatorViewDataKey = "menu_activator";
        public const string RequestSuccessMessagesViewDataKey = "requestMessages_vd";
        public const string RequestErrorMessagesViewDataKey = "requestErrors_vd";

        public const string RequestSuccessMessagesTempDataKey = "requestMessages_td";
        public const string RequestErrorMessagesTempDataKey = "requestErrors_td";

        #region antiforgery salts
        public const string Salt_Account_Edit = "062e973d-e20a-4029-af84-e34ecb66787e";
        public const string Salt_StorageAccount_Edit = "f8d6f50d-5c65-490d-87c1-a901644e0721";
        public const string Salt_StorageAccount_Delete = "f6221aae-2a6f-4559-b7de-b4b1886f3711";
        public const string Salt_StorageAccount_Add = "e290c087-1368-4a7d-a3a6-f2d51683be53";
        public const string Salt_StorageAccount_GetFolder = "c68040d9-fcb3-4b93-8fa4-cf355e9fb8eb";
        #endregion

        #region user roles
        public const string RoleUser = "ROLE_USER";
        public const string RoleAdmin = "ROLE_ADMIN";
        #endregion

#warning move to config
        public const string UserNameRegexp = "^[a-zA-Z1-9 ]{1,100}$";
        public const string StorageAccountNameRegexp = "^[a-zA-Z1-9 ]{1,100}$";
        public const string EmailRegexp = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
    }
}