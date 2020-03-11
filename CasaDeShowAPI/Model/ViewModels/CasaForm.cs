using System.ComponentModel.DataAnnotations;
using CasaDeShowAPI.Repositories;

namespace CasaDeShowAPI.Model.ViewModels
{
    public class CasaForm
    {
        [Required]
        [StringLength(200)]
        public string Nome { get; set; }
        [Required]
        [StringLength(200)]
        public string Endereco { get; set; }
    }
}