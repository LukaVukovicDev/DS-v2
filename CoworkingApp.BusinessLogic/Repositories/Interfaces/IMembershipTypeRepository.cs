using CoworkingApp.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoworkingApp.BusinessLogic.Repositories.Interfaces
{
    public interface IMembershipTypeRepository
    {
        MembershipType GetById(int id);
        List<MembershipType> GetAll();
        void Add(MembershipType membershipType);
        void Update(MembershipType membershipType);
        void Delete(int id);
    }
}
