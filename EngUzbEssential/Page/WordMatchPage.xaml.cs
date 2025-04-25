using EngUzbEssential.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EngUzbEssential.Desktop.Page
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
