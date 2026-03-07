using System.Collections.Generic;
using System.Linq;
using CoworkingApp.BusinessLogic.Models;
using CoworkingApp.BusinessLogic.Repositories.Interfaces;

namespace CoworkingApp.BusinessLogic.Services
{
    public class ResourceService
    {
        private IResourceRepository _resourceRepository;

        public ResourceService(IResourceRepository resourceRepository)
        {
            _resourceRepository = resourceRepository;
        }

        public List<Resource> GetByLocation(int locationId)
        {
            return _resourceRepository.GetByLocation(locationId);
        }

        public List<Resource> GetAvailableByLocation(int locationId)
        {
            return _resourceRepository.GetAvailableByLocation(locationId);
        }

        public List<Resource> GetDesksByLocation(int locationId)
        {
            return _resourceRepository.GetByLocation(locationId)
                .Where(r => r is Desk)
                .ToList();
        }

        public List<Resource> GetMeetingRoomsByLocation(int locationId)
        {
            return _resourceRepository.GetByLocation(locationId)
                .Where(r => r is MeetingRoom)
                .ToList();
        }

        public Resource GetById(int id)
        {
            return _resourceRepository.GetById(id);
        }

        public void AddResource(Resource resource)
        {
            if (string.IsNullOrEmpty(resource.Name))
            {
                throw new System.InvalidOperationException(
                    "Resource name cannot be empty."
                );
            }

            _resourceRepository.Add(resource);
        }

        public void UpdateResource(Resource resource)
        {
            _resourceRepository.Update(resource);
        }

        public void DeleteResource(int id)
        {
            _resourceRepository.Delete(id);
        }

        public void UpdateAvailability(int resourceId, bool isAvailable)
        {
            _resourceRepository.UpdateAvailability(resourceId, isAvailable);
        }
    }
}