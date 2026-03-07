using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoworkingApp.BusinessLogic.Models
{
    public enum DeskSubType
    {
        HotDesk,
        DedicatedDesk
    }

    public class Desk : Resource
    {
        public DeskSubType SubType { get; set; }
    }
}
