using Sat.Recruitment.Api.Model;
using System.Collections.Generic;

namespace Sat.Recruitment.Api.Constants
{
    public  class UserConstants
    {
        public static List<UserModel> Users = new List<UserModel>()
        {
            new UserModel() { Account = "jperez", Password = "admin123", Rol = "Administrador", Email = "jperez@gmail.com", Name = "Juan", LastName = "Perez"},
            new UserModel() { Account = "mgonzalez", Password = "admin123", Rol = "Vendedor", Email = "mgonzalez@gmail.com", Name = "Maria", LastName = "Gonzalez"},
        };
    }
}
