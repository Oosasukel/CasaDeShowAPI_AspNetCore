using System.ComponentModel.DataAnnotations;

namespace CasaDeShowAPI.Model
{
    public enum VendaStatus{Normal, Cancelada}
    public class Venda
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string EventoId { get; set; }
        public VendaStatus Status { get; set; }
        [Required]
        public decimal PrecoUnitario { get; set; }
        public Evento Evento { get; set; }
        [Required]
        public string UsuarioEmail { get; set; }
        public Usuario Usuario { get; set; }
        [Required]
        public int Quantidade { get; set; }

        public Venda(){
            Status = VendaStatus.Normal;
        }
    }
}