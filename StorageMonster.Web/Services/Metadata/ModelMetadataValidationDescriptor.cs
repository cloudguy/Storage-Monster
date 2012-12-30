using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StorageMonster.Web.Services.Metadata
{
    public class ModelMetadataValidationDescriptor
    {
        public IEnumerable<ModelClientValidationRule> ValidationRules { get; set; }
    }
}