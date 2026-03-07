using System;
using System.Collections.Generic;
using CoworkingApp.BusinessLogic.Builders;
using CoworkingApp.BusinessLogic.Database;
using CoworkingApp.BusinessLogic.Models;
using CoworkingApp.BusinessLogic.Repositories;

namespace CoworkingApp.BusinessLogic.Services
{
    public class CoworkingFacade
    {
        // Singleton — only one Facade exists
        private static CoworkingFacade _instance;
        private static readonly object _lock = new object();

        // Services
        private UserService _userService;
        private LocationService _locationService;
        private ResourceService _resourceService;
        private ReservationService _reservationService;

        // Observer events — GUI subscribes to these
        public event Action<Reservation> ReservationCreated;
        public event Action<Reservation> ReservationUpdated;
        public event Action<Reservation> ReservationCancelled;

        private CoworkingFacade(string connectionString)
        {
            // Initialize database connection
            DatabaseConnection.GetInstance(connectionString);

            // Create repositories
            var userRepo = new UserRepository();
            var locationRepo = new LocationRepository();
            var resourceRepo = new ResourceRepository();
            var reservationRepo = new ReservationRepository();
            var membershipRepo = new MembershipTypeRepository();

            // Create services
            _userService = new UserService(userRepo);
            _locationService = new LocationService(locationRepo);
            _resourceService = new ResourceService(resourceRepo);
            _reservationService = new ReservationService(
                reservationRepo, resourceRepo, locationRepo, userRepo
            );

            // Forward reservation events to facade events
            _reservationService.ReservationCreated += (r) => ReservationCreated?.Invoke(r);
            _reservationService.ReservationUpdated += (r) => ReservationUpdated?.Invoke(r);
            _reservationService.ReservationCancelled += (r) => ReservationCancelled?.Invoke(r);
        }

        public static CoworkingFacade GetInstance(string connectionString = null)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new CoworkingFacade(connectionString);
                    }
                }
            }
            return _instance;
        }

        // ─── Users ────────────────────────────────────────────
        public List<User> GetAllUsers() => _userService.GetAll();
        public List<User> GetUsersByMembershipType(int membershipTypeId) => _userService.GetByMembershipType(membershipTypeId);
        public List<User> GetUsersByStatus(AccountStatus status) => _userService.GetByStatus(status);
        public List<User> GetUsersByLocation(int locationId) => _userService.GetByLocation(locationId);
        public User GetUser(int id) => _userService.GetById(id);
        public void AddUser(User user) => _userService.AddUser(user);
        public void UpdateUser(User user) => _userService.UpdateUser(user);
        public void DeleteUser(int id) => _userService.DeleteUser(id);

        // ─── Locations ────────────────────────────────────────
        public List<Location> GetAllLocations() => _locationService.GetAll();
        public Location GetLocation(int id) => _locationService.GetById(id);
        public LocationStats GetLocationStats(int locationId) => _locationService.GetLocationStats(locationId);
        public void AddLocation(Location location) => _locationService.AddLocation(location);
        public void UpdateLocation(Location location) => _locationService.UpdateLocation(location);
        public void DeleteLocation(int id) => _locationService.DeleteLocation(id);

        // ─── Resources ────────────────────────────────────────
        public List<Resource> GetResourcesByLocation(int locationId) => _resourceService.GetByLocation(locationId);
        public List<Resource> GetAvailableResources(int locationId) => _resourceService.GetAvailableByLocation(locationId);
        public List<Resource> GetDesksByLocation(int locationId) => _resourceService.GetDesksByLocation(locationId);
        public List<Resource> GetMeetingRoomsByLocation(int locationId) => _resourceService.GetMeetingRoomsByLocation(locationId);
        public Resource GetResource(int id) => _resourceService.GetById(id);
        public void AddResource(Resource resource) => _resourceService.AddResource(resource);
        public void UpdateResource(Resource resource) => _resourceService.UpdateResource(resource);
        public void DeleteResource(int id) => _resourceService.DeleteResource(id);
        public void UpdateResourceAvailability(int resourceId, bool isAvailable) => _resourceService.UpdateAvailability(resourceId, isAvailable);

        // ─── Reservations ─────────────────────────────────────
        public void CreateReservation(ReservationBuilder builder) => _reservationService.CreateReservation(builder);
        public void UpdateReservation(Reservation reservation) => _reservationService.UpdateReservation(reservation);
        public void CancelReservation(int id) => _reservationService.CancelReservation(id);
        public List<Reservation> GetUserReservations(int userId) => _reservationService.GetUserReservations(userId);
        public List<Reservation> GetReservationsByDateAndLocation(DateTime date, int locationId) => _reservationService.GetReservationsByDateAndLocation(date, locationId);
    }
}
