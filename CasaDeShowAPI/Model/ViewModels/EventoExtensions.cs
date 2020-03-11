using System.Collections.Generic;

namespace CasaDeShowAPI.Model.ViewModels
{
    public static class EventoExtensions
    {
        public static EventoResponse ToEventoResponse(this Evento evento, string uriCasa, string uriVendas){
            return new EventoResponse(evento, uriCasa, uriVendas);
        }

        public static Evento ToEvento(this EventoRequest eventoRequest){
            var evento = new Evento();
            evento.Nome = eventoRequest.Nome;
            evento.Preco = eventoRequest.Preco;
            evento.CasaId = eventoRequest.CasaId;
            evento.Capacidade = eventoRequest.Capacidade;
            evento.Data = eventoRequest.Data;
            
            return evento;
        }
    }
}