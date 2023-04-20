using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sat.Recruitment.Api.Dto;
using Sat.Recruitment.Api.Model;
using Sat.Recruitment.Api.Services;
using System;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        
        private readonly ILogger<LoginController> _logger;
        private readonly ILoginService _logserv;
        public LoginController( ILogger<LoginController> logger,ILoginService logserv)
        {
            _logger = logger;
            _logserv = logserv;
        }
        [HttpPost]
        public async Task<ActionResult> Login(UserLogin user)
        {
            var token = _logserv.Authenticate(user);
            if (!(string.IsNullOrEmpty(token)))
                return Ok(token);
            else
                return NotFound("User not found");
       
            throw new NotImplementedException();
        }
    }
}
