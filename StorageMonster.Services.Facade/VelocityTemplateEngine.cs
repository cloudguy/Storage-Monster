using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Commons.Collections;
using NVelocity;
using NVelocity.App;

namespace StorageMonster.Services.Facade
{
    public class VelocityTemplateEngine : ITemplateEngine
    {
        public string TransformTemplate(IDictionary<string,object> templateData, string template)
        {
            if (templateData == null)
                throw new ArgumentNullException("templateData");
            var context = new VelocityContext();
            foreach(var dataItem in templateData)
                context.Put(dataItem.Key, dataItem.Value);
            return ApplyTemplate(template, context);
        }

        private static string ApplyTemplate(string template, VelocityContext context)
        {
            VelocityEngine velocity = new VelocityEngine();
            ExtendedProperties props = new ExtendedProperties();            
            velocity.Init(props);
            using (var writer = new StringWriter(CultureInfo.CurrentCulture))
            {
                velocity.Evaluate(context, writer, string.Empty, template);
                return writer.GetStringBuilder().ToString();
            }
        }
    }
}
