using CoworkingApp.BusinessLogic.Models;
using CoworkingApp.BusinessLogic.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoworkingApp.BusinessLogic.Validators
{
    public class HoursLimitValidator : IReservationValidator
    {
        private IReservationRepository _reservationRepository;
        private IUserRepository _userRepositrory;

        public HoursLimitValidator(IReservationRepository reservationRepository, IUserRepository userRepository)
        {
            _reservationRepository = reservationRepository;
            _userRepositrory = userRepository;
        }

        public void Validate(Reservation reservation)
        {
            User user = _userRepositrory.GetById(reservation.UserId);
            MembershipType membership = user.MembershipType;

            int year = reservation.StartDateTime.Year;
            int month = reservation.StartDateTime.Month;
            double hoursUsed = _reservationRepository.GetMonthlyHoursUsed(reservation.UserId, year, month);

            double newReservationHours = reservation.DurationHours;

            if (hoursUsed + newReservationHours > membership.MaxReservationHoursPerMonth)
            {
                throw new InvalidOperationException($"User has used {hoursUsed} hours this month. " +
                    $"Adding {newReservationHours} hours would exceed the " +
                    $"monthly limit of {membership.MaxReservationHoursPerMonth} hours.");
            }

            if (reservation.Resource is MeetingRoom)
            {
                if (!membership.IncludesMeetingRooms)
                {
                    throw new InvalidOperationException(
                        $"User's membership '{membership.Name}' does not include meeting room access."
                    );
                }

                double meetingRoomHoursUsed = _reservationRepository
                    .GetMonthlyMeetingRoomHoursUsed(reservation.UserId, year, month);

                if (meetingRoomHoursUsed + newReservationHours > membership.MeetingRoomHoursPerMonth)
                {
                    throw new InvalidOperationException(
                        $"User has used {meetingRoomHoursUsed} meeting room hours this month. " +
                        $"Adding {newReservationHours} hours would exceed the " +
                        $"meeting room limit of {membership.MeetingRoomHoursPerMonth} hours."
                    );
                }
            }
        }
    }
}
