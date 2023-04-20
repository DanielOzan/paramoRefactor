using Microsoft.Extensions.Logging;
using Sat.Recruitment.Api.Dto;
using Sat.Recruitment.Api.Model;
using Sat.Recruitment.Api.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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
            if (string.IsNullOrEmpty(userVal.Password) || !decimal.TryParse(userVal.Money, out decimal _))
                errors.AppendJoin("|", " The money is empty or incorrect.");


            return errors.ToString();
        }
        public List<UserDto> GetUsers()
        {
          return  _userRepository.GetUsersAsync().Result.Select(x=> MapUserModelToDto(x)).ToList();
        }
        public UserResult CreateUser(UserDto user)
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
                _logger.LogInformation($"Add User successfuly created: Name:{userInput.Name} , Address:{userInput.Address}, UserType:{userInput.UserType} ,  Money:{userInput.Money}, Email:{userInput.Email}, Phone:{userInput.Phone}");
                return  _userRepository.AddUserAsync(userInput).Result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new UserResult { IsSuccess = false, ErrorDescription = "Create User service fail" };
            }

        }

        private UserModel MapUserDtoToModel(UserDto user)
        {

            UserModel userInput = new UserModel
            {
                Name = user.Name,
                Email = user.Email,
                Address = user.Address,
                Phone = user.Phone,
                Money = decimal.Parse(user.Money),
                UserType = user.UserType,
                Account = user.Email,
                Password = user.Password


            };

            return userInput;
        }
        private UserDto MapUserModelToDto(UserModel user)
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
                Account= user.Account
                

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
