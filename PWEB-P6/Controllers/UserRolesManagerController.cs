using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PWEB_P6.Models;
using PWEB_P6.ViewModels;

namespace PWEB_P6.Controllers
{
    public class UserRolesManagerController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserRolesManagerController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRolesViewModel = new List<UserRolesViewModel>();

            foreach (var user in users)
            {
                userRolesViewModel.Add(new UserRolesViewModel {
                    UserId = user.Id,
                    PrimeiroNome = user.PrimeiroNome,
                    UltimoNome = user.UltimoNome,
                    UserName = user.UserName,
                    Roles = await _userManager.GetRolesAsync(user),
                    Avatar = user.Avatar
                });
            }

            return View(userRolesViewModel);
        }

        private async Task<List<string>> GetUserRoles(ApplicationUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }

        public async Task<IActionResult> Details(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var manageUserRolesViewModels = new List<ManageUserRolesViewModel>();

            foreach (var role in await _roleManager.Roles.ToListAsync())
            {
                manageUserRolesViewModels.Add(new ManageUserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    Selected = await _userManager.IsInRoleAsync(user, role.Name)
                });
            }

            ViewBag.UserName = user.UserName;
            ViewBag.Avatar = user.Avatar;
            return View(manageUserRolesViewModels);
        }
        [HttpPost]
        public async Task<IActionResult> Details(List<ManageUserRolesViewModel> model,
            string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return View();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }

            result = await _userManager.AddToRolesAsync(
                user, 
                model
                    .Where(r => r.Selected)
                    .Select(r => r.RoleName)
                );

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }

            return RedirectToAction("Index");
        }
    }
}
