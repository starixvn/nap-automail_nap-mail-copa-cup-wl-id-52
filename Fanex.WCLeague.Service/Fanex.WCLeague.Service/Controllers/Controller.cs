using Microsoft.AspNetCore.Mvc;

namespace Fanex.WCLeague.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Controller : ControllerBase
    {
        private const string OK = "OK";

        [HttpGet]
        public string Index()
        {
            return OK;
        }
    }
}