using System.Web.Mvc;

namespace StorageMonster.Web.Services.Metadata
{
    public interface IModelMetadataExtractor
    {
        string GetMetadataFromModel<T>(T model, ControllerContext controllerContext);
    }
}
