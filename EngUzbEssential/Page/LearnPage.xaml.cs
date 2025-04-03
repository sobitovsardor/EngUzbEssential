using System.Windows;
using System.Windows.Controls;

namespace EngUzbEssential.Page
{
    /// <summary>
    /// Interaction logic for LearnPage.xaml
    /// </summary>
    public partial class LearnPage : System.Windows.Controls.Page
    {
        public LearnPage()
        {
            InitializeComponent();
        }
        
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the main window
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                // Call the HomeButton_Click method
                mainWindow.NavigateToHome();
            }
        }
    }
} 