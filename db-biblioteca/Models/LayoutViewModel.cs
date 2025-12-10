using System;
using System.Collections.Generic;

namespace BibliotecaDB.Models
{
    public class LayoutViewModel
    {
        public List<MenuItem> OpcionItems { get; set; }
        public List<Modulo> ModuleNavigation { get; set; }
        public Usuario CurrentUser { get; set; }
    }
}