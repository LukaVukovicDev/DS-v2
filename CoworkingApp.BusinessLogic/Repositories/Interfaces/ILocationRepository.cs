using CoworkingApp.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoworkingApp.BusinessLogic.Repositories.Interfaces
{
    public interface ILocationRepository
    {
        Location GetById(int id);
        List<Location> GetAll();
        void Add(Location location);
        void Update(Location location);
        void Delete(int id);
        LocationStats GetLocationStats(int locationId);
    }
}
