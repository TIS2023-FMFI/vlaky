using Microsoft.AspNetCore.Authorization;

namespace code.Auth
{
    public class AuthRequirement : IAuthorizationRequirement
    {
        public int WitchBit {get;}

        public AuthRequirement(int witchBit) =>
            WitchBit = witchBit;
    }
}