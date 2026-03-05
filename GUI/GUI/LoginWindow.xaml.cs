using GUI;
using System.Windows;
using System.Windows.Controls;

namespace GUI
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        public void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text;
            string password = PasswordBox.Password;

            // privremena provera (kasnije ide baza)
            if (username == "admin" && password == "admin")
            {
                MainWindow main = new MainWindow();
                main.Show();

                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password");
            }
        }
    }
}