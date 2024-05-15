using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Entities.DTOs.AccountDTOs
{
    public class ResetPasswordDto
    {
        [Required, MaxLength(50)]
        public string newPassword { get; set; }
    }
}
