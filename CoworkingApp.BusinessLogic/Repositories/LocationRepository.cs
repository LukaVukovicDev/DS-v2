using CoworkingApp.BusinessLogic.Database;
using CoworkingApp.BusinessLogic.Models;
using CoworkingApp.BusinessLogic.Repositories.Interfaces;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoworkingApp.BusinessLogic.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private IDbConnection _connection;

        public LocationRepository()
        {
            _connection = DatabaseConnection.GetInstance(null).GetConnection();
        }
        public void Add(Location location)
        {
            string sql = @"insert into Locations
(Name, Address, City, WorkingHours, MaxCapacity, Description)
values
(@Name, @Address, @City, @WorkingHours, @MaxCapacity, @Description)";
            _connection.Execute(sql, location);
        }

        public void Delete(int id)
        {
            string sql = "delete from Locations where Id = @Id";
            _connection.Execute(sql, new { Id = id });
        }

        public List<Location> GetAll()
        {

            string sql = "select * from Locations";
            return _connection.Query<Location>(sql).ToList();
        }

        public Location GetById(int id)
        {
            string sql = "select * from Locations where Id = @Id";
            return _connection.QueryFirstOrDefault<Location>(sql, new { Id = id });
        }

        public void Update(Location location)
        {
            string sql = @"UPDATE Locations SET 
                          Name = @Name,
                          Address = @Address,
                          City = @City,
                          WorkingHours = @WorkingHours,
                          MaxCapacity = @MaxCapacity,
                          Description = @Description
                          WHERE Id = @Id";
            _connection.Execute(sql, location);
        }
        public LocationStats GetLocationStats(int locationId)
        {
            string sql = @"SELECT 
                    l.Id AS LocationId,
                    l.Name AS LocationName,
                    COUNT(res.Id) AS TotalResources,
                    SUM(CASE WHEN res.IsAvailable = 0 THEN 1 ELSE 0 END) AS CurrentlyReserved
                  FROM Locations l
                  LEFT JOIN Resources res ON l.Id = res.LocationId
                  WHERE l.Id = @LocationId
                  GROUP BY l.Id, l.Name";
            return _connection.QueryFirstOrDefault<LocationStats>(sql, new { LocationId = locationId });
        }
    }
}
