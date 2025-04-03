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
            // Get the main window
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                // Navigate back to home
                mainWindow.NavigateToHome();
            }
        }
    }
} 