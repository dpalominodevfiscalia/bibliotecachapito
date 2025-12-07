using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using BibliotecaDB.Services;
using BibliotecaDB.Models;
using System;
using System.Security.Claims;

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
            SetOpcionItems();

            // Check if user is admin (profile 1) or regular user
            var profileId = GetProfileId();
            if (profileId == 1) // Admin profile
            {
                var adminCounters = _dataService.GetAdminDashboardCounters();
                var generalCounters = _dataService.GetGeneralDashboardCounters();

                var viewModel = new DashboardViewModel
                {
                    AdminData = adminCounters,
                    GeneralData = generalCounters,
                    CurrentUserName = User.Identity?.Name,
                    CurrentUserRole = User.FindFirstValue("RoleName"),
                    CurrentUserProfile = User.FindFirstValue("ProfileName"),
                    CurrentDate = DateTime.Now
                };

                return View("AdminDashboard", viewModel);
            }
            else
            {
                var generalCounters = _dataService.GetGeneralDashboardCounters();

                var viewModel = new DashboardViewModel
                {
                    GeneralData = generalCounters,
                    CurrentUserName = User.Identity?.Name,
                    CurrentUserRole = User.FindFirstValue("RoleName"),
                    CurrentUserProfile = User.FindFirstValue("ProfileName"),
                    CurrentDate = DateTime.Now
                };

                return View("GeneralDashboard", viewModel);
            }
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

        // Method to create admin opcion items with all actions
        public IActionResult CreateAdminOpcionItems()
        {
            _dataService.CreateAdminOpcionItems();
            return RedirectToAction("Index");
        }
    }
}