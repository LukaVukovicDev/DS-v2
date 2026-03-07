using CoworkingApp.BusinessLogic.Database;
using CoworkingApp.BusinessLogic.Models;
using CoworkingApp.BusinessLogic.Repositories.Interfaces;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Resources;

namespace CoworkingApp.BusinessLogic.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private IDbConnection _connection;

        public ReservationRepository()
        {
            _connection = DatabaseConnection.GetInstance(null).GetConnection();
        }

        public Reservation GetById(int id)
        {
            string sql = "SELECT * FROM Reservations WHERE Id = @Id";
            return _connection.QueryFirstOrDefault<Reservation>(sql, new { Id = id });
        }

        public List<Reservation> GetByUser(int userId)
        {
            string sql = "SELECT * FROM Reservations WHERE UserId = @UserId";
            return _connection.Query<Reservation>(sql, new { UserId = userId }).ToList();
        }

        public List<Reservation> GetByDateAndLocation(DateTime date, int locationId)
        {
            string sql = @"SELECT r.* FROM Reservations r
                          INNER JOIN Resources res ON r.ResourceId = res.Id
                          WHERE res.LocationId = @LocationId
                          AND CAST(r.StartDateTime AS DATE) = CAST(@Date AS DATE)
                          AND r.Status = 'Active'";
            return _connection.Query<Reservation>(sql, new { LocationId = locationId, Date = date }).ToList();
        }

        public List<Reservation> GetActiveByResource(int resourceId)
        {
            string sql = @"SELECT * FROM Reservations 
                          WHERE ResourceId = @ResourceId 
                          AND Status = 'Active'";
            return _connection.Query<Reservation>(sql, new { ResourceId = resourceId }).ToList();
        }

        public double GetMonthlyHoursUsed(int userId, int year, int month)
        {
            string sql = @"SELECT COALESCE(SUM(
                            DATEDIFF(MINUTE, StartDateTime, EndDateTime) / 60.0
                          ), 0)
                          FROM Reservations
                          WHERE UserId = @UserId
                          AND YEAR(StartDateTime) = @Year
                          AND MONTH(StartDateTime) = @Month
                          AND Status != 'Cancelled'";
            return _connection.QueryFirstOrDefault<double>(sql, new { UserId = userId, Year = year, Month = month });
        }

        public double GetMonthlyMeetingRoomHoursUsed(int userId, int year, int month)
        {
            string sql = @"SELECT COALESCE(SUM(
                            DATEDIFF(MINUTE, r.StartDateTime, r.EndDateTime) / 60.0
                          ), 0)
                          FROM Reservations r
                          INNER JOIN Resources res ON r.ResourceId = res.Id
                          WHERE r.UserId = @UserId
                          AND YEAR(r.StartDateTime) = @Year
                          AND MONTH(r.StartDateTime) = @Month
                          AND r.Status != 'Cancelled'
                          AND res.Type = 'MeetingRoom'";
            return _connection.QueryFirstOrDefault<double>(sql, new { UserId = userId, Year = year, Month = month });
        }

        public void Add(Reservation reservation)
        {
            string sql = @"INSERT INTO Reservations
                          (UserId, ResourceId, StartDateTime, EndDateTime, Status)
                          VALUES
                          (@UserId, @ResourceId, @StartDateTime, @EndDateTime, @Status)";
            _connection.Execute(sql, new
            {
                reservation.UserId,
                reservation.ResourceId,
                reservation.StartDateTime,
                reservation.EndDateTime,
                Status = reservation.Status.ToString()
            });
        }

        public void Update(Reservation reservation)
        {
            string sql = @"UPDATE Reservations SET
                          UserId = @UserId,
                          ResourceId = @ResourceId,
                          StartDateTime = @StartDateTime,
                          EndDateTime = @EndDateTime,
                          Status = @Status
                          WHERE Id = @Id";
            _connection.Execute(sql, new
            {
                reservation.Id,
                reservation.UserId,
                reservation.ResourceId,
                reservation.StartDateTime,
                reservation.EndDateTime,
                Status = reservation.Status.ToString()
            });
        }

        public void Cancel(int id)
        {
            string sql = "UPDATE Reservations SET Status = 'Cancelled' WHERE Id = @Id";
            _connection.Execute(sql, new { Id = id });
        }
    }
}