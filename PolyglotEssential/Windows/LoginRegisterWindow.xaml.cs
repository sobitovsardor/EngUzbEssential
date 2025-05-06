using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace PolyglotEssential.Windows
{
    /// <summary>
    /// Interaction logic for LoginRegisterWindow.xaml
    /// </summary>
    public partial class LoginRegisterWindow : Window
    {
        public LoginRegisterWindow()
        {
            InitializeComponent();

            // Initialize with Login tab selected
            LoginTabButton.Foreground = new SolidColorBrush(Color.FromRgb(66, 133, 244));
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        private void LoginTabButton_Click(object sender, RoutedEventArgs e)
        {
            // Update tab indicators
            ShowLoginTab();
        }

        private void RegisterTabButton_Click(object sender, RoutedEventArgs e)
        {
            // Update tab indicators
            ShowRegisterTab();
        }

        private void ShowLoginTab()
        {
            // Change tab colors
            LoginTabButton.Foreground = new SolidColorBrush(Color.FromRgb(66, 133, 244));
            RegisterTabButton.Foreground = new SolidColorBrush(Color.FromRgb(102, 106, 122));

            // Animate tab indicators
            DoubleAnimation fadeIn = new(1, TimeSpan.FromSeconds(0.3));
            DoubleAnimation fadeOut = new(0, TimeSpan.FromSeconds(0.3));
            LoginTabIndicator.BeginAnimation(Border.OpacityProperty, fadeIn);
            RegisterTabIndicator.BeginAnimation(Border.OpacityProperty, fadeOut);

            // Show/hide appropriate panels
            LoginPanel.Visibility = Visibility.Visible;
            RegisterPanel.Visibility = Visibility.Collapsed;
        }

        private void ShowRegisterTab()
        {
            // Change tab colors
            LoginTabButton.Foreground = new SolidColorBrush(Color.FromRgb(102, 106, 122));
            RegisterTabButton.Foreground = new SolidColorBrush(Color.FromRgb(66, 133, 244));

            // Animate tab indicators
            DoubleAnimation fadeIn = new(1, TimeSpan.FromSeconds(0.3));
            DoubleAnimation fadeOut = new(0, TimeSpan.FromSeconds(0.3));
            LoginTabIndicator.BeginAnimation(Border.OpacityProperty, fadeOut);
            RegisterTabIndicator.BeginAnimation(Border.OpacityProperty, fadeIn);

            // Show/hide appropriate panels
            LoginPanel.Visibility = Visibility.Collapsed;
            RegisterPanel.Visibility = Visibility.Visible;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Add login logic here
            string email = LoginEmail.Text;
            string password = LoginPassword.Password;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both email and password.", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // TODO: Implement actual authentication logic
            MessageBox.Show("Login successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            // Open the main window
            OpenMainWindow();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Add registration logic here
            string name = UserRegisterName.Text;
            string email = RegisterEmail.Text;
            string password = RegisterPassword.Password;
            string confirmPassword = RegisterConfirmPassword.Password;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Please fill out all fields.", "Registration Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.", "Registration Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // TODO: Implement actual registration logic
            MessageBox.Show("Registration successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            // Open the main window
            OpenMainWindow();
        }

        private void OpenMainWindow()
        {
            // Create and show the main window
            MainWindow mainWindow = new();
            mainWindow.Show();

            // Close the login window
            this.Close();
        }
    }
}
