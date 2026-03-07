using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoworkingApp.BusinessLogic.Models
{
    public class LocationStats
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public int TotalResources { get; set; }
        public int CurrentlyReserved { get; set; }
        public double OccupancyPercentage => TotalResources == 0 ? 0 : (double)CurrentlyReserved / TotalResources * 100;
    }
}
