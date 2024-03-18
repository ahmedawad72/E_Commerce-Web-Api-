using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Entities.DTOs.Responses
{
    public class UserLoginResponseDto
    {
        public string Token { get; set; }
        public List<string> Errors { get; set; }
        public bool IsAuthenticated { get; set; }
        public List<string> Roles { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
