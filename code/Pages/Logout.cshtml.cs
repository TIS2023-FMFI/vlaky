using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;

public class LogoutModel : PageModel
{
    public async Task<IActionResult> OnGet()
    {
        if (!HttpContext.User.Identity.IsAuthenticated)
        {
            return Redirect("/Login");
        }
        else
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
        return Redirect("/Login");
    }
}
