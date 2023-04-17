using Microsoft.AspNetCore.Mvc;
using Sat.Recruitment.Api.Model;

namespace Sat.Recruitment.Api.Services
{
    public interface IUserService
    {
         string ValidateUserInputErrors(UserModel userVal);
         UserResult CreateUser(string in_name, string in_email, string in_address, string in_phone, string in_money, string in_userType);
        string NormalizeEmail(string newEmail);
    }

}
