using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model.Identity
{
    public class ApplicationRole
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Action> RoleActions { get; set; }
    }
}
