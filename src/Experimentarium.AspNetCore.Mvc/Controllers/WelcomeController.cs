using Microsoft.AspNetCore.Mvc;

namespace Experimentarium.AspNetCore.Mvc.Controllers
{
    public class WelcomeController : Controller
    {
        // This action will not be executed via '/welcome' route, as request will be handled by  WelcomePage middleware:
        // there is a app.UseWelcomePage in Configure methos in Startup.cs
        //
        // But '/welcome/index' works OK as expected
        public IActionResult Index()
        {
            return View();
        }
    }
}