using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using StorageMonster.Common;
using StorageMonster.Services;

namespace StorageMonster.Web.Services.Metadata
{
    public class ModelMetadataExtractor : IModelMetadataExtractor
    {
        private const string MetaDataCacheKeyTemplate = "ModelMeta_{0}_{1}";
        private readonly ICacheService _cacheService;
        private readonly ILocaleProvider _localeProvider;

        public ModelMetadataExtractor(ICacheService cacheService, ILocaleProvider localeProvider)
        {
            _cacheService = cacheService;
            _localeProvider = localeProvider;
        }
        public IDictionary<string, ModelMetadataForJs> GetMetadataFromModel<T>(T model, ControllerContext controllerContext)
        {
            Type modelType = typeof (T);
            string cacheKey = string.Format(CultureInfo.InvariantCulture, MetaDataCacheKeyTemplate, _localeProvider.GetThreadLocale().ShortName, modelType.FullName);
            return _cacheService.Get(cacheKey, () =>
                {
                    var meta = ModelMetadataProviders.Current.GetMetadataForType(() => model, modelType);
                    return meta.Properties.ToDictionary(p => p.PropertyName, p => ExtractPropertyMetadata(p, controllerContext));
                    //var jsonMeta = new JavaScriptSerializer().Serialize(metaForJs);
                    //return Uri.EscapeDataString(jsonMeta);
                });
            
        }
        
        private static ModelMetadataForJs ExtractPropertyMetadata(ModelMetadata metadata, ControllerContext controllerContext)
        {
            ModelMetadataForJs jsMeta = new ModelMetadataForJs
                {
                    DisplayName = metadata.GetDisplayName(),
                    ReadOnly = metadata.IsReadOnly
                };
            List<ModelMetadataValidationDescriptor> validationDescriptors = new List<ModelMetadataValidationDescriptor>();
            jsMeta.Validators = validationDescriptors;
            foreach (var modelValidator in metadata.GetValidators(controllerContext))
            {
                ModelMetadataValidationDescriptor descriptor = new ModelMetadataValidationDescriptor
                    {
                        ValidationRules = modelValidator.GetClientValidationRules()
                    };
                validationDescriptors.Add(descriptor);
            }
            return jsMeta;
        }
    }
}