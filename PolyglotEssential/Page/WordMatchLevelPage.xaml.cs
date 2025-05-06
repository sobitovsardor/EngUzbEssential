using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace PolyglotEssential.Page
{
    /// <summary>
    /// Interaction logic for WordMatchLevelPage.xaml
    /// </summary>
    public partial class WordMatchLevelPage : System.Windows.Controls.Page
    {
        public WordMatchLevelPage()
        {
            InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        private void LevelButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            string levelText = ((TextBlock)((StackPanel)clickedButton.Content).Children[0]).Text;
            int levelNumber = int.Parse(levelText.Replace("Level ", ""));

            // Navigate to WordMatchLevelTypePage with the selected level number
            NavigationService.Navigate(new WordMatchLevelTypePage(levelNumber));
        }
    }
}
