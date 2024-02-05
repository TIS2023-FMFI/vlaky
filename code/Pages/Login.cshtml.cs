using code.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Microsoft.AspNetCore.Authentication.Cookies;

namespace code.Pages
{
    public class LoginModel(ILogger<IndexModel> logger,
        AccountManagerService accountManagerService, 
        LoggerService loggerService) : PageModel
    {
        public bool fail = false;
        private readonly ILogger<IndexModel> _logger = logger;
        private LoggerService _loggerService = loggerService;
        private readonly AccountManagerService accountManager = accountManagerService;

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [EmailAddress]
            public string? Email { get; set; } = string.Empty;

            [DataType(DataType.Password)]
            public string? Password { get; set; } = string.Empty;
        }

        public void OnGet(string returnUrl = null)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Response.Redirect("/Index");
                return;
            }

            ErrorMessage = null;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = "/Index") {
            ReturnUrl = returnUrl;

            if (string.IsNullOrEmpty(Input.Email) || string.IsNullOrEmpty(Input.Password)) 
            {
                ErrorMessage = "Nesprávne meno alebo heslo.";
                return Page();
            }

            var account = await accountManager.GetAccount(Input.Email, Input.Password);

            if (account == null) 
            {
                Input.Email = null;
                Input.Password = null;
                ErrorMessage = "Nesprávne meno alebo heslo.";
                return Page();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, account.Name),
                new Claim(ClaimTypes.Email, account.Mail),
                new Claim("Id", account.Id.ToString()),
                new Claim("Privileges", account.Privileges.ToString()),
                new Claim("TimeCreated", DateTime.Now.ToString())
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

            _loggerService.writeLogIn(account.Name);

            ErrorMessage = null;
            return LocalRedirect(returnUrl);
        }
    }
}
