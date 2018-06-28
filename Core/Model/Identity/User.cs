using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace Core.Model.Identity
{
    [Table("Users")]
    public class ApplicationUser
    {
        [Key]
        public int Id { get ; set ; }
        public string Email { get ; set ; }
        public string UserName { get ; set ; }
        public string PasswordHash { get ; set ; }
        public bool Active { get; set; }        
    }
}
