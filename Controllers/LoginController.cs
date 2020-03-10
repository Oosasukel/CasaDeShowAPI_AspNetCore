using CasaDeShowAPI.Model.ViewModels;
using CasaDeShowAPI.Repositories;
using CasaDeShowAPI.Token;
using Microsoft.AspNetCore.Mvc;

namespace CasaDeShowAPI.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class LoginController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public LoginController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        [HttpPost]
        public IActionResult Login([FromBody] Login login){
            if(ModelState.IsValid){
                bool isValid = _usuarioRepository.IsValid(login);
                if(isValid){
                    var usuario = _usuarioRepository.Get(login.Email);
                    var loginResponse = TokenService.GenerateToken(usuario);
                    return Ok(loginResponse);
                }
                else{
                    return BadRequest("Login inválido");
                }
            }
            return BadRequest("Requisição inválida");
        }
    }
}