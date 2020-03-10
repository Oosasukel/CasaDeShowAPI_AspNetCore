using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CasaDeShowAPI.Model
{
    public enum CasaStatus{Excluida, Ativa}
    public class Casa
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public string Endereco { get; set; }
        public List<Evento> Eventos { get; set; }
        public CasaStatus Status{get;set;}
        public string UsuarioEmail { get; set; }
        public Usuario Usuario { get; set; }

        public Casa(){
            Status = CasaStatus.Ativa;
        }
    }
}