using CoworkingApp.BusinessLogic.Models;
using CoworkingApp.BusinessLogic.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoworkingApp.BusinessLogic.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        public void Add(Reservation reservation)
        {
            string sql = @"insert into Reservations
(";
        }

        public void Cancel(int id)
        {
            throw new NotImplementedException();
        }

        public List<Reservation> GetActiveByResource(int resourceId)
        {
            throw new NotImplementedException();
        }

        public List<Reservation> GetByDateAndLocation(DateTime date, int locationId)
        {
            throw new NotImplementedException();
        }

        public Reservation GetById(int id)
        {
            throw new NotImplementedException();
        }

        public List<Reservation> GetByUser(int userId)
        {
            throw new NotImplementedException();
        }

        public double GetMonthlyHoursUsed(int userId, int year, int month)
        {
            throw new NotImplementedException();
        }

        public double GetMonthlyMeetingRoomHoursUsed(int userId, int year, int month)
        {
            throw new NotImplementedException();
        }

        public void Update(Reservation reservation)
        {
            throw new NotImplementedException();
        }
    }
}
