using System.Collections.Generic;
using System.Data;
using System.Linq;
using CoworkingApp.BusinessLogic.Database;
using CoworkingApp.BusinessLogic.Factories;
using CoworkingApp.BusinessLogic.Models;
using CoworkingApp.BusinessLogic.Repositories.Interfaces;
using Dapper;

namespace CoworkingApp.BusinessLogic.Repositories
{
    public class ResourceRepository : IResourceRepository
    {
        private IDbConnection _connection;

        public ResourceRepository()
        {
            _connection = DatabaseConnection.GetInstance(null).GetConnection();
        }

        public Resource GetById(int id)
        {
            string sql = "SELECT * FROM Resources WHERE Id = @Id";
            var row = _connection.QueryFirstOrDefault<dynamic>(sql, new { Id = id });
            return row == null ? null : ResourceFactory.Create(row);
        }

        public List<Resource> GetByLocation(int locationId)
        {
            string sql = "SELECT * FROM Resources WHERE LocationId = @LocationId";
            var rows = _connection.Query<dynamic>(sql, new { LocationId = locationId });
            return rows.Select(row => ResourceFactory.Create(row)).Cast<Resource>().ToList();
        }

        public List<Resource> GetByType(ResourceType type)
        {
            string sql = "SELECT * FROM Resources WHERE Type = @Type";
            var rows = _connection.Query<dynamic>(sql, new { Type = type.ToString() });
            return rows.Select(row => ResourceFactory.Create(row)).Cast<Resource>().ToList();
        }

        public List<Resource> GetAvailableByLocation(int locationId)
        {
            string sql = "SELECT * FROM Resources WHERE LocationId = @LocationId AND IsAvailable = 1";
            var rows = _connection.Query<dynamic>(sql, new { LocationId = locationId });
            return rows.Select(row => ResourceFactory.Create(row)).Cast<Resource>().ToList();
        }

        public void UpdateAvailability(int resourceId, bool isAvailable)
        {
            string sql = "UPDATE Resources SET IsAvailable = @IsAvailable WHERE Id = @Id";
            _connection.Execute(sql, new { Id = resourceId, IsAvailable = isAvailable });
        }

        public void Add(Resource resource)
        {
            string sql = @"INSERT INTO Resources 
                          (LocationId, Name, Type, Description, IsAvailable,
                           Capacity, HasProjector, HasTV, HasWhiteboard, 
                           HasOnlineMeetingEquipment, SubType) 
                          VALUES 
                          (@LocationId, @Name, @Type, @Description, @IsAvailable,
                           @Capacity, @HasProjector, @HasTV, @HasWhiteboard,
                           @HasOnlineMeetingEquipment, @SubType)";
            _connection.Execute(sql, ResourceFactory.ToParams(resource));
        }

        public void Update(Resource resource)
        {
            string sql = @"UPDATE Resources SET 
                          LocationId = @LocationId,
                          Name = @Name,
                          Type = @Type,
                          Description = @Description,
                          IsAvailable = @IsAvailable,
                          Capacity = @Capacity,
                          HasProjector = @HasProjector,
                          HasTV = @HasTV,
                          HasWhiteboard = @HasWhiteboard,
                          HasOnlineMeetingEquipment = @HasOnlineMeetingEquipment,
                          SubType = @SubType
                          WHERE Id = @Id";
            _connection.Execute(sql, ResourceFactory.ToParams(resource));
        }

        public void Delete(int id)
        {
            string sql = "DELETE FROM Resources WHERE Id = @Id";
            _connection.Execute(sql, new { Id = id });
        }
    }
}
