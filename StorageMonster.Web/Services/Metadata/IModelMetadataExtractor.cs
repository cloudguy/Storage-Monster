using System.Collections.Generic;
using System.Web.Mvc;

namespace StorageMonster.Web.Services.Metadata
{
    public interface IModelMetadataExtractor
    {
        IDictionary<string, ModelMetadataForJs> GetMetadataFromModel<T>(T model, ControllerContext controllerContext);
    }
}
