using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.Mvc;
using Common.Logging;
using StorageMonster.Utilities.Serialization;

namespace StorageMonster.Web.Services
{
    public class CookieTempDataProvider : ITempDataProvider
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(CookieTempDataProvider));

        private const string CookieName = "MonsterTempData";

        private ISerializer _serializer;

        public CookieTempDataProvider()
        {
            _serializer = new BinarySerializer();
        }

        public IDictionary<string, object> LoadTempData(ControllerContext controllerContext)
        {
            HttpCookie cookie = controllerContext.HttpContext.Request.Cookies[CookieName];
            if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
            {                
                try
                {
                    return _serializer.Deserialize<IDictionary<string, object>>(cookie.Value);
                }               
                catch(SerializationException ex)
                {
                    Logger.Warn(ex);
                    Logger.WarnFormat(CultureInfo.InvariantCulture, "Source string was: {0}", cookie.Value);
                }                
            }

            return new Dictionary<string, object>();
        }

        public void SaveTempData(ControllerContext controllerContext, IDictionary<string, object> values)
        {
            var cookie = new HttpCookie(CookieName);
            cookie.HttpOnly = true;

            if (values.Count == 0)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                cookie.Value = string.Empty;
                controllerContext.HttpContext.Response.Cookies.Set(cookie);
                return;
            }

            cookie.Value = _serializer.Serialize<IDictionary<string, object>>(values);
            controllerContext.HttpContext.Response.Cookies.Add(cookie);
        }
    }
}