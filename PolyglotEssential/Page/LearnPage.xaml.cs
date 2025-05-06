using System;
using System.Windows;
using System.Windows.Controls;

namespace PolyglotEssential.Page
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
            try
            {
                // Get the main window
                if (Application.Current.MainWindow is MainWindow mainWindow)
                {
                    // Call the NavigateToHome method to properly restore UI
                    mainWindow.NavigateToHome();
                    return;
                }
                
                // Fallback to using NavigationService if available
                if (this.NavigationService != null && this.NavigationService.CanGoBack)
                {
                    this.NavigationService.GoBack();
                    return;
                }
                
                // No navigation options worked
                MessageBox.Show("Could not navigate back. Please restart the application.", 
                    "Navigation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating back: {ex.Message}", "Navigation Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CategoryButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("CategoryButton_Click triggered!");
            try
            {
                // Get the main window
                if (Application.Current.MainWindow is MainWindow mainWindow)
                {
                    // Navigate to LearnLevelPage
                    mainWindow.NavigateToLearnLevel();
                    return;
                }
                
                // Fallback to using NavigationService if available
                if (this.NavigationService != null)
                {
                    this.NavigationService.Navigate(new LearnLevelPage());
                    return;
                }
                
                // No navigation options worked
                MessageBox.Show("Could not navigate to level selection. Please restart the application.", 
                    "Navigation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to level selection: {ex.Message}", "Navigation Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
} 
