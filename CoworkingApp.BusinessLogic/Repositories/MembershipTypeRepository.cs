using System.Collections.Generic;
using System.Data;
using System.Linq;
using CoworkingApp.BusinessLogic.Database;
using CoworkingApp.BusinessLogic.Models;
using CoworkingApp.BusinessLogic.Repositories.Interfaces;
using Dapper;

namespace CoworkingApp.BusinessLogic.Repositories
{
    public class MembershipTypeRepository : IMembershipTypeRepository
    {
        private IDbConnection _connection;

        public MembershipTypeRepository()
        {
            _connection = DatabaseConnection.GetInstance(null).GetConnection();
        }

        public MembershipType GetById(int id)
        {
            string sql = "SELECT * FROM MembershipTypes WHERE Id = @Id";
            return _connection.QueryFirstOrDefault<MembershipType>(sql, new { Id = id });
        }

        public List<MembershipType> GetAll()
        {
            string sql = "SELECT * FROM MembershipTypes";
            return _connection.Query<MembershipType>(sql).ToList();
        }

        public void Add(MembershipType membershipType)
        {
            string sql = @"INSERT INTO MembershipTypes 
                          (Name, Price, DurationDays, MaxReservationHoursPerMonth, 
                           IncludesMeetingRooms, MeetingRoomHoursPerMonth) 
                          VALUES 
                          (@Name, @Price, @DurationDays, @MaxReservationHoursPerMonth,
                           @IncludesMeetingRooms, @MeetingRoomHoursPerMonth)";
            _connection.Execute(sql, membershipType);
        }

        public void Update(MembershipType membershipType)
        {
            string sql = @"UPDATE MembershipTypes SET 
                          Name = @Name,
                          Price = @Price,
                          DurationDays = @DurationDays,
                          MaxReservationHoursPerMonth = @MaxReservationHoursPerMonth,
                          IncludesMeetingRooms = @IncludesMeetingRooms,
                          MeetingRoomHoursPerMonth = @MeetingRoomHoursPerMonth
                          WHERE Id = @Id";
            _connection.Execute(sql, membershipType);
        }

        public void Delete(int id)
        {
            string sql = "DELETE FROM MembershipTypes WHERE Id = @Id";
            _connection.Execute(sql, new { Id = id });
        }
    }
}
