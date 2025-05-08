using PolyglotEssential.Page;
using System.Windows;

namespace PolyglotEssential.Desktop.Page
{
    /// <summary>
    /// Interaction logic for WordMatchLevelTestResultPage.xaml
    /// </summary>
    public partial class WordMatchLevelTestResultPage : System.Windows.Controls.Page
    {
        public WordMatchLevelTestResultPage()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new WordMatchLevelPage());
        }

        private void RewardsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new WordMatchLevelPage());
        }
    }
}
