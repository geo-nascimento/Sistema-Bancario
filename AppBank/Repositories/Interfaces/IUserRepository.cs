using AppBank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBank.Repositories.Interfaces
{
    internal interface IUserReporitory 
    {
        public void CreateUser(User user);
        public User GetUserById(int id);
        public List<User> GetUsers();
        public void UpdateUser(User user);
        public void DeleteUser(int id);
    }
}
