using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using GmailWebClient.Entities;
using GmailWebClient.Mappers;
using GmailWebClient.Models.Repositories;
using Microsoft.Practices.Unity;

namespace GmailWebClient
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // set up the Unity container and the controller factory that will use it
            IUnityContainer container = new UnityContainer();

            // AccountController dependencies
            container.RegisterType<IUserMappper, UserMapper>(new ContainerControlledLifetimeManager());
            container.RegisterType<IUserRepository, UserRepository>(new ContainerControlledLifetimeManager());

            SetMailRepositoryDependencies(container);

            // Create the controller factory using the Unity container and set it in the current ControllerBuilder
            var factory = new UnityControllerFactory(container);
            ControllerBuilder.Current.SetControllerFactory(factory);
        }

        private void SetMailRepositoryDependencies(IUnityContainer container)
        {
            if (User != null)
            {
                using (var db = new DbDataContext())
                {
                    var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

                    if (user != null)
                    {
                        container.RegisterType<IMailRepository, MailRepository>(new ContainerControlledLifetimeManager())
                            .RegisterInstance(new MailRepository(user.GmailUser, user.GmailPassword));
                    }
                }
            }
            

        }
    }
}