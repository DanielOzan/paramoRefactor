using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.IdentityModel.Tokens;
using Sat.Recruitment.Api.Constants;
using Sat.Recruitment.Api.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Sat.Recruitment.Api.Services
{
    public  static class LoginService
    {

        public static string Authenticate (UserLogin user)
        {
            UserModel userResult = UserConstants.Users.FirstOrDefault(x => (x.Account == user.account && x.Password == user.password));
            if (userResult != null) {
                string token = GenerateToken(userResult);
                return token;
            }


            return string.Empty;
        }
        private static string GenerateToken(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Startup.Configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Crear los claims
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.Name),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Role, user.Rol),
            };


            // Crear el token

            var token = new JwtSecurityToken(
                Startup.Configuration["Jwt:Issuer"],
                Startup.Configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    
    }
}
