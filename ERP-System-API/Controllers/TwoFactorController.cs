using ERP_System.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ERP_System_API.Controllers
{
    [Authorize]
    public class TwoFactorController : APIBaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public TwoFactorController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [Authorize]
        [HttpPost("enable")]
        public async Task<IActionResult> EnableTwoFactor()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            user.TwoFactorEnabled = true;
            await _userManager.UpdateAsync(user);

            return Ok(new { message = "Two-Factor Authentication enabled." });
        }

        [HttpPost("disable")]
        public async Task<IActionResult> DisableTwoFactor()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            user.TwoFactorEnabled = false;
            await _userManager.UpdateAsync(user);

            return Ok(new { message = "Two-Factor Authentication disabled." });
        }

        [HttpGet("generate-authenticator-key")]
        public async Task<IActionResult> GenerateAuthenticatorKey()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var key = await _userManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(key))
            {
                await _userManager.ResetAuthenticatorKeyAsync(user);
                key = await _userManager.GetAuthenticatorKeyAsync(user);
            }

            return Ok(new { key });
        }
    }
}
