using CoworkingApp.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoworkingApp.BusinessLogic.Repositories.Interfaces
{
    public interface IReservationRepository
    {
        Reservation GetById(int id);
        List<Reservation> GetByUser(int userId);
        List<Reservation> GetByDateAndLocation(DateTime date, int locationId);
        List<Reservation> GetActiveByResource(int resourceId);
        double GetMonthlyHoursUsed(int userId, int year, int month);
        double GetMonthlyMeetingRoomHoursUsed(int userId, int year, int month);
        void Add(Reservation reservation);
        void Update(Reservation reservation);
        void Cancel(int id);
    }
}
