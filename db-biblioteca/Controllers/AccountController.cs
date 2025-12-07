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
            HttpContext.Response.Cookies.Delete("ProfileId");
            return RedirectToAction("Login", "Account");
        }
    }
}