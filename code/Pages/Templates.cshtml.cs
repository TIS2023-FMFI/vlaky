using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using code.Services;
using code.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace code.Pages
{
    [Authorize(Policy = "TrainManagementPolicy")]
    public class TemplatesModel : PageModel
    {
        private readonly TemplateManagerService _TemplateManagerService;
        private readonly ILogger<TemplatesModel> _logger;
        private readonly LoggerService _loggerService;
        private readonly UserValidationService _userValidationService;

        public List<TrainTemplate> Templates { get; set; }

        public TemplatesModel(ILogger<TemplatesModel> logger, 
            TemplateManagerService templateManagerService,
            LoggerService loggerService,
            UserValidationService userValidationService)
        {
            _logger = logger;
            _TemplateManagerService = templateManagerService;
            _loggerService = loggerService;
            _userValidationService = userValidationService;
            Templates = new List<TrainTemplate>();
        }

        public async void OnGetAsync()
        {
            _userValidationService.ValidateUser(HttpContext);
            
            var templateId = HttpContext.Request.Query["templateId"].ToString();
            if (!string.IsNullOrEmpty(templateId))
            {
                await _TemplateManagerService.DeleteTemplate(Convert.ToInt32(templateId));
            }

            Templates = await _TemplateManagerService.GetTemplates();
        }
    }
}
