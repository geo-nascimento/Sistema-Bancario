using AppBank.Models;
using AppBank.Models.Enums;
using AppBank.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

//Conterá operações de: acesso a conta, saque, depósito, emprestimo, leitura de saldo
namespace AppBank.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private IDbConnection _connection;

        public AccountRepository()
        {
            _connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BankingSystem;Integrated Security=True");
        }

        #region Autenticação
        public int AccountLoginAutentication(string accountNumber, string password)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT AccountId FROM Account acc WHERE acc.AccountNumber = @AccountNumber AND acc.Password = @Password";
                cmd.Connection = (SqlConnection)_connection;
                cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);
                cmd.Parameters.AddWithValue("@Password", password);

                _connection.Open();

                SqlDataReader dataReader = cmd.ExecuteReader();

                int accountId = 0;
                while (dataReader.Read())
                {
                    accountId = dataReader.GetInt32("accountId");
                }

                if (dataReader != null)
                {
                    return accountId;
                }

                return 0;
                
            }
            finally
            {
                _connection.Close();
            }
        }
        #endregion
        #region Criação de conta
        public int CreateAccount(Account account)
        {
            int execution = 0;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "INSERT INTO Account (UserId, AccountNumber, Password, Balance, AccounType, DataCadastro) VALUES (@UserId, @AccountNumber, @Password, @Balance, @AccounType, @DataCadastro)";
                cmd.Connection = (SqlConnection)_connection;
            
                cmd.Parameters.AddWithValue("@UserId", account.UserId);
                cmd.Parameters.AddWithValue("@AccountNumber", account.AccountNumber);
                cmd.Parameters.AddWithValue("@Password", account.Password);
                cmd.Parameters.AddWithValue("@Balance", 0.0M);
                cmd.Parameters.AddWithValue("@AccounType", account.AccountType.ToString());
                cmd.Parameters.AddWithValue("@DataCadastro", account.RegistrationDate);
               
                _connection.Open();
                
                execution = cmd.ExecuteNonQuery();

                
            }
            finally
            {
                _connection.Close();
            }

            return execution;
        }
        #endregion
        #region Trazer uma conta por id
        public Account GetAccountById(int id)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT * FROM Account WHERE AccountId = @id";
                cmd.Connection = (SqlConnection)_connection;

                cmd.Parameters.AddWithValue("@id", id);

                _connection.Open();

                SqlDataReader dataReader = cmd.ExecuteReader();

                Account account = new Account();

                while (dataReader.Read())
                {
                    account.AccountId = dataReader.GetInt32(0);
                    account.UserId = dataReader.GetInt32(1);
                    account.AccountNumber = dataReader.GetString(2);
                    account.Password = dataReader.GetString(3);
                    account.Balance = dataReader.GetDecimal(4);
                    account.AccountType = (AccountType)Enum.Parse(typeof(AccountType), dataReader.GetString(5));
                    account.RegistrationDate = dataReader.GetDateTimeOffset(6);  
                }

                return account;
            }
            finally
            {
                _connection.Close();
            }

        }
        #endregion
        #region Tazer Saldo da conta
        public decimal GetBalance(int id)
        {
            decimal balance = 0.0M;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT Balance From Account WHERE AccountId = @id";
                cmd.Connection = (SqlConnection)_connection;
                cmd.Parameters.AddWithValue("@id", id);

                _connection.Open();

                SqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    balance = dataReader.GetDecimal("Balance");

                }

            }
            finally
            {
                _connection.Close();
            }

            return balance;
        }
        #endregion
        #region Atualizar saldo da conta
        public int UpdateBalance(int id, decimal amount)
        {
            int execution = 0;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "UPDATE Account SET Balance = @amount WHERE AccountId = @id";
                cmd.Connection = (SqlConnection)_connection;

                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@id", id);

                _connection.Open();

                execution = cmd.ExecuteNonQuery();
            }
            finally
            {
                _connection.Close();
            }

            return execution;
        }
        #endregion
        #region Apagar conta
        public int RemoveAccount(int id)
        {
            int execution = 0;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "DELETE FROM Accounts WHERE AccountId = @id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Connection = (SqlConnection) _connection;

                _connection.Open();
                execution = cmd.ExecuteNonQuery();
            }
            finally
            {
                _connection.Close();
            }

            return execution;
        }
        #endregion
    }
}
