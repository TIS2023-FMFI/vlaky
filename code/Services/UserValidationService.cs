using System.Security.Claims;
using code.Auth;
using Microsoft.AspNetCore.Authentication;

namespace code.Services{
    public class UserValidationService{
        private static int COOKIE_EXPIRATION_TIME = 20;

        public static string DEFAULT_LOGIN_PATH = "/Login";
        public static string DEFAULT_ACCESS_DENIED_PATH = "/Forbidden";

        private Dictionary<int, DateTime> ChangeTime = new Dictionary<int, DateTime>();

        public UserValidationService()
        {

        }

        public static void ConfigureServise(ConfigurationManager config)
        {
            var authConfig = config.GetSection("AuthenticationOptions");
            string CookieTime = authConfig["CookieExpirationTime"] ?? "20";
            COOKIE_EXPIRATION_TIME = Convert.ToInt32(CookieTime);
            DEFAULT_LOGIN_PATH = authConfig["DefaultLoginPath"] ?? DEFAULT_LOGIN_PATH;
            DEFAULT_ACCESS_DENIED_PATH = authConfig["DefaultAccessDeniedPath"] ?? DEFAULT_ACCESS_DENIED_PATH;
        }

        public void AddChange(int id)
        {
            ChangeTime.Add(id, DateTime.Now);
        }

        public bool UserChanged(int id, DateTime cookieDate)
        {
            if (!ChangeTime.Keys.Contains(id))
            {
                return false;
            }
            if ((cookieDate - ChangeTime[id]).TotalMinutes < 0) 
            {
                return true;
            }
            if ((cookieDate - ChangeTime[id]).TotalMinutes > COOKIE_EXPIRATION_TIME * 3) 
            {
                ChangeTime.Remove(id);
                return false;
            }
            return false;
        }

        public async void ValidateUser(HttpContext context)
        {
            if (context.User == null ||
                context.User.Identity == null ||
                !context.User.Identity.IsAuthenticated)
            {
                await context.ChallengeAsync();
                return;
            }

            int id = Convert.ToInt32(context.User.FindFirst("Id").Value);
            int privileges = Convert.ToInt32(context.User.FindFirst("Privileges").Value);
            DateTime created = DateTime.Parse(context.User.FindFirst("TimeCreated").Value);
            if (UserChanged(id, created))
            {
                await context.SignOutAsync();
                await context.ChallengeAsync();
                return;
            }

            if (!AuthRequirementHandler.isBitSet(privileges, 0))
            {
                await context.ForbidAsync();
            }
        }
    }
}