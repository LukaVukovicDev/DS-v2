using CoworkingApp.BusinessLogic.Models;
using CoworkingApp.BusinessLogic.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoworkingApp.BusinessLogic.Validators
{
    public class AvailabilityValidator : IReservationValidator
    {
        private IReservationRepository _reservationRepository;

        public AvailabilityValidator(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public void Validate(Reservation reservation)
        {
            List<Reservation> activeReservations = _reservationRepository.GetActiveByResource(reservation.ResourceId);

            foreach(Reservation existing in activeReservations)
            {
                if (existing.Id == reservation.Id)
                    continue;

                bool overlaps = reservation.StartDateTime < existing.EndDateTime && reservation.EndDateTime > existing.StartDateTime;

                if (overlaps)
                {
                    throw new InvalidOperationException($"Resource is already reserved from {existing.StartDateTime} to {existing.EndDateTime}.");
                }
            }
        }
    }
}
