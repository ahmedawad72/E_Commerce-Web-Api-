using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Entities.DTOs.Requests
{
    public class UserRegistrationRequestDto
    {
        [Required]
        public string FirstName { get; set; } 
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        
    }
}
