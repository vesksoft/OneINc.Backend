using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OneINc.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public async Task<IActionResult> CheckLogin() 
        {
            return Ok();
        }
    }
}
