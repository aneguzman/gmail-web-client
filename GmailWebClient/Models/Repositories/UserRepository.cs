using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GmailWebClient.Entities;

namespace GmailWebClient.Models.Repositories
{
    public class UserRepository : IUserRepository
    {
        public User GetById(Guid id)
        {
            using (var db = new DbDataContext())
            {
                return db.Users.FirstOrDefault(x => x.UserId == id);
            }
        }

        public User GetByUserName(string userName)
        {
            using (var db = new DbDataContext())
            {
                return db.Users.FirstOrDefault(x => x.UserName == userName);
            }
        }


        public IEnumerable<User> GetUsers()
        {
            using (var db = new DbDataContext())
            {
                var users = db.Users;
                return users;
            }
        }

        public void Update(User user)
        {
            using (var db = new DbDataContext())
            {
                var _user = db.Users.FirstOrDefault(x => x.UserId == user.UserId);
                if (user != null)
                {
                    _user.Password = user.Password;
                    _user.UserName = user.UserName;
                    _user.GmailUser = user.GmailUser;
                    _user.GmailPassword = user.GmailPassword;
                    _user.CreateDate = user.CreateDate;
                    db.SubmitChanges();
                }
            }
        }

        public void Remove(User user)
        {
            using (var db = new DbDataContext())
            {
                var _user = db.Users.FirstOrDefault(x => x.UserId == user.UserId);
                if (_user != null)
                {
                    db.Users.DeleteOnSubmit(_user);
                    db.SubmitChanges();
                }
            }
        }

        public User Create(User user)
        {
            using (var db = new DbDataContext())
            {
                if (user != null)
                {
                    db.Users.InsertOnSubmit(user);
                    db.SubmitChanges();
                }
            }

            return user;
        }
    }
}