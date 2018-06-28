using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Model.Identity;
using Core.Repositories;
using Identity.CustomAttributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    [Authorize]
    public class UserManagementController : Controller
    {
        private IUserRepository _userRepository;

        public UserManagementController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [RequiresPermission("UserManagement.List")]
        public async Task<IActionResult> Index()
        {
            var list = await _userRepository.GetAllUsers();

            return View(list);
        }
    }
}