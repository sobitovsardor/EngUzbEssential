using System;
using System.Windows;
using System.Windows.Controls;

namespace EngUzbEssential.Page
{
    /// <summary>
    /// Interaction logic for GamePage.xaml
    /// </summary>
    public partial class GamePage : System.Windows.Controls.Page
    {
        public GamePage()
        {
            InitializeComponent();
        }
        
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Navigate back to home
                if (Application.Current.MainWindow is MainWindow mainWindow)
                {
                    mainWindow.NavigateToHome();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Navigation error: {ex.Message}", 
                               "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
} 