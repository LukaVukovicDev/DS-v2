using System.Collections.Generic;
using CoworkingApp.BusinessLogic.Models;
using CoworkingApp.BusinessLogic.Repositories.Interfaces;

namespace CoworkingApp.BusinessLogic.Services
{
    public class LocationService
    {
        private ILocationRepository _locationRepository;

        public LocationService(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public List<Location> GetAll()
        {
            return _locationRepository.GetAll();
        }

        public Location GetById(int id)
        {
            return _locationRepository.GetById(id);
        }

        public LocationStats GetLocationStats(int locationId)
        {
            return _locationRepository.GetLocationStats(locationId);
        }

        public void AddLocation(Location location)
        {
            if (string.IsNullOrEmpty(location.Name))
            {
                throw new System.InvalidOperationException(
                    "Location name cannot be empty."
                );
            }

            if (string.IsNullOrEmpty(location.WorkingHours) ||
                !location.WorkingHours.Contains("-"))
            {
                throw new System.InvalidOperationException(
                    "Working hours must be in format HH:MM-HH:MM."
                );
            }

            _locationRepository.Add(location);
        }

        public void UpdateLocation(Location location)
        {
            _locationRepository.Update(location);
        }

        public void DeleteLocation(int id)
        {
            _locationRepository.Delete(id);
        }
    }
}
