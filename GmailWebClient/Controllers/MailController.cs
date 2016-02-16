using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GmailWebClient.Models.Repositories;
using ImapX;
using ImapX.Constants;
using ImapX.Enums;
using ImapX.Flags;
using Microsoft.Practices.Unity;

namespace GmailWebClient.Controllers
{
    public class MailController : Controller
    {
        //
        // GET: /Mail/
        [Dependency]
        public IUserRepository UserRepository { get; set; }
        public IMailRepository MailRepository;
        private string _folder = "Inbox";

        public ActionResult Index()
        {
            SetMailRepository();
            var mails = MailRepository.GetAllMails(_folder);
            ViewBag.Mails = mails;
            ViewBag.Folder = _folder;
            return View();
        }
        
        public JsonResult Delete(long uId, string folder)
        {
            SetMailRepository();
            var isDeleted = false;
            if (uId > 0)
            {
                isDeleted = MailRepository.Delete(folder, uId);
            }
            return Json(new { success = isDeleted });
        }

        public JsonResult Compose(string body, string subject, string to)
        {
            SetMailRepository();
            var isSent = false;
            var mail = MailRepository.SendMail(to, subject, body);
            if (mail != null)
            {
                isSent = true;
                MailRepository.AddToSentFolder(mail);
            }

            return Json(new { success = isSent });
        }

        private void SetMailRepository()
        {
            var currentUser = UserRepository.GetByUserName(User.Identity.Name);
            MailRepository = new MailRepository(currentUser.GmailUser, currentUser.GmailPassword);
        }
    }

    public class CMail
    {
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
