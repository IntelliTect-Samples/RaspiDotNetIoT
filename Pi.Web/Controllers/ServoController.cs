using Microsoft.AspNetCore.Http;
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
    public class ServoController : ControllerBase
    {

        private readonly ILogger<ServoController> _logger;

        IO.IPWMServoController _IOServoController;

        public ServoController(IO.IPWMServoController servoController, ILogger<ServoController> logger)
        {
            _IOServoController = servoController;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public string Get()
        {
            return $"Current PWM impulse: {_IOServoController.ReadPwm()}";
        }
        [HttpGet("Up")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public void Up()
        {
            _IOServoController.IncreasePulse();
        }

        [HttpGet("down")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public void Down()
        {
            _IOServoController.DecreasePulse();
        }
    }
}
