using Sat.Recruitment.Api.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Repository
{
    public interface IUserRepository
    {
        Task<UserResult> EditUserAsync(UserModel editUser,string account);
        Task<UserModel> GetUserAsyncByAccount(string account);
        Task<UserResult> RemoveUser(string account);
        Task<List<UserModel>> GetUsersAsync();
        Task<UserResult> AddUserAsync(UserModel newUser);
    }
}
