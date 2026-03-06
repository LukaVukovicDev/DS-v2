using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoworkingApp.BusinessLogic.Models
{
    public class MembershipType
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int DurationDays { get; set; }
        public int MaxReservationHoursPerMonth { get; set; }
        public bool IncludesMeetingRooms { get; set; }
        public int MeetingRoomHoursPerMonth { get; set; }
    }
}
