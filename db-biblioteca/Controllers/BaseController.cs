using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BibliotecaDB.Services;

namespace BibliotecaDB.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IWebHostEnvironment _env;
        protected readonly DataService _dataService;

        public BaseController(IWebHostEnvironment env, DataService dataService)
        {
            _env = env;
            _dataService = dataService;
        }

        protected int? GetProfileId()
        {
            if (HttpContext.Request.Cookies.TryGetValue("ProfileId", out var profileIdStr))
            {
                if (int.TryParse(profileIdStr, out var profileId))
                {
                    return profileId;
                }
            }
            return null;
        }

        protected ActionResult EnsureLoggedIn()
        {
            if (GetProfileId() == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return null;
        }

        protected void SetMenuItems()
        {
            ViewBag.MenuItems = _dataService.GetMenuItemsForProfile(GetProfileId() ?? 1);
        }
    }
}