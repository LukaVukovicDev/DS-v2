using GUI.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

public class MainWindowViewModel : INotifyPropertyChanged
{
    public string ChainName { get; set; }

    public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();
    public ObservableCollection<Location> Locations { get; set; } = new ObservableCollection<Location>();
    public ObservableCollection<Reservation> Reservations { get; set; } = new ObservableCollection<Reservation>();

    public MainWindowViewModel()
    {
        ChainName = ConfigManager.Instance.ChainName;

        // Dummy data za test
        Users.Add(new User { Id = 1, Name = "Marko Markovic", Email = "marko@email.com", MembershipType = "premium", Status = "Aktivan" });
        Locations.Add(new Location { Id = 1, Name = "Beograd", City = "Beograd", MaxCapacity = 50 });
        Reservations.Add(new Reservation { Id = 1, UserName = "Marko Markovic", ResourceName = "Desk 1", StartTime = "2026-03-05 09:00", EndTime = "2026-03-05 12:00", Status = "Active" });

        // Users
        Users.Add(new User { Id = 2, Name = "Ana Jovanovic", Email = "ana@email.com", MembershipType = "fleksibilni sto", Status = "Aktivan" });
        Users.Add(new User { Id = 3, Name = "Petar Petrovic", Email = "petar@email.com", MembershipType = "fiksni sto", Status = "Pauziran" });
        Users.Add(new User { Id = 4, Name = "Milica Nikolic", Email = "milica@email.com", MembershipType = "premium", Status = "Aktivan" });
        Users.Add(new User { Id = 5, Name = "Nikola Ilic", Email = "nikola@email.com", MembershipType = "dnevna karta", Status = "Istekao" });


        // Locations
        Locations.Add(new Location { Id = 2, Name = "Cowork Niš", City = "Niš", MaxCapacity = 40 });
        Locations.Add(new Location { Id = 3, Name = "Cowork Novi Sad", City = "Novi Sad", MaxCapacity = 35 });
        Locations.Add(new Location { Id = 4, Name = "Cowork Kragujevac", City = "Kragujevac", MaxCapacity = 30 });
        Locations.Add(new Location { Id = 5, Name = "Cowork Zemun", City = "Beograd", MaxCapacity = 25 });


        // Reservations
        Reservations.Add(new Reservation { Id = 2, UserName = "Ana Jovanovic", ResourceName = "Meeting Room A", StartTime = "2026-03-06 10:00", EndTime = "2026-03-06 11:30", Status = "Active" });
        Reservations.Add(new Reservation { Id = 3, UserName = "Petar Petrovic", ResourceName = "Desk 5", StartTime = "2026-03-07 09:00", EndTime = "2026-03-07 17:00", Status = "Active" });
        Reservations.Add(new Reservation { Id = 4, UserName = "Milica Nikolic", ResourceName = "Meeting Room B", StartTime = "2026-03-08 13:00", EndTime = "2026-03-08 15:00", Status = "Cancelled" });
        Reservations.Add(new Reservation { Id = 5, UserName = "Nikola Ilic", ResourceName = "Desk 2", StartTime = "2026-03-09 08:00", EndTime = "2026-03-09 12:00", Status = "Completed" });
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}