using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BibliotecaDB.Models;
using BibliotecaDB.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace BibliotecaDB.Controllers
{
    public class OpcionController : BaseController
    {
        public OpcionController(IWebHostEnvironment env, DataService dataService) : base(env, dataService)
        {
        }

        // GET: Opcion
        public ActionResult Index()
        {
            var redirect = EnsureLoggedIn();
            if (redirect != null) return redirect;
            SetOpcionItems();

            // Create a dictionary to store action permissions for each menu item
            var menuItems = _dataService.GetOpcionItems();
            var actionPermissions = new Dictionary<int, Dictionary<string, bool>>();

            foreach (var menuItem in menuItems)
            {
                var permissions = new Dictionary<string, bool>();
                permissions["Editar"] = HasMenuItemAction(menuItem, "Editar");
                permissions["Eliminar"] = HasMenuItemAction(menuItem, "Eliminar");
                permissions["Detalles"] = HasMenuItemAction(menuItem, "Editar"); // Details uses Edit permission

                // Toggle permission: allow if user has Desactivar permission OR if they have any other action permission
                bool hasDesactivarPermission = HasMenuItemAction(menuItem, "Desactivar");
                bool hasAnyActionPermission = permissions["Editar"] || permissions["Eliminar"] || permissions["Detalles"];
                permissions["ToggleEstado"] = hasDesactivarPermission || hasAnyActionPermission;

                actionPermissions[menuItem.Id] = permissions;
            }

            ViewBag.ActionPermissions = actionPermissions;
            return View(menuItems);
        }

        // Check if current user has permission to perform an action on a specific menu item
        private bool CanPerformActionOnMenuItem(MenuItem menuItem, string actionType)
        {
            // Check if the menu item has this action configured
            return HasMenuItemAction(menuItem, actionType);
        }

        // GET: Opcion/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var menuItem = _dataService.GetOpcionItemById(id.Value);
            if (menuItem == null)
            {
                return NotFound();
            }

            // Prepare action names for display
            var actionNames = _dataService.GetActionNamesFromIds(menuItem.ActionIds);
            ViewBag.ActionNames = actionNames;

            // Prepare profile names for display
            var profileNames = _dataService.GetProfileNamesFromIds(menuItem.Profiles);
            ViewBag.ProfileNames = profileNames;

            // Prepare role names for display
            var roleNames = _dataService.GetRoleNamesFromIds(menuItem.Roles);
            ViewBag.RoleNames = roleNames;

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DetailsPartial", menuItem);
            }
            return View(menuItem);
        }

        // GET: Opcion/Create
        public ActionResult Create()
        {
            var viewModel = new MenuViewModel
            {
                MenuItem = new MenuItem(),
                Roles = _dataService.GetRoles(),
                Profiles = _dataService.GetPerfiles(),
                AvailableActions = _dataService.GetAcciones()
            };

            // Add modules for module selection using ViewBag for SelectList
            ViewBag.Modulos = new SelectList(_dataService.GetModulos(), "Id", "Nombre");

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial", viewModel);
            }
            return View(viewModel);
        }

        // POST: Opcion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id,Title,Url,Icon,Estado,ModuloId")] MenuItem menuItem, string[] selectedProfiles, string[] selectedRoles, string[] selectedActions)
        {
            if (ModelState.IsValid)
            {
                if (selectedProfiles != null)
                {
                    menuItem.Profiles = new List<int>();
                    foreach (var profileId in selectedProfiles)
                    {
                        if (int.TryParse(profileId, out var id))
                        {
                            menuItem.Profiles.Add(id);
                        }
                    }
                }

                if (selectedRoles != null)
                {
                    menuItem.Roles = new List<int>();
                    foreach (var roleId in selectedRoles)
                    {
                        if (int.TryParse(roleId, out var id))
                        {
                            menuItem.Roles.Add(id);
                        }
                    }
                }

                if (selectedActions != null)
                {
                    menuItem.ActionIds = new List<int>();
                    foreach (var actionId in selectedActions)
                    {
                        if (int.TryParse(actionId, out var id))
                        {
                            menuItem.ActionIds.Add(id);
                        }
                    }
                }

                _dataService.AddOpcionItem(menuItem);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Elemento de opción creado exitosamente" });
                }
                return RedirectToAction("Index");
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial", menuItem);
            }
            return View(menuItem);
        }

        // GET: Opcion/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var menuItem = _dataService.GetOpcionItemById(id.Value);
            if (menuItem == null)
            {
                return NotFound();
            }

            var viewModel = new MenuViewModel
            {
                MenuItem = menuItem,
                Roles = _dataService.GetRoles(),
                Profiles = _dataService.GetPerfiles(),
                AvailableActions = _dataService.GetAcciones()
            };

            // Add modules for module selection using ViewBag for SelectList
            ViewBag.Modulos = new SelectList(_dataService.GetModulos(), "Id", "Nombre", menuItem.ModuloId);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", viewModel);
            }
            return View(viewModel);
        }

        // POST: Opcion/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,Title,Url,Icon,Estado,ModuloId")] MenuItem menuItem, string[] selectedProfiles, string[] selectedRoles, string[] selectedActions)
        {
            if (ModelState.IsValid)
            {
                if (selectedProfiles != null)
                {
                    menuItem.Profiles = new List<int>();
                    foreach (var profileId in selectedProfiles)
                    {
                        if (int.TryParse(profileId, out var id))
                        {
                            menuItem.Profiles.Add(id);
                        }
                    }
                }

                if (selectedRoles != null)
                {
                    menuItem.Roles = new List<int>();
                    foreach (var roleId in selectedRoles)
                    {
                        if (int.TryParse(roleId, out var id))
                        {
                            menuItem.Roles.Add(id);
                        }
                    }
                }

                if (selectedActions != null)
                {
                    menuItem.ActionIds = new List<int>();
                    foreach (var actionId in selectedActions)
                    {
                        if (int.TryParse(actionId, out var id))
                        {
                            menuItem.ActionIds.Add(id);
                        }
                    }
                }

                //_dataService.UpdateOpcion(menuItem);
                _dataService.UpdateOpcionItem(menuItem);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Elemento de opción actualizado exitosamente" });
                }
                return RedirectToAction("Index");
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EditPartial", menuItem);
            }
            return View(menuItem);
        }

        // GET: Opcion/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var menuItem = _dataService.GetOpcionItemById(id.Value);
            if (menuItem == null)
            {
                return NotFound();
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_DeletePartial", menuItem);
            }
            return View(menuItem);
        }

        // POST: Opcion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            _dataService.DeleteOpcionItem(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, message = "Elemento de opción eliminado exitosamente" });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ToggleEstado(int id)
        {
            _dataService.ToggleOpcionItemEstado(id);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }

        // GET: Opcion/BulkAssign
        public ActionResult BulkAssign()
        {
            ViewBag.OpcionItems = new SelectList(_dataService.GetOpcionItems(), "Id", "Title");
            ViewBag.IdRol = new SelectList(_dataService.GetRoles(), "Id", "Nombre");
            ViewBag.IdPerfil = new SelectList(_dataService.GetPerfiles(), "Id", "Nombre");
            return View();
        }

        // POST: Opcion/BulkAssign
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> BulkAssign(int menuId, int? rolId, int? perfilId, string[] selectedActions)
        {
            if (ModelState.IsValid)
            {
                _dataService.BulkAssignOpcionActions(menuId, rolId, perfilId, selectedActions);

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Acciones de menú asignadas exitosamente" });
                }
                return RedirectToAction("Index");
            }

            ViewBag.OpcionItems = new SelectList(_dataService.GetOpcionItems(), "Id", "Title", menuId);
            ViewBag.IdRol = new SelectList(_dataService.GetRoles(), "Id", "Nombre", rolId);
            ViewBag.IdPerfil = new SelectList(_dataService.GetPerfiles(), "Id", "Nombre", perfilId);
            return View();
        }
    }
}