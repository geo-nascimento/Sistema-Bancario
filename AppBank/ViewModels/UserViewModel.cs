using AppBank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace AppBank.ViewModels
{
    public static class UserViewModel
    {
        #region Veficar padrão de senha
        public  static bool VerifyPassword(string text)
        {
            string pattern = @"^(?=.*[a-zA-Z0-9]{4})(?=.*[@#$%^&+=])";

            if (Regex.IsMatch(text, pattern))
            {
                return true;
            }

            return false;
        }
        #endregion

        #region Exibir dados do usuário e contas
        public static string UserData(User user)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("__Welcome " + user.Name +"__");
            sb.AppendLine("Registration data");
            sb.AppendLine("E-mail: " + user.Email);
            sb.AppendLine("CPF: " + user.CPF);
            sb.AppendLine("User registration date: " + user.RegistrationDate.ToString("dd/MMMM/yyyy HH:mm"));
            sb.AppendLine();
            sb.AppendLine("Accounts data");
            foreach (var account in user.Accounts!)
            {
                sb.AppendLine("Account number: " + account.AccountNumber);
                sb.AppendLine("Account type: " + account.AccountType.ToString());
                sb.AppendLine("Account registration date: " + account.RegistrationDate.ToString("dd/MMMM/yyyy HH:mm"));
                sb.AppendLine();
            }
            return sb.ToString();
        }
        #endregion

        public static (string, string, string, string, string, string) TakeUserData()
        {
            Console.Write("Name: ");
            string name = Console.ReadLine()!;
            Console.Write("E-mail: ");
            string email = Console.ReadLine()!;
            Console.Write("CPF: ");
            string cpf = Console.ReadLine()!;
            Console.Write("Password: ");
            string password = "";
            do
            {
                password = Console.ReadLine()!;

            } while(VerifyPassword(password) == false);
            Console.Write("Phone: ");
            string phone = Console.ReadLine()!;
            Console.Write("Cell phone: ");
            string cellPhone = Console.ReadLine()!;

            return (name, email, cpf, password, phone, cellPhone);
        }

    }
}
