using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pi.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServoApiController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<ServoApiController> _logger;

        public ServoApiController(ILogger<ServoApiController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            
        }
    }
}
