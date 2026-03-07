using CoworkingApp.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoworkingApp.BusinessLogic.Factories
{
    public class ResourceFactory
    {
        public static Resource Create(dynamic row)
        {
            ResourceType type = (ResourceType)Enum.Parse(typeof(ResourceType), (string)row.Type);

            switch (type)
            {
                case ResourceType.MeetingRoom:
                    return CreateMeetingRoom(row, type);

                case ResourceType.HotDesk:
                case ResourceType.DedicatedDesk:
                    return CreateDesk(row, type);

                default:
                    return CreatePrivateOffice(row, type);
            }
        }

        private static MeetingRoom CreateMeetingRoom(dynamic row, ResourceType type)
        {
            return new MeetingRoom
            {
                Id = row.Id,
                LocationId = row.LocationId,
                Name = row.Name,
                Type = type,
                Description = row.Description,
                IsAvailable = row.IsAvailable,
                Capacity = row.Capacity,
                HasProjector = row.HasProjector,
                HasTV = row.HasTV,
                HasWhiteboard = row.HasWhiteboard,
                HasOnlineMeetingEquipment = row.HasOnlineMeetingEquipment
            };
        }

        private static Desk CreateDesk(dynamic row, ResourceType type)
        {
            return new Desk
            {
                Id = row.Id,
                LocationId = row.LocationId,
                Name = row.Name,
                Type = type,
                Description = row.Description,
                IsAvailable = row.IsAvailable,
                SubType = (DeskSubType)Enum.Parse(typeof(DeskSubType), (string)row.SubType)
            };
        }

        private static PrivateOffice CreatePrivateOffice(dynamic row, ResourceType type)
        {
            return new PrivateOffice
            {
                Id = row.Id,
                LocationId = row.LocationId,
                Name = row.Name,
                Type = type,
                Description = row.Description,
                IsAvailable = row.IsAvailable,
                Capacity = row.Capacity
            };
        }

        public static object ToParams(Resource resource)
        {
            int? capacity = null;
            bool hasProjector = false, hasTV = false,
                 hasWhiteboard = false, hasOnline = false;
            string subType = null;

            if (resource is MeetingRoom mr)
            {
                capacity = mr.Capacity;
                hasProjector = mr.HasProjector;
                hasTV = mr.HasTV;
                hasWhiteboard = mr.HasWhiteboard;
                hasOnline = mr.HasOnlineMeetingEquipment;
            }
            else if (resource is Desk desk)
            {
                subType = desk.SubType.ToString();
            }
            else if (resource is PrivateOffice po)
            {
                capacity = po.Capacity;
            }

            return new
            {
                resource.Id,
                resource.LocationId,
                resource.Name,
                Type = resource.Type.ToString(),
                resource.Description,
                resource.IsAvailable,
                Capacity = capacity,
                HasProjector = hasProjector,
                HasTV = hasTV,
                HasWhiteboard = hasWhiteboard,
                HasOnlineMeetingEquipment = hasOnline,
                SubType = subType
            };
        }
    }
}
