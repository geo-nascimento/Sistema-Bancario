using AppBank.Models;

namespace AppBank.Repositories.Interfaces
{
    internal interface IAccountRepository
    {
        public bool Autentication(int id, string accountNumber, string password);
        public void CreateAccount(int id, Account account);
        public decimal GetBalance(int id);
        public void UpdateBalance(int id, decimal amount);
        public User GetUserAccounts(int id);
    }
}
