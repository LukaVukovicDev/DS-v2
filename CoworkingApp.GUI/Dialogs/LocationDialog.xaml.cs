using CoworkingApp.BusinessLogic.Models;
using System.Windows;

namespace CoworkingApp.GUI.Dialogs
{
    public partial class LocationDialog : Window
    {
        public Location Location { get; private set; }

        public LocationDialog(Location location)
        {
            InitializeComponent();
            Location = location ?? new Location();
            NameTextBox.Text = Location.Name;
            AddressTextBox.Text = Location.Address;
            CityTextBox.Text = Location.City;
            WorkingHoursTextBox.Text = string.IsNullOrWhiteSpace(Location.WorkingHours) ? "08:00-20:00" : Location.WorkingHours;
            MaxCapacityTextBox.Text = Location.MaxCapacity.ToString();
            DescriptionTextBox.Text = Location.Description;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(MaxCapacityTextBox.Text, out var capacity))
            {
                MessageBox.Show("Kapacitet mora biti ceo broj.");
                return;
            }

            Location.Name = NameTextBox.Text?.Trim();
            Location.Address = AddressTextBox.Text?.Trim();
            Location.City = CityTextBox.Text?.Trim();
            Location.WorkingHours = WorkingHoursTextBox.Text?.Trim();
            Location.MaxCapacity = capacity;
            Location.Description = DescriptionTextBox.Text?.Trim();

            if (string.IsNullOrWhiteSpace(Location.Name) || string.IsNullOrWhiteSpace(Location.WorkingHours))
            {
                MessageBox.Show("Naziv i radno vreme su obavezni.");
                return;
            }
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
