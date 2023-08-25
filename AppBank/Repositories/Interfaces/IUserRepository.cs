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
        public int UserLoginAutentication(string email, string password);
        public int CreateUser(User user);
        public User GetUserById(int id);
        public int UpdateUser(User user);
        public int RemoveUser(int id);
    }
}
