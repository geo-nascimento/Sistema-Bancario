using AppBank.Models;
using AppBank.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AppBank.ViewModels
{
    public class AccountViewModel
    {
        public static bool VerifyPassword(string text)
        {
            string pattern = @"^(?=.*[a-zA-Z0-9]{4})(?=.*[@#$%^&+=])";

            if (Regex.IsMatch(text, pattern))
            {
                return true;
            }

            return false;
        }

        public static string CreateRandomAccountNumber(AccountType type)
        {
            Random random = new Random();

            int number = random.Next(11, 10001);

            string accountNumber = "C-" + number;

            if (type == AccountType.Poupanca)
            {
                accountNumber = "P-" + number;
            }
            
            return accountNumber;
        }

        public static string AccountData(Account account)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("__Dados da conta__");
            sb.AppendLine("Account Id: " + account.AccountId);
            sb.AppendLine("Account Number: " + account.AccountNumber);
            sb.AppendLine("Account Type: " + account.AccountType.ToString());
            sb.AppendLine("Balance: " + account.Balance);
            sb.AppendLine("Registration Date: " + account.RegistrationDate.ToString("dd/MMMM/yyyy HH:mm"));

            return sb.ToString();
        }

    }
}
