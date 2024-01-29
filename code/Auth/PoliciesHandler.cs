using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace code.Auth
{
    public class AuthRequirementHandler : AuthorizationHandler<AuthRequirement>
    {
        protected override Task HandleRequirementAsync
            (AuthorizationHandlerContext context, 
            AuthRequirement requirement)
        {
            var claim = context.User.FindFirst(
                c => c.Type == "Privileges"
            );

            if(claim == null)
            {
                return Task.CompletedTask;
            }

            int privileges = Convert.ToInt32(claim.Value);

            if ((privileges >> requirement.WitchBit) % 2 == 1) {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }

        public static bool isBitSet(int privileges, int bit) 
        {
            return (privileges >> bit) % 2 == 1;
        }

        public static bool isBitSet(string value, int bit)
        {
            return isBitSet(Convert.ToInt32(value), bit);
        }

        public static bool isBitSet(Claim ?claim, int bit)
        {
            if(claim == null)
            {
                throw new InvalidDataException();
            }

            return isBitSet(claim.Value, bit);
        }

        public static int setBit(int privileges, int bit) 
        {
            return privileges + (int)Math.Pow(2, bit);
        }
    }
}