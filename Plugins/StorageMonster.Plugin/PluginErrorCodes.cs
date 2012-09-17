using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageMonster.Plugin
{
    public enum PluginErrorCodes
    {
        PluginError = 0,
        CouldNotContactStorageService = 1,
        InvalidFileOrDirectoryName = 2,
        FileNotFound = 3,
        InvalidCredentialsOrConfiguration = 4,
        CouldNotRetrieveDirectoryList = 5,
        CreateOperationFailed = 6,
        LimitExceeded = 7,
        InsufficientDiskSpace = 8,
        TransferAbortedManually = 9
    }
}
