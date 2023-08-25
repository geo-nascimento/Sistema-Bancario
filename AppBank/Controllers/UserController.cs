using AppBank.Models;
using AppBank.Repositories;
using AppBank.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


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

        #region login do usuário
        public int LoginUser(string email, string password)
        {
            int userId = _usrDb.UserLoginAutentication(email, password);

            if (userId == 0)
            {
                throw new Exception("Usuário não encontrado");
            }

            return userId;
        }
        #endregion
        #region Cadastrar Usuário
        public int RegistrationUser(string name, string email, string password, string cpf, string telephone, string cellPhone)
        {

            User user = new User() { Name = name, Email = email, CPF = cpf, RegistrationDate = DateTimeOffset.Now };
            Contact contact = new Contact() { Telephone = telephone, CellPhone = cellPhone };

            user.Contact = contact;

            int sucess = _usrDb.CreateUser(user);

            if (sucess == 0)
            {
                throw new Exception("Não ou possível criar o usuário");
            }

            return sucess;
        }
        #endregion
        #region Buscar usuário
        public User GetUSerInfo(int userId)
        {
            var user = _usrDb.GetUserById(userId);
            if (user == null)
            {
                throw new Exception("Usuário não encontrato ou inexistente");
            }

            return user;
        }
        #endregion
        #region Atualizar dados do usuário
        public int UpdateRegister(int id, string name, string email, string telephone, string cellPhone)
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

            int sucess = _usrDb.UpdateUser(userToUpdate);

            if (sucess == 0)
            {
                throw new Exception("Não foi possível completar a operação");
            }

            return sucess; 
        }
        #endregion
        #region Remover usuario
        public int DeleteUser(int id)
        {
            int sucess = _usrDb.RemoveUser(id);

            if(sucess == 0)
            {
                throw new Exception("Não foi possível completar a operação");
            }

            return sucess;
        }
        #endregion
    }

}
