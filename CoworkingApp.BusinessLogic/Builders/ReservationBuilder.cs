using System;
using CoworkingApp.BusinessLogic.Models;

namespace CoworkingApp.BusinessLogic.Builders
{
    public class ReservationBuilder
    {
        private Reservation _reservation;

        public ReservationBuilder()
        {
            _reservation = new Reservation();
            _reservation.Status = ReservationStatus.Active;
        }

        public ReservationBuilder ForUser(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("UserId must be a positive number.");
            _reservation.UserId = userId;
            return this;
        }

        public ReservationBuilder ForResource(int resourceId)
        {
            if (resourceId <= 0)
                throw new ArgumentException("ResourceId must be a positive number.");
            _reservation.ResourceId = resourceId;
            return this;
        }

        public ReservationBuilder From(DateTime startDateTime)
        {
            if (startDateTime < DateTime.Now)
                throw new ArgumentException("Start time cannot be in the past.");
            _reservation.StartDateTime = startDateTime;
            return this;
        }

        public ReservationBuilder To(DateTime endDateTime)
        {
            _reservation.EndDateTime = endDateTime;
            return this;
        }

        public Reservation Build()
        {
            // Validate all required fields are set
            if (_reservation.UserId == 0)
                throw new InvalidOperationException("User must be set before building.");

            if (_reservation.ResourceId == 0)
                throw new InvalidOperationException("Resource must be set before building.");

            if (_reservation.StartDateTime == default)
                throw new InvalidOperationException("Start time must be set before building.");

            if (_reservation.EndDateTime == default)
                throw new InvalidOperationException("End time must be set before building.");

            if (_reservation.EndDateTime <= _reservation.StartDateTime)
                throw new InvalidOperationException("End time must be after start time.");

            return _reservation;
        }
    }
}