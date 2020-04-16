using Microsoft.AspNetCore.Mvc;

namespace Portal.Website.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        [HttpGet("isauthor")]
        public bool IsAuthor()
        {
            var tmp = User.IsInRole("Author");
            return User.IsInRole("Author");
        }
    }
}