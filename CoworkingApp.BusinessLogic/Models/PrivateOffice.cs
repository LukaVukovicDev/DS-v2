using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoworkingApp.BusinessLogic.Models
{
    public class PrivateOffice : Resource
    {
        public int RoomNumber { get; set; }
        public int Capacity { get; set; }
    }
}
