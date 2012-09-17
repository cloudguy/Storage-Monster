using System;
using System.IO;
using System.Web.Mvc;
using System.Globalization;

namespace StorageMonster.Web.Services.Extensions
{
    public static class ControllerExtensions
    {
        public static string RenderViewToString(this ControllerBase controller, string viewName, object model)
        {
            if (controller == null)
                throw new ArgumentNullException("controller");

            if (string.IsNullOrEmpty(viewName))
                viewName = controller.ControllerContext.RouteData.GetRequiredString("action");

            controller.ViewData.Model = model;

            using (StringWriter sw = new StringWriter(CultureInfo.CurrentCulture))
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                if (viewResult.View == null)
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "View '{0}' not found in the context of controller {1}", viewName, controller.GetType().FullName));
                ViewContext viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
    }
}
