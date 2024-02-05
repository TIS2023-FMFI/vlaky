using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using code.Models;
using code.Services;
using Microsoft.Net.Http.Headers;
using System.Text;

namespace code.Pages
{
    public class HistoryModel : PageModel
    {
        private readonly TrainManagerService _trainManagerService;
        private readonly ILogger<HistoryModel> _logger;
        private readonly LoggerService _loggerService;
        private readonly UserValidationService _userValidationService;

        public List<DaySchedule> Schedule { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TrainsInState3Count { get; set; }
        public int TrainsInState4Count { get; set; }
        public int TotalTrainsCount { get; set; }

        public HistoryModel(TrainManagerService trainManagerService, 
            ILogger<HistoryModel> logger,
            LoggerService loggerService,
            UserValidationService userValidationService)
        {
            _trainManagerService = trainManagerService;
            _logger = logger;
            _loggerService = loggerService;
            _userValidationService = userValidationService;
        }

        public async Task<IActionResult> OnGet(DateTime? startDate, DateTime? endDate)
        {
            if (await _userValidationService.IsUserInvalid(HttpContext)) {
                return Redirect("/Login");
            }

            Schedule = new List<DaySchedule>();
            StartDate = startDate ?? DateTime.Today;
            EndDate = endDate ?? StartDate.AddDays(7);

            var allTrainsByDate = await _trainManagerService.GetTrainsByDate(StartDate, EndDate);
            TrainsInState3Count = allTrainsByDate.Count(t => t.Status == 3);
            TrainsInState4Count = allTrainsByDate.Count(t => t.Status == 4);
            TotalTrainsCount = TrainsInState3Count + TrainsInState4Count;

            for (DateTime date = StartDate; date <= EndDate; date = date.AddDays(1))
            {
                var daySchedule = new DaySchedule
                {
                    Date = date,
                    Trains = allTrainsByDate
                        .Where(t => t.Date.Date == date.Date && (t.Status == 3 || t.Status == 4))
                        .OrderBy(t => t.Id)
                        .ToList()
                };

                Schedule.Add(daySchedule);
            }

            return Page();
        }

        public string GetTrainStatusImage(int status)
        {
            switch(status)
            {
                case 0: return "~/images/train_naplanovany.svg";
                case 1: return "~/images/train_vpriprave.svg";
                case 2: return "~/images/train_pripraveny.svg";
                case 3: return "~/images/train_expedovany.svg";
                case 4: return "~/images/train_zruseny.svg";
                default: return "~/images/train_default.svg";
            }
        }

        public string EditDateName(string name)
        {
            switch (name.ToLower())
            {
                case "pondelok": return "Pondelok";
                case "utorok": return "Utorok";
                case "streda": return "Streda";
                case "štvrtok": return "Štvrtok";
                case "piatok": return "Piatok";
                case "sobota": return "Sobota";
                case "nedeľa": return "Nedeľa";
                default: return "";
            }
        }

        public async Task<IActionResult> OnPost(DateTime? StartDate, DateTime? EndDate)
        {
            DateTime startDate = StartDate ?? DateTime.Today;
            DateTime endDate = EndDate ?? startDate.AddDays(7);

            int startDay = startDate.Day;
            int startMonth = startDate.Month;
            int startYear = startDate.Year;

            int endDay = endDate.Day;
            int endMonth = endDate.Month;
            int endYear = endDate.Year;

            string fileName = $"CEVA {startDay:D2}.{startMonth:D2}.{startYear} - {endDay:D2}.{endMonth:D2}.{endYear}.csv";

            var allTrainsByDate = await _trainManagerService.GetTrainsByDate(startDate, endDate);

            StringBuilder csvContent = new StringBuilder();
            csvContent.AppendLine("\"datum\",\"nazov vlaku\",\"status vlaku\",\"pocet vagonov\",\"pocet nalozenych vagonov\"");

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                var dayTrains = allTrainsByDate
                    .Where(t => t.Date.Date == date.Date && (t.Status == 3 || t.Status == 4))
                    .OrderBy(t => t.Id)
                    .ToList();

                foreach (var train in dayTrains)
                {
                    var dateString = $"{train.Date.Day:D2}/{train.Date.Month:D2}/{train.Date.Year}";
                    string line = $"\"{dateString}\",\"{train.Name}\",\"{GetTrainStatus(train.Status)}\",\"{train.Wagons.Count}\",\"{train.Wagons.Count(w => w.State == 3)}\"";
                    csvContent.AppendLine(line);
                }
            }

            byte[] csvFileContent = Encoding.UTF8.GetBytes(csvContent.ToString());

            return File(csvFileContent, "text/csv", fileName);
        }

        private string GetTrainStatus(int status)
        {
            switch (status)
            {
                case 3: return "expedovany";
                case 4: return "zruseny";
                default: return "";
            }
        }
    }
}
