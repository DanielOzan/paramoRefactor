using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Sat.Recruitment.Api.Dto;
using Sat.Recruitment.Api.Model;
using Serilog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;

namespace Sat.Recruitment.Api.Services
{
    public  class LoginService:ILoginService
    {
        private readonly IUserService _usrServ;
        private readonly ILogger<LoginService> _log;
        private readonly IConfiguration _conf;

        public LoginService(IUserService usrServ,ILogger<LoginService> log,IConfiguration conf)
        {
            _usrServ = usrServ;
            _log= log;
            _conf = conf;
        }

        public void CreateDefaultAdmin()
            {
            UserDto newAdminDefault = new UserDto
            {
                Name = "AdministradorDefault",
                Email = "admin@default.com",
                Account= "admin",
                Address = "RandomAddress 123",
                Phone = "333-333",
                UserType = "Admin",
                Money = "0",
                Password = "admin123",
            };
            //ensure creation
            var result=_usrServ.CreateUser(newAdminDefault);
            }

        public  string Authenticate (UserLogin user)
        {
            
            var userResult = _usrServ.GetUsers().FirstOrDefault(x => (x.Account == user.account && x.Password == user.password));
           
            if (userResult!=null) {
                UserModel userModelResult = _usrServ.MapUserDtoToModel(userResult);
                string token = GenerateToken(userModelResult);
                return token;
            }


            return string.Empty;
        }
        private  string GenerateToken(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_conf["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Crear los claims
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Account),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.Name)

            
            };


            // Crear el token

            var token = new JwtSecurityToken(
               _conf["Jwt:Issuer"],
               _conf["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    
    }
}
