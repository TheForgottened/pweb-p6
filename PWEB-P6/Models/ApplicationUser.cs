using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace PWEB_P6.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string PrimeiroNome { get; set; }
        public string UltimoNome { get; set; }
        public DateTime DataNascimento { get; set; }

        [PersonalData]
        public int NIF { get; set; }

        [Display(Name = "O meu Avatar")]
        public byte[]? Avatar { get; set; }

        // Relation Agendamento
        public ICollection<Agendamento> Agendamentos { get; set; }
    }
}
