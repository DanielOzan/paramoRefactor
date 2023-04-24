using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Sat.Recruitment.Api.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Sat.Recruitment.Api.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace Sat.Recruitment.Api.Repository
{
    public class UserRepository_SqLite : IUserRepository
    {
        readonly ILogger<UserRepository> _logger;
        readonly IConfiguration _configuration;
        readonly SatDbContext _sqLiteDbContext;
        public UserRepository_SqLite(ILogger<UserRepository> logger, IConfiguration conf,SatDbContext sqLiteDbContext)
        {

            _logger = logger;
            _configuration = conf;
            _sqLiteDbContext = sqLiteDbContext;
        }
        public async Task<List<UserModel>> GetUsersAsync()
        {
            var userList = await _sqLiteDbContext.User.AsNoTracking().ToListAsync();

            return userList;
        }
        public async Task<UserModel> GetUserAsyncByAccount(string account)
        {
            var userFound =  await _sqLiteDbContext.User.AsNoTracking().FirstOrDefaultAsync(x=>x.Account==account);

            return userFound;
        }
        public  UserModel GetUserAsyncById(int Id)
        {
            var userFound =  _sqLiteDbContext.User.AsNoTracking().FirstOrDefaultAsync(x => x.Id == Id).Result;

            return userFound;
        }
        public async Task<UserResult> EditUserAsync(UserModel editUser,string account)
        {
            
            var userToEdit =   GetUserAsyncByAccount(account).Result;

            if (userToEdit != null)
            {
                //edit the user 
                userToEdit.Address = editUser.Address;
                userToEdit.Password = editUser. Password;
                userToEdit.Email = editUser.Email;
                userToEdit.Account = editUser.Account;
                userToEdit.Money = editUser.Money;
                userToEdit.Role = editUser.Role;
                userToEdit.Phone= editUser.Phone;
                userToEdit.UserType=editUser.UserType;
                

                _sqLiteDbContext.User.Update(userToEdit);
                await _sqLiteDbContext.SaveChangesAsync();
                _logger.LogInformation("the User was edited succesfully");
                return new UserResult { IsSuccess = true, ErrorDescription = "User updated" };
            }
            _logger.LogError("User not found");
            return new UserResult { IsSuccess = false, ErrorDescription = "User not found" };
        }
        public async Task<UserResult> RemoveUser(string account)
        {
            UserModel userToRemove = await GetUserAsyncByAccount(account);
            if (userToRemove != null)
            {
                _sqLiteDbContext.User.Remove(userToRemove);
                await _sqLiteDbContext.SaveChangesAsync();
                _logger.LogInformation("the User was removed succesfully");
                return new UserResult { IsSuccess = true, ErrorDescription = "The user was removed" };
            }
            _logger.LogError("User not found");
            return   new UserResult { IsSuccess = false, ErrorDescription = "User not found" };


        }
        public async Task<UserResult> AddUserAsync(UserModel newUser)
        {
            List<UserModel> usersList = await GetUsersAsync().ConfigureAwait(false);

            try
            {
                if (isDuplicate(usersList, newUser))
                {
                    _logger.LogError("User already exists, duplicated user found");
                    return new UserResult { IsSuccess = false, ErrorDescription = "The user is duplicated" };

                }

                _sqLiteDbContext.User.Add(newUser);
                _sqLiteDbContext.SaveChanges();


                return new UserResult { IsSuccess = true, ErrorDescription = null, SuccessMsg = "User Created" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new UserResult { IsSuccess = false, ErrorDescription = "Add User Fail" };
            }
        }

        private bool isDuplicate(List<UserModel> usersList, UserModel newEntry)
        {
            var dupFound = usersList.Where(x => (string.Compare(x.Phone, newEntry.Phone, false) == 0 || string.Compare(x.Email, newEntry.Email, StringComparison.Ordinal) == 0) || (string.Compare(x.Name, newEntry.Name, StringComparison.Ordinal) == 0 && string.Compare(x.Address, newEntry.Address, StringComparison.Ordinal) == 0));
            if (dupFound.Count() > 0)
                return true;
            else
                return false;
        }


    }
}

