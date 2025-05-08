using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PolyglotEssential.Page
{
    /// <summary>
    /// Interaction logic for WordMatchLevelTypePage.xaml
    /// </summary>
    public partial class WordMatchLevelTypePage : System.Windows.Controls.Page
    {
        // Selected options
        private bool isEngToUzb = true;
        private string selectedDifficulty = "Easy";
        private int levelNumber = 1;

        public WordMatchLevelTypePage()
        {
            InitializeComponent();

            // Loaded event ensures the visual tree is fully populated
            this.Loaded += (s, e) =>
            {
                try
                {
                    // Initialize radio buttons
                    InitializeRadioButtons();
                }
                catch
                {
                    // Fallback in case anything goes wrong
                }
            };
        }

        // Constructor with level parameter
        public WordMatchLevelTypePage(int level)
        {
            InitializeComponent();
            this.levelNumber = level;

            // Loaded event ensures the visual tree is fully populated
            this.Loaded += (s, e) =>
            {
                try
                {
                    // Update level title in UI
                    LevelTitle.Text = $"Level {level}";

                    // Initialize radio buttons
                    InitializeRadioButtons();
                }
                catch
                {
                    // Fallback in case anything goes wrong
                }
            };
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                // Make sure we're working with a Grid
                Grid parentGrid = sender as Grid;
                if (parentGrid == null) return;

                // Helper to find all TextBlocks (including nested)
                TextBlock FindMainLabel(DependencyObject obj)
                {
                    if (obj is TextBlock tb && tb.FontSize >= 16) // heuristic for main label
                        return tb;
                    int count = VisualTreeHelper.GetChildrenCount(obj);
                    for (int i = 0; i < count; i++)
                    {
                        var result = FindMainLabel(VisualTreeHelper.GetChild(obj, i));
                        if (result != null) return result;
                    }
                    return null;
                }

                TextBlock mainLabel = FindMainLabel(parentGrid);
                if (mainLabel == null) return;
                string text = mainLabel.Text ?? string.Empty;

                // Update selected options based on the text and determine group
                bool isLanguageGroup = false;
                bool isDifficultyGroup = false;

                if (parentGrid.Name == "EnglishToUzbekGrid")
                {
                    isEngToUzb = true;
                    isLanguageGroup = true;
                }
                else if (parentGrid.Name == "UzbekToEnglishGrid")
                {
                    isEngToUzb = false;
                    isLanguageGroup = true;
                }
                else if (parentGrid.Name == "EasyGrid")
                {
                    selectedDifficulty = "Easy";
                    isDifficultyGroup = true;
                }
                else if (parentGrid.Name == "MediumGrid")
                {
                    selectedDifficulty = "Medium";
                    isDifficultyGroup = true;
                }
                else if (parentGrid.Name == "ExpertGrid")
                {
                    selectedDifficulty = "Expert";
                    isDifficultyGroup = true;
                }

                // Reset other radio buttons in the same group using FindName
                if (isLanguageGroup)
                {
                    Grid engToUzbGrid = this.FindName("EnglishToUzbekGrid") as Grid;
                    Grid uzbToEngGrid = this.FindName("UzbekToEnglishGrid") as Grid;

                    if (engToUzbGrid != null && parentGrid != engToUzbGrid)
                        ResetRadioButton(engToUzbGrid, true);

                    if (uzbToEngGrid != null && parentGrid != uzbToEngGrid)
                        ResetRadioButton(uzbToEngGrid, true);
                }
                if (isDifficultyGroup)
                {
                    Grid easyGrid = this.FindName("EasyGrid") as Grid;
                    Grid mediumGrid = this.FindName("MediumGrid") as Grid;
                    Grid expertGrid = this.FindName("ExpertGrid") as Grid;

                    if (easyGrid != null && parentGrid != easyGrid)
                        ResetRadioButton(easyGrid, true);
                    if (mediumGrid != null && parentGrid != mediumGrid)
                        ResetRadioButton(mediumGrid, true);
                    if (expertGrid != null && parentGrid != expertGrid)
                        ResetRadioButton(expertGrid, true);
                }

                // Update visual selection for the clicked radio button
                SelectRadioButton(parentGrid);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error selecting option: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Helper method to reset a radio button
        private void ResetRadioButton(Grid grid, bool shouldReset)
        {
            if (grid == null || !shouldReset) return;
            try
            {
                // Find and reset the outer ellipse
                Ellipse outerEllipse = grid.Children.OfType<Ellipse>().FirstOrDefault();
                if (outerEllipse == null) return;
                outerEllipse.Fill = new SolidColorBrush(Colors.Transparent);
                outerEllipse.Stroke = new SolidColorBrush(Color.FromRgb(187, 187, 187)); // #BBBBBB

                // Remove inner ellipse if it exists
                var innerEllipses = grid.Children.OfType<Ellipse>().Skip(1).ToList();
                foreach (var ellipse in innerEllipses)
                {
                    grid.Children.Remove(ellipse);
                }

                // Recursively reset all TextBlocks
                void ResetTextBlocks(DependencyObject obj)
                {
                    if (obj is TextBlock tb)
                        tb.Foreground = new SolidColorBrush(Color.FromRgb(102, 106, 122)); // #666A7A
                    int count = VisualTreeHelper.GetChildrenCount(obj);
                    for (int i = 0; i < count; i++)
                        ResetTextBlocks(VisualTreeHelper.GetChild(obj, i));
                }
                ResetTextBlocks(grid);
            }
            catch { }
        }

        // Helper method to select a radio button
        private void SelectRadioButton(Grid grid)
        {
            if (grid == null) return;
            try
            {
                // Find and update the outer ellipse
                Ellipse outerEllipse = grid.Children.OfType<Ellipse>().FirstOrDefault();
                if (outerEllipse == null) return;
                outerEllipse.Fill = new SolidColorBrush(Color.FromRgb(209, 67, 75)); // #D1434B
                outerEllipse.Stroke = new SolidColorBrush(Colors.White);

                // Remove any existing inner ellipses first
                var existingInnerEllipses = grid.Children.OfType<Ellipse>().Skip(1).ToList();
                foreach (var ellipse in existingInnerEllipses)
                {
                    grid.Children.Remove(ellipse);
                }

                // Add inner ellipse
                Ellipse innerEllipse = new Ellipse
                {
                    Width = 10,
                    Height = 10,
                    Fill = new SolidColorBrush(Colors.White),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                grid.Children.Insert(1, innerEllipse);

                // Recursively set all TextBlocks to selected color
                void SetSelectedTextBlocks(DependencyObject obj)
                {
                    if (obj is TextBlock tb)
                        tb.Foreground = new SolidColorBrush(Color.FromRgb(36, 40, 74)); // #24284A
                    int count = VisualTreeHelper.GetChildrenCount(obj);
                    for (int i = 0; i < count; i++)
                        SetSelectedTextBlocks(VisualTreeHelper.GetChild(obj, i));
                }
                SetSelectedTextBlocks(grid);
            }
            catch { }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // Get time and points based on difficulty
            int timeSeconds = 240;
            int points = 1;

            switch (selectedDifficulty)
            {
                case "Medium":
                    timeSeconds = 120;
                    points = 2;
                    break;
                case "Expert":
                    timeSeconds = 60;
                    points = 3;
                    break;
            }

            // Optionally show confirmation (commented out)
            // MessageBox.Show($"Selected options:\nLevel: {levelNumber}\nDirection: {(isEngToUzb ? "English to Uzbek" : "Uzbek to English")}\nDifficulty: {selectedDifficulty}\nTime: {timeSeconds}s\nPoints: {points}pt", 
            //    "Selection Confirmed", MessageBoxButton.OK, MessageBoxImage.Information);

            // Navigate to the test page
            NavigationService.Navigate(new WordMatchLevelTestPage());
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        // Method to initialize radio buttons to their default state
        private void InitializeRadioButtons()
        {
            // Make sure default selections are properly displayed
            Grid engToUzbGrid = this.FindName("EnglishToUzbekGrid") as Grid;
            Grid easyGrid = this.FindName("EasyGrid") as Grid;

            if (engToUzbGrid != null)
            {
                SelectRadioButton(engToUzbGrid);
            }

            if (easyGrid != null)
            {
                SelectRadioButton(easyGrid);
            }
        }
    }
}
