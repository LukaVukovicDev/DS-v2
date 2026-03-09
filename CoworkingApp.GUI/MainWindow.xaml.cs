using CoworkingApp.BusinessLogic.Builders;
using CoworkingApp.BusinessLogic.Database;
using CoworkingApp.BusinessLogic.Models;
using CoworkingApp.BusinessLogic.Services;
using CoworkingApp.BusinessLogic.Report;
using CoworkingApp.GUI.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CoworkingApp.GUI
{
    public partial class MainWindow : Window
    {
        private readonly CoworkingFacade _facade;
        private readonly ConfigReader _config;

        private List<User> _users = new List<User>();
        private List<Location> _locations = new List<Location>();
        private List<MembershipType> _membershipTypes = new List<MembershipType>();
        private readonly Dictionary<string, double> _reportIntervals = new Dictionary<string, double>();

        public MainWindow()
        {
            InitializeComponent();

            _config = new ConfigReader("C:\\Users\\Nemanja\\Desktop\\Dizajniranje-softvera-csv-izmena\\GUI\\GUI\\config.txt");
            _facade = CoworkingFacade.GetInstance(_config.ConnectionString);

            _facade.ReservationCreated += OnReservationCreated;
            _facade.ReservationUpdated += OnReservationUpdated;
            _facade.ReservationCancelled += OnReservationCancelled;

            BrandNameTextBlock.Text = _config.ChainName;

            LoadStaticCombos();
            InitializeReportControls();
            RefreshAll();
        }

        private void LoadStaticCombos()
        {
            UserStatusFilterComboBox.ItemsSource = new List<string> { "Svi statusi", AccountStatus.Active.ToString(), AccountStatus.Paused.ToString(), AccountStatus.Expired.ToString() };
            UserStatusFilterComboBox.SelectedIndex = 0;

            ResourcesTypeComboBox.ItemsSource = new List<string> { "Svi", "Samo stolovi", "Samo sale", "Samo dostupni" };
            ResourcesTypeComboBox.SelectedIndex = 0;

            ReservationDayPicker.SelectedDate = DateTime.Today;
        }

        private void InitializeReportControls()
        {
            var now = DateTime.Now;
            ReportFromDatePicker.SelectedDate = new DateTime(now.Year, now.Month, 1);
            ReportToDatePicker.SelectedDate = now.Date;

            _reportIntervals["1h"] = TimeSpan.FromHours(1).TotalMilliseconds;
            _reportIntervals["5h"] = TimeSpan.FromHours(5).TotalMilliseconds;
            _reportIntervals["1d"] = TimeSpan.FromDays(1).TotalMilliseconds;
            _reportIntervals["1w"] = TimeSpan.FromDays(7).TotalMilliseconds;
            _reportIntervals["1M"] = TimeSpan.FromDays(30).TotalMilliseconds;

            ReportIntervalComboBox.ItemsSource = _reportIntervals.Keys.ToList();
            ReportIntervalComboBox.SelectedIndex = 2;
            ReportFolderTextBox.Text = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "reports");
            ReportStatusTextBlock.Text = "Izaberi period i klikni na 'Prikaži pregled' ili 'Izvezi CSV'.";
        }

        private void RefreshAll()
        {
            RefreshUsers();
            RefreshLocations();
            RefreshMemberships();
            RefreshResourceTabLookups();
            RefreshReservationTabLookups();
            RefreshResourceList();
            RefreshReservationLists();
            RefreshReportPreview();
        }

        private void RefreshUsers()
        {
            _users = _facade.GetAllUsers() ?? new List<User>();
            _membershipTypes = _facade.GetAllMembershipTypes() ?? new List<MembershipType>();
            _locations = _facade.GetAllLocations() ?? new List<Location>();

            UserLocationFilterComboBox.ItemsSource = new[] { new Location { Id = 0, Name = "Sve lokacije" } }.Concat(_locations).ToList();
            UserMembershipFilterComboBox.ItemsSource = new[] { new MembershipType { Id = 0, Name = "Sva članstva" } }.Concat(_membershipTypes).ToList();
            if (UserLocationFilterComboBox.SelectedIndex < 0) UserLocationFilterComboBox.SelectedIndex = 0;
            if (UserMembershipFilterComboBox.SelectedIndex < 0) UserMembershipFilterComboBox.SelectedIndex = 0;

            UsersDataGrid.ItemsSource = ApplyUserFilters(_users);
        }

        private IEnumerable<User> ApplyUserFilters(IEnumerable<User> users)
        {
            var query = users;

            var search = (UserSearchTextBox.Text ?? string.Empty).Trim().ToLowerInvariant();
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u =>
                    (u.FirstName ?? string.Empty).ToLowerInvariant().Contains(search) ||
                    (u.LastName ?? string.Empty).ToLowerInvariant().Contains(search) ||
                    (u.Email ?? string.Empty).ToLowerInvariant().Contains(search) ||
                    (u.Phone ?? string.Empty).ToLowerInvariant().Contains(search));
            }

            if (UserMembershipFilterComboBox.SelectedItem is MembershipType mt && mt.Id > 0)
                query = query.Where(u => u.MembershipTypeId == mt.Id);

            if (UserStatusFilterComboBox.SelectedItem is string status && !status.StartsWith("Svi"))
                query = query.Where(u => string.Equals(u.Status.ToString(), status, StringComparison.OrdinalIgnoreCase));

            if (UserLocationFilterComboBox.SelectedItem is Location loc && loc.Id > 0)
            {
                var ids = (_facade.GetUsersByLocation(loc.Id) ?? new List<User>()).Select(u => u.Id).ToHashSet();
                query = query.Where(u => ids.Contains(u.Id));
            }

            return query.ToList();
        }

        private void RefreshLocations()
        {
            _locations = _facade.GetAllLocations() ?? new List<Location>();
            LocationsDataGrid.ItemsSource = _locations;
            UpdateLocationStatsPanel(LocationsDataGrid.SelectedItem as Location ?? _locations.FirstOrDefault());
        }

        private void UpdateLocationStatsPanel(Location location)
        {
            if (location == null)
            {
                LocationStatsName.Text = "";
                LocationStatsTotalResources.Text = "";
                LocationStatsReserved.Text = "";
                LocationStatsOccupancy.Text = "";
                LocationStatsDescription.Text = "";
                return;
            }

            var stats = _facade.GetLocationStats(location.Id);
            LocationStatsName.Text = location.Name;
            LocationStatsTotalResources.Text = $"Ukupno resursa: {stats?.TotalResources ?? 0}";
            LocationStatsReserved.Text = $"Trenutno rezervisano: {stats?.CurrentlyReserved ?? 0}";
            LocationStatsOccupancy.Text = $"Zauzetost: {(stats?.OccupancyPercentage ?? 0):0.##}%";
            LocationStatsDescription.Text = $"{location.City}, {location.Address}\nRadno vreme: {location.WorkingHours}\nOpis: {location.Description}";
        }

        private void RefreshMemberships()
        {
            _membershipTypes = _facade.GetAllMembershipTypes() ?? new List<MembershipType>();
            MembershipsDataGrid.ItemsSource = _membershipTypes;
        }

        private void RefreshResourceTabLookups()
        {
            _locations = _facade.GetAllLocations() ?? new List<Location>();
            ResourcesLocationComboBox.ItemsSource = _locations;
            if (ResourcesLocationComboBox.SelectedIndex < 0 && _locations.Any())
                ResourcesLocationComboBox.SelectedIndex = 0;
        }

        private void RefreshReservationTabLookups()
        {
            _users = _facade.GetAllUsers() ?? new List<User>();
            _locations = _facade.GetAllLocations() ?? new List<Location>();

            ReservationUserComboBox.ItemsSource = _users;
            ReservationLocationComboBox.ItemsSource = _locations;
            if (ReservationUserComboBox.SelectedIndex < 0 && _users.Any()) ReservationUserComboBox.SelectedIndex = 0;
            if (ReservationLocationComboBox.SelectedIndex < 0 && _locations.Any()) ReservationLocationComboBox.SelectedIndex = 0;
        }

        private void RefreshResourceList()
        {
            if (!(ResourcesLocationComboBox.SelectedItem is Location location))
            {
                ResourcesDataGrid.ItemsSource = null;
                return;
            }

            List<Resource> resources;
            switch (ResourcesTypeComboBox.SelectedIndex)
            {
                case 1:
                    resources = _facade.GetDesksByLocation(location.Id);
                    break;
                case 2:
                    resources = _facade.GetMeetingRoomsByLocation(location.Id);
                    break;
                case 3:
                    resources = _facade.GetAvailableResources(location.Id);
                    break;
                default:
                    resources = _facade.GetResourcesByLocation(location.Id);
                    break;
            }

            ResourcesDataGrid.ItemsSource = (resources ?? new List<Resource>()).Select(ToGridItem).ToList();
        }

        private ResourceGridItem ToGridItem(Resource resource)
        {
            var details = resource.Description ?? string.Empty;
            if (resource is MeetingRoom mr)
                details = $"Kapacitet: {mr.Capacity}; Projektor: {YesNo(mr.HasProjector)}; TV: {YesNo(mr.HasTV)}; Tabla: {YesNo(mr.HasWhiteboard)}; Online oprema: {YesNo(mr.HasOnlineMeetingEquipment)}";
            else if (resource is Desk desk)
                details = $"Tip stola: {desk.SubType}";
            else if (resource is PrivateOffice office)
                details = $"Kancelarija, kapacitet: {office.Capacity}";

            return new ResourceGridItem
            {
                Id = resource.Id,
                LocationId = resource.LocationId,
                Name = resource.Name,
                Type = resource.Type.ToString(),
                Details = details,
                IsAvailable = resource.IsAvailable,
                Source = resource
            };
        }

        private void RefreshReportPreview()
        {
            try
            {
                var range = GetSelectedReportRange();
                var report = _facade.GetReportByDateRange(range.from, range.to);
                ReportsDataGrid.ItemsSource = report;
                ReportStatusTextBlock.Text = $"Pregled za period {range.from:dd.MM.yyyy HH:mm} - {range.to:dd.MM.yyyy HH:mm}. Ukupno stavki: {report.Count}.";
            }
            catch (Exception ex)
            {
                ReportStatusTextBlock.Text = $"Greška pri učitavanju izveštaja: {ex.Message}";
            }
        }

        private (DateTime from, DateTime to) GetSelectedReportRange()
        {
            var from = (ReportFromDatePicker.SelectedDate ?? DateTime.Today).Date;
            var toDate = (ReportToDatePicker.SelectedDate ?? DateTime.Today).Date.AddDays(1);

            if (toDate <= from)
                throw new InvalidOperationException("Datum 'do' mora biti veći od datuma 'od'.");

            return (from, toDate);
        }

        private void RefreshReservationLists()
        {
            if (ReservationUserComboBox.SelectedItem is User user)
            {
                var userReservations = _facade.GetUserReservations(user.Id) ?? new List<Reservation>();
                UserReservationsDataGrid.ItemsSource = userReservations.Select(ToReservationGridItem).ToList();
            }
            else
            {
                UserReservationsDataGrid.ItemsSource = null;
            }

            if (ReservationLocationComboBox.SelectedItem is Location location)
            {
                var date = ReservationDayPicker.SelectedDate ?? DateTime.Today;
                var dayReservations = _facade.GetReservationsByDateAndLocation(date, location.Id) ?? new List<Reservation>();
                DayReservationsDataGrid.ItemsSource = dayReservations.Select(ToReservationGridItem).ToList();
            }
            else
            {
                DayReservationsDataGrid.ItemsSource = null;
            }
        }

        private ReservationGridItem ToReservationGridItem(Reservation reservation)
        {
            var user = reservation.User ?? _users.FirstOrDefault(u => u.Id == reservation.UserId);
            var resource = reservation.Resource ?? TryGetResource(reservation.ResourceId);

            return new ReservationGridItem
            {
                Id = reservation.Id,
                UserId = reservation.UserId,
                ResourceId = reservation.ResourceId,
                UserDisplay = user != null ? $"{user.FullName} ({user.Email})" : $"User #{reservation.UserId}",
                ResourceDisplay = resource != null ? $"{resource.Name} [{resource.Type}]" : $"Resource #{reservation.ResourceId}",
                StartDateTime = reservation.StartDateTime,
                EndDateTime = reservation.EndDateTime,
                Status = reservation.Status.ToString(),
                Source = reservation
            };
        }

        private Resource TryGetResource(int id)
        {
            try { return _facade.GetResource(id); }
            catch { return null; }
        }

        private static string YesNo(bool value) => value ? "Da" : "Ne";

        private T RequireSelection<T>(object selectedItem, string message) where T : class
        {
            var item = selectedItem as T;
            if (item == null)
            {
                MessageBox.Show(message, "Izbor je obavezan", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }
            return item;
        }

        private void RefreshAllButton_Click(object sender, RoutedEventArgs e) => RefreshAll();

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Ovaj GUI koristi CoworkingFacade kao jedinu ulaznu tačku prema backend-u.\n\n" +
                "Dodat je i pregled + CSV eksport izveštaja sa mogućnošću automatskog generisanja na intervalu 1h, 5h, 1d, 1w ili 1M.",
                "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void UserFilterChanged(object sender, EventArgs e)
        {
            if (_users != null)
                UsersDataGrid.ItemsSource = ApplyUserFilters(_users);
        }

        private void ResetUserFilters_Click(object sender, RoutedEventArgs e)
        {
            UserSearchTextBox.Text = string.Empty;
            UserLocationFilterComboBox.SelectedIndex = 0;
            UserMembershipFilterComboBox.SelectedIndex = 0;
            UserStatusFilterComboBox.SelectedIndex = 0;
        }

        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new UserDialog(_membershipTypes, null) { Owner = this };
            if (dialog.ShowDialog() == true)
            {
                _facade.AddUser(dialog.User);
                RefreshUsers();
                RefreshReservationTabLookups();
            }
        }

        private void EditUserButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = RequireSelection<User>(UsersDataGrid.SelectedItem, "Izaberi korisnika.");
            if (selected == null) return;
            var dialog = new UserDialog(_membershipTypes, CloneUser(selected)) { Owner = this };
            if (dialog.ShowDialog() == true)
            {
                _facade.UpdateUser(dialog.User);
                RefreshUsers();
                RefreshReservationTabLookups();
            }
        }

        private void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = RequireSelection<User>(UsersDataGrid.SelectedItem, "Izaberi korisnika.");
            if (selected == null) return;
            if (MessageBox.Show($"Da li želiš da obrišeš korisnika {selected.FullName}?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _facade.DeleteUser(selected.Id);
                RefreshUsers();
                RefreshReservationTabLookups();
            }
        }

        private void LocationsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateLocationStatsPanel(LocationsDataGrid.SelectedItem as Location);

        private void AddLocationButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new LocationDialog(null) { Owner = this };
            if (dialog.ShowDialog() == true)
            {
                _facade.AddLocation(dialog.Location);
                RefreshLocations();
                RefreshResourceTabLookups();
                RefreshReservationTabLookups();
            }
        }

        private void EditLocationButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = RequireSelection<Location>(LocationsDataGrid.SelectedItem, "Izaberi lokaciju.");
            if (selected == null) return;
            var dialog = new LocationDialog(CloneLocation(selected)) { Owner = this };
            if (dialog.ShowDialog() == true)
            {
                _facade.UpdateLocation(dialog.Location);
                RefreshLocations();
                RefreshResourceTabLookups();
                RefreshReservationTabLookups();
            }
        }

        private void DeleteLocationButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = RequireSelection<Location>(LocationsDataGrid.SelectedItem, "Izaberi lokaciju.");
            if (selected == null) return;
            if (MessageBox.Show($"Obrisati lokaciju {selected.Name}?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _facade.DeleteLocation(selected.Id);
                RefreshLocations();
                RefreshResourceTabLookups();
                RefreshReservationTabLookups();
            }
        }

        private void AddMembershipButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new MembershipDialog(null) { Owner = this };
            if (dialog.ShowDialog() == true)
            {
                _facade.AddMembershipType(dialog.MembershipType);
                RefreshMemberships();
                RefreshUsers();
            }
        }

        private void EditMembershipButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = RequireSelection<MembershipType>(MembershipsDataGrid.SelectedItem, "Izaberi tip članstva.");
            if (selected == null) return;
            var dialog = new MembershipDialog(CloneMembership(selected)) { Owner = this };
            if (dialog.ShowDialog() == true)
            {
                _facade.UpdateMembershipType(dialog.MembershipType);
                RefreshMemberships();
                RefreshUsers();
            }
        }

        private void DeleteMembershipButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = RequireSelection<MembershipType>(MembershipsDataGrid.SelectedItem, "Izaberi tip članstva.");
            if (selected == null) return;
            if (MessageBox.Show($"Obrisati članstvo {selected.Name}?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _facade.DeleteMembershipType(selected.Id);
                RefreshMemberships();
                RefreshUsers();
            }
        }

        private void ResourcesFilterChanged(object sender, EventArgs e) => RefreshResourceList();

        private void AddResourceButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_locations.Any())
            {
                MessageBox.Show("Prvo dodaj lokaciju.");
                return;
            }
            var selectedLocation = ResourcesLocationComboBox.SelectedItem as Location ?? _locations.First();
            var dialog = new ResourceDialog(_locations, selectedLocation.Id, null) { Owner = this };
            if (dialog.ShowDialog() == true)
            {
                _facade.AddResource(dialog.Resource);
                RefreshResourceList();
                RefreshLocations();
            }
        }

        private void EditResourceButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = RequireSelection<ResourceGridItem>(ResourcesDataGrid.SelectedItem, "Izaberi resurs.");
            if (selected == null) return;
            var dialog = new ResourceDialog(_locations, selected.LocationId, CloneResource(selected.Source)) { Owner = this };
            if (dialog.ShowDialog() == true)
            {
                _facade.UpdateResource(dialog.Resource);
                RefreshResourceList();
                RefreshLocations();
            }
        }

        private void ToggleAvailabilityButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = RequireSelection<ResourceGridItem>(ResourcesDataGrid.SelectedItem, "Izaberi resurs.");
            if (selected == null) return;
            _facade.UpdateResourceAvailability(selected.Id, !selected.IsAvailable);
            RefreshResourceList();
        }

        private void DeleteResourceButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = RequireSelection<ResourceGridItem>(ResourcesDataGrid.SelectedItem, "Izaberi resurs.");
            if (selected == null) return;
            if (MessageBox.Show($"Obrisati resurs {selected.Name}?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _facade.DeleteResource(selected.Id);
                RefreshResourceList();
                RefreshLocations();
            }
        }

        private void ReservationFilterChanged(object sender, EventArgs e) => RefreshReservationLists();

        private void AddReservationButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_users.Any() || !_locations.Any())
            {
                MessageBox.Show("Potrebni su korisnici i lokacije pre kreiranja rezervacije.");
                return;
            }
            var dialog = new ReservationDialog(_facade, _users, _locations, null) { Owner = this };
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    _facade.CreateReservation(
                        new ReservationBuilder()
                            .ForUser(dialog.UserId)
                            .ForResource(dialog.ResourceId)
                            .From(dialog.StartDateTime)
                            .To(dialog.EndDateTime));
                    MessageBox.Show("Rezervacija uspešno kreirana.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void EditReservationButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = RequireSelection<ReservationGridItem>(UserReservationsDataGrid.SelectedItem ?? DayReservationsDataGrid.SelectedItem, "Izaberi rezervaciju.");
            if (selected == null) return;
            var dialog = new ReservationDialog(_facade, _users, _locations, CloneReservation(selected.Source)) { Owner = this };
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    var reservation = dialog.Reservation;
                    _facade.UpdateReservation(reservation);
                    MessageBox.Show("Rezervacija uspešno izmenjena.");
                    RefreshReservationLists();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CancelReservationButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = RequireSelection<ReservationGridItem>(UserReservationsDataGrid.SelectedItem ?? DayReservationsDataGrid.SelectedItem, "Izaberi rezervaciju.");
            if (selected == null) return;
            if (MessageBox.Show($"Otkaži rezervaciju #{selected.Id}?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _facade.CancelReservation(selected.Id);
                RefreshReservationLists();
            }
        }

        private void OnReservationCreated(Reservation reservation) => Dispatcher.Invoke(() => { RefreshReservationLists(); RefreshResourceList(); RefreshLocations(); });
        private void OnReservationUpdated(Reservation reservation) => Dispatcher.Invoke(() => RefreshReservationLists());
        private void OnReservationCancelled(Reservation reservation) => Dispatcher.Invoke(() => { RefreshReservationLists(); RefreshResourceList(); RefreshLocations(); });

        private static User CloneUser(User user) => new User
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Phone = user.Phone,
            MembershipTypeId = user.MembershipTypeId,
            MembershipStartDate = user.MembershipStartDate,
            MembershipEndDate = user.MembershipEndDate,
            Status = user.Status
        };

        private static Location CloneLocation(Location location) => new Location
        {
            Id = location.Id,
            Name = location.Name,
            Address = location.Address,
            City = location.City,
            WorkingHours = location.WorkingHours,
            MaxCapacity = location.MaxCapacity,
            Description = location.Description
        };

        private static MembershipType CloneMembership(MembershipType membership) => new MembershipType
        {
            Id = membership.Id,
            Name = membership.Name,
            Price = membership.Price,
            DurationDays = membership.DurationDays,
            MaxReservationHoursPerMonth = membership.MaxReservationHoursPerMonth,
            IncludesMeetingRooms = membership.IncludesMeetingRooms,
            MeetingRoomHoursPerMonth = membership.MeetingRoomHoursPerMonth
        };

        private static Resource CloneResource(Resource resource)
        {
            if (resource is MeetingRoom mr)
            {
                return new MeetingRoom
                {
                    Id = mr.Id,
                    LocationId = mr.LocationId,
                    Name = mr.Name,
                    Type = mr.Type,
                    Description = mr.Description,
                    IsAvailable = mr.IsAvailable,
                    Capacity = mr.Capacity,
                    HasProjector = mr.HasProjector,
                    HasTV = mr.HasTV,
                    HasWhiteboard = mr.HasWhiteboard,
                    HasOnlineMeetingEquipment = mr.HasOnlineMeetingEquipment
                };
            }
            if (resource is PrivateOffice office)
            {
                return new PrivateOffice
                {
                    Id = office.Id,
                    LocationId = office.LocationId,
                    Name = office.Name,
                    Type = office.Type,
                    Description = office.Description,
                    IsAvailable = office.IsAvailable,
                    RoomNumber = office.RoomNumber,
                    Capacity = office.Capacity
                };
            }
            if (resource is Desk desk)
            {
                return new Desk
                {
                    Id = desk.Id,
                    LocationId = desk.LocationId,
                    Name = desk.Name,
                    Type = desk.Type,
                    Description = desk.Description,
                    IsAvailable = desk.IsAvailable,
                    SubType = desk.SubType
                };
            }
            throw new InvalidOperationException("Nepoznat tip resursa.");
        }

        private static Reservation CloneReservation(Reservation reservation) => new Reservation
        {
            Id = reservation.Id,
            UserId = reservation.UserId,
            ResourceId = reservation.ResourceId,
            StartDateTime = reservation.StartDateTime,
            EndDateTime = reservation.EndDateTime,
            Status = reservation.Status
        };
        private void PreviewReportButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RefreshReportPreview();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExportCsvButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var range = GetSelectedReportRange();
                var folder = string.IsNullOrWhiteSpace(ReportFolderTextBox.Text) ? null : ReportFolderTextBox.Text.Trim();
                var path = _facade.ExportReportToCsv(range.from, range.to, folder);
                RefreshReportPreview();
                ReportStatusTextBlock.Text = $"CSV uspešno kreiran: {path}";
                MessageBox.Show($"CSV izveštaj je sačuvan na putanji:\n{path}", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void StartAutoReportButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedKey = ReportIntervalComboBox.SelectedItem as string;
                if (string.IsNullOrWhiteSpace(selectedKey) || !_reportIntervals.ContainsKey(selectedKey))
                    throw new InvalidOperationException("Izaberi interval za automatski eksport.");

                var folder = string.IsNullOrWhiteSpace(ReportFolderTextBox.Text) ? null : ReportFolderTextBox.Text.Trim();
                _facade.StartAutomaticCsvReporting(_reportIntervals[selectedKey], folder);
                ReportStatusTextBlock.Text = $"Automatski CSV eksport je pokrenut. Interval: {selectedKey}. Folder: {folder ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "reports")}";
                MessageBox.Show("Automatski CSV eksport je pokrenut.", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void StopAutoReportButton_Click(object sender, RoutedEventArgs e)
        {
            _facade.StopAutomaticCsvReporting();
            ReportStatusTextBlock.Text = "Automatski CSV eksport je zaustavljen.";
            MessageBox.Show("Automatski CSV eksport je zaustavljen.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }
}
