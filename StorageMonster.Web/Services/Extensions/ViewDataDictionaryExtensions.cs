using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StorageMonster.Web.Services.Extensions
{
    public static class ViewDataDictionaryExtensions
    {
        public static void AddRequestSuccessMessage(this ViewDataDictionary dictionary, string message)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            IList<string> messages = dictionary[Constants.RequestSuccessMessagesViewDataKey] as IList<string>;
            if (messages == null)
                messages = new List<string>();

            messages.Add(message);
            dictionary[Constants.RequestSuccessMessagesViewDataKey] = messages;
        }

        public static IEnumerable<string> GetRequestSuccessMessages(this ViewDataDictionary dictionary)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            return dictionary[Constants.RequestSuccessMessagesViewDataKey] as IEnumerable<string>;
        }
    }
}