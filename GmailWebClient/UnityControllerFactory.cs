using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;

namespace GmailWebClient
{
    public class UnityControllerFactory : DefaultControllerFactory
    {
        private readonly IUnityContainer _container;

        public UnityControllerFactory(IUnityContainer container)
        {
            _container = container;
        }

        public override IController CreateController(System.Web.Routing.RequestContext requestContext, string controllerName)
        {
            return base.CreateController(requestContext, controllerName);
        }

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            if (controllerType != null)
            {
                return _container.Resolve(controllerType) as IController;
            }
            return null;
        }

        public bool HasMappingFor(Type type)
        {
            try
            {
                _container.Resolve(type);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}