using CoworkingApp.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoworkingApp.BusinessLogic.Repositories.Interfaces
{
    public interface IUserRepository
    {
        User GetById(int id);
        List<User> GetAll();
        List<User> GetByMembershipType(int membershipTypeId);
        List<User> GetByStatus(AccountStatus status);
        void Add(User user);
        void Update(User user);
        void Delete(int id);
        List<User> GetByLocation(int locationId);
    }
}
