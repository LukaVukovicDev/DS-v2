using CoworkingApp.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CoworkingApp.GUI.Dialogs
{
    public partial class ResourceDialog : Window
    {
        public Resource Resource { get; private set; }

        public ResourceDialog(List<Location> locations, int selectedLocationId, Resource resource)
        {
            InitializeComponent();
            LocationComboBox.ItemsSource = locations;
            TypeComboBox.ItemsSource = Enum.GetValues(typeof(ResourceType));
            DeskSubtypeComboBox.ItemsSource = Enum.GetValues(typeof(DeskSubType));

            Resource = resource ?? new Desk { Type = ResourceType.HotDesk, SubType = DeskSubType.HotDesk, IsAvailable = true, LocationId = selectedLocationId };

            LocationComboBox.SelectedItem = locations.FirstOrDefault(l => l.Id == Resource.LocationId) ?? locations.FirstOrDefault();
            NameTextBox.Text = Resource.Name;
            DescriptionTextBox.Text = Resource.Description;
            IsAvailableCheckBox.IsChecked = Resource.IsAvailable;
            TypeComboBox.SelectedItem = Resource.Type;

            if (Resource is Desk desk)
                DeskSubtypeComboBox.SelectedItem = desk.SubType;
            if (Resource is MeetingRoom room)
            {
                CapacityTextBox.Text = room.Capacity.ToString();
                ProjectorCheckBox.IsChecked = room.HasProjector;
                TvCheckBox.IsChecked = room.HasTV;
                WhiteboardCheckBox.IsChecked = room.HasWhiteboard;
                OnlineEqCheckBox.IsChecked = room.HasOnlineMeetingEquipment;
            }
            if (Resource is PrivateOffice office)
                CapacityTextBox.Text = office.Capacity?.ToString() ?? "1";

            UpdateVisibility();
        }

        private void TypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateVisibility();

        private void UpdateVisibility()
        {
            var selectedType = TypeComboBox.SelectedItem is ResourceType type ? type : ResourceType.HotDesk;
            var isDesk = selectedType == ResourceType.HotDesk || selectedType == ResourceType.DedicatedDesk;
            var isRoom = selectedType == ResourceType.MeetingRoom;
            var isOffice = selectedType == ResourceType.PrivateOffice;

            DeskSubtypeLabel.Visibility = DeskSubtypeComboBox.Visibility = isDesk ? Visibility.Visible : Visibility.Collapsed;
            CapacityLabel.Visibility = CapacityTextBox.Visibility = (isRoom || isOffice) ? Visibility.Visible : Visibility.Collapsed;
            ProjectorCheckBox.Visibility = TvCheckBox.Visibility = WhiteboardCheckBox.Visibility = OnlineEqCheckBox.Visibility = isRoom ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (LocationComboBox.SelectedItem is not Location location)
            {
                MessageBox.Show("Izaberi lokaciju.");
                return;
            }
            if (TypeComboBox.SelectedItem is not ResourceType type)
            {
                MessageBox.Show("Izaberi tip resursa.");
                return;
            }
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                MessageBox.Show("Naziv resursa je obavezan.");
                return;
            }

            Resource newResource;
            switch (type)
            {
                case ResourceType.MeetingRoom:
                    if (!int.TryParse(CapacityTextBox.Text, out var roomCapacity))
                    {
                        MessageBox.Show("Kapacitet sale mora biti ceo broj.");
                        return;
                    }
                    newResource = new MeetingRoom
                    {
                        Capacity = roomCapacity,
                        HasProjector = ProjectorCheckBox.IsChecked == true,
                        HasTV = TvCheckBox.IsChecked == true,
                        HasWhiteboard = WhiteboardCheckBox.IsChecked == true,
                        HasOnlineMeetingEquipment = OnlineEqCheckBox.IsChecked == true
                    };
                    break;
                case ResourceType.PrivateOffice:
                    if (!int.TryParse(CapacityTextBox.Text, out var officeCapacity))
                    {
                        MessageBox.Show("Kapacitet kancelarije mora biti ceo broj.");
                        return;
                    }
                    newResource = new PrivateOffice { Capacity = officeCapacity };
                    break;
                default:
                    newResource = new Desk
                    {
                        SubType = (type == ResourceType.DedicatedDesk) ? DeskSubType.DedicatedDesk : DeskSubType.HotDesk
                    };
                    break;
            }

            newResource.Id = Resource?.Id ?? 0;
            newResource.LocationId = location.Id;
            newResource.Name = NameTextBox.Text.Trim();
            newResource.Type = type;
            newResource.Description = DescriptionTextBox.Text?.Trim();
            newResource.IsAvailable = IsAvailableCheckBox.IsChecked == true;

            Resource = newResource;
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
