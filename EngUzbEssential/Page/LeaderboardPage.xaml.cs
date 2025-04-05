using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace EngUzbEssential.Page
{
    /// <summary>
    /// Interaction logic for LeaderboardPage.xaml
    /// </summary>
    public partial class LeaderboardPage : System.Windows.Controls.Page
    {
        // Current selected leaderboard type
        private string currentLeaderboard = "Overall";

        public LeaderboardPage()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during LeaderboardPage initialization: {ex.Message}\n\nStack trace: {ex.StackTrace}", 
                               "Critical Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LeaderboardPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // Make sure the Overall tab is selected by default
                if (OverallTab != null)
                {
                    OverallTab.IsChecked = true;
                }

                // Load initial leaderboard data
                LoadLeaderboardData(currentLeaderboard);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during LeaderboardPage load: {ex.Message}", 
                              "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LeaderboardTab_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get the selected tab
                if (sender is RadioButton radioButton && radioButton.Tag != null)
                {
                    string tabName = radioButton.Tag.ToString();
                    
                    // Update current leaderboard and reload data
                    currentLeaderboard = tabName;
                    LoadLeaderboardData(currentLeaderboard);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in LeaderboardTab_Checked: {ex.Message}");
            }
        }

        private void LoadLeaderboardData(string leaderboardType)
        {
            // In a real application, this would load data from a database or API
            // For now, we just have the static data defined in the XAML
            
            // You could dynamically replace the entries in LeaderboardList based on the selected tab
            // For example:
            switch (leaderboardType)
            {
                case "Weekly":
                    // Update displayed data for weekly leaderboard
                    // This would be implemented in a real application
                    break;
                    
                case "Monthly":
                    // Update displayed data for monthly leaderboard
                    // This would be implemented in a real application
                    break;
                    
                case "Overall":
                default:
                    // Current default view is already the Overall leaderboard 
                    break;
            }
        }
    }
} 