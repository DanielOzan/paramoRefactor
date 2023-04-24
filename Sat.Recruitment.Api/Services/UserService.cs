using Microsoft.Extensions.Logging;
using Sat.Recruitment.Api.Dto;
using Sat.Recruitment.Api.Model;
using Sat.Recruitment.Api.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Services
{
    public class UserService:IUserService
    {
        private readonly IUserRepository _userRepository;   
        private readonly ILogger<UserService> _logger;
        public UserService(IUserRepository repo, ILogger<UserService> logger)
        {
            _userRepository= repo;
            _logger = logger;
        }

        //Validate request errors
        public string ValidateUserInputErrors(UserDto userVal)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrEmpty(userVal.Name))
                errors.Append("The name is required.");
            if (string.IsNullOrEmpty(userVal.Email) || !userVal.Email.Contains("@"))
                 errors.AppendJoin("|"," The email is empty or incorrect.");
            if (string.IsNullOrEmpty(userVal.Address))
                errors.AppendJoin("|"," The address is required.");
            if (string.IsNullOrEmpty(userVal.Phone))
                errors.AppendJoin("|"," The phone is required.");
            if (string.IsNullOrEmpty(userVal.Money) || !decimal.TryParse(userVal.Money, out decimal _))
                errors.AppendJoin("|"," The money is empty or incorrect.");
            if (string.IsNullOrEmpty(userVal.Password)) //password input validations
                errors.AppendJoin("|", " The password is empty or incorrect.");
            if (string.IsNullOrEmpty(userVal.Role) )
                errors.AppendJoin("|", " The Role is empty or incorrect.");
            if (string.IsNullOrEmpty(userVal.Account))
                errors.AppendJoin("|", " The Account is empty or incorrect.");


            return errors.ToString();
        }
        public  async Task<List<UserDto>> GetUsers()
        {
          return await   _userRepository.GetUsersAsync().Result.Select(x=> MapUserModelToDto(x)).AsQueryable().ToListAsync();

        }
        public async Task<UserDto> GetUser(string account)
        {
             var userFound= await _userRepository.GetUserAsyncByAccount(account);
            if (userFound == null)
                return null;
            else
            {
                return MapUserModelToDto(userFound);
            }
        }
        public async Task<UserResult> EditUser(UserDto userEdit,string account)
        {
            var errorDescription = ValidateUserInputErrors(userEdit);
            if (!string.IsNullOrEmpty(errorDescription) || string.IsNullOrEmpty(account))
            {
                _logger.LogError($"Validations fail: {errorDescription}");
                return new UserResult
                {
                        IsSuccess = false,
                        ErrorDescription = errorDescription
                };
            }
            var result = await _userRepository.EditUserAsync(MapUserDtoToModel(userEdit),account);
            
            if (result == null)
                return null;
            else
            {
                return result;
            }
        }
        public async Task<UserResult> RemoveUser(string account)
        {
            var resultUser =await  _userRepository.RemoveUser(account);
            if (resultUser.IsSuccess)
                return resultUser;
            else
                return null;



        }
        public async Task<UserResult> CreateUser(UserDto user)
        {
            string errorDescription = string.Empty;
            decimal gif;
            decimal percentage = 0; //default value <no gift>

            try
            {
                errorDescription = ValidateUserInputErrors(user);
                if (!string.IsNullOrEmpty(errorDescription))
                {
                    _logger.LogError($"Validations fail: {errorDescription}");
                    return new UserResult
                    {
                        IsSuccess = false,
                        ErrorDescription = errorDescription

                    };
                }
                UserModel userInput = MapUserDtoToModel(user);

                switch (userInput.UserType)
                {
                    case "Normal":
                        {
                            if (userInput.Money > 100)
                            {
                                percentage = Convert.ToDecimal(0.12);
                            }
                            if (userInput.Money > 10 && userInput.Money < 100)
                            {
                                percentage = Convert.ToDecimal(0.8);
                            }
                            break;
                        }
                    case "SuperUser":
                        {
                            if (userInput.Money > 100)
                                percentage = Convert.ToDecimal(0.20);
                            break;
                        }
                    default:
                        break;
                }
                gif = (decimal)userInput.Money * percentage;


                if (string.Compare(userInput.UserType, "Premium", StringComparison.Ordinal) == 0 && userInput.Money > 100)
                    gif = (decimal)userInput.Money * 2;


                userInput.Money = userInput.Money + gif;

                userInput.Email = NormalizeEmail(userInput.Email);
                _logger.LogInformation($"Add User successfuly created: Name:{userInput.Name} , Address:{userInput.Address}, UserType:{userInput.UserType} ,  Role:{userInput.Role}, Email:{userInput.Email}, Phone:{userInput.Phone}");
                return await _userRepository.AddUserAsync(userInput);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new UserResult { IsSuccess = false, ErrorDescription = "Create User service fail" };
            }

        }

        public UserModel MapUserDtoToModel(UserDto user)
        {

            UserModel userInput = new UserModel
            {
                Name = user.Name,
                Email = user.Email,
                Address = user.Address,
                Phone = user.Phone,
                Money = decimal.Parse(user.Money),
                UserType = user.UserType,
                Account = string.IsNullOrEmpty(user.Account) ? user.Email : user.Account,
                Password = user.Password,
                Role=user.Role
            };

            return userInput;
        }
        public UserDto MapUserModelToDto(UserModel user)
        {

            UserDto userInput = new UserDto
            {
                Name = user.Name,
                Email = user.Email,
                Address = user.Address,
                Phone = user.Phone,
                Money = user.Money.ToString(),
                UserType = user.UserType,
                Password= user.Password,
                Account= user.Account,
                Role=user.Role
                

            };

            return userInput;
        }

        public string NormalizeEmail(string newEmail)
        {

            var aux = newEmail.Split(new char[] { '@' });
            var charsToRemove = new string[] { " ", ".", "+" };
            foreach (var c in charsToRemove)
            {
                aux[0] = aux[0].Replace(c, string.Empty);
            }
            return string.Join("@", new string[] { aux[0], aux[1] });
        }

    }
}
