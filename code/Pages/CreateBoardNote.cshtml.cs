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
            private readonly BlackBoardService _blackBoardService;
            private readonly ILogger<CreateBoardNoteModel> _logger;

            [BindProperty]
            public string Title { get; set; }

            [BindProperty]
            public int Priority { get; set; }

            [BindProperty]
            public string Content { get; set; }

            public string ErrorMessage { get; set; }

            public string UId { get; private set; }

            public CreateBoardNoteModel(BlackBoardService blackBoardService, ILogger<CreateBoardNoteModel> logger)
            {
                _blackBoardService = blackBoardService;
                _logger = logger;
            }

            public async Task<IActionResult> OnGetAsync(int? noteId)
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

                return Page();
            }

            public async Task<IActionResult> OnPostAsync(string Title, int Priority, string Content)
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return Redirect("/Login");
                }
                else
                {
                    UId = User.FindFirst("Id")?.Value;
                }

                if (!ModelState.IsValid) 
                {
                    return Page();
                }

                if (string.IsNullOrEmpty(Title) || string.IsNullOrEmpty(Content)) 
                {
                    ErrorMessage = "Vyplňte všetky polia.";
                    return Page();
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
                }
                else
                {
                    await _blackBoardService.AddNote(newNote);
                }

                return RedirectToPage("/Index");
            }
        }
    }
