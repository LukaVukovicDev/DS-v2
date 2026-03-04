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
        Users.Add(new User { Id = 1, Name = "Marko Markovic", Email = "marko@email.com", MembershipType = "Premium", Status = "Active" });
        Locations.Add(new Location { Id = 1, Name = "Beograd", City = "Belgrade", MaxCapacity = 50 });
        Reservations.Add(new Reservation { Id = 1, UserName = "Marko Markovic", ResourceName = "Desk 1", StartTime = "2026-03-05 09:00", EndTime = "2026-03-05 12:00", Status = "Active" });
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}