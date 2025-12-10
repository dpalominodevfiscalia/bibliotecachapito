using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BibliotecaDB.Services;
using BibliotecaDB.Models;
using System.Linq;

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

        protected LayoutViewModel CreateLayoutViewModel()
        {
            return new LayoutViewModel
            {
                OpcionItems = _dataService.GetOpcionItemsForProfile(GetProfileId() ?? 1),
                ModuleNavigation = _dataService.GetModulos(),
                CurrentUser = GetCurrentUser()
            };
        }

        protected bool HasActionPermission(string controller, string actionMethod)
        {
            var profileId = GetProfileId();
            if (!profileId.HasValue) return false;

            var actions = _dataService.GetAcciones();
            var userActions = actions.Where(a =>
                a.Controlador == controller &&
                a.AccionMetodo == actionMethod &&
                a.Estado == "Activo");

            return userActions.Any();
        }

        // Check if a menu item has a specific action type configured
        protected bool HasMenuItemAction(MenuItem menuItem, string actionType)
        {
            if (menuItem?.ActionIds == null || !menuItem.ActionIds.Any())
                return false;

            var actions = _dataService.GetAcciones();
            var menuItemActions = actions.Where(a => menuItem.ActionIds.Contains(a.Id));

            return menuItemActions.Any(a => a.Tipo == actionType && a.Estado == "Activo");
        }

        // Check if current user has permission to perform an action based on Opcion configuration
        protected bool HasOpcionActionPermission(string controllerName, string actionType)
        {
            var profileId = GetProfileId();
            if (!profileId.HasValue) return false;

            // Get menu items for the current profile that match this controller
            var menuItems = _dataService.GetOpcionItemsForProfile(profileId.Value);
            var matchingMenuItems = menuItems.Where(m =>
                m.Url != null &&
                m.Url.Contains($"/{controllerName}/", System.StringComparison.OrdinalIgnoreCase));

            // Check if any matching menu item has the required action
            foreach (var menuItem in matchingMenuItems)
            {
                if (HasMenuItemAction(menuItem, actionType))
                {
                    return true;
                }
            }

            return false;
        }

        // Get current logged-in user information
        protected Usuario GetCurrentUser()
        {
            var profileId = GetProfileId();
            if (!profileId.HasValue) return null;

            var users = _dataService.GetUsuarios();
            return users.FirstOrDefault(u => u.IdPerfil == profileId.Value);
        }

        // Set OpcionItems for the current request
        protected void SetOpcionItems()
        {
            var layoutViewModel = CreateLayoutViewModel();
            HttpContext.Items["LayoutViewModel"] = layoutViewModel;
        }
    }
}