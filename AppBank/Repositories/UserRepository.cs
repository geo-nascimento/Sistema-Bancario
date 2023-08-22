using AppBank.Models;
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

        public void CreateUser(User user)
        {
            
           
            try
            {   //Dados de usuario
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "INSERT INTO Users (Name, Email, CPF, DataCadastro) VALUES (@Name, @Email, @CPF, @DataCadastro); SELECT CAST(scope_identity() AS int)";
                cmd.Connection = (SqlConnection)_connection;

                cmd.Parameters.AddWithValue("@Name", user.Name);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@CPF", user.CPF);
                cmd.Parameters.AddWithValue("@DataCadastro", DateTimeOffset.Now);

                _connection.Open();

                user.UserId = (int) cmd.ExecuteScalar();//Retorna o id inserido no usuário pelo banco

                //Dados de contato
                cmd = new SqlCommand();
                cmd.CommandText = "INSERT INTO Contacts (UserId, Telefone, Celular) VALUES (@UserId, @Telefone, @Celular)";
               
                cmd.Connection = (SqlConnection)_connection;

                cmd.Parameters.AddWithValue("@UserId", user.UserId);
                cmd.Parameters.AddWithValue("@Telefone", user.Contact!.Telephone);
                cmd.Parameters.AddWithValue("@Celular", user.Contact.CellPhone);

                cmd.ExecuteNonQuery();

                
            }
            finally
            {
                _connection.Close();
            }
           
        }

        public List<User> GetUsers()
        {
            List<User> users = new List<User>();

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT * FROM Users u LEFT JOIN Contacts cto ON u.UserId = cto.UserId;";
                cmd.Connection = (SqlConnection) _connection;
                _connection.Open();
                SqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    User user = new User();
                    user.UserId = dataReader.GetInt32(0);
                    user.Name = dataReader.GetString(1);
                    user.Email = dataReader.GetString(2);
                    user.CPF = dataReader.GetString(3);
                    user.RegistrationDate = dataReader.GetDateTimeOffset(4);

                    Contact contact = new Contact();
                    contact.ContactId = dataReader.GetInt32(5);
                    contact.UserId = dataReader.GetInt32(6);
                    contact.Telephone = dataReader.GetString(7);
                    contact.CellPhone = dataReader.GetString(8);

                    user.Contact = contact;
                    
                    users.Add(user);
                }

            }
            finally
            {
                _connection.Close();
            }
            return users;
        }


        public User GetUserById(int id)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT * FROM Users u LEFT JOIN Contacts cto ON u.UserId = cto.UserId WHERE u.UserId = @id;";
                cmd.Connection = (SqlConnection)_connection;
                _connection.Open();
                cmd.Parameters.AddWithValue("@id", id);

                SqlDataReader dataReader = cmd.ExecuteReader();

                User user = new User();
                Contact contact = new Contact();
                while (dataReader.Read())
                {
                    user.UserId = dataReader.GetInt32(0);
                    user.Name = dataReader.GetString(1);
                    user.Email = dataReader.GetString(2);
                    user.CPF = dataReader.GetString(3);
                    user.RegistrationDate = dataReader.GetDateTimeOffset(4);

                    
                    contact.ContactId = dataReader.GetInt32(5);
                    contact.UserId = dataReader.GetInt32(6);
                    contact.Telephone = dataReader.GetString(7);
                    contact.CellPhone = dataReader.GetString(8);

                    user.Contact = contact; 
                }
                return user;
            }
            finally
            {
                _connection.Close();
            }
        }

        public void UpdateUser(User user)
        {
            try
            {
                //Usuario
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "UPDATE Users SET Name = @Name, Email = @Email, CPF = @CPF WHERE UserId = @id" ;
                cmd.Connection = (SqlConnection) _connection;

                cmd.Parameters.AddWithValue("@Name", user.Name);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@CPF", user.CPF);

                cmd.Parameters.AddWithValue("@id", user.UserId);

                _connection.Open();
                cmd.ExecuteNonQuery();
                
                //Contato
                cmd = new SqlCommand();
                cmd.CommandText = "UPDATE Contacts SET Telefone = @Telefone, Celular = @Celular WHERE UserId = @id";
                cmd.Connection = (SqlConnection)_connection;

                cmd.Parameters.AddWithValue("@Telefone", user.Contact!.Telephone);
                cmd.Parameters.AddWithValue("@Celular", user.Contact.Telephone);

                cmd.Parameters.AddWithValue("@id", user.Contact.UserId);

                cmd.ExecuteNonQuery();

            }
            finally
            {
                _connection.Close();
            }
        }


        public void DeleteUser(int id)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "DELETE FROM Users WHERE UsersId = @Id";
                cmd.Connection = (SqlConnection)_connection;

                cmd.Parameters.AddWithValue("@Id", id);

                _connection.Open();
                cmd.ExecuteNonQuery();
            }
            finally
            {
                _connection.Close();
            }
        }

    }
}
