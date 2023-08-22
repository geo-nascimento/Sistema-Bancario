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
        public bool Autentication(int id, string accountNumber, string password)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT AccountNumber, Password FROM Account WHERE AccountId = @id";
                cmd.Connection = (SqlConnection)_connection;
                cmd.Parameters.AddWithValue("@id", id);

                _connection.Open();

                SqlDataReader dataReader = cmd.ExecuteReader();
                string accNumber = dataReader.GetString("AccountNumber");
                string pass = dataReader.GetString("Password");


                if (dataReader == null)
                {
                    throw new Exception("Não foram encontrados dados referentes a consulta desejada");
                }

                if (pass == password && accNumber == accountNumber)
                {
                    return true;
                }

                return false;
            }
            finally
            {
                _connection.Close();
            }
        }
        #endregion
        #region Criação de conta
        public void CreateAccount(int id, Account account)
        {
            _connection.Open();
            SqlTransaction transaction = (SqlTransaction)_connection.BeginTransaction();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "INSERT INTO Account (UserId, AccountNumber, Password, Balance, AccounType, DataCadastro) VALUES (@UserId, @AccountNumber, @Password, @Balance, @AccounType, @DataCadastro)";
                cmd.Connection = (SqlConnection)_connection;
                cmd.Transaction = transaction;

                cmd.Parameters.AddWithValue("@UserId", id);
                cmd.Parameters.AddWithValue("@AccountNumber", account.AccountNumber);
                cmd.Parameters.AddWithValue("@Password", account.Password);
                cmd.Parameters.AddWithValue("@Balance", 0.0M);
                cmd.Parameters.AddWithValue("@AccounType", account.AccountType.ToString());
                cmd.Parameters.AddWithValue("@DataCadastro", DateTimeOffset.Now);

                cmd.ExecuteNonQuery();

                transaction.Commit();
            }
            catch
            {
                try
                {
                    transaction.Rollback();
                }
                catch (Exception)
                {
                    throw new Exception("Operação falhou");
                }
            }
            finally
            {
                _connection.Close();
            }
        }
        #endregion
        #region Trazer usuario e suas contas por id
        public User GetUserAccounts(int id)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT * FROM Users usr LEFT JOIN Account acc ON usr.UserId = acc.UserId WHERE usr.UserId = @id;";
                cmd.Connection = (SqlConnection)_connection;

                cmd.Parameters.AddWithValue("@id", id);

                _connection.Open();

                SqlDataReader dataReader = cmd.ExecuteReader();

                Dictionary<int, User> userAccounts = new Dictionary<int, User>();

                while (dataReader.Read())
                {
                    User user = new User();

                    if (!userAccounts.ContainsKey(dataReader.GetInt32(0)))
                    {
                        user.UserId = dataReader.GetInt32(0);
                        user.Name = dataReader.GetString(1);
                        user.Email = dataReader.GetString(2);
                        user.CPF = dataReader.GetString(3);
                        user.RegistrationDate = dataReader.GetDateTimeOffset(4);

                        userAccounts.Add(user.UserId, user);
                    }
                    else
                    {
                        user = userAccounts[dataReader.GetInt32(0)];
                    }

                    Account account = new Account();
                    account.AccountId = dataReader.GetInt32(5);
                    account.UserId = dataReader.GetInt32(6);
                    account.AccountNumber = dataReader.GetString(7);
                    account.Password = dataReader.GetString(8);
                    account.Balance = dataReader.GetDecimal(9);
                    account.AccountType = (AccountType)Enum.Parse(typeof(AccountType), dataReader.GetString(10));
                    account.RegistrationDate = dataReader.GetDateTimeOffset(11);

                    //Verificação de segurança
                    user.Accounts = (user.Accounts == null) ? new List<Account>() : user.Accounts;

                    if(user.Accounts.FirstOrDefault(a => a.AccountId == account.AccountId) == null)
                    {
                        user.Accounts.Add(account);
                    }
                }

                return userAccounts[userAccounts.Keys.First()];
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
            decimal balance;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT Balance From Account WHERE UserId = @id";
                cmd.Connection = (SqlConnection)_connection;
                cmd.Parameters.AddWithValue("@id", id);

                _connection.Open();

                SqlDataReader dataReader = cmd.ExecuteReader();

                balance = dataReader.GetDecimal(0);

            }
            finally
            {
                _connection.Close();
            }

            return balance;
        }
        #endregion
        #region Atualizar saldo da conta
        public void UpdateBalance(int id, decimal amount)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "UPDATE Account SET Balance = @mount WHERE AccountId = @id";
                cmd.Connection = (SqlConnection)_connection;

                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@id", id);

                _connection.Open();

                cmd.ExecuteNonQuery();
            }
            finally
            {
                _connection.Close();
            }
        }
        #endregion

    }
}
