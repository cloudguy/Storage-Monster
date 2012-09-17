using System;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using StorageMonster.Services;
using StorageMonster.Utilities.Serialization;

namespace StorageMonster.Web.Services
{
    public class ControllerFactory : DefaultControllerFactory
    {
        private readonly IocContainer _container;

        public ControllerFactory(IocContainer container)
        {
            _container = container;
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

            return GetControllerInstance(requestContext, controllerType);

        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            var controller = _container.Resolve(controllerType) as Controller ?? base.GetControllerInstance(requestContext, controllerType) as Controller;
            if (controller == null)
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Can't create controller {0}", controllerType.FullName));
            controller.TempDataProvider = new CookieTempDataProvider();
            return controller;
        }
    }
}