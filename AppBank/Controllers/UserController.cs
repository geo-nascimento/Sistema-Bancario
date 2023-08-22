using AppBank.Models;
using AppBank.Repositories;
using AppBank.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace AppBank.Controllers
{
    //conterá funções para intermediar dados do front e banco de dados
    public class UserController
    {
        private readonly IUserReporitory _usrDb;

        public UserController()
        {
            _usrDb = new UserRepository();
        }

        #region Cadastrar Usuário
        public void RegistrationUser(string name, string email, string cpf, string telephone, string cellPhone)
        {
            User user = new User() { Name = name, Email = email, CPF = cpf, RegistrationDate = DateTimeOffset.Now };
            Contact contact = new Contact() { Telephone = telephone, CellPhone = cellPhone };

            user.Contact = contact;

            _usrDb.CreateUser(user);
        }
        #endregion

        #region Listar usuários
        public List<User> ListUsers()
        {
            var users = _usrDb.GetUsers();
            if (users == null)
            {
                throw new Exception("Não existem usuários cadastrados!");
            }
            return users;
        }
        #endregion

        #region Encontrar usuário por id
        public User GetUser(int id)
        {
            var user = _usrDb.GetUserById(id);
            if (user == null)
            {
                throw new Exception("Usuário não encontrado");
            }
            return user;
        }
        #endregion

        #region Atualizar dados do usuário
        public void UpdateRegister(int id, string name, string email, string telephone, string cellPhone)
        {
            var userToUpdate = _usrDb.GetUserById(id);
            if (userToUpdate == null)
            {
                throw new Exception("Usuário não encontrado");
            }
            userToUpdate.Name = name;
            userToUpdate.Email = email;
            userToUpdate.Contact!.Telephone = telephone;
            userToUpdate.Contact.CellPhone = cellPhone;

            _usrDb.UpdateUser(userToUpdate);
        }
        #endregion
    }

    /*
         * TODO:
         * Realizar saque
         * Realizar depósito
         * Realizar empréstimo
         * Exibir dados de usuário e suas contas
         * Realizar transferências entre contas
         */
}
