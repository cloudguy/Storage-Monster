using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace StorageMonster.Web.Services.Extensions
{
    public static class TempDataDictionaryExtensions
    {
        public const string RequestSuccessMessagesTempDataKey = "requestMessages_td";
        public const string RequestErrorMessagesTempDataKey = "requestErrors_td";

        public static void AddRequestSuccessMessage(this TempDataDictionary dictionary, string message)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            IList<string> messages = dictionary[RequestSuccessMessagesTempDataKey] as IList<string> ?? new List<string>();

            messages.Add(message);
            dictionary[RequestSuccessMessagesTempDataKey] = messages;
        }

        public static IEnumerable<string> GetRequestSuccessMessages(this TempDataDictionary dictionary)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            return dictionary[RequestSuccessMessagesTempDataKey] as IEnumerable<string>;
        }

        public static void AddRequestErrorMessage(this TempDataDictionary dictionary, string message)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            IList<string> messages = dictionary[RequestErrorMessagesTempDataKey] as IList<string> ?? new List<string>();

            messages.Add(message);
            dictionary[RequestErrorMessagesTempDataKey] = messages;
        }

        public static IEnumerable<string> GetRequestErrorMessages(this TempDataDictionary dictionary)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            return dictionary[RequestErrorMessagesTempDataKey] as IEnumerable<string>;
        }
    }
}