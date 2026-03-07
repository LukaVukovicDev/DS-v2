using CoworkingApp.BusinessLogic.Models;
using CoworkingApp.BusinessLogic.Repositories.Interfaces;
using CoworkingApp.BusinessLogic.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoworkingApp.BusinessLogic.Services
{
    public class ReservationService
    {
        private IReservationRepository _reservationRepository;
        private IResourceRepository _resourceRepository;
        private ILocationRepository _locationRepository;
        private IUserRepository _userRepository;

        private List<IReservationValidator> _validators;

        public ReservationService(
            IReservationRepository reservationRepository,
            IResourceRepository resourceRepository,
            ILocationRepository locationRepository,
            IUserRepository userRepository)
        {
            _reservationRepository = reservationRepository;
            _resourceRepository = resourceRepository;
            _locationRepository = locationRepository;
            _userRepository = userRepository;

            // Strategy pattern — build the list of validators
            _validators = new List<IReservationValidator>
            {
                new AvailabilityValidator(_reservationRepository),
                new HoursLimitValidator(_reservationRepository, _userRepository),
                new WorkingHoursValidator(_resourceRepository, _locationRepository)
            };
        }

        public void CreateReservation(Reservation reservation)
        {
            reservation.Status = ReservationStatus.Active;

            foreach(IReservationValidator validator in _validators)
            {
                validator.Validate(reservation);
            }

            _reservationRepository.Add(reservation);

            OnReservationCreated(reservation);
        }

        public void UpdateReservation(Reservation reservation)
        {
            // Run all validators for the updated reservation too
            foreach (IReservationValidator validator in _validators)
            {
                validator.Validate(reservation);
            }

            _reservationRepository.Update(reservation);
            OnReservationUpdated(reservation);
        }

        public void CancelReservation(int id)
        {
            Reservation reservation = _reservationRepository.GetById(id);

            if (reservation == null)
                throw new InvalidOperationException("Reservation not found.");

            if (reservation.Status == ReservationStatus.Cancelled)
                throw new InvalidOperationException("Reservation is already cancelled.");

            _reservationRepository.Cancel(id);
            OnReservationCancelled(reservation);
        }

        public List<Reservation> GetUserReservations(int userId)
        {
            return _reservationRepository.GetByUser(userId);
        }

        public List<Reservation> GetReservationsByDateAndLocation(DateTime date, int locationId)
        {
            return _reservationRepository.GetByDateAndLocation(date, locationId);
        }

        // Observer pattern — events that GUI can subscribe to
        public event Action<Reservation> ReservationCreated;
        public event Action<Reservation> ReservationUpdated;
        public event Action<Reservation> ReservationCancelled;

        private void OnReservationCreated(Reservation reservation)
        {
            ReservationCreated?.Invoke(reservation);
        }

        private void OnReservationUpdated(Reservation reservation)
        {
            ReservationUpdated?.Invoke(reservation);
        }

        private void OnReservationCancelled(Reservation reservation)
        {
            ReservationCancelled?.Invoke(reservation);
        }

    }
}
