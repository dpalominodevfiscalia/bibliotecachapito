using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BibliotecaDB.Models;
using BibliotecaDB.Services;

namespace BibliotecaDB.Controllers
{
    public class AccountController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly DataService _dataService;

        public AccountController(IWebHostEnvironment env, DataService dataService)
        {
            _env = env;
            _dataService = dataService;
        }
        // GET: Account/Login
        public ActionResult Login()
        {
            // Always prevent caching for login page to ensure fresh load
            Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
            Response.Headers.Add("Pragma", "no-cache");
            Response.Headers.Add("Expires", "0");

            // Clear any existing authentication if user somehow got here while logged in
            if (HttpContext.Request.Cookies["ProfileId"] != null)
            {
                HttpContext.Response.Cookies.Delete("ProfileId");
                HttpContext.Session.Clear();
            }

            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Usuario model)
        {
            if (ModelState.IsValid)
            {
                var users = _dataService.GetUsuarios();
                var user = users.FirstOrDefault(u => u.NombreUsuario == model.NombreUsuario && u.Contraseña == model.Contraseña);
                if (user != null)
                {
                    HttpContext.Response.Cookies.Append("ProfileId", user.IdPerfil.ToString());
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Nombre de usuario o contraseña inválidos");
                }
            }
            return View(model);
        }

        // POST: Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            // Clear authentication cookie
            HttpContext.Response.Cookies.Delete("ProfileId");


            // Check if this is an AJAX request
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Ok(new { success = true });
            }

            // For non-AJAX requests, redirect to login
            return RedirectToAction("Login", "Account");
        }
    }
}