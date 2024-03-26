using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Entities.DTOs.Requests
{
    public class AddAccountRoleRequestDto
    {
        public Guid UserId { get; set; }

        public string Role { get; set; }
    
    }
}
