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

        public string UId { get; private set; }


        public TrainModel(TrainManagerService trainManagerService, WagonManagerService wagonManagerService, ILogger<ScheduleModel> logger)
        {
            _TrainManagerService = trainManagerService;
            _WagonManagerService = wagonManagerService;
            _logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            UId = "helooo";


            return Page();
        }

    }
}
