using System.Collections.Specialized;

namespace CloudBin.Scheduling.Core
{
    public interface IJobParameters
    {
        NameValueCollection Parameters { get; set; }
    }
}
