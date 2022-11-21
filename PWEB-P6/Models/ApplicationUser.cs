using Microsoft.AspNetCore.Identity;

namespace PWEB_P6.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string PrimeiroNome { get; set; }
        public string UltimoNome { get; set; }
        public DateTime DataNascimento { get; set; }

        [PersonalData]
        public int NIF { get; set; }
    }
}
