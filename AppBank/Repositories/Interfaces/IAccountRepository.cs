using AppBank.Models;

namespace AppBank.Repositories.Interfaces
{
    internal interface IAccountRepository
    {
        public int AccountLoginAutentication(string AccountNumber, string password);
        public int CreateAccount(Account account);
        public decimal GetBalance(int id);
        public int UpdateBalance(int id, decimal amount);
        public Account GetAccountById(int id);
        public int RemoveAccount(int id);
    }
}
