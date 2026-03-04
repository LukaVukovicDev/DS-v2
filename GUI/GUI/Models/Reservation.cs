using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string ResourceName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Status { get; set; }

    }
}
