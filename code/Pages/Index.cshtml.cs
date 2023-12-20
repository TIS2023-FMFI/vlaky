using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;

namespace code.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public async void OnGet()
    {
        if (!HttpContext.User.Identity.IsAuthenticated)
        {
            await HttpContext.ChallengeAsync();
        }
    }
}
