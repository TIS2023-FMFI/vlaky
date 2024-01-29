using code.Models;
using code.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using code.Auth;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace code.Pages
{
    [Authorize(Policy = "UserManagementPolicy")]
    public class ManageModel : PageModel
    {
        private readonly ILogger<ManageModel> _logger;
            private readonly LoggerService _loggerService;
            private readonly UserValidationService _userValidationService;
        
        public bool allow;

        public ManageModel(ILogger<ManageModel> logger,
            LoggerService loggerService,
            UserValidationService userValidationService)
        {
            _logger = logger;
            _loggerService = loggerService;
            _userValidationService = userValidationService;
            allow = false;
        }
        

        public Account? Acc{get;private set;}
        public async void OnGet()
        {
            _userValidationService.ValidateUser(HttpContext);
        }


    }
}
