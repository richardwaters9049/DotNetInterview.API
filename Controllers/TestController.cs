using Microsoft.AspNetCore.Mvc;

namespace DotNetInterview.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("API is working!");
    }
}