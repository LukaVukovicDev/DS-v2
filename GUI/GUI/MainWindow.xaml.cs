using GUI.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private ICollectionView usersView;
        private List<User> allUsers = new List<User>();
        public MainWindow()
        {
            InitializeComponent();
            this.Title = ConfigManager.Instance.ChainName;
            DataContext = new MainWindowViewModel();

            usersView = CollectionViewSource.GetDefaultView(
    ((MainWindowViewModel)DataContext).Users
);
            usersView.Filter = FilterUsers;
        }

        private bool FilterUsers(object obj)
        {
            if (SearchBox == null)
                return true;
            if (obj is not User user)
                return false;

            // SEARCH
            string searchText = SearchBox.Text?.ToLower() ?? "";

            if (!string.IsNullOrEmpty(searchText))
            {
                if (!user.Name.ToLower().Contains(searchText) &&
                    !user.Email.ToLower().Contains(searchText))
                    return false;
            }

            // MEMBERSHIP
            string membership = "All";
            if (MembershipFilter.SelectedItem is ComboBoxItem m)
                membership = m.Content.ToString();

            if (membership != "All")
            {
                if (user.MembershipType != membership)
                    return false;
            }

            // STATUS
            string status = "All";
            if (StatusFilter.SelectedItem is ComboBoxItem s)
                status = s.Content.ToString();

            if (status != "All")
            {
                if (user.Status != status)
                    return false;
            }

            return true;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (usersView != null)
                usersView.Refresh();

        }

        private void Filter_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (usersView != null)
                usersView.Refresh();

            
        }

        public void Logout_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();

            this.Close();
        }
    }


}