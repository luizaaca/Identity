using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace Core.Model.Identity
{
    [Table("Users")]
    public class ApplicationUser : IIdentity
    {
        [Key]
        public int Id { get ; set ; }
        public string Email { get ; set ; }
        public string UserName { get ; set ; }
        public string PasswordHash { get ; set ; }
        public bool Active { get; set; }
        [Computed]
        [Write(false)]
        public string AuthenticationType { get; set; }
        [Computed]
        [Write(false)]
        public bool IsAuthenticated { get; set; }
        [Computed]
        [Write(false)]
        public string Name { get; set; }
        //[Computed]
        //[Write(false)]
        //public List<ApplicationRole> Roles { get; set; }
    }
}
