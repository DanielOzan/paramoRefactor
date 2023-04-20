using JsonConverter;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sat.Recruitment.Api.Model
{
    [Serializable]
    public class UserModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string UserType { get; set; }
        public decimal Money { get; set; }
        public string Password { get; internal set; }
        public string Rol { get; internal set; }
        public string Account { get; internal set; }
        public string LastName { get; internal set; }
    }

}
