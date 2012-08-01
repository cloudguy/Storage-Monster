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

            IList<string> messages = dictionary[Constants.RequestInfoMessagesViewDataKey] as IList<string>;
            if (messages == null)
                messages = new List<string>();

            messages.Add(message);
            dictionary[Constants.RequestInfoMessagesViewDataKey] = messages;
        }

        public static IEnumerable<string> GetRequestSuccessMessage(this ViewDataDictionary dictionary)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            return dictionary[Constants.RequestInfoMessagesViewDataKey] as IEnumerable<string>;
        }
    }
}