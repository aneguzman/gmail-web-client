using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GmailWebClient.Entities;
using GmailWebClient.Models;

namespace GmailWebClient.Mappers
{
    public interface IUserMappper
    {
        User MapToUser(RegisterModel viewModel, User user);
    }
}
