using System.Collections.Generic;

namespace StorageMonster.Services
{
    public interface ITemplateEngine
    {
        string TransformTemplate(IDictionary<string, object> templateData, string template);
    }
}
