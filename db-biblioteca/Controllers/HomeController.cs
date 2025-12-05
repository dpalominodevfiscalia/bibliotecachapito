using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using BibliotecaDB.Services;

namespace BibliotecaDB.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IWebHostEnvironment env, DataService dataService) : base(env, dataService)
        {
        }

        public IActionResult Index()
        {
            var redirect = EnsureLoggedIn();
            if (redirect != null) return redirect;
            SetMenuItems();
            return View();
        }

        public IActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}