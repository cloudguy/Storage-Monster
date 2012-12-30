using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StorageMonster.Web.Services.Metadata
{
    public class ModelMetadataForJs
    {
        public string DisplayName { get; set; }
        public bool ReadOnly { get; set; }
        public IEnumerable<ModelMetadataValidationDescriptor> Validators { get; set; }
    }
}