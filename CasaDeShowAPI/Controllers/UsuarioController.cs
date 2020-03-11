using System.Collections.Generic;
using CasaDeShowAPI.Model.ViewModels;
using CasaDeShowAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CasaDeShowAPI.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        [HttpGet]
        public IActionResult GetUsuarios(){
            var usuarios = _usuarioRepository.Get();
            var usuarioResponses = new List<UsuarioResponse>();

            foreach (var usuario in usuarios)
            {
                var comprasUri = Url.Action("GetCompras", "Venda", new{email = usuario.Email});
                usuarioResponses.Add(usuario.ToUsuarioResponse(comprasUri));
            }

            return Ok(usuarioResponses);
        }

        [HttpGet("{email}")]
        public IActionResult GetUsuario(string email){
            var usuario = _usuarioRepository.Get(email);
            if(usuario != null){
                var comprasUri = Url.Action("GetCompras", "Venda", new{email = usuario.Email});
                return Ok(usuario.ToUsuarioResponse(comprasUri));
            }
            return NotFound("Usuário não encontrado.");
        }

        [HttpPost]
        public IActionResult PostUsuario([FromBody] UsuarioRequest usuarioRequest){
            if(ModelState.IsValid){
                var usuario = _usuarioRepository.Get(usuarioRequest.Email);
                if(usuario == null){
                    usuario = usuarioRequest.ToUsuario();
                    _usuarioRepository.Add(usuario);
                    var uri = Url.Action("GetUsuario", "Usuario", new {email = usuario.Email});
                    var comprasUri = Url.Action("GetCompras", "Venda", new{email = usuario.Email});
                    return Created(uri, usuario.ToUsuarioResponse(comprasUri));
                }
                return BadRequest("Usuário já existe.");
            }
            return BadRequest("Requisição inválida.");
        }

        [HttpPut]
        [Authorize]
        public IActionResult UpdateUsuario([FromBody] UsuarioRequest usuarioRequest){
            if(ModelState.IsValid){
                var usuario = _usuarioRepository.Get(usuarioRequest.Email);
                if(usuario != null){
                    if(usuario.Email != User.Identity.Name) return BadRequest("Você não pode editar a conta de outra pessoa.");
                    usuario = usuarioRequest.ToUsuario();
                    _usuarioRepository.Update(usuario);
                    var comprasUri = Url.Action("GetCompras", "Venda", new{email = usuario.Email});
                    return Ok(usuario.ToUsuarioResponse(comprasUri));
                }
                return BadRequest("Usuário não cadastrado");
            }
            return BadRequest("Requisição inválida");
        }
        

        [HttpGet("{email}/compras")]
        public IActionResult GetCompras(string email){
            var compras = _usuarioRepository.GetCompras(email);
            var vendasResponses = new List<VendaResponse>();
            foreach (var compra in compras)
            {
                var uriEvento = Url.Action("GetEvento", "Evento", new{id = compra.EventoId});
                var uriUsuario = Url.Action("GetUsuario", "Usuario", new{email = compra.UsuarioEmail});
                vendasResponses.Add(compra.ToVendaResponse(uriEvento, uriUsuario));
            }
            return Ok(vendasResponses);
        }

        [HttpDelete("{email}")]
        [Authorize]
        public IActionResult Delete(string email){
            var usuario = _usuarioRepository.Get(email);
            if(usuario != null){
                if(usuario.Email != User.Identity.Name) return BadRequest("Você não pode deletar a conta de outra pessoa.");
                _usuarioRepository.Delete(usuario);
                return NoContent();
            }
            return NotFound("Usuário não encontrado.");
        }
    }
}