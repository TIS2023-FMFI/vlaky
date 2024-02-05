using code.Auth;
using code.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace code.Pages;

public class ForbiddenModel : PageModel
{
    public bool ban {get; set;}
    private readonly ILogger<IndexModel> _logger;
    private readonly UserValidationService _userValidationService;

    public ForbiddenModel(ILogger<IndexModel> logger,
        UserValidationService userValidationService)
    {
        _logger = logger;
        _userValidationService = userValidationService;
    }

    public async void OnGet()
    {
        if (!HttpContext.User.Identity.IsAuthenticated ||
            _userValidationService.UserChanged(
                Convert.ToInt32(HttpContext.User.FindFirst("Id").Value),
                DateTime.Parse(HttpContext.User.FindFirst("TimeCreated").Value)
            ))
        {
            await HttpContext.ChallengeAsync();
            return;
        }

        var claim = HttpContext.User.FindFirst(
                c => c.Type == "Privileges"
            );

        ban = !AuthRequirementHandler.isBitSet(claim, 0);
    }
}
