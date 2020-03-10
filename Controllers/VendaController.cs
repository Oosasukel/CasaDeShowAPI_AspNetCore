using System.Collections.Generic;
using CasaDeShowAPI.Model;
using CasaDeShowAPI.Model.ViewModels;
using CasaDeShowAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CasaDeShowAPI.Controllers
{
    [ApiController]
    [Route("api/vendas")]
    public class VendaController : ControllerBase
    {
        private readonly IVendaRepository _vendaRepository;

        public VendaController(IVendaRepository vendaRepository)
        {
            _vendaRepository = vendaRepository;
        }

        [HttpGet]
        public IActionResult GetVendas(){
            var vendas = _vendaRepository.Get();
            var vendasResponses = VendaToVendaResponse(vendas);
            return Ok(vendasResponses);
        }

        private List<VendaResponse> VendaToVendaResponse(List<Venda> vendas){
            var vendasResponses = new List<VendaResponse>();
            foreach (var venda in vendas)
            {
                var uriEvento = Url.Action("GetEvento", "Evento", new{id = venda.EventoId});
                var uriUsuario = Url.Action("GetUsuario", "Usuario", new{email = venda.UsuarioEmail});
                vendasResponses.Add(venda.ToVendaResponse(uriEvento, uriUsuario));
            }
            return vendasResponses;
        }

        [HttpGet("{id}")]
        public IActionResult GetVenda(string id){
            var venda = _vendaRepository.Get(id);
            if(venda != null){
                var uriEvento = Url.Action("GetEvento", "Evento", new{id = venda.EventoId});
                var uriUsuario = Url.Action("GetUsuario", "Usuario", new{email = venda.UsuarioEmail});

                return Ok(venda.ToVendaResponse(uriEvento, uriUsuario));
            }
            return NotFound("Venda não encontrada");
        }

        [HttpGet("{email}")]
        public IActionResult GetCompras(string email){
            var vendas = _vendaRepository.GetCompras(email);
            var vendasResponses = VendaToVendaResponse(vendas);
            return Ok(vendasResponses);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult CancelarVenda(string id){
            var venda = _vendaRepository.Get(id);
            if(venda != null && venda.Status != VendaStatus.Cancelada){
                if(venda.UsuarioEmail != User.Identity.Name) return BadRequest("Você não pode cancelar a compra de outra pessoa");
                _vendaRepository.Cancelar(venda);
                return NoContent();
            }
            return NotFound("Venda não encontrada.");
        }

        [HttpPost]
        [Authorize]
        public IActionResult PostVenda([FromBody] VendaRequest vendaRequest){
            if(ModelState.IsValid){
                var venda = vendaRequest.ToVenda();
                venda.UsuarioEmail = User.Identity.Name;
                var deu = _vendaRepository.Add(venda);
                if(deu){
                    var uri = Url.Action("GetVenda", new{id = venda.Id});
                    var uriEvento = Url.Action("GetEvento", "Evento", new{id = venda.EventoId});
                    var uriUsuario = Url.Action("GetUsuario", "Usuario", new{email = venda.UsuarioEmail});
                    return Created(uri, venda.ToVendaResponse(uriEvento, uriUsuario));
                }
                return BadRequest("Evento ou Email inválidos.");
            }
            return BadRequest("Requisição inválida");
        }
    }
}