using Microsoft.AspNetCore.Authorization;

namespace ErgoNames.Api.Security
{
    public class BasicAuthorizationAttribute : AuthorizeAttribute
    {
        public BasicAuthorizationAttribute()
        {
            Policy = "BasicAuthentication";
        }
    }
}
