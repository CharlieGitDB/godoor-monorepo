using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [Route("")]
    public class SwaggerRedirectController : Controller
    {
        [Route(""), HttpGet]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Swagger()
        {
            return Redirect("~/swagger");
        }
    }
}