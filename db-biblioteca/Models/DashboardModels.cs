using System;
using System.Collections.Generic;

namespace BibliotecaDB.Models
{
    public class AdminDashboardModel
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int TotalBooks { get; set; }
        public int ActiveBooks { get; set; }
        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public int TotalProfiles { get; set; }
        public int ActiveProfiles { get; set; }
        public int TotalRoles { get; set; }
        public int ActiveRoles { get; set; }
        public int TotalMenuItems { get; set; }
        public int ActiveMenuItems { get; set; }
        public int TotalActions { get; set; }
        public int ActiveActions { get; set; }
    }

    public class GeneralDashboardModel
    {
        public int TotalBooks { get; set; }
        public int ActiveBooks { get; set; }
        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public int TotalServices { get; set; }
        public int ActiveServices { get; set; }
        public int TotalSchools { get; set; }
        public int ActiveSchools { get; set; }
    }

    public class DashboardViewModel
    {
        public AdminDashboardModel AdminData { get; set; }
        public GeneralDashboardModel GeneralData { get; set; }
        public string CurrentUserName { get; set; }
        public string CurrentUserRole { get; set; }
        public string CurrentUserProfile { get; set; }
        public DateTime CurrentDate { get; set; }
    }

    public class MenuViewModel
    {
        public MenuItem MenuItem { get; set; }
        public List<Rol> Roles { get; set; }
        public List<Perfil> Profiles { get; set; }
        public List<Accion> AvailableActions { get; set; }
    }
}