using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace StorageMonster.Web.Services.Extensions
{
    public static class ViewDataDictionaryExtensions
    {
        public const string RequestSuccessMessagesViewDataKey = "requestMessages_vd";
        public const string RequestErrorMessagesViewDataKey = "requestErrors_vd";

        public static void AddRequestSuccessMessage(this ViewDataDictionary dictionary, string message)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            IList<string> messages = dictionary[RequestSuccessMessagesViewDataKey] as IList<string> ?? new List<string>();

            messages.Add(message);
            dictionary[RequestSuccessMessagesViewDataKey] = messages;
        }

        public static IEnumerable<string> GetRequestSuccessMessages(this ViewDataDictionary dictionary)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            return dictionary[RequestSuccessMessagesViewDataKey] as IEnumerable<string>;
        }

        public static void AddRequestErrorMessage(this ViewDataDictionary dictionary, string message)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            IList<string> messages = dictionary[RequestErrorMessagesViewDataKey] as IList<string> ?? new List<string>();

            messages.Add(message);
            dictionary[RequestErrorMessagesViewDataKey] = messages;
        }

        public static IEnumerable<string> GetRequestErrorMessages(this ViewDataDictionary dictionary)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            return dictionary[RequestErrorMessagesViewDataKey] as IEnumerable<string>;
        }
    }
}