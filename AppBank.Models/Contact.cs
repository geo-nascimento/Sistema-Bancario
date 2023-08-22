using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBank.Models
{
    public class Contact
    {
        public int ContactId { get; set; }
        public int UserId { get; set; }
        public string Telephone { get; set; } = null!;
        public string CellPhone { get; set; } = null!;

        //Relacionamento
        public User? User { get; set; }
    }
}
