using Microsoft.AspNetCore.Mvc;
using Sat.Recruitment.Api.Dto;
using Sat.Recruitment.Api.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Services
{
    public interface IUserService
    {
        Task<UserResult> EditUser(UserDto userEdit,string account);
        Task<UserResult> RemoveUser(string account);
        Task<UserDto> GetUser(string account);
        string ValidateUserInputErrors(UserDto userVal);
        Task<UserResult> CreateUser(UserDto user);
        string NormalizeEmail(string newEmail);
         List<UserDto> GetUsers();
        UserDto MapUserModelToDto(UserModel user);
        UserModel MapUserDtoToModel(UserDto user);
    }

}
