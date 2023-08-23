using AppBank.Models;
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

        public User GetAccountsInfo(int id)
        {
            var user = _accDb.GetUserAccounts(id);
            
            if(user == null)
            {
                throw new Exception("Usuário não encontrado");
            }

            return user;
        }


        public void ToWithdraw(decimal amount, int id)
        {
            var balance = _accDb.GetBalance(id);

            var newBalance = balance - amount;

            _accDb.UpdateBalance(id, newBalance);
        }

        public void ToDeposit(decimal amount, int id)
        {
            var newbalance = _accDb.GetBalance(id) + amount;
            
            _accDb.UpdateBalance(id, newbalance);
        }

        public void GetLoan(int id, decimal amount)
        {
            var balance = _accDb.GetBalance(id);

            var newBalance = balance + amount;

            _accDb.UpdateBalance(id, newBalance);
        }

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

    }
    /*
     * TODO:
     * Realizar saque
     * Realizar depósito
     * Realizar empréstimo
     * Realizar transferências entre contas
     */
}
