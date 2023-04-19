using Microsoft.AspNetCore.Mvc;
using Sat.Recruitment.Api.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sat.Recruitment.Api.Model;
using Microsoft.Extensions.Logging;
using Sat.Recruitment.Api.Dto;

namespace Sat.Recruitment.Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public partial class UsersController : ControllerBase
    {
        private readonly IUserService _uServ;
        private readonly ILogger<UsersController> _logger;
        public UsersController(IUserService service, ILogger<UsersController> logger)
        {
            _uServ= service;
            _logger= logger;
        }

        [HttpPost]
        [Route("/create-user")]
        public async Task<IActionResult> CreateUser(UserDto user)
        {

            var resultModel = await Task.Run(()=> _uServ.CreateUser(user));
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
