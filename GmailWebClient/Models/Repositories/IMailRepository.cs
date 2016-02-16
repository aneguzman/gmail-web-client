using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using ImapX;

namespace GmailWebClient.Models.Repositories
{
    public interface IMailRepository
    {
        IEnumerable<Message> GetAllMails(string mailBox);
        IEnumerable<Message> GetUnreadMails(string mailBox);
        Message GetMail(string folder, long id);
        bool Delete(string folder, long id);
        MailMessage SendMail(string to, string subject, string text);
        void AddToSentFolder(MailMessage newMsg);
    }
}
