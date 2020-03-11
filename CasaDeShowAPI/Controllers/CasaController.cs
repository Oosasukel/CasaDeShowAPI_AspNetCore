using System;
using System.Collections.Generic;
using CasaDeShowAPI.Model;
using CasaDeShowAPI.Model.ViewModels;
using CasaDeShowAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CasaDeShowAPI.Controllers
{
    [ApiController]
    [Route("api/casas")]
    public class CasaController : ControllerBase
    {
        private readonly ICasaRepository _casaRepository;

        public CasaController(ICasaRepository casaRepository)
        {
            _casaRepository = casaRepository;
        }

        [HttpGet]
        public List<CasaResponse> GetCasas(){
            var casas = _casaRepository.GetCasas();
            List<CasaResponse> casaResponses = new List<CasaResponse>();
            foreach (var casa in casas)
            {
                var eventosUri = Url.Action("GetEventos", new {id = casa.Id});
                casaResponses.Add(casa.ToCasaResponse(eventosUri));
            }
            return casaResponses;
        }
        
        [HttpGet("{id}")]
        public ActionResult<CasaResponse> GetCasa(string id){
            Casa casa = _casaRepository.GetCasa(id);
            var eventosUri = Url.Action("GetEventos", new{id = id});
            if(casa != null){
                return casa.ToCasaResponse(eventosUri);
            }

            return NotFound("Casa não encontrada");
        }

        [HttpPost]
        [Authorize]
        public IActionResult PostCasa([FromBody] CasaForm casaForm){
            if(ModelState.IsValid){
                Casa casa = casaForm.ToCasa();
                casa.UsuarioEmail = User.Identity.Name;

                try{
                    _casaRepository.Add(casa);
                } catch (Exception e){
                    return StatusCode(500, "Erro ao conectar com o banco. " + e.Message);
                }

                var uri = Url.Action("GetCasa", new { id = casa.Id});
                var eventosUri = Url.Action("GetEventos", new {id = casa.Id});
                return Created(uri, casa.ToCasaResponse(eventosUri));
            }
            
            return BadRequest("Requisição inválida.");
        }

        [HttpGet("{id}/eventos")]
        public List<EventoResponse> GetEventos(string id){
            var eventos = _casaRepository.GetEventos(id);
            List<EventoResponse> eventoResponses = new List<EventoResponse>();

            var uriCasa = Url.Action("GetCasa", new{id = id});
            foreach(var evento in eventos){
                var uriVendas = Url.Action("GetVendas", "Evento", new {id = evento.Id});
                eventoResponses.Add(evento.ToEventoResponse(uriCasa, uriVendas));
            }

            return eventoResponses;
        }

        [HttpPut("{id}")]
        [Authorize]
        public ActionResult<CasaResponse> UpdateCasa([FromBody] CasaForm casaForm, string id){
            var casa = _casaRepository.GetCasa(id);
            if(casa != null){
                if(casa.UsuarioEmail != User.Identity.Name) return BadRequest("Só é possível editar uma casa que você criou.");

                var casaUpdate = casaForm.ToCasa();
                casaUpdate.Id = id;
                _casaRepository.Update(casaUpdate);
                var eventosUri = Url.Action("GetEventos", new{id = casaUpdate.Id});
                return casaUpdate.ToCasaResponse(eventosUri);
            }
            return NotFound("Casa não encontrada.");
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteCasa(string id){
            var casa = _casaRepository.GetCasa(id);
            if(casa != null){
                if(casa.UsuarioEmail != User.Identity.Name) return BadRequest("Só é possível deletar uma casa que você criou.");

                _casaRepository.Delete(casa);
                return NoContent();
            }
            return NotFound("Casa não encontrada.");
        }

        [HttpGet("asc")]
        public List<CasaResponse> GetCasasAsc(){
            var casas = _casaRepository.GetCasasAsc();
            var casasResponses = CasaToCasaResponse(casas);
            return casasResponses;
        }
        [HttpGet("desc")]
        public List<CasaResponse> GetCasasDesc(){
            var casas = _casaRepository.GetCasasDesc();
            var casasResponses = CasaToCasaResponse(casas);
            return casasResponses;
        }

        private List<CasaResponse> CasaToCasaResponse(List<Casa> casas){
            List<CasaResponse> casaResponses = new List<CasaResponse>();
            foreach (var casa in casas)
            {
                var eventosUri = Url.Action("GetEventos", new{id = casa.Id});
                casaResponses.Add(casa.ToCasaResponse(eventosUri));
            }
            return casaResponses;
        }

        [HttpGet("nome/{nome}")]
        public ActionResult<CasaResponse> GetCasaByNome(string nome){
            var casa = _casaRepository.GetCasaByNome(nome);

            if(casa != null){
                var eventosUri = Url.Action("GetEventos", new{id = casa.Id});
                return Ok(casa.ToCasaResponse(eventosUri));
            }

            return NotFound("Casa não encontrada.");
        }

        [HttpGet("usuario/{email}")]
        public IActionResult GetByEmail(string email){
            var casas = _casaRepository.GetByEmail(email);
            var casasResponses = CasaToCasaResponse(casas);
            return Ok(casasResponses);
        }
    }
}