using System.Web;

namespace CloudBin.Web.Core
{
    public interface IOpenDatabaseConnectionPolicy
    {
        bool DatabaseConnectionRequired(HttpContext context);
    }
}
