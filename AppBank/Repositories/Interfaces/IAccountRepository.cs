using AppBank.Models;

namespace AppBank.Repositories.Interfaces
{
    internal interface IAccountRepository
    {
        public int Autentication(string email, string password);
        public void CreateAccount(Account account);
        public decimal GetBalance(int id);
        public void UpdateBalance(int id, decimal amount);
        public User GetUserAccounts(int id);
    }
}
