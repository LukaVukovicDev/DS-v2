using CoworkingApp.BusinessLogic.Models;
using CoworkingApp.BusinessLogic.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoworkingApp.BusinessLogic.Services
{
    public class UserService
    {
        private IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public List<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        public List<User> GetByMembershipType(int membershipTypeId)
        {
            return _userRepository.GetByMembershipType(membershipTypeId);
        }

        public List<User> GetByStatus(AccountStatus status)
        {
            return _userRepository.GetByStatus(status);
        }

        public List<User> GetByLocation(int locationId)
        {
            return _userRepository.GetByLocation(locationId);
        }

        public User GetById(int id)
        {
            return _userRepository.GetById(id);
        }

        public void AddUser(User user)
        {
            // Check if email already exists
            List<User> allUsers = _userRepository.GetAll();
            foreach (User existing in allUsers)
            {
                if (existing.Email == user.Email)
                {
                    throw new System.InvalidOperationException(
                        $"User with email '{user.Email}' already exists."
                    );
                }
            }
            _userRepository.Add(user);
        }

        public void UpdateUser(User user)
        {
            _userRepository.Update(user);
        }

        public void DeleteUser(int id)
        {
            _userRepository.Delete(id);
        }
    }
}
