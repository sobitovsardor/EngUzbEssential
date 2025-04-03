// MainWindow.xaml.cs
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Linq;
using System.Windows.Controls;

namespace EngUzbEssential
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Create drop shadow effect and add to resources
            DropShadowEffect dropShadow = new DropShadowEffect
            {
                Color = Colors.Black,
                Direction = 270,
                ShadowDepth = 10,
                Opacity = 0.2,
                BlurRadius = 20
            };

            this.Resources.Add("DropShadow", dropShadow);
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Get Started button implementation
        }

        private void LearnButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to the Learn page
            MainFrame.Navigate(new Page.LearnPage());
            
            // Hide the home content
            HomeContent.Visibility = Visibility.Collapsed;
            
            // Hide the right blue panel (this is the key fix)
            var parentGrid = MainFrame.Parent as FrameworkElement;
            while (parentGrid != null && !(parentGrid is Grid grid && grid.ColumnDefinitions.Count > 1))
            {
                parentGrid = parentGrid.Parent as FrameworkElement;
            }
            
            if (parentGrid is Grid parentContentGrid)
            {
                // Find the right panel (column 1) and hide it
                var rightPanel = parentContentGrid.Children.Cast<UIElement>()
                    .FirstOrDefault(x => Grid.GetColumn(x) == 1);
                
                if (rightPanel != null)
                {
                    rightPanel.Visibility = Visibility.Collapsed;
                }
                
                // Make the main frame span both columns
                Grid.SetColumnSpan(MainFrame.Parent as UIElement, 2);
            }
            
            // Show the frame
            MainFrame.Visibility = Visibility.Visible;
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToHome();
        }
        
        // Public method for navigation to home from other pages
        public void NavigateToHome()
        {
            // Clear the frame
            MainFrame.Content = null;
            
            // Hide the frame
            MainFrame.Visibility = Visibility.Collapsed;
            
            // Restore the right panel and reset column span
            var parentGrid = MainFrame.Parent as FrameworkElement;
            while (parentGrid != null && !(parentGrid is Grid grid && grid.ColumnDefinitions.Count > 1))
            {
                parentGrid = parentGrid.Parent as FrameworkElement;
            }
            
            if (parentGrid is Grid parentContentGrid)
            {
                // Find the right panel (column 1) and show it
                var rightPanel = parentContentGrid.Children.Cast<UIElement>()
                    .FirstOrDefault(x => Grid.GetColumn(x) == 1);
                
                if (rightPanel != null)
                {
                    rightPanel.Visibility = Visibility.Visible;
                }
                
                // Reset column span of the left content panel
                Grid.SetColumnSpan(MainFrame.Parent as UIElement, 1);
            }
            
            // Show the home content
            HomeContent.Visibility = Visibility.Visible;
        }

        private void GamesButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to the Games page
            MainFrame.Navigate(new Page.GamePage());
            
            // Hide the home content
            HomeContent.Visibility = Visibility.Collapsed;
            
            // Hide the right blue panel (this is the key fix)
            var parentGrid = MainFrame.Parent as FrameworkElement;
            while (parentGrid != null && !(parentGrid is Grid grid && grid.ColumnDefinitions.Count > 1))
            {
                parentGrid = parentGrid.Parent as FrameworkElement;
            }
            
            if (parentGrid is Grid parentContentGrid)
            {
                // Find the right panel (column 1) and hide it
                var rightPanel = parentContentGrid.Children.Cast<UIElement>()
                    .FirstOrDefault(x => Grid.GetColumn(x) == 1);
                
                if (rightPanel != null)
                {
                    rightPanel.Visibility = Visibility.Collapsed;
                }
                
                // Make the main frame span both columns
                Grid.SetColumnSpan(MainFrame.Parent as UIElement, 2);
            }
            
            // Show the frame
            MainFrame.Visibility = Visibility.Visible;
        }
    }
}