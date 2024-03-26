using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Entities.DbSet
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [Required, MaxLength(50)]
        public string FirstName { get; set; } = String.Empty;

        [Required, MaxLength(50)]
        public string LastName { get; set; } = String.Empty;
        [Required, MaxLength(250)]
        public string Adress{ get; set; } = String.Empty;
        public virtual Cart Cart { get; set; }  

    }
}
