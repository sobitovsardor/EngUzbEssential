using System.Windows;
using System.Windows.Controls;

namespace EngUzbEssential.Page
{
    /// <summary>
    /// Interaction logic for UserProfilePage.xaml
    /// </summary>
    public partial class UserProfilePage : System.Windows.Controls.Page
    {
        public UserProfilePage()
        {
            InitializeComponent();
        }
        
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the main window
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                // Navigate back to home
                mainWindow.NavigateToHome();
            }
        }
    }
} 