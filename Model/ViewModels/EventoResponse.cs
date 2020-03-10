using System;

namespace CasaDeShowAPI.Model.ViewModels
{
    public class EventoResponse
    {
        public EventoResponse(Evento evento, string uriCasa, string uriVendas)
        {
            Id = evento.Id;
            Nome = evento.Nome;
            Casa = uriCasa;
            Capacidade = evento.Capacidade;
            Data = evento.Data;
            Preco = evento.Preco;
            Vendas = uriVendas;
        }

        public string Id { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public DateTime Data { get; set; }
        public string Casa { get; set; }
        public int Capacidade {get;set;}
        public string Vendas { get; set; }
    }
}