using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;

public class LogoutModel : PageModel
{
    public async void OnGet()
    {
        if (HttpContext.User.Identity.IsAuthenticated)
        {
            await HttpContext.SignOutAsync();
        }
        HttpContext.ChallengeAsync();
    }
}
