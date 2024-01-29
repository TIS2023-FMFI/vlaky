using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using code.Services;
using code.Models;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace code.Pages
{
    [Authorize(Policy = "TrainManagementPolicy")]
    public class CreateEditTemplate : PageModel
    {
        private readonly ILogger<CreateEditTemplate> _logger;
        private readonly LoggerService _loggerService;
        private readonly UserValidationService _userValidationService;
        private readonly TemplateManagerService _templateManagerService;

        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string Destination { get; set; }

        public string ErrorMessage { get; set; }

        public CreateEditTemplate(TemplateManagerService templateManagerService, 
            ILogger<CreateEditTemplate> logger,
            LoggerService loggerService,
            UserValidationService userValidationService)
        {
            _templateManagerService = templateManagerService;
            _logger = logger;
            _loggerService = loggerService;
            _userValidationService = userValidationService;
        }

        public async void OnGetAsync(int? templateId)
        {
            _userValidationService.ValidateUser(HttpContext);

            ErrorMessage = null;

            if (templateId.HasValue)
            {
                var template = await _templateManagerService.GetTemplateById(templateId.Value);
                if (template != null)
                {
                    Name = template.Name;
                    Destination = template.Destination;
                }
                else
                {
                    Redirect("/Templates");
                }
            }
        }

        public async void OnPostAsync()
        {
            _userValidationService.ValidateUser(HttpContext);

            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Destination))
            {
                ErrorMessage = "Vyplňte všetky polia.";
                return;
            }

            var templateId = HttpContext.Request.Query["templateId"].ToString();
            var newTemplate = new TrainTemplate
            {
                Name = Name,
                Destination = Destination
            };

            if (!string.IsNullOrEmpty(templateId))
            {
                newTemplate.Id = Convert.ToInt32(templateId);
                await _templateManagerService.UpdateTemplate(newTemplate);
            }
            else
            {
                await _templateManagerService.AddTemplate(newTemplate);
            }
            Redirect("/Templates");
        }
    }
}
