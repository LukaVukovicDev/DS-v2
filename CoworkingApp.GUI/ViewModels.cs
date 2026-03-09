using System;
using CoworkingApp.BusinessLogic.Models;

namespace CoworkingApp.GUI
{
    public class ResourceGridItem
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Details { get; set; }
        public bool IsAvailable { get; set; }
        public Resource Source { get; set; }
    }

    public class ReservationGridItem
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ResourceId { get; set; }
        public string UserDisplay { get; set; }
        public string ResourceDisplay { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Status { get; set; }
        public Reservation Source { get; set; }
    }
}
