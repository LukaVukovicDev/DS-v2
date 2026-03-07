using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoworkingApp.BusinessLogic.Models
{
    public enum ResourceType
    {
        HotDesk,
        DedicatedDesk,
        PrivateOffice,
        MeetingRoom
    }

    public abstract class Resource
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public string Name { get; set; }
        public ResourceType Type { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
    }
}
