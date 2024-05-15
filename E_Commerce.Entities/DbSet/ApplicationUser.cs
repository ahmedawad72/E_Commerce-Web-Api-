using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Entities.DbSet
{
    public class ApplicationUser : IdentityUser
    {
        [Required, MaxLength(50)]
        public string FirstName { get; set; } = String.Empty;

        [Required, MaxLength(50)]
        public string LastName { get; set; } = String.Empty;
   
        [Required, MaxLength(250)]
        public string Address { get; set; } 
        
        public virtual Cart Cart { get; set; }  // list of orders instead

    }
}
