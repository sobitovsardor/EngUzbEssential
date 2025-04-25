using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Media;

namespace EngUzbEssential.Desktop.Page
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
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        private void WordMatchButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new WordMatchPage());
        }

        private void FlashcardsButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Flashcards game coming soon!", "Coming Soon", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void WordPuzzleButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Word Puzzle game coming soon!", "Coming Soon", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ListeningChallengeButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Listening Challenge game coming soon!", "Coming Soon", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SpellingBeeButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Spelling Bee game coming soon!", "Coming Soon", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void TranslationRaceButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Translation Race game coming soon!", "Coming Soon", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
} 