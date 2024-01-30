using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using code.Services;
using code.Models;
using System.Linq;
using System.Security.Claims;

namespace code.Pages
{
    public class IndexModel : PageModel
    {
        private readonly BlackBoardService _blackBoardService;
        private readonly AccountManagerService _accountManagerService;
        private readonly ILogger<IndexModel> _logger;
        private readonly LoggerService _loggerService;
        private readonly UserValidationService _userValidationService;

        public List<BlackBoardNote> Notes { get; set; }

        public string UserName { get; private set; }
        public int UserId { get; private set; }
        public bool IsManager {get; private set;}
        public bool HasAddRights {get; private set;}

        public IndexModel(ILogger<IndexModel> logger,
            BlackBoardService blackBoardService,
            AccountManagerService accountManagerService, 
            LoggerService loggerService,
            UserValidationService userValidationService)
        {
            _logger = logger;
            _accountManagerService = accountManagerService;
            _blackBoardService = blackBoardService;
            _loggerService = loggerService;
            _userValidationService = userValidationService;
            Notes = new List<BlackBoardNote>();
        }

        public async Task OnGetAsync()
        {
            if (await _userValidationService.IsUserInvalid(HttpContext))
            {
                return;
            }
            else
            {
                UserName = User.FindFirst(ClaimTypes.Name)?.Value;
                UserId = Convert.ToInt32(HttpContext.User.FindFirst("Id").Value);
                int priv = Convert.ToInt32(HttpContext.User.FindFirst("Privileges").Value);
                IsManager = (priv >> 1) % 2 == 1;
                HasAddRights = (priv >> 6) % 2 == 1;
            }

            var noteId = HttpContext.Request.Query["noteId"].ToString();
            if (!string.IsNullOrEmpty(noteId))
            {
                var note = await _blackBoardService.GetNoteById(Convert.ToInt32(noteId));
                await _blackBoardService.RemoveNoteById(Convert.ToInt32(noteId));
                _loggerService.writeCommDelete(HttpContext, note);
            }

            var blackBoardNotes = await _blackBoardService.GetNotes();
            Notes = blackBoardNotes
                .Select(note => new BlackBoardNote
                {
                    Id = note.Id,
                    Priority = note.Priority,
                    Title = note.Title,
                    Text = note.Text,
                    Date = note.Date,
                    UserId = note.UserId
                })
                .OrderBy(note => note.Priority)
                .ToList();
        }

        public async Task<string> GetAccountName(int userId)
        {
            var accountName = await _accountManagerService.GetUserNameById(userId);
            if (accountName!= null)
            {
                return accountName;
            }
            return "vymazaný používateľ";
        }
    }
}
