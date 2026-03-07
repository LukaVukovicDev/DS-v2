using System;

namespace CoworkingApp.BusinessLogic.Models
{
    public enum ReservationStatus
    {
        Active,
        Completed,
        Cancelled
    }

    public class Reservation
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ResourceId { get; set; }
        public Resource Resource { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public ReservationStatus Status { get; set; }

        public double DurationHours => (EndDateTime - StartDateTime).TotalHours;
    }
}
