using System.ComponentModel.DataAnnotations;

namespace CasaDeShowAPI.Model.ViewModels
{
    public class VendaRequest
    {
        [Required]
        public string EventoId { get; set; }
        [Required]
        public int Quantidade { get; set; }
    }
}