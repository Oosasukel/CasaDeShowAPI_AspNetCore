using System.Collections.Generic;
using CasaDeShowAPI.Model;
using CasaDeShowAPI.Model.ViewModels;
using CasaDeShowAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CasaDeShowAPI.Controllers
{
    [ApiController]
    [Route("api/eventos")]
    public class EventoController : ControllerBase
    {
        private readonly IEventoRepository _eventoRepository;

        public EventoController(IEventoRepository eventoRepository)
        {
            _eventoRepository = eventoRepository;
        }

        [HttpGet]
        public List<EventoResponse> GetEventos(){
            var eventos = _eventoRepository.GetEventos();
            var eventosResponses = ToEventoResponses(eventos);
            return eventosResponses;
        }

        [HttpPut("{id}")]
        [Authorize]
        public ActionResult<EventoResponse> PutEvento([FromBody] EventoRequest eventoRequest, string id){
            Evento evento = _eventoRepository.GetEvento(id);
            if(evento != null){
                if(evento.Casa.UsuarioEmail != User.Identity.Name) return BadRequest("Só é possível editar um evento que você criou.");

                var eventoUpdate = eventoRequest.ToEvento();
                eventoUpdate.Id = id;

                bool deu = _eventoRepository.Update(eventoUpdate);

                if(deu){
                    var casaUri = Url.Action("GetCasa", "Casa", new{id = eventoUpdate.CasaId});
                    var uriVendas = Url.Action("GetVendas", new{id = eventoUpdate.Id});
                    return eventoUpdate.ToEventoResponse(casaUri, uriVendas);
                }

                return NotFound("Casa não existe.");
            }
            return BadRequest("Evento não existe.");
        }

        [HttpGet("{id}/vendas")]
        public IActionResult GetVendas(string id){
            var vendas = _eventoRepository.GetVendas(id);
            var vendasResponses = new List<VendaResponse>();
            foreach (var venda in vendas)
            {
                var uriEvento = Url.Action("GetEvento", "Evento", new{id = venda.EventoId});
                var uriUsuario = Url.Action("GetUsuario", "Usuario", new{email = venda.UsuarioEmail});
                vendasResponses.Add(venda.ToVendaResponse(uriEvento, uriUsuario));
            }
            return Ok(vendasResponses);
        }

        [HttpGet("{id}")]
        public ActionResult<EventoResponse> GetEvento(string id){
            var evento = _eventoRepository.GetEvento(id);
            if(evento != null){
                var uriCasa = Url.Action("GetCasa", "Casa", new{id = id});
                var uriVendas = Url.Action("GetVendas", new{id = evento.Id});
                return evento.ToEventoResponse(uriCasa, uriVendas);
            }
            
            return NotFound("Evento não encontrado.");
        }

        [HttpGet("usuario/{email}")]
        public IActionResult GetByEmail(string email){
            var eventos = _eventoRepository.GetByEmail(email);
            var eventosResponses = ToEventoResponses(eventos);
            return Ok(eventosResponses);
        }

        [HttpPost]
        [Authorize]
        public ActionResult<EventoResponse> PostEvento([FromBody] EventoRequest eventoRequest){
            if(ModelState.IsValid){
                Evento evento = eventoRequest.ToEvento();
                bool deu = _eventoRepository.Add(evento, User.Identity.Name);
                if(deu){
                    var uriEvento = Url.Action("GetEvento", new{id = evento.Id});
                    var uriCasa = Url.Action("GetCasa", "Casa", new{id = evento.CasaId});
                    var uriVendas = Url.Action("GetVendas", new{id = evento.Id});
                    return Created(uriEvento, evento.ToEventoResponse(uriCasa, uriVendas));
                }
                return BadRequest("Você não criou esta casa.");
            }
            return BadRequest("Requisição inválida.");
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteEvento(string id){
            var evento = _eventoRepository.GetEvento(id);
            if(evento != null){
                if(evento.Casa.UsuarioEmail != User.Identity.Name) return BadRequest("Só é possivel excluir um evento que você criou.");
                _eventoRepository.Delete(id);
                return NoContent();
            }
            return NotFound("Evento não encontrado.");
        }

        [HttpGet("capacidade/asc")]
        public List<EventoResponse> GetEventosByCapacidadeAsc(){
            var eventos = _eventoRepository.GetEventosCapacidadeAsc();
            
            return ToEventoResponses(eventos);
        }

        [HttpGet("capacidade/desc")]
        public List<EventoResponse> GetEventosByCapacidadeDesc(){
            var eventos = _eventoRepository.GetEventosCapacidadeDesc();
            
            return ToEventoResponses(eventos);
        }
        [HttpGet("data/asc")]
        public List<EventoResponse> GetEventosByDataAsc(){
            var eventos = _eventoRepository.GetEventosDataAsc();
            
            return ToEventoResponses(eventos);
        }
        [HttpGet("data/desc")]
        public List<EventoResponse> GetEventosByDataDesc(){
            var eventos = _eventoRepository.GetEventosDataDesc();
            
            return ToEventoResponses(eventos);
        }
        [HttpGet("nome/asc")]
        public List<EventoResponse> GetEventosByNomeAsc(){
            var eventos = _eventoRepository.GetEventosNomeAsc();
            
            return ToEventoResponses(eventos);
        }
        [HttpGet("nome/desc")]
        public List<EventoResponse> GetEventosByNomeDesc(){
            List<Evento> eventos = _eventoRepository.GetEventosNomeDesc();
            
            return ToEventoResponses(eventos);
        }
        private List<EventoResponse> ToEventoResponses(List<Evento> eventos){
            List<EventoResponse> eventoResponses = new List<EventoResponse>();

            foreach (var evento in eventos)
            {
                var uriCasa = Url.Action("GetCasa", "Casa", new{id = evento.CasaId});
                var uriVendas = Url.Action("GetVendas", new{id = evento.Id});
                eventoResponses.Add(evento.ToEventoResponse(uriCasa, uriVendas));
            }

            return eventoResponses;
        }

        [HttpGet("preco/asc")]
        public List<EventoResponse> GetEventosByPrecoAsc(){
            var eventos = _eventoRepository.GetEventosPrecoAsc();
            
            return ToEventoResponses(eventos);
        }
        [HttpGet("preco/desc")]
        public List<EventoResponse> GetEventosByPrecoDesc(){
            var eventos = _eventoRepository.GetEventosPrecoDesc();
            
            return ToEventoResponses(eventos);
        }
    }
}