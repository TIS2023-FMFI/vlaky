using code.Auth;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace code.Pages;

public class ForbiddenModel : PageModel
{
    public bool ban {get; set;}
    private readonly ILogger<IndexModel> _logger;

    public ForbiddenModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        var claim = HttpContext.User.FindFirst(
                c => c.Type == "Privileges"
            );

        ban = !AuthRequirementHandler.isBitSet(claim, 0);
    }
}
