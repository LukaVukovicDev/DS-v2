using CoworkingApp.BusinessLogic.Models;
using CoworkingApp.BusinessLogic.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoworkingApp.BusinessLogic.Validators
{
    public class WorkingHoursValidator : IReservationValidator
    {
        private ILocationRepository _locationRepository;
        private IResourceRepository _resourceRepository;

        public WorkingHoursValidator(IResourceRepository resourceRepository, ILocationRepository locationRepository)
        {
            _resourceRepository = resourceRepository;
            _locationRepository = locationRepository;
        }

        public void Validate(Reservation reservation)
        {
            Resource resource = _resourceRepository.GetById(reservation.ResourceId);
            Location location = _locationRepository.GetById(resource.LocationId);

            TimeSpan reservationStart = reservation.StartDateTime.TimeOfDay;
            TimeSpan reservationEnd = reservation.EndDateTime.TimeOfDay;

            if(reservationStart < location.OpneTime)
            {
                throw new InvalidOperationException(
                    $"Reservation starts at {reservationStart} but location" +
                    $" {location.Name} opens at {location.OpneTime}");
            }

            if( reservationEnd > location.CloseTime)
            {
                throw new InvalidOperationException(
                    $"Reservation ends at {reservationEnd} but location " +
                    $"{location.Name} closes at {location.CloseTime}");
            }
            if (reservation.StartDateTime.Date != reservation.EndDateTime.Date)
            {
                throw new InvalidOperationException(
                    "Reservation cannot span multiple days."
                );
            }

        }
    }
}
