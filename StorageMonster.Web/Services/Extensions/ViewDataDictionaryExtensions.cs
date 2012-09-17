using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace StorageMonster.Web.Services.Extensions
{
    public static class ViewDataDictionaryExtensions
    {
        public static void AddRequestSuccessMessage(this ViewDataDictionary dictionary, string message)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            IList<string> messages = dictionary[Constants.RequestSuccessMessagesViewDataKey] as IList<string> ?? new List<string>();

            messages.Add(message);
            dictionary[Constants.RequestSuccessMessagesViewDataKey] = messages;
        }

        public static IEnumerable<string> GetRequestSuccessMessages(this ViewDataDictionary dictionary)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            return dictionary[Constants.RequestSuccessMessagesViewDataKey] as IEnumerable<string>;
        }

        public static void AddRequestErrorMessage(this ViewDataDictionary dictionary, string message)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            IList<string> messages = dictionary[Constants.RequestErrorMessagesViewDataKey] as IList<string> ?? new List<string>();

            messages.Add(message);
            dictionary[Constants.RequestErrorMessagesViewDataKey] = messages;
        }

        public static IEnumerable<string> GetRequestErrorMessages(this ViewDataDictionary dictionary)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            return dictionary[Constants.RequestErrorMessagesViewDataKey] as IEnumerable<string>;
        }
    }
}