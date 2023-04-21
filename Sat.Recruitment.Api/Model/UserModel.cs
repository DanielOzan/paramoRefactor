using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace Sat.Recruitment.Api.Model
{
    [Serializable]
    public class UserModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        
        public string Address { get; set; }
        public string Phone { get; set; }
        [Required]
        public string UserType { get; set; }
        [Required]
        public decimal Money { get; set; }
        [Required]
        public string Password { get;  set; }
        [Required]
        public string Role { get;  set; }
        [Required]
        public string Account { get;  set; }
        public string LastName { get;  set; }
    }

}
