using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBank.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string CPF { get; set; } = null!;
        public DateTimeOffset RegistrationDate { get; set; }

        //Relacionamentos
        public Contact? Contact { get; set; }
        public ICollection<Account>? Accounts { get; set; }

    }
}
