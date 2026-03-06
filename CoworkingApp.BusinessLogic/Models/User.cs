using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoworkingApp.BusinessLogic.Models
{
    public enum AccountStatus
    {
        Active,
        Paused,
        Expired
    }
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int MembershipTypeId { get; set; }
        public MembershipType MembershipType { get; set; }
        public DateTime MembershipStartDate { get; set; }
        public DateTime MembershipEndDate { get; set; }
        public AccountStatus Status { get; set; }

        public string FullName => FirstName + " " + LastName;
    }
}
