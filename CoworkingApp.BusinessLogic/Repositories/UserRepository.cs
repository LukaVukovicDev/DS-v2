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
    public class UserRepository : IUserRepository
    {
        private IDbConnection _connection;

        public UserRepository()
        {
            _connection = DatabaseConnection.GetInstance(null).GetConnection();
        }
        public void Add(User user)
        {
            string sql = @"insert into Users
(FirstName, LastName, Email, Phone, MembershipTypeId, MembershipStartDate, MembershipEndDate, Status)
values
(@FirstName, @LastName, @Email, @Phone, @MembershipTypeId, @MembershipStartDate, @MembershipEndDate, @Status";
            _connection.Execute(sql, user);
        }

        public void Delete(int id)
        {
            string sql = "delete from Users where Id = @Id";
            _connection.Execute(sql, new { Id = id });
        }

        public List<User> GetAll()
        {
            string sql = "select * from Users";
            return _connection.Query<User>(sql).ToList();
        }

        public User GetById(int id)
        {
            string sql = "SELECT * FROM Users WHERE Id = @Id";
            return _connection.QueryFirstOrDefault<User>(sql, new { Id = id });
        }

        public List<User> GetByLocation(int locationId)
        {
            string sql = @"SELECT u.* FROM Users u
                  INNER JOIN Reservations r ON u.Id = r.UserId
                  INNER JOIN Resources res ON r.ResourceId = res.Id
                  WHERE res.LocationId = @LocationId
                  AND u.Status = 'Active'
                  GROUP BY u.Id, u.FirstName, u.LastName, u.Email, 
                           u.Phone, u.MembershipTypeId, u.MembershipStartDate, 
                           u.MembershipEndDate, u.Status";
            return _connection.Query<User>(sql, new { LocationId = locationId }).ToList();
        }

        public List<User> GetByMembershipType(int membershipTypeId)
        {
            string sql = "select * from Users where MembershipTypeId = @MembershipTypeId";
            return _connection.Query<User>(sql, new { MembershipTypeId = membershipTypeId }).ToList();
        }

        public List<User> GetByStatus(AccountStatus status)
        {
            string sql = "select * from Users where Status = @Status";
            return _connection.Query<User>(sql, new { Status = status }).ToList();
        }

        public void Update(User user)
        {
            string sql = @"update Users set
FirstName = @FirstName
LastName = @LastName
Email = @Email
Phone = @Phone
MembershipTypeId = @MembershipTypeId
MembershipStartDate = @MembershipStartDate,
MembershipEndDate = @MembershipEndDate,
Status = @Status
where Id = @Id";
            _connection.Execute(sql, user);
        }


    }
}
