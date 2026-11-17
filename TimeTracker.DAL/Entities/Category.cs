using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker.DAL.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Color { get; set; } = "#808080";
        public bool IsProductive { get; set; }

        // קשר: לקטגוריה אחת יש הרבה תוכנות
        public ICollection<TrackedApp> Apps { get; set; } = new List<TrackedApp>();

    }
}
