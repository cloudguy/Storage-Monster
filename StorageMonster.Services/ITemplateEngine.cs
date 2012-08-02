using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorageMonster.Services
{
    public interface ITemplateEngine
    {
        string TransformTemplate(IDictionary<string, object> templateData, string template);
    }
}
