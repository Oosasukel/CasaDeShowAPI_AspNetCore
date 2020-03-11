using System.ComponentModel.DataAnnotations;

namespace CasaDeShowAPI.Model.ViewModels
{
    public class Login
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Senha { get; set; }
    }
}