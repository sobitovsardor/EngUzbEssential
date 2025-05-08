using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace PolyglotEssential.Page
{
    /// <summary>
    /// Interaction logic for LearnLevelPage.xaml
    /// </summary>
    public partial class LearnLevelPage : System.Windows.Controls.Page
    {
        public LearnLevelPage()
        {
            InitializeComponent();
            this.Loaded += Page_Loaded; // Add loaded event handler for animations
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Try navigating back using NavigationService first
                if (this.NavigationService != null && this.NavigationService.CanGoBack)
                {
                    this.NavigationService.GoBack();
                    return;
                }

                // Fallback: Try navigating to LearnPage via MainWindow if NavigationService fails
                if (Application.Current.MainWindow is MainWindow mainWindow)
                {
                    // Assuming LearnPage is where we came from
                    mainWindow.MainFrame.Navigate(new LearnPage());
                    // You might want to restore layout here if necessary, similar to NavigateToHome
                    return;
                }

                // No navigation options worked
                MessageBox.Show("Could not navigate back.", "Navigation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating back: {ex.Message}", "Navigation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LevelButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Content is StackPanel stackPanel)
            {
                // Find the TextBlock containing the level number (assuming it's the first TextBlock)
                var textBlock = stackPanel.Children.OfType<TextBlock>().FirstOrDefault();
                if (textBlock != null)
                {
                    string levelText = textBlock.Text; // e.g., "Level 1"

                    // Extract level number
                    int levelNumber;
                    if (int.TryParse(levelText.Replace("Level ", ""), out levelNumber))
                    {
                        try
                        {
                            // Create a new LearnWordPage and navigate to it
                            var learnWordPage = new LearnWordPage();

                            // Apply transition animation
                            NavigationService?.Navigate(learnWordPage);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error navigating to learning content: {ex.Message}",
                                "Navigation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AnimateElements();
        }

        private void AnimateElements()
        {
            try
            {
                // Find elements to animate
                var title = FindVisualChildren<TextBlock>(this).FirstOrDefault(tb => tb.Text == "SELECT LEVEL");
                var backButton = FindVisualChildren<Button>(this).FirstOrDefault(b => b.Content is StackPanel sp && sp.Children.OfType<TextBlock>().Any(tb => tb.Text == "BACK"));
                var levelButtons = FindVisualChildren<Button>(this).Where(b => b.Style == (Style)FindResource("LevelButtonStyle")).ToList();

                int delay = 100;

                // Animate Back Button
                if (backButton != null)
                {
                    backButton.Opacity = 0;
                    ApplyAnimation(backButton, delay, AnimationType.SlideFromLeft);
                    delay += 50;
                }

                // Animate Title
                if (title != null)
                {
                    title.Opacity = 0;
                    ApplyAnimation(title, delay, AnimationType.SlideFromTop);
                    delay += 100;
                }

                // Animate Level Buttons (staggered)
                foreach (var button in levelButtons)
                {
                    button.Opacity = 0;
                    ApplyAnimation(button, delay, AnimationType.SlideUpAndFade); // Changed to SlideUpAndFade
                    delay += 50; // Stagger delay
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LearnLevelPage animation error: {ex.Message}");
                // Ensure elements are visible even if animation fails
                Opacity = 1;
            }
        }

        // Reusable animation helper
        private void ApplyAnimation(UIElement element, int delayMs, AnimationType type)
        {
            if (element == null) return;

            Storyboard storyboard = new Storyboard();
            element.Opacity = 0; // Ensure starting state

            // Fade In
            DoubleAnimation fadeAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromMilliseconds(400)),
                BeginTime = TimeSpan.FromMilliseconds(delayMs),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };
            Storyboard.SetTarget(fadeAnimation, element);
            Storyboard.SetTargetProperty(fadeAnimation, new PropertyPath(UIElement.OpacityProperty));
            storyboard.Children.Add(fadeAnimation);

            TranslateTransform transform = null;
            if (element.RenderTransform is TranslateTransform existingTransform)
            {
                transform = existingTransform;
            }
            else
            {
                transform = new TranslateTransform();
                element.RenderTransform = transform;
            }

            // Motion
            if (type == AnimationType.SlideUpAndFade)
            {
                transform.Y = 30;
                DoubleAnimation slideAnimation = new DoubleAnimation
                {
                    From = 30,
                    To = 0,
                    Duration = new Duration(TimeSpan.FromMilliseconds(500)),
                    BeginTime = TimeSpan.FromMilliseconds(delayMs),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };
                Storyboard.SetTarget(slideAnimation, element);
                Storyboard.SetTargetProperty(slideAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));
                storyboard.Children.Add(slideAnimation);
            }
            else if (type == AnimationType.SlideFromLeft)
            {
                transform.X = -30;
                DoubleAnimation slideAnimation = new DoubleAnimation
                {
                    From = -30,
                    To = 0,
                    Duration = new Duration(TimeSpan.FromMilliseconds(500)),
                    BeginTime = TimeSpan.FromMilliseconds(delayMs),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };
                Storyboard.SetTarget(slideAnimation, element);
                Storyboard.SetTargetProperty(slideAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
                storyboard.Children.Add(slideAnimation);
            }
            else if (type == AnimationType.SlideFromTop)
            {
                transform.Y = -20;
                DoubleAnimation slideAnimation = new DoubleAnimation
                {
                    From = -20,
                    To = 0,
                    Duration = new Duration(TimeSpan.FromMilliseconds(500)),
                    BeginTime = TimeSpan.FromMilliseconds(delayMs),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };
                Storyboard.SetTarget(slideAnimation, element);
                Storyboard.SetTargetProperty(slideAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));
                storyboard.Children.Add(slideAnimation);
            }

            storyboard.Begin();
        }

        private enum AnimationType
        {
            FadeOnly,
            SlideUpAndFade,
            SlideFromLeft,
            SlideFromTop
        }

        // Helper method to find all children of a specific type (needed for animations)
        private IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T t)
                    {
                        yield return t;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}
