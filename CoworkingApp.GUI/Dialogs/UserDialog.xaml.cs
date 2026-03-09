using CoworkingApp.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Windows;

namespace CoworkingApp.GUI.Dialogs
{
    public partial class UserDialog : Window
    {
        public User User { get; private set; }

        public UserDialog(List<MembershipType> memberships, User user)
        {
            InitializeComponent();
            MembershipComboBox.ItemsSource = memberships;
            StatusComboBox.ItemsSource = Enum.GetValues(typeof(AccountStatus));

            User = user ?? new User
            {
                MembershipStartDate = DateTime.Today,
                MembershipEndDate = DateTime.Today.AddMonths(1),
                Status = AccountStatus.Active
            };

            FirstNameTextBox.Text = User.FirstName;
            LastNameTextBox.Text = User.LastName;
            EmailTextBox.Text = User.Email;
            PhoneTextBox.Text = User.Phone;
            StartDatePicker.SelectedDate = User.MembershipStartDate == default ? DateTime.Today : User.MembershipStartDate;
            EndDatePicker.SelectedDate = User.MembershipEndDate == default ? DateTime.Today.AddMonths(1) : User.MembershipEndDate;
            MembershipComboBox.SelectedValue = User.MembershipTypeId;
            if (User.MembershipType != null) MembershipComboBox.SelectedItem = User.MembershipType;
            else MembershipComboBox.SelectedIndex = memberships.Count > 0 ? 0 : -1;
            StatusComboBox.SelectedItem = User.Status;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (MembershipComboBox.SelectedItem is not MembershipType membership)
            {
                MessageBox.Show("Izaberi tip članstva.");
                return;
            }
            User.FirstName = FirstNameTextBox.Text?.Trim();
            User.LastName = LastNameTextBox.Text?.Trim();
            User.Email = EmailTextBox.Text?.Trim();
            User.Phone = PhoneTextBox.Text?.Trim();
            User.MembershipTypeId = membership.Id;
            User.MembershipType = membership;
            User.MembershipStartDate = StartDatePicker.SelectedDate ?? DateTime.Today;
            User.MembershipEndDate = EndDatePicker.SelectedDate ?? DateTime.Today;
            User.Status = (AccountStatus)StatusComboBox.SelectedItem;

            if (string.IsNullOrWhiteSpace(User.FirstName) || string.IsNullOrWhiteSpace(User.LastName) || string.IsNullOrWhiteSpace(User.Email))
            {
                MessageBox.Show("Ime, prezime i email su obavezni.");
                return;
            }
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
