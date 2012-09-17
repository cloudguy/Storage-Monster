using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StorageMonster.Web.Services.Extensions
{
    public static class TempDataDictionaryExtensions
    {
        public static void AddRequestSuccessMessage(this TempDataDictionary dictionary, string message)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            IList<string> messages = dictionary[Constants.RequestSuccessMessagesTempDataKey] as IList<string> ?? new List<string>();

            messages.Add(message);
            dictionary[Constants.RequestSuccessMessagesTempDataKey] = messages;
        }

        public static IEnumerable<string> GetRequestSuccessMessages(this TempDataDictionary dictionary)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            return dictionary[Constants.RequestSuccessMessagesTempDataKey] as IEnumerable<string>;
        }

        public static void AddRequestErrorMessage(this TempDataDictionary dictionary, string message)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            IList<string> messages = dictionary[Constants.RequestErrorMessagesTempDataKey] as IList<string> ?? new List<string>();

            messages.Add(message);
            dictionary[Constants.RequestErrorMessagesTempDataKey] = messages;
        }

        public static IEnumerable<string> GetRequestErrorMessages(this TempDataDictionary dictionary)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            return dictionary[Constants.RequestErrorMessagesTempDataKey] as IEnumerable<string>;
        }
    }
}