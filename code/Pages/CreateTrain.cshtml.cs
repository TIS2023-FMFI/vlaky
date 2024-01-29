using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using code.Models;
using code.Services;
using System.Security.Claims;

namespace code.Pages
{
    public class CreateTrainModel : PageModel
    {
        private readonly TrainManagerService _TrainManagerService;
        private readonly TemplateManagerService _TemplateManagerService;
        private readonly ILogger<CreateTrainModel> _logger;
        private readonly LoggerService _loggerService;


        [BindProperty]
        public string Template { get; set; }

        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string Destination { get; set; }

        [BindProperty]
        public double Maxlength { get; set; }

        [BindProperty]
        public double RealLength { get; set; }

        [BindProperty]
        public int WagonCount { get; set; }

        [BindProperty]
        public DateTime Date { get; set; }

        [BindProperty]
        public bool Coll { get; set; }

        [BindProperty]
        public bool SaveTemplate { get; set; }

        [BindProperty]
        public string TrainNote { get; set; }

        public string ErrorMessage { get; set; }

        public string UId { get; private set; }

        public List<TrainTemplate> Templates { get; set; }


        public CreateTrainModel(TrainManagerService trainManagerService, ILogger<CreateTrainModel> logger, LoggerService loggerService, TemplateManagerService templateManagerService)
        {
            _TrainManagerService = trainManagerService;
            _TemplateManagerService = templateManagerService;
            _logger = logger;
            _loggerService = loggerService;
        }

        [HttpGet]
        public async Task<IActionResult> OnGet(int? trainId)
        {
            ErrorMessage = null;

            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Redirect("/Login");
            }

            if (!ModelState.IsValid) 
            {
                return Page();
            }

            if (trainId.HasValue || trainId != null) 
            {
                var train = await _TrainManagerService.GetTrainById(trainId.Value);

                if (train == null) 
                {
                    return Redirect("/Schedule");
                }

                Name = train.Name;
                Destination = train.Destination;
                Maxlength = train.MaxLength;
                RealLength = train.Lenght;
                WagonCount = train.Wagons.Count;
                Date = train.Date;
                Coll = train.Coll;

                var note = await _TrainManagerService.GetTrainNoteByTrainId(trainId.Value);

                if (note == null)
                {
                    TrainNote = "";
                }
                else
                {
                    TrainNote = note.Text;
                }
                
            }
            else 
            {
                Date = DateTime.Now;
            }


            Templates = await _TemplateManagerService.GetTemplates();
            if (Templates == null)
            {
                Templates = new List<TrainTemplate>();
            }


            return Page();
        }

        public async Task<IActionResult> OnPost()
        {   
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Redirect("/Login");
            }
            else 
            {
                UId = User.FindFirst("Id")?.Value;
            }

            Templates = await _TemplateManagerService.GetTemplates();
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Destination)) 
            {
                ErrorMessage = "Vyplňte názov aj destináciu .";
                return Page();
            }

            var trainId = HttpContext.Request.Query["trainId"].ToString();
            
            var newTrain = new Train
            {
                Name = this.Name,
                Destination = this.Destination,
                MaxLength = this.Maxlength,
                Lenght = this.RealLength,
                Date = this.Date,
                Status = 0,
                Coll = this.Coll,
                nWagons = this.WagonCount
            };

            if (!string.IsNullOrEmpty(trainId))
            {
                newTrain.Id = Convert.ToInt32(trainId);
                var currentTrain = await _TrainManagerService.GetTrainById(newTrain.Id);
                
                if (currentTrain == null) 
                {
                    return Redirect("/Schedule");
                }           

                newTrain.Status = currentTrain.Status;
                await _TrainManagerService.UpdateTrain(newTrain);

                var nn = await _TrainManagerService.GetTrainNoteByTrainId(newTrain.Id);
                nn.Text = TrainNote;
                nn.UserId = Convert.ToInt32(UId);
                
                if (nn.Text == null) {
                    nn.Text = "";
                }
                
                await _TrainManagerService.UpdateTrainNote(nn);
                
                _loggerService.writeTrainChange(HttpContext, currentTrain, newTrain);
                _loggerService.writeCommChange(HttpContext, nn);

            }
            else
            {
                var idd = await _TrainManagerService.AddTrain(newTrain);

                _loggerService.writeTrainNew(HttpContext, newTrain);

                var nn = new TrainNote
                {
                    TrainId = idd,
                    UserId = Convert.ToInt32(UId),
                    Text = TrainNote
                };
                if (nn.Text == null)
                    {
                        nn.Text = "";
                    }else{_loggerService.writeCommNew(HttpContext, nn);}

                await _TrainManagerService.AddTrainNote(nn);
            }

            if(this.SaveTemplate)
            {
                TrainTemplate t = new TrainTemplate{
                    Name = this.Name,
                    Destination = this.Destination
                };

                if (Templates == null) {
                    Templates = new List<TrainTemplate>();
                }

                var flag = false;

                foreach (var tt in Templates) {
                    if (tt.Name == this.Name) {
                        flag = true;
                        break;
                    }
                }
                if (!flag) {
                    await _TemplateManagerService.AddTemplate(t);
                }
            }

            if (!string.IsNullOrEmpty(trainId)) 
            {
                Console.WriteLine(trainId);
                string path = "/Train?trainId=" + trainId;
                return Redirect(path);
            }

            return Redirect("/Schedule");
        }
    }
}
