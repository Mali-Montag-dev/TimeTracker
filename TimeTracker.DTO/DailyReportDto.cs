using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker.DTO
{
    public class DailyReportDto
    {
        public DateTime Date {  get; set; }

        public double TotalMinutes { get; set; }

        public double ProductiveMinutes { get; set; }

        public List<AppUsageDto> Apps { get; set; } = new List<AppUsageDto>();
        public List<CategoryUsageDto> Categories { get; set; } = new List<CategoryUsageDto>();

    }
}
