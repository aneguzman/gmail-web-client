using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GmailWebClient.Models.Repositories;
using Microsoft.Practices.Unity;

namespace GmailWebClient.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        [Dependency]
        public IUserRepository Repository { get; set; }

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var currentUser = Repository.GetByUserName(User.Identity.Name);
                if (string.IsNullOrEmpty(currentUser.GmailUser))
                {
                    return View();
                }
                else
                {
                    //Redirect to MailController
                    return RedirectToAction("Index", "Mail");
                }
            }
           
            return RedirectToAction("Login", "Account");
        }

    }
}
