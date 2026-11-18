using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker.DTO
{
    public class AppUsageDto
    {
        public int AppId { get; set; }

        public string DisplayName { get; set; } = string.Empty;

        public string? CategoryName { get; set; }

        public double Minutes { get; set; }

    }
}
