using CloudBin.Core;
using System;
using System.Web.Mvc;

namespace CloudBin.Web.Utilities
{
    public sealed class ControllerFactory : DefaultControllerFactory
    {
        private readonly IDependencyContainer _container;

        public ControllerFactory(IDependencyContainer container)
        {
            _container = container;
        }

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            if (controllerType != null && _container.HasService(controllerType))
            {
                return (IController) _container.Resolve(controllerType);
            }

            return base.GetControllerInstance(requestContext, controllerType);
        }

        public override void ReleaseController(IController controller)
        {
            _container.Release(controller);
        }
    }
}
