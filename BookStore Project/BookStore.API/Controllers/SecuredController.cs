using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookStore.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/secured")]
    public class SecuredController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetSecuredData()
        {
            await Task.Delay(1000);

            return Ok("This secured data is available only for authenticated users.");
        }
    }
}
