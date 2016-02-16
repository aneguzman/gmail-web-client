using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GmailWebClient.Entities;

namespace GmailWebClient.Models.Repositories
{
    public interface IUserRepository
    {
        User GetById(Guid id);
        User GetByUserName(string userName);
        IEnumerable<User> GetUsers();
        void Update(User user);
        void Remove(User user);
        User Create(User user);
    }
}
