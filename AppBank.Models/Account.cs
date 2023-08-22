using AppBank.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBank.Models
{
    public class Account
    {
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public string AccountNumber { get; set; } = null!;
        public string Password { get; set; } = null!;
        public Decimal Balance { get; set; }
        public AccountType AccountType { get; set; }
        public DateTimeOffset RegistrationDate { get; set; }

        //Relacionamentos
        public User? User { get; set; }
    }
}
