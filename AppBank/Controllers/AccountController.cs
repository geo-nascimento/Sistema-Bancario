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
        public int LoginAccount(string accountNumber, string password)
        {
            int accountId = _accDb.AccountLoginAutentication(accountNumber, password);
            if (accountId == 0)
            {
                throw new Exception("Conta não encontrado");
            }

            return accountId;
        }
        #endregion
        #region Criar uma conta
        public int RegisterAccount(int userId, string accountNumber, string password, AccountType type)
        {
            Account account = new Account() 
            { 
               UserId = userId, AccountNumber = accountNumber, Password = password, AccountType = type, RegistrationDate = DateTimeOffset.Now
            };

            int sucess = _accDb.CreateAccount(account);

            if (sucess == 0)
            {
                throw new Exception("Não foi possível completar a operação");
            }
            return sucess;

        }
        #endregion
        #region Trazer informações da conta
        public Account GetAccountsInfo(int accountId)
        {
            var account = _accDb.GetAccountById(accountId);
            
            if(account == null)
            {
                throw new Exception("Usuário não encontrado");
            }

            return account;
        }
        #endregion
        #region Sacar
        public int ToWithdraw(int id, decimal amount) 
        {
            var balance = _accDb.GetBalance(id);

            var newBalance = balance - amount;

            int sucess = _accDb.UpdateBalance(id, newBalance);

            if (sucess == 0)
            {
                throw new Exception("Não foi possível realizar essa operação");
            }

            return sucess;
        }
        #endregion
        #region Depositar
        public int ToDeposit(int id, decimal amount)
        {
            var newbalance = _accDb.GetBalance(id) + amount;
            
            int sucess = _accDb.UpdateBalance(id, newbalance);

            if (sucess == 0)
            {
                throw new Exception("Não foi possível concluir a operação");
            }

            return sucess;
        }
        #endregion
        #region Pedir empréstimo
        public void GetLoan(int id, decimal amount)
        {
            var balance = _accDb.GetBalance(id);

            var newBalance = balance + amount;

            int sucess = _accDb.UpdateBalance(id, newBalance);

            if(sucess == 0)
            {
                throw new Exception("Não foi possível concluir a operação");
            }
        }
        #endregion
        #region Fazer transferência
        public int ToTranfer(int idAccount,int idDestinationAccount, decimal amount, ITransferTaxService taxService, decimal tax)
        {
            //Altera o saldo da conta destino
            var destinationBalance = _accDb.GetBalance(idDestinationAccount);
            
            var taxedAmount = taxService.TaxFee(amount, tax);//aplica a taxa de transferência no valor a ser transferido

            var newDestinationBalance = destinationBalance + taxedAmount;

            int sucess = _accDb.UpdateBalance(idDestinationAccount,newDestinationBalance);

            //Altera o saldo da conta de origem
            var originAccountBalance = _accDb.GetBalance(idAccount);
            
            var newBalance = originAccountBalance - amount;

            sucess += _accDb.UpdateBalance(idAccount, newBalance);

            if (sucess == 0)
            {
                throw new Exception("Não foi possível concluir a operação");
            }

            return sucess;
        }
        #endregion
        #region Excluir uma conta (Não funciona)
        public int DeleteAccount(int accountId)
        {
            var balance = _accDb.GetBalance(accountId);
            if (balance > 0.0M)
            {
                throw new Exception("Não posso excluir uma conta que contem um saldo maior que 0.0 R$. Por gentileza, transfira o saldo para outra conta para que seja possível excluí-la");
            }

            int sucess = _accDb.RemoveAccount(accountId);

            if(sucess == 0)
            {
                throw new Exception("Não foi possível completar a aoperação");
            }
            
            return sucess;
        }
        #endregion
    }
}
