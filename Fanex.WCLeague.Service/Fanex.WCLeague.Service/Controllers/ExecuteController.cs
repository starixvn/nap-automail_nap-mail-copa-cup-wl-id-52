using Fanex.MailDelivery.Core.Models;
using Fanex.WCLeague.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.WCLeague.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExecuteController(MainService service) : ControllerBase
    {
        [HttpGet]
        public JobMessage Get()
        {
            return service.Execute("WC2025USA");
        }
    }
}