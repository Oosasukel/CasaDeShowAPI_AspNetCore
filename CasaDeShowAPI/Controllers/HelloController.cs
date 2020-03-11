using Microsoft.AspNetCore.Mvc;

namespace CasaDeShowAPI_AspNetCore.Controllers
{
    [ApiController]
    [Route("")]
    public class HelloController : ControllerBase
    {
        [HttpGet]
        public string Hello(){
            return "Hello, it's works!";
        }
    }
}