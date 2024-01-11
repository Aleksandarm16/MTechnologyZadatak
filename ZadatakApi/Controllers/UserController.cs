using Microsoft.AspNetCore.Mvc;

namespace ZadatakApi.Controllers
{
    [Route("[controller]")]
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
