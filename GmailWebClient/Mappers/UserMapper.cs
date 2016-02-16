using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GmailWebClient.Entities;
using GmailWebClient.Models;

namespace GmailWebClient.Mappers
{
    public class UserMapper : IUserMappper
    {
        public User MapToUser(RegisterModel viewModel, User user)
        {
            user.UserId = Guid.NewGuid();
            user.CreateDate = DateTime.Now;
            user.Password = viewModel.Password;
            user.UserName = viewModel.UserName;
            return user;
        }
    }
}