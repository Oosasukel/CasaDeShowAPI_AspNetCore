using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CasaDeShowAPI.Model
{
    public enum EventoStatus{Ativo, Excluido, Calcelado}
    public class Evento
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public decimal Preco { get; set; }
        [Required]
        public string CasaId { get; set; }
        public Casa Casa { get; set; }
        public List<Venda> Vendas { get; set; }
        public EventoStatus Status { get; set; }
        [Required]
        public DateTime Data { get; set; }
        [Required]
        public int Capacidade { get; set; }
        public Evento(){
            Status = EventoStatus.Ativo;
        }
    }
}