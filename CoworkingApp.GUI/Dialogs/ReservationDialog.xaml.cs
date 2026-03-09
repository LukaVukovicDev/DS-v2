using CoworkingApp.BusinessLogic.Models;
using CoworkingApp.BusinessLogic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CoworkingApp.GUI.Dialogs
{
    public partial class ReservationDialog : Window
    {
        private readonly CoworkingFacade _facade;
        public Reservation Reservation { get; private set; }
        public int UserId => ((User)UserComboBox.SelectedItem).Id;
        public int ResourceId => ((Resource)ResourceComboBox.SelectedItem).Id;
        public DateTime StartDateTime => (StartDatePicker.SelectedDate ?? DateTime.Today).Date + ParseTime(StartTimeTextBox.Text);
        public DateTime EndDateTime => (StartDatePicker.SelectedDate ?? DateTime.Today).Date + ParseTime(EndTimeTextBox.Text);

        public ReservationDialog(CoworkingFacade facade, List<User> users, List<Location> locations, Reservation reservation)
        {
            InitializeComponent();
            _facade = facade;
            Reservation = reservation ?? new Reservation { Status = ReservationStatus.Active, StartDateTime = DateTime.Now.AddHours(1), EndDateTime = DateTime.Now.AddHours(2) };

            UserComboBox.ItemsSource = users;
            LocationComboBox.ItemsSource = locations;

            UserComboBox.SelectedItem = users.FirstOrDefault(u => u.Id == Reservation.UserId) ?? users.FirstOrDefault();
            var selectedResource = Reservation.Resource ?? (Reservation.ResourceId > 0 ? _facade.GetResource(Reservation.ResourceId) : null);
            var initialLocationId = selectedResource?.LocationId ?? locations.FirstOrDefault()?.Id ?? 0;
            LocationComboBox.SelectedItem = locations.FirstOrDefault(l => l.Id == initialLocationId) ?? locations.FirstOrDefault();

            StartDatePicker.SelectedDate = Reservation.StartDateTime == default ? DateTime.Today : Reservation.StartDateTime.Date;
            StartTimeTextBox.Text = (Reservation.StartDateTime == default ? DateTime.Now.AddHours(1) : Reservation.StartDateTime).ToString("HH:mm");
            EndTimeTextBox.Text = (Reservation.EndDateTime == default ? DateTime.Now.AddHours(2) : Reservation.EndDateTime).ToString("HH:mm");

            LoadResources(selectedResource?.Id);
        }

        private void LocationComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => LoadResources();

        private void LoadResources(int? selectResourceId = null)
        {
            if (LocationComboBox.SelectedItem is not Location location)
            {
                ResourceComboBox.ItemsSource = null;
                return;
            }
            var resources = _facade.GetResourcesByLocation(location.Id) ?? new List<Resource>();
            ResourceComboBox.ItemsSource = resources;
            ResourceComboBox.SelectedItem = resources.FirstOrDefault(r => r.Id == selectResourceId) ?? resources.FirstOrDefault();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (UserComboBox.SelectedItem is not User || ResourceComboBox.SelectedItem is not Resource)
            {
                MessageBox.Show("Korisnik i resurs su obavezni.");
                return;
            }
            if (!TimeSpan.TryParse(StartTimeTextBox.Text, out _) || !TimeSpan.TryParse(EndTimeTextBox.Text, out _))
            {
                MessageBox.Show("Vreme mora biti u formatu HH:mm.");
                return;
            }
            Reservation.UserId = UserId;
            Reservation.ResourceId = ResourceId;
            Reservation.StartDateTime = StartDateTime;
            Reservation.EndDateTime = EndDateTime;
            if (Reservation.Status == default) Reservation.Status = ReservationStatus.Active;
            DialogResult = true;
        }

        private static TimeSpan ParseTime(string value)
        {
            if (!TimeSpan.TryParse(value, out var time))
                throw new InvalidOperationException("Neispravan format vremena.");
            return time;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
