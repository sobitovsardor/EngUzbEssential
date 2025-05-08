using PolyglotEssential.Page;
using System.Windows;
using System.Windows.Navigation;

namespace PolyglotEssential.Desktop.Page
{
    /// <summary>
    /// Interaction logic for WordMatchPage.xaml
    /// </summary>
    public partial class WordMatchPage : System.Windows.Controls.Page
    {
        public WordMatchPage()
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

        private void AnimalsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new WordMatchLevelPage());
        }

        private void FoodButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new WordMatchLevelPage());
        }

        private void ColorsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new WordMatchLevelPage());
        }

        private void NumbersButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new WordMatchLevelPage());
        }

        private void FamilyButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new WordMatchLevelPage());
        }

        private void WeatherButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new WordMatchLevelPage());
        }
    }
}
