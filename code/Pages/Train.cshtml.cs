using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using code.Services;
using code.Models;


namespace code.Pages
{
    public class TrainModel : PageModel
    {
        private readonly TrainManagerService _TrainManagerService;
        private readonly WagonManagerService _WagonManagerService;
        private readonly ILogger<ScheduleModel> _logger;
        private readonly LoggerService _loggerService;
        private readonly UserValidationService _userValidationService;

        public string UId { get; private set; }

        public TrainModel(TrainManagerService trainManagerService, 
            WagonManagerService wagonManagerService, 
            ILogger<ScheduleModel> logger,
            LoggerService loggerService,
            UserValidationService userValidationService)
        {
            _TrainManagerService = trainManagerService;
            _WagonManagerService = wagonManagerService;
            _logger = logger;
            _loggerService = loggerService;
            _userValidationService = userValidationService;
        }

        public async void OnGet()
        {
            if (await _userValidationService.IsUserInvalid(HttpContext)) {return;}

            UId = "helooo";
        }

    }
}
