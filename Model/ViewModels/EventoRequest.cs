using System;
using System.ComponentModel.DataAnnotations;

namespace CasaDeShowAPI.Model.ViewModels
{
    public class EventoRequest
    {
        [Required]
        public string Nome { get; set; }
        [Required]
        public decimal Preco { get; set; }
        [Required]
        public string CasaId { get; set; }
        [Required]
        public DateTime Data { get; set; }
        [Required]
        public int Capacidade{get;set;}
    }
}