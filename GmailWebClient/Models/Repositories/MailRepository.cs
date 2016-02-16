using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Web;
using ImapX;
using ImapX.Collections;
using ImapX.Constants;
using ImapX.Enums;
using MailAddress = ImapX.MailAddress;

namespace GmailWebClient.Models.Repositories
{
    public class MailRepository : IMailRepository
    {
        private ImapClient _client = null;
        private string _mailServer = "imap.gmail.com";
        private string _gmailServer = "smtp.gmail.com";
        private string _login;
        private string _password;

        public MailRepository(string login, string password)
        {
            _login = login;
            _password = password;
            SetMailSettings();
            Client.Connect();
            Client.Login(login, password);
        }

        protected ImapClient Client
        {
            get
            {
                if (_client == null)
                    _client = new ImapClient(_mailServer, true);
                return _client;
            }
        }

        public IEnumerable<Message> GetAllMails(string mailBox)
        {
            return GetMails(mailBox, "ALL").Cast<Message>();
        }

        public IEnumerable<Message> GetUnreadMails(string mailBox)
        {
            return GetMails(mailBox, "UNSEEN").Cast<Message>();
        }

        public Message GetMail(string mailBox, long messageId)
        {
            var folder = GetFolder(mailBox);
            var message = folder.Search(string.Format("UID {0}", messageId));
            return message.FirstOrDefault();
        }

        public bool Delete(string folder, long id)
        {
            var isDeleted = false;
            var message = GetMail(folder, id);
            if (message != null)
            {
                try
                {
                    isDeleted = message.Remove();
                }
                catch (Exception)
                {
                    
                }
            }
            return isDeleted;
        }

        public MailMessage SendMail(string to, string subject, string text)
        {
            return SendMail(_gmailServer, _login, _password, _login, to, subject, text);
        }

        public void AddToSentFolder(MailMessage newMsg)
        {
            Client.Folders.Sent.AppendMessage(newMsg);
        }

        private MailMessage SendMail(string server, string login, string password, string from, string to, string subject, string text)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new System.Net.Mail.MailAddress(from);
                mail.To.Add(new System.Net.Mail.MailAddress(to));
                mail.Subject = subject;
                mail.Body = text;

                SmtpClient client = new SmtpClient();
                client.Host = server;
                client.Port = 587;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(login, password);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(mail);

                return mail;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private IEnumerable<Message> GetMails(string mailBox, string searchPhrase, int count = 10)
        {
            var messages = new List<Message>();
            var folder = GetFolder(mailBox);
            if (folder != null)
            {
                messages = folder.Search(searchPhrase, MessageFetchMode.ClientDefault, count).ToList();
            }
            return messages;
        }

        private Folder GetFolder(string mailBox = "Inbox")
        {
            Folder folder;
            
            //TODO: ADD ALL OPTIONS

            switch (mailBox)
            {
                case "Trash":
                    folder = Client.Folders.Trash;
                    break;
                default:
                    folder = Client.Folders.Inbox;
                    break;
            }
            return folder;
        }

        private void SetMailSettings()
        {
            // Set the client to download only flags, headers and message bodies (no attachments)
            Client.Behavior.MessageFetchMode = MessageFetchMode.Flags | MessageFetchMode.Headers | MessageFetchMode.Body;

            // Limit the headers to be fetched
            Client.Behavior.RequestedHeaders = new[] {
                  MessageHeader.From,
                  MessageHeader.To,
                  MessageHeader.Subject,
                  MessageHeader.Date,
                  MessageHeader.MessageId,
                  MessageHeader.ContentType,
            };
        }
    }
}