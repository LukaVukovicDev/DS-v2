using CoworkingApp.BusinessLogic.Models;
using System.Globalization;
using System.Windows;

namespace CoworkingApp.GUI.Dialogs
{
    public partial class MembershipDialog : Window
    {
        public MembershipType MembershipType { get; private set; }

        public MembershipDialog(MembershipType membershipType)
        {
            InitializeComponent();
            MembershipType = membershipType ?? new MembershipType();
            NameTextBox.Text = MembershipType.Name;
            PriceTextBox.Text = MembershipType.Price.ToString(CultureInfo.InvariantCulture);
            DurationTextBox.Text = MembershipType.DurationDays.ToString();
            MaxHoursTextBox.Text = MembershipType.MaxReservationHoursPerMonth.ToString();
            IncludesRoomsCheckBox.IsChecked = MembershipType.IncludesMeetingRooms;
            MeetingHoursTextBox.Text = MembershipType.MeetingRoomHoursPerMonth.ToString();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(PriceTextBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var price) ||
                !int.TryParse(DurationTextBox.Text, out var duration) ||
                !int.TryParse(MaxHoursTextBox.Text, out var maxHours) ||
                !int.TryParse(MeetingHoursTextBox.Text, out var roomHours))
            {
                MessageBox.Show("Proveri brojčana polja.");
                return;
            }

            MembershipType.Name = NameTextBox.Text?.Trim();
            MembershipType.Price = price;
            MembershipType.DurationDays = duration;
            MembershipType.MaxReservationHoursPerMonth = maxHours;
            MembershipType.IncludesMeetingRooms = IncludesRoomsCheckBox.IsChecked == true;
            MembershipType.MeetingRoomHoursPerMonth = roomHours;

            if (string.IsNullOrWhiteSpace(MembershipType.Name))
            {
                MessageBox.Show("Naziv je obavezan.");
                return;
            }
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
