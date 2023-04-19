using Microsoft.AspNetCore.Mvc;
using Sat.Recruitment.Api.Dto;
using Sat.Recruitment.Api.Model;
using System.Collections.Generic;

namespace Sat.Recruitment.Api.Services
{
    public interface IUserService
    {
        string ValidateUserInputErrors(UserDto userVal);
        UserResult CreateUser(UserDto user);
        string NormalizeEmail(string newEmail);
         List<UserDto> GetUsers();
    }

}
