using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using StorageMonster.Services;

namespace StorageMonster.Web.Services
{
    public class ControllerFactory : DefaultControllerFactory
    {
        protected IocContainer Container { get; set; }

        public ControllerFactory(IocContainer container)
        {
            Container = container;
        }


        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            if (requestContext == null)
            {
                throw new ArgumentNullException("requestContext");
            }
            if (string.IsNullOrEmpty(controllerName))
            {
                throw new ArgumentException("Controller name not specified");
            }


            Type controllerType = GetControllerType(requestContext, controllerName);
            if (controllerType == null)
                throw new HttpException(404, requestContext.HttpContext.Request.FilePath);

            return GetControllerInstance(controllerType);

        }

        protected virtual IController GetControllerInstance(Type controllerType)
        {
            return Container.Resolve(controllerType) as Controller;
        }
    }
}