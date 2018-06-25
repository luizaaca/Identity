using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.CustomAttributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    [RequiresPermission("UserManagement.List")]
    public class UserManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}