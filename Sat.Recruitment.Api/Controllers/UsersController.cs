using Microsoft.AspNetCore.Mvc;
using Sat.Recruitment.Api.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sat.Recruitment.Api.Model;
using Serilog;
using Microsoft.Extensions.Logging;

namespace Sat.Recruitment.Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public partial class UsersController : ControllerBase
    {
        private readonly IUserService _uServ;
        private readonly ILogger<UsersController> _logger;

        private readonly List<UserModel> _users = new List<UserModel>();
        public UsersController(IUserService service, ILogger<UsersController> logger)
        {
            _uServ= service;
            _logger= logger;
        }

        [HttpPost]
        [Route("/create-user")]
        public async Task<IActionResult> CreateUser(string name, string email, string address, string phone, string userType, string money)
        {

            var resultModel = await Task.Run(()=> _uServ.CreateUser(name, email, address, phone, userType, money));
            if (resultModel.IsSuccess)
            {
                _logger.LogInformation("Create user success");
                return Ok(resultModel);
               
            }
            else
            {
                _logger.LogError("Create user  error");
                return BadRequest(resultModel);
               
            }

        }

    }

}
