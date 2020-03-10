using System.ComponentModel.DataAnnotations;

namespace CasaDeShowAPI.Model.ViewModels
{
    public class UsuarioRequest
    {
        [Required]
        public string Nome { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Senha { get; set; }
    }
}