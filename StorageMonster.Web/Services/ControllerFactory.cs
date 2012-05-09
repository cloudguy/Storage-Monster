using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using StorageMonster.Services;

namespace StorageMonster.Web.Services
{
    public class ControllerFactory : DefaultControllerFactory
    {
        protected readonly IoCcontainer Container;
        public ControllerFactory(IoCcontainer container)
        {
            Container = container;
        }

        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            try
            {
                Type controllerType = GetControllerType(requestContext, controllerName);
                return GetControllerInstance(controllerType);
            }
            catch (Exception ex)
            {
                throw new HttpException(404, requestContext.HttpContext.Request.FilePath, ex);
            }
        }

        protected virtual IController GetControllerInstance(Type controllerType)
        {
            return Container.Resolve(controllerType) as Controller;
        }

        public override void ReleaseController(IController controller)
        {
            base.ReleaseController(controller);
            if (controller is IDisposable)
                (controller as IDisposable).Dispose();

            controller = null;
        }
    }
}