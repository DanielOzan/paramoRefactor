using Microsoft.AspNetCore.Mvc;
using Sat.Recruitment.Api.Dto;
using Sat.Recruitment.Api.Model;

namespace Sat.Recruitment.Api.Services
{
    public interface IUserService
    {
        string ValidateUserInputErrors(UserDto userVal);
        UserResult CreateUser(UserDto user);
        string NormalizeEmail(string newEmail);
    }

}
