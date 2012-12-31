using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StorageMonster.Web.Services.Security
{
    public static class AntiForgerySalts
    {
#warning add logon and register salt
        public const string ResetPassword = "94d5c0d4-e5bc-4fac-a5b9-18fa68d6b640";
        public const string ResetPasswordRequest = "c7001e65-4827-472a-8eba-59dc02cc2b06";
    }
}