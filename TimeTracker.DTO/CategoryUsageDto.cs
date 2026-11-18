using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker.DTO
{
    public class CategoryUsageDto
    {
        public int? CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Color { get; set; } = "#808080";

        public bool IsProductive { get; set; }

        public double Minutes { get; set; }
    }
}
