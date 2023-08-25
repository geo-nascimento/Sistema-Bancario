using AppBank.Models;
using AppBank.Models.Enums;
using AppBank.Repositories;
using AppBank.Repositories.Interfaces;
using AppBank.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppBank.Controllers
{
    public class AccountController
    {
        private readonly IAccountRepository _accDb;

        public AccountController()
        {
            _accDb = new AccountRepository();
        }
        #region Autenticação
        public int UserLogin(string email, string password)
        {
            int userId = _accDb.Autentication(email, password);
            if (userId == 0)
            {
                throw new Exception("Usuário não encontrado");
            }

            return userId;
        }
        #endregion
        #region Criar uma conta
        public void RegisterAccount(int userId, string accountNumber, string password, AccountType type)
        {
            Account account = new Account() 
            { 
               UserId = userId, AccountNumber = accountNumber, Password = password, AccountType = type, RegistrationDate = DateTimeOffset.Now
            };

            _accDb.CreateAccount(account);

        }
        #endregion
        #region Trazer informações da conta
        public User GetAccountsInfo(int id)
        {
            var user = _accDb.GetUserAccounts(id);
            
            if(user == null)
            {
                throw new Exception("Usuário não encontrado");
            }

            return user;
        }
        #endregion
        #region Sacar
        public void ToWithdraw(int id, decimal amount) 
        {
            var balance = _accDb.GetBalance(id);

            var newBalance = balance - amount;

            _accDb.UpdateBalance(id, newBalance);
        }
        #endregion
        #region Depositar
        public void ToDeposit(int id, decimal amount)
        {
            var newbalance = _accDb.GetBalance(id) + amount;
            
            _accDb.UpdateBalance(id, newbalance);
        }
        #endregion
        #region Pedir empréstimo
        public void GetLoan(int id, decimal amount)
        {
            var balance = _accDb.GetBalance(id);

            var newBalance = balance + amount;

            _accDb.UpdateBalance(id, newBalance);
        }
        #endregion
        #region Fazer transferência
        public void ToTranfer(int idAccount,int idDestinationAccount, decimal amount, ITransferTaxService taxService, decimal tax)
        {
            //Altera o saldo da conta destino
            var destinationBalance = _accDb.GetBalance(idDestinationAccount);
            
            var taxedAmount = taxService.TaxFee(amount, tax);//aplica a taxa de transferência no valor a ser transferido

            var newDestinationBalance = destinationBalance + taxedAmount;

            _accDb.UpdateBalance(idDestinationAccount,newDestinationBalance);

            //Altera o saldo da conta de origem
            var originAccountBalance = _accDb.GetBalance(idAccount);
            
            var newBalance = originAccountBalance - amount;

            _accDb.UpdateBalance(idAccount, newBalance);
        }
        #endregion
        #region Excluir uma conta (Não funciona)
        public void RemoveAccount(int accountId)
        {
            var balance = _accDb.GetBalance(accountId);
            if (balance > 0.0M)
            {
                throw new Exception("Não posso excluir uma conta que contem um saldo maior que 0.0 R$. Por gentileza, transfira o saldo para outra conta para que seja possível excluí-la");
            }
    
        }
        #endregion
    }
}
