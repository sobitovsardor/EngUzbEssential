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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EngUzbEssential.Page
{
    /// <summary>
    /// Interaction logic for LearnWordPage.xaml
    /// </summary>
    public partial class LearnWordPage : System.Windows.Controls.Page
    {
        // Current word index and total words for the level
        private int currentWordIndex = 0;
        private int totalWords = 20;
        private int currentLevel = 1;
        private int currentPoints = 150;

        // Sample word data - in a real app, this would come from a database or API
        private List<WordPair> wordList = new List<WordPair>
        {
            new WordPair { 
                EnglishWord = "Apple", 
                UzbekWord = "Olma", 
                Pronunciation = "/ˈæp.əl/", 
                Example = "I eat an apple every day.", 
                UzbekExample = "Men har kuni olma yeyman.",
                Definition = "A round fruit with red, yellow, or green skin and a white inside.",
                Level = 1
            },
            new WordPair { 
                EnglishWord = "Book", 
                UzbekWord = "Kitob", 
                Pronunciation = "/bʊk/", 
                Example = "She reads a book before bed.", 
                UzbekExample = "U yotishdan oldin kitob o'qiydi.",
                Definition = "A set of pages with text and sometimes pictures, bound together.",
                Level = 1
            },
            new WordPair { 
                EnglishWord = "Happiness", 
                UzbekWord = "Baxt", 
                Pronunciation = "/ˈhæpinəs/", 
                Example = "Money can't buy happiness.", 
                UzbekExample = "Pul baxtni sotib ololmaydi.",
                Definition = "The state of being happy; a feeling of contentment, joy, or pleasure.",
                Level = 1
            },
            new WordPair { 
                EnglishWord = "Water", 
                UzbekWord = "Suv", 
                Pronunciation = "/ˈwɔːtə(r)/", 
                Example = "Drink plenty of water.", 
                UzbekExample = "Ko'p suv iching.",
                Definition = "A clear liquid without color, smell, or taste that falls as rain.",
                Level = 1
            },
            new WordPair { 
                EnglishWord = "Friend", 
                UzbekWord = "Do'st", 
                Pronunciation = "/frend/", 
                Example = "A true friend is always there for you.", 
                UzbekExample = "Haqiqiy do'st har doim siz uchun bor.",
                Definition = "A person you know well and like, but who is not related to you.",
                Level = 1
            }
        };

        public LearnWordPage()
        {
            InitializeComponent();
            this.Loaded += Page_Loaded;
            
            // Initialize word data
            UpdateWordDisplay();
        }

        public LearnWordPage(int level)
        {
            InitializeComponent();
            this.Loaded += Page_Loaded;
            
            // Set the current level
            this.currentLevel = level;
            
            // Initialize word data
            UpdateWordDisplay();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AnimateElements();
        }

        private void AnimateElements()
        {
            try
            {
                // Find all elements to animate
                var title = FindVisualChildren<TextBlock>(this)
                    .FirstOrDefault(tb => tb.Text == "LEARN WORDS");
                var backButton = FindVisualChildren<Button>(this)
                    .FirstOrDefault(b => b.Content is StackPanel sp && sp.Children.OfType<TextBlock>()
                    .Any(tb => tb.Text == "BACK"));
                var progressPanel = FindVisualChildren<StackPanel>(this)
                    .FirstOrDefault(sp => sp.Children.OfType<TextBlock>()
                    .Any(tb => tb.Text.StartsWith("Level")));
                var wordCards = FindVisualChildren<Border>(this)
                    .Where(b => b.Style == FindResource("WordCardStyle")).ToList();
                var navButtons = FindVisualChildren<Button>(this)
                    .Where(b => b.Style == FindResource("ActionButtonStyle") && b.Width >= 100).ToList();

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

                // Animate Progress Panel
                if (progressPanel != null)
                {
                    progressPanel.Opacity = 0;
                    ApplyAnimation(progressPanel, delay, AnimationType.SlideUpAndFade);
                    delay += 150;
                }

                // Animate Word Cards
                foreach (var card in wordCards)
                {
                    card.Opacity = 0;
                    ApplyAnimation(card, delay, AnimationType.SlideUpAndFade);
                    delay += 100;
                }

                // Animate Navigation Buttons
                foreach (var button in navButtons)
                {
                    button.Opacity = 0;
                    ApplyAnimation(button, delay, AnimationType.SlideUpAndFade);
                    delay += 50;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LearnWordPage animation error: {ex.Message}");
                // Ensure elements are visible even if animation fails
                Opacity = 1;
            }
        }

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
                    From = 30, To = 0,
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
                    From = -30, To = 0,
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
                    From = -20, To = 0,
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

                // Fallback: Try navigating to LearnLevelPage via MainWindow if NavigationService fails
                if (Application.Current.MainWindow is MainWindow mainWindow)
                {
                    // Navigate back to the learn level page
                    var learnLevelPage = new LearnLevelPage();
                    this.NavigationService?.Navigate(learnLevelPage);
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

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentWordIndex > 0)
            {
                currentWordIndex--;
                UpdateWordDisplay();
                AnimateWordTransition(false);
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentWordIndex < wordList.Count - 1)
            {
                currentWordIndex++;
                UpdateWordDisplay();
                AnimateWordTransition(true);
            }
        }

        private void UpdateWordDisplay()
        {
            // Get word based on current index
            if (currentWordIndex >= 0 && currentWordIndex < wordList.Count)
            {
                // Filter words based on the current level
                var levelWords = wordList.Where(w => w.Level == currentLevel).ToList();
                totalWords = levelWords.Count;
                
                if (levelWords.Count > 0 && currentWordIndex < levelWords.Count)
                {
                    var currentWord = levelWords[currentWordIndex];
                    
                    // Find the TextBlocks in the current visible word card
                    var visibleWordCards = FindVisualChildren<Border>(this)
                        .Where(b => b.Style != null && b.Style.ToString().Contains("WordCardStyle"))
                        .ToList();
                    
                    if (visibleWordCards.Count > 0)
                    {
                        // We'll use the first visible word card
                        var wordCard = visibleWordCards[0];
                        
                        // Find TextBlocks within the word card
                        var englishWordBlock = FindVisualChildren<TextBlock>(wordCard)
                            .FirstOrDefault(tb => tb.FontSize == 36 && tb.Foreground is SolidColorBrush brush && 
                                         ((SolidColorBrush)tb.Foreground).Color.ToString() == "#FF24284A");
                        
                        var uzbekWordBlock = FindVisualChildren<TextBlock>(wordCard)
                            .FirstOrDefault(tb => tb.FontSize == 36 && tb.Foreground is SolidColorBrush brush && 
                                         ((SolidColorBrush)tb.Foreground).Color.ToString() == "#FFD1434B");
                        
                        // For pronunciation, find the TextBlock next to "Pronunciation: "
                        var pronunciationLabel = FindVisualChildren<TextBlock>(wordCard)
                            .FirstOrDefault(tb => tb.Text == "Pronunciation: ");
                        var pronunciationBlock = pronunciationLabel != null && pronunciationLabel.Parent is StackPanel sp ? 
                            sp.Children.OfType<TextBlock>().ElementAtOrDefault(1) : null;
                        
                        // For definition, find the TextBlock after "Definition:"
                        var definitionLabel = FindVisualChildren<TextBlock>(wordCard)
                            .FirstOrDefault(tb => tb.Text == "Definition:");
                        var definitionBlock = definitionLabel != null ? 
                            FindNextTextBlock(definitionLabel) : null;
                        
                        // For examples, find the TextBlocks after "Example:"
                        var exampleLabel = FindVisualChildren<TextBlock>(wordCard)
                            .FirstOrDefault(tb => tb.Text == "Example:");
                        var exampleBlock = exampleLabel != null ? 
                            FindNextTextBlock(exampleLabel) : null;
                        var uzbekExampleBlock = FindVisualChildren<TextBlock>(wordCard)
                            .FirstOrDefault(tb => tb.FontStyle == FontStyles.Italic);
                        
                        // Update UI with current word
                        if (englishWordBlock != null) englishWordBlock.Text = currentWord.EnglishWord;
                        if (uzbekWordBlock != null) uzbekWordBlock.Text = currentWord.UzbekWord;
                        if (pronunciationBlock != null) pronunciationBlock.Text = currentWord.Pronunciation;
                        if (definitionBlock != null) definitionBlock.Text = currentWord.Definition;
                        if (exampleBlock != null) exampleBlock.Text = currentWord.Example;
                        if (uzbekExampleBlock != null) uzbekExampleBlock.Text = currentWord.UzbekExample;
                        
                        // Update progress information
                        // Find or create TextBlocks for level and progress
                        var levelTextBlock = FindVisualChildren<TextBlock>(this)
                            .FirstOrDefault(tb => tb.Name == "LevelText" || (tb.Text != null && tb.Text.StartsWith("LEVEL")));
                        
                        var progressTextBlock = FindVisualChildren<TextBlock>(this)
                            .FirstOrDefault(tb => tb.Name == "ProgressText" || (tb.Text != null && tb.Text.Contains("/")));
                        
                        if (levelTextBlock != null) levelTextBlock.Text = $"LEVEL {currentLevel}";
                        if (progressTextBlock != null) progressTextBlock.Text = $"{currentWordIndex + 1}/{totalWords}";
                        
                        // Update progress bar if it exists
                        var progressBar = FindName("ProgressBar") as ProgressBar;
                        if (progressBar != null)
                        {
                            progressBar.Value = ((double)(currentWordIndex + 1) / totalWords) * 100;
                        }
                        
                        // Handle button states
                        var prevButton = FindName("PreviousButton") as Button;
                        var nextButton = FindName("NextButton") as Button;
                        
                        if (prevButton != null) prevButton.IsEnabled = currentWordIndex > 0;
                        if (nextButton != null) nextButton.IsEnabled = currentWordIndex < totalWords - 1;
                    }
                }
            }
        }

        private void AnimateWordTransition(bool goingForward)
        {
            // Animate word cards
            var cards = FindVisualChildren<Border>(this)
                .Where(b => b.Style == FindResource("WordCardStyle")).ToList();
            
            foreach (var card in cards)
            {
                // Create a slide animation
                var transform = new TranslateTransform();
                card.RenderTransform = transform;
                
                DoubleAnimation slideInAnimation = new DoubleAnimation
                {
                    From = goingForward ? 100 : -100,
                    To = 0,
                    Duration = TimeSpan.FromMilliseconds(300),
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
                };
                
                // Create a fade animation
                DoubleAnimation fadeAnimation = new DoubleAnimation
                {
                    From = 0.3,
                    To = 1,
                    Duration = TimeSpan.FromMilliseconds(300),
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
                };
                
                // Apply the animations
                Storyboard storyboard = new Storyboard();
                
                Storyboard.SetTarget(slideInAnimation, card);
                Storyboard.SetTargetProperty(slideInAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
                storyboard.Children.Add(slideInAnimation);
                
                Storyboard.SetTarget(fadeAnimation, card);
                Storyboard.SetTargetProperty(fadeAnimation, new PropertyPath(UIElement.OpacityProperty));
                storyboard.Children.Add(fadeAnimation);
                
                storyboard.Begin();
            }
        }

        // Helper method to find visual children of a specific type
        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
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

        // Helper method to find the next TextBlock in the visual tree
        private TextBlock FindNextTextBlock(TextBlock current)
        {
            if (current == null)
                return null;

            // Get the parent
            var parent = VisualTreeHelper.GetParent(current);
            
            // If parent is a panel, try to find the next TextBlock
            if (parent is Panel panel)
            {
                int index = panel.Children.IndexOf(current);
                if (index >= 0 && index < panel.Children.Count - 1)
                {
                    // Try to find the next TextBlock directly
                    for (int i = index + 1; i < panel.Children.Count; i++)
                    {
                        if (panel.Children[i] is TextBlock tb)
                            return tb;
                        
                        // Or look inside if it's a container
                        var nestedBlock = FindVisualChildren<TextBlock>(panel.Children[i]).FirstOrDefault();
                        if (nestedBlock != null)
                            return nestedBlock;
                    }
                }
            }
            
            // If not found, try siblings in the parent's parent
            var grandparent = parent != null ? VisualTreeHelper.GetParent(parent) : null;
            if (grandparent is Panel gpanel)
            {
                int parentIndex = gpanel.Children.IndexOf(parent as UIElement);
                if (parentIndex >= 0 && parentIndex < gpanel.Children.Count - 1)
                {
                    for (int i = parentIndex + 1; i < gpanel.Children.Count; i++)
                    {
                        var sibling = gpanel.Children[i];
                        var siblingTextBlock = FindVisualChildren<TextBlock>(sibling).FirstOrDefault();
                        if (siblingTextBlock != null)
                            return siblingTextBlock;
                    }
                }
            }
            
            return null;
        }
    }

    // Class to hold word data
    public class WordPair
    {
        public string EnglishWord { get; set; }
        public string UzbekWord { get; set; }
        public string Pronunciation { get; set; }
        public string Example { get; set; }
        public string UzbekExample { get; set; }
        public string Definition { get; set; }
        public int Level { get; set; }
    }
}
