namespace CoworkingApp.BusinessLogic.Report
{
    public class ReportItem
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}".Trim();
        public string Email { get; set; }
        public string MembershipTypeName { get; set; }
        public int TotalReservations { get; set; }
        public double TotalHours { get; set; }
        public int UniqueResources { get; set; }
        public int UniqueLocations { get; set; }
    }
}
