using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneINc.Web.Common.Auth.Basic.Attributes;

namespace OneINc.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [BasicAuthorization]
    public class EncodingController : ControllerBase
    {
        [HttpPost("encode")]
        public async Task<IActionResult> EncodeAsync([FromBody] string input)
        {
            return Ok();
        }
    }
}
