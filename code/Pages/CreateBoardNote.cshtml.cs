using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using code.Services;
using code.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System;
using System.Security.Claims;

namespace code.Pages
{
    public class CreateBoardNoteModel : PageModel
    {
        private readonly ILogger<CreateBoardNoteModel> _logger;
        private readonly LoggerService _loggerService;
        private readonly UserValidationService _userValidationService;
        private readonly BlackBoardService _blackBoardService;

        [BindProperty]
        public string Title { get; set; }

        [BindProperty]
        public int Priority { get; set; }

        [BindProperty]
        public string Content { get; set; }

        public string ErrorMessage { get; set; }

        public string UId { get; private set; }

        public CreateBoardNoteModel(BlackBoardService blackBoardService, 
            ILogger<CreateBoardNoteModel> logger, 
            LoggerService loggerService,
            UserValidationService userValidationService)
        {
            _blackBoardService = blackBoardService;
            _logger = logger;
            _loggerService = loggerService;
            _userValidationService = userValidationService;
        }

        public async void OnGetAsync(int? noteId)
        {
            _userValidationService.ValidateUser(HttpContext);

            ErrorMessage = null;

            if (!ModelState.IsValid) 
            {
                return;
            }

            if (noteId.HasValue && noteId != null)
            {
                var note = await _blackBoardService.GetNoteById(noteId.Value);

                if (note != null)
                {
                    Title = note.Title;
                    Priority = note.Priority;
                    Content = note.Text;
                }
            }
        }

        public async void OnPostAsync(string Title, int Priority, string Content)
        {
            _userValidationService.ValidateUser(HttpContext);

            UId = User.FindFirst("Id")?.Value;

            if (!ModelState.IsValid) 
            {
                return;
            }

            if (string.IsNullOrEmpty(Title) || string.IsNullOrEmpty(Content)) 
            {
                ErrorMessage = "Vyplňte všetky polia.";
                return;
            }

            var noteId = HttpContext.Request.Query["noteId"].ToString();
            var newNote = new BlackBoardNote
            {
                Title = Title,
                Text = Content,
                Priority = Priority,
                UserId = Convert.ToInt32(UId),
                Date = DateTime.Now
            };

            if (!string.IsNullOrEmpty(noteId))
            {
                newNote.Id = Convert.ToInt32(noteId);
                await _blackBoardService.EditNote(newNote);
                _loggerService.writeCommChange(HttpContext, newNote);
            }
            else
            {
                await _blackBoardService.AddNote(newNote);
                _loggerService.writeCommNew(HttpContext, newNote);
            }
            Redirect("/Index");
        }
    }
}
