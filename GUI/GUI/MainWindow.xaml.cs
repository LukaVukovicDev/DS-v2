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

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Title = ConfigManager.Instance.ChainName;
            DataContext = new MainWindowViewModel();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // ovde ce kasnije ici logika za search
        }

        private void Filter_Changed(object sender, SelectionChangedEventArgs e)
        {
            // ovde ce kasnije ici logika za filter
        }

        public void Logout_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();

            this.Close();
        }
    }


}