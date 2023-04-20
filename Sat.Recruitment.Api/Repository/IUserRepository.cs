using Sat.Recruitment.Api.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Repository
{
    public interface IUserRepository
    {
          Task<List<UserModel>> GetUsersAsync();
          Task<UserResult> AddUserAsync(UserModel newUser);
    }
}
