using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model.Identity
{
    [Table("Roles")]
    public class ApplicationRole
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }

        [Write(false)]
        [Computed]
        public List<Action> RoleActions { get; set; }
    }
}
