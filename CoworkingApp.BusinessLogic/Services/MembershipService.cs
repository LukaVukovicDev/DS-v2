using System.Collections.Generic;
using CoworkingApp.BusinessLogic.Models;
using CoworkingApp.BusinessLogic.Repositories.Interfaces;

namespace CoworkingApp.BusinessLogic.Services
{
    public class MembershipService
    {
        private IMembershipTypeRepository _membershipRepository;

        public MembershipService(IMembershipTypeRepository membershipRepository)
        {
            _membershipRepository = membershipRepository;
        }

        public List<MembershipType> GetAll() => _membershipRepository.GetAll();
        public MembershipType GetById(int id) => _membershipRepository.GetById(id);
        public void AddMembershipType(MembershipType m) => _membershipRepository.Add(m);
        public void UpdateMembershipType(MembershipType m) => _membershipRepository.Update(m);
        public void DeleteMembershipType(int id) => _membershipRepository.Delete(id);
    }
}