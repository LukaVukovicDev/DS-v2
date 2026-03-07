using CoworkingApp.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoworkingApp.BusinessLogic.Repositories.Interfaces
{
    public interface IResourceRepository
    {
        Resource GetById(int id);
        List<Resource> GetByLocation(int locationId);
        List<Resource> GetByType(ResourceType type);
        List<Resource> GetAvailableByLocation(int locationId);
        void UpdateAvailability(int resourceId, bool isAvailable);
        void Add(Resource resource);
        void Update(Resource resource);
        void Delete(int id);
    }
}
