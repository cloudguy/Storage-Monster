using StorageMonster.Web.Models;
using StorageMonster.Web.Services.Metadata;
using System;
using System.Web.Mvc;

namespace StorageMonster.Web.Services.ActionResults
{
    public class JsonWithMetadataResult<T>: JsonResult  where T : BaseAjaxDataModel
    {
        private readonly T _model;
        private readonly IModelMetadataExtractor _metadataExtractor;
        public JsonWithMetadataResult(T model, IModelMetadataExtractor metadataExtractor, JsonRequestBehavior requestBehavior)
        {
            _model = model;
            _metadataExtractor = metadataExtractor;
            JsonRequestBehavior = requestBehavior;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            if (_model == null)
                throw new ArgumentNullException("model");

            if (_metadataExtractor == null)
                throw new ArgumentNullException("metadataExtractor");

            _model.MetaData = _metadataExtractor.GetMetadataFromModel(_model, context);

            Data = _model;
            base.ExecuteResult(context);
        }
    }
}