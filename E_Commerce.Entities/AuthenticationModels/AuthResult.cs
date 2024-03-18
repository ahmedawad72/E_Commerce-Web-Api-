using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Entities.AuthenticationModels
{
    public class AuthResult
    {
        public string Token { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public bool IsAuthenticated { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public DateTime ExpiryDate { get; set; }
        private AuthResult(bool isAuthenticated, string token, DateTime expiryDate,
                            IEnumerable<string> roles, IEnumerable<string> errors)
        {
            IsAuthenticated = isAuthenticated;
            Token = token;
            ExpiryDate = expiryDate;
            Roles = roles ?? new List<string>();
            Errors = errors ?? new List<string>();
        }

        public static AuthResult Successful(string token, DateTime expiryDate, IEnumerable<string> roles)
        {
            return new AuthResult(true, token, expiryDate, roles, null);
        }

        public static AuthResult Failed(IEnumerable<string> errors)
        {
            return new AuthResult(false, null, default, null, errors);
        }
    }
}
