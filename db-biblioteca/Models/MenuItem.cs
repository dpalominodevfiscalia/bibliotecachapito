using System.Collections.Generic;

namespace BibliotecaDB.Models
{
    public class MenuItem
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public List<int> Profiles { get; set; }
    }
}