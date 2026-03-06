using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoworkingApp.BusinessLogic.Models
{
    public class MeetingRoom : Resource
    {
        public int Capacity { get; set; }
        public bool HasProjector { get; set; }
        public bool HasTV { get; set; }
        public bool HasWhiteboard { get; set; }
        public bool HasOnlineMeetingEquipment { get; set; }
    }
}
