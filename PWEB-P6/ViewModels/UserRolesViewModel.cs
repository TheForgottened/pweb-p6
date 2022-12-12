using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace PWEB_P6.ViewModels
{
    public class UserRolesViewModel
    {
        public string UserId { get; set; }
        public string PrimeiroNome { get; set; }
        public string UltimoNome { get; set; }
        public string UserName { get; set; }
        public IEnumerable<string> Roles { get; set; }

        [Display(Name = "O meu Avatar")]
        public byte[]? Avatar { get; set; }
    }
}
