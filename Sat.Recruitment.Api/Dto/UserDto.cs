using System.ComponentModel.DataAnnotations;
using System;

namespace Sat.Recruitment.Api.Dto
{
    public class UserDto
    {

            [Required]
            public string Name { get; set; }
            [Required]
            public string Email { get; set; }
            [Required]
            public string Address { get; set; }
            [Required]
            public string Phone { get; set; }
            [Required]
            public string UserType { get; set; }
            [Required]
            public string Money { get; set; }
            public string Account { get; set; }
            [Required]
            public string Password { get; set; }
            [Required]
            public string Role { get; set; }

    }
}
