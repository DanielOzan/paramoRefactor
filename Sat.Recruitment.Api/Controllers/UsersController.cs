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
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> CreateUser(UserDto user)
        {

            var resultModel =  await Task.Run(()=> _uServ.CreateUser(user));
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
        [HttpPost]
        [Route("Remove/{account}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Remove(string account)
        {

            var resultModel = await Task.Run(() => _uServ.RemoveUser(account));
            if (resultModel!=null)
            {
                _logger.LogInformation("Remove user success");
                return Ok(resultModel);
            }
            else
            {
                _logger.LogError("Remove user  error");
                return NotFound(resultModel);
            }
        }
        [HttpPost]
        [Route("Edit")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> EditUser(UserDto user,string account)
        {
            var resultModel =await Task.Run(() => _uServ.EditUser(user,account));
            if (resultModel!=null && (resultModel.IsSuccess))
            {
                _logger.LogInformation("Edit user success");
                return Ok(resultModel);
            }
            else
            {
                _logger.LogError("Edit user  error");
                return NotFound(resultModel);
            }
        }
        [HttpGet]
        [Route("/GetUser/{account}")]
        public async Task<ActionResult> GetUser(string account)
        {

            var resultModel =await Task.Run(() => _uServ.GetUser(account));
            if (resultModel!=null)
            {
                return Ok(resultModel);
            }
            else
            {
                return NotFound(resultModel);
            }
        }
        [HttpGet]
        [Route("/GetUsers")]
        public async Task<ActionResult<List<UserDto>>> GetUsers()
        {
            var userListTask = await Task.Run(() => _uServ.GetUsers());
                return Ok(userListTask);
        }
    }
}
