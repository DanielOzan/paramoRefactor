using Microsoft.AspNetCore.Mvc;
using Sat.Recruitment.Api.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sat.Recruitment.Api.Model;
using Microsoft.Extensions.Logging;
using Sat.Recruitment.Api.Dto;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize]
        public async Task<ActionResult> CreateUser(UserDto user)
        {

            var resultModel =  Task.Run(()=> _uServ.CreateUser(user)).Result;
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
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<UserDto>>> GetUsers()
        {
            var userListTask = Task.Run(() => _uServ.GetUsers());
                return Ok(userListTask.Result);
        }

    }

}
