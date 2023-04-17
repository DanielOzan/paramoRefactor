
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Extensions;
using Sat.Recruitment.Api.Model;
using Sat.Recruitment.Api.Repository;
using Serilog;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Security.Policy;
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

        //Validate errors
        public string ValidateUserInputErrors(UserModel userVal)
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
            if (userVal.Money==null)
                errors.AppendJoin("|"," The money is empty or incorrect.");


            return errors.ToString();
        }
        public UserResult CreateUser(string in_name, string in_email, string in_address, string in_phone,  string in_userType,string in_money)
        {
            string errorDescription = string.Empty;
            decimal moneyParsed;
            decimal gif;
            decimal percentage = 1;
          

            UserModel userInput = new UserModel
            {
                Name = in_name,
                Email = in_email,
                Address = in_address,
                Phone = in_phone,
                Money = decimal.TryParse(in_money, out moneyParsed) ? moneyParsed : (decimal?)null,
                UserType = in_userType

            };
            try
            {
                errorDescription = ValidateUserInputErrors(userInput);
                if (!string.IsNullOrEmpty(errorDescription))
                {
                    _logger.LogError("Validations fail: " + errorDescription);
                    return new UserResult
                    {
                        IsSuccess = false,
                        ErrorDescription = errorDescription

                    };
                }

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
                        percentage = 1;
                        break;
                }
                gif = (decimal)userInput.Money * percentage;


                if (userInput.UserType == "Premium" && userInput.Money > 100)
                    gif = (decimal)userInput.Money * 2;


                userInput.Money = userInput.Money + gif;

                userInput.Email = NormalizeEmail(userInput.Email);
                _logger.LogInformation($"Add User successfuly created: Name:{userInput.Name} , Address:{userInput.Address}, UserType:{userInput.UserType} ,  Money:{userInput.Money}, Email:{userInput.Email}, Phone:{userInput.Phone}");
                return _userRepository.AddUser(userInput);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new UserResult { IsSuccess = false, ErrorDescription = "Create User service fail" };
            }

        }
        public string NormalizeEmail(string newEmail)
        {

            var aux = newEmail.Split(new char[] { '@' });
            var charsToRemove = new string[] { " ", ".", "+" };
            foreach (var c in charsToRemove)
            {
                aux[0] = aux[0].Replace(c, string.Empty);
            }

            string firstPart= Regex.Replace(aux[0], "[A-Za-z ]", "");
            return string.Join("@", new string[] { aux[0], aux[1] });
        }

    }
}
