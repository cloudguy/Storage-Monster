using System.Web;

namespace CloudBin.Web.Utilities
{
    public interface IOpenDatabaseConnectionPolicy
    {
        bool DatabaseConnectionRequired(HttpContext context);
    }
}
