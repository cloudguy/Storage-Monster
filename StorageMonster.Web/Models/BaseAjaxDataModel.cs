using StorageMonster.Web.Services.Metadata;
using System.Collections.Generic;

namespace StorageMonster.Web.Models
{
    public class BaseAjaxDataModel
    {
        public IDictionary<string, ModelMetadataForJs> MetaData { get; set; }
    }
}