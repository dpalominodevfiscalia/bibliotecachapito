using System.Collections.Generic;

namespace BibliotecaDB.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public List<int> Profiles { get; set; }
        public List<int> Roles { get; set; } // Add roles support
        public List<int> ActionIds { get; set; } // Store action IDs instead of action type strings
        public string Estado { get; set; } = "Activo";

        // Module assignment for MenuItem
        public int ModuloId { get; set; }
        public virtual Modulo Modulo { get; set; }

        public MenuItem()
        {
            Profiles = new List<int>();
            Roles = new List<int>();
            ActionIds = new List<int>();
        }
    }
}