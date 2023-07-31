using Microsoft.AspNetCore.Authorization;

namespace OneINc.Web.Common.Auth.Basic.Attributes
{
    public class BasicAuthorizationAttribute : AuthorizeAttribute 
    {
        public BasicAuthorizationAttribute() 
        {
            AuthenticationSchemes = BasicAuthenticationDefaults.AuthenticationScheme;
        }
    }
}
