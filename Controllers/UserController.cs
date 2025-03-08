using System.Threading.Tasks;
using App.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimoshStore;

namespace MyApp.Namespace
{
    [Authorize(Roles = "admin")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public ActionResult DeleteUser(int id)
        {
            _userService.DeleteUserAsync(id);
            return RedirectToAction("ManageUsers", "Admin");
        }
        public async Task<IActionResult> UpdateUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return RedirectToAction("ManageUsers", "Admin");
            }

            var viewModel = new UpdateUserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                RoleId = user.RoleId,
                Enabled = user.Enabled,
                Address = user.Address
            };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUserAsync(UpdateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =await _userService.UpdateUserAsync(model);

                if (result.Success)
                {
                    TempData["SuccessMessage"] = "User updated successfully.";
                    return RedirectToAction("ManageUsers","Admin");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to update user.");
                }
            }

            return View(model);
        }

    }
}
