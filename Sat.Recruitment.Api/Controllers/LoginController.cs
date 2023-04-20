using Microsoft.AspNetCore.Http;
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
        private readonly IUserService _uServ;
        private readonly ILogger<LoginController> _logger;
        public LoginController(IUserService service, ILogger<LoginController> logger)
        {
            _uServ = service;
            _logger = logger;
        }
        [HttpPost]
        public async Task<ActionResult> Login(UserLogin user)
        {
            var token =LoginService.Authenticate(user);
            if (!(string.IsNullOrEmpty(token)))
                return Ok(token);
            else
                return NotFound("User not found");
       
            throw new NotImplementedException();
        }
    }
}
