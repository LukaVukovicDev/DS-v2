using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoworkingApp.BusinessLogic.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string WorkingHours { get; set; }
        public int MaxCapacity { get; set; }
        public string Description { get; set; }
        public TimeSpan OpneTime => TimeSpan.Parse(WorkingHours.Split('-')[0]);
        public TimeSpan CloseTime => TimeSpan.Parse(WorkingHours.Split('-')[1]);
    }
}
