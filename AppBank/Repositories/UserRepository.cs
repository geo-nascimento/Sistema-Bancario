using AppBank.Models;
using AppBank.Models.Enums;
using AppBank.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;



//Conterá operações de: cadastrar usuários e seus contatos
namespace AppBank.Repositories
{
    public class UserRepository : IUserReporitory
    {
        private IDbConnection _connection;

        public UserRepository()
        {
            _connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BankingSystem;Integrated Security=True");
        }

        #region Autenticar usuário
        public int UserLoginAutentication(string email, string password)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT UserID FROM Users WHERE Email = @Email AND Password = @Password";
                cmd.Connection = (SqlConnection)_connection;
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                _connection.Open();

                SqlDataReader dataReader = cmd.ExecuteReader();

                int userId = 0;
                while (dataReader.Read())
                {
                    userId = dataReader.GetInt32(0);
                }

                if (dataReader != null)
                {
                    return userId;
                }

                return 0;

            }
            finally
            {
                _connection.Close();
            }
        }
        #endregion
        #region Criar usuário
        public int CreateUser(User user)
        {
            int execution = 0;
            try
            {   //Dados de usuario
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "INSERT INTO Users (Name, Email, Password, CPF, DataCadastro) VALUES (@Name, @Email, @Password, @CPF, @DataCadastro); SELECT CAST(scope_identity() AS int)";
                cmd.Connection = (SqlConnection)_connection;

                cmd.Parameters.AddWithValue("@Name", user.Name);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@CPF", user.CPF);
                cmd.Parameters.AddWithValue("@DataCadastro", DateTimeOffset.Now);

                _connection.Open();

                user.UserId = (int)cmd.ExecuteScalar();//Retorna o id inserido no usuário pelo banco

                //Dados de contato
                cmd = new SqlCommand();
                cmd.CommandText = "INSERT INTO Contacts (UserId, Telefone, Celular) VALUES (@UserId, @Telefone, @Celular)";

                cmd.Connection = (SqlConnection)_connection;

                cmd.Parameters.AddWithValue("@UserId", user.UserId);
                cmd.Parameters.AddWithValue("@Telefone", user.Contact!.Telephone);
                cmd.Parameters.AddWithValue("@Celular", user.Contact.CellPhone);

                execution = cmd.ExecuteNonQuery();


            }
            finally
            {
                _connection.Close();
            }

            return execution;

        }
        #endregion
        #region Encontrar usuário
        public User GetUserById(int id)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT * FROM Users usr LEFT JOIN Contacts cto ON usr.UserId = cto.UserId LEFT JOIN Account acc ON usr.UserId = acc.UserId WHERE usr.UserId = @id;";
                cmd.Connection = (SqlConnection)_connection;

                cmd.Parameters.AddWithValue("@id", id);

                _connection.Open();

                SqlDataReader dataReader = cmd.ExecuteReader();

                Dictionary<int, User> userAccounts = new Dictionary<int, User>();

                while (dataReader.Read())
                {
                    User user = new User();
                    Contact contact = new Contact();

                    if (!userAccounts.ContainsKey(dataReader.GetInt32(0)))
                    {
                        user.UserId = dataReader.GetInt32(0);
                        user.Name = dataReader.GetString(1);
                        user.Email = dataReader.GetString(2);
                        user.CPF = dataReader.GetString(3);
                        user.RegistrationDate = dataReader.GetDateTimeOffset(4);
                        user.Password = dataReader.GetString(5);
                        userAccounts.Add(user.UserId, user);

                        contact.ContactId = dataReader.GetInt32(6);
                        contact.UserId = dataReader.GetInt32(7);
                        contact.Telephone = dataReader.GetString(8);
                        contact.CellPhone = dataReader.GetString(9);

                        user.Contact = contact;

                    }
                    else
                    {
                        user = userAccounts[dataReader.GetInt32(0)];
                    }

                    Account account = new Account();
                    account.AccountId = dataReader.GetInt32(10);
                    account.UserId = dataReader.GetInt32(11);
                    account.AccountNumber = dataReader.GetString(12);
                    account.Password = dataReader.GetString(13);
                    account.Balance = dataReader.GetDecimal(14);
                    account.AccountType = (AccountType)Enum.Parse(typeof(AccountType), dataReader.GetString(15));
                    account.RegistrationDate = dataReader.GetDateTimeOffset(16);

                    //Verificação de segurança
                    user.Accounts = (user.Accounts == null) ? new List<Account>() : user.Accounts;

                    if (user.Accounts.FirstOrDefault(a => a.AccountId == account.AccountId) == null)
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
        #region Atualizar usuário
        public int UpdateUser(User user)
        {
            int execution = 0;
            try
            {
                //Usuario
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "UPDATE Users SET Name = @Name, Email = @Email WHERE UserId = @id";
                cmd.Connection = (SqlConnection)_connection;

                cmd.Parameters.AddWithValue("@Name", user.Name);
                cmd.Parameters.AddWithValue("@Email", user.Email);


                cmd.Parameters.AddWithValue("@id", user.UserId);

                _connection.Open();
                execution = cmd.ExecuteNonQuery();

                //Contato
                cmd = new SqlCommand();
                cmd.CommandText = "UPDATE Contacts SET Telefone = @Telefone, Celular = @Celular WHERE UserId = @id";
                cmd.Connection = (SqlConnection)_connection;

                cmd.Parameters.AddWithValue("@Telefone", user.Contact!.Telephone);
                cmd.Parameters.AddWithValue("@Celular", user.Contact.Telephone);

                cmd.Parameters.AddWithValue("@id", user.Contact.UserId);

                execution += cmd.ExecuteNonQuery();

            }
            finally
            {
                _connection.Close();
            }

            return execution;
        }
        #endregion
        #region Excluir usuário
        public int RemoveUser(int id)
        {
            int execution = 0;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "DELETE FROM Users WHERE UsersId = @Id";
                cmd.Connection = (SqlConnection)_connection;

                cmd.Parameters.AddWithValue("@Id", id);

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
