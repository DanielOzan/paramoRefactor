using Sat.Recruitment.Api.Model;
using System.Collections.Generic;

namespace Sat.Recruitment.Api.Repository
{
    public interface IUserRepository
    {
         List<UserModel> GetUsers();
         UserResult AddUser(UserModel newUser);
    }
}
