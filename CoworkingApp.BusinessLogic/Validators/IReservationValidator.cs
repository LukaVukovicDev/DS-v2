using CoworkingApp.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoworkingApp.BusinessLogic.Validators
{
    public interface IReservationValidator
    {
        void Validate(Reservation reservation);
    }
}
