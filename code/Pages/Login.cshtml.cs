using code.Services;
using code.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Microsoft.AspNetCore.Authentication.Cookies;

namespace code.Pages
{
    public class LoginModel(ILogger<IndexModel> logger,
        AccountManagerService accountManagerService) : PageModel
    {
        public bool fail = false;
        private readonly ILogger<IndexModel> _logger = logger;
        private readonly AccountManagerService accountManager = accountManagerService;

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [EmailAddress]
            public string? Email { get; set; }

            [DataType(DataType.Password)]
            public string? Password { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {

        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = "/Index") {
            ReturnUrl = returnUrl;

            if (Input.Password == null || Input.Email == null) {
                return Page();
            }

            var account = await accountManager.GetAccount(Input.Email, Input.Password);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, account.Name),
                new Claim(ClaimTypes.Email, account.Mail),
                new Claim("Id", account.Id.ToString()),
                new Claim("Privileges", account.Privileges.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            var authProperties = new AuthenticationProperties{
                AllowRefresh = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties
            );

            return LocalRedirect(returnUrl);
        }
    }
}
