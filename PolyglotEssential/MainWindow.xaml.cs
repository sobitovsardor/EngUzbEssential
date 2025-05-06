// MainWindow.xaml.cs
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Shapes;

namespace PolyglotEssential
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
            
            // Add window load event to trigger initial animations
            this.Loaded += MainWindow_Loaded;
            
            // Add closing event handler
            this.Closing += MainWindow_Closing;
        }
        
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Animate initial window elements
            AnimateInitialElements();
            
            // Add hover effects to buttons
            AddHoverEffectsToButtons();
            
            // Add text input animations
            AddTextInputAnimations();
            
            // Add particle background animation
            AddParticleBackground();
            
            // Set up parallax effect
            SetupParallaxEffect();
            
            // Show welcome notification
            ShowNotification("Welcome to Eng-Uzb Essential!", NotificationType.Success);
        }
        
        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            // Cancel the default closing behavior
            e.Cancel = true;
            
            // Create fade out animation for graceful exit
            Storyboard closingStoryboard = new Storyboard();
            
            // Fade out the entire window
            DoubleAnimation fadeOutAnimation = new DoubleAnimation
            {
                From = this.Opacity,
                To = 0,
                Duration = new Duration(TimeSpan.FromMilliseconds(300)),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
            };
            Storyboard.SetTarget(fadeOutAnimation, this);
            Storyboard.SetTargetProperty(fadeOutAnimation, new PropertyPath(Window.OpacityProperty));
            closingStoryboard.Children.Add(fadeOutAnimation);
            
            // Scale down slightly
            ScaleTransform scaleTransform = new ScaleTransform(1, 1);
            this.RenderTransform = scaleTransform;
            this.RenderTransformOrigin = new Point(0.5, 0.5);
            
            DoubleAnimation scaleXAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0.95,
                Duration = new Duration(TimeSpan.FromMilliseconds(300)),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
            };
            Storyboard.SetTarget(scaleXAnimation, this);
            Storyboard.SetTargetProperty(scaleXAnimation, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleX)"));
            closingStoryboard.Children.Add(scaleXAnimation);
            

            DoubleAnimation scaleYAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0.95,
                Duration = new Duration(TimeSpan.FromMilliseconds(300)),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
            };
            Storyboard.SetTarget(scaleYAnimation, this);
            Storyboard.SetTargetProperty(scaleYAnimation, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleY)"));
            closingStoryboard.Children.Add(scaleYAnimation);
            
            // When animation completes, close the window for real
            closingStoryboard.Completed += (s, args) => 
            {
                // Set a flag to prevent infinite recursion
                this.Tag = "ClosingAnimationCompleted";
                this.Close();
            };
            
            // If we're already in the closing animation, just close normally
            if (this.Tag != null && this.Tag.ToString() == "ClosingAnimationCompleted")
            {
                e.Cancel = false;
                return;
            }
            
            // Start the animation
            closingStoryboard.Begin();
        }
        
        private void AnimateInitialElements()
        {
            try
            {
                // Animate home content elements with cascade effect
                if (HomeContent != null)
                {
                    // Find all child elements that we want to animate
                    var textElements = FindVisualChildren<TextBlock>(HomeContent)
                        .Where(t => t.FontSize > 20)  // Larger text (headings)
                        .ToList();
                    
                    var buttons = FindVisualChildren<Button>(HomeContent).ToList();
                    var decorativeElements = FindVisualChildren<UIElement>(HomeContent)
                        .Where(e => e is Ellipse || e is Path)
                        .ToList();
                    
                    // Set initial states
                    foreach (var element in textElements)
                    {
                        element.Opacity = 0;
                        if (element.RenderTransform == null || !(element.RenderTransform is TranslateTransform))
                            element.RenderTransform = new TranslateTransform(0, 30);
                    }
                    
                    foreach (var button in buttons)
                    {
                        button.Opacity = 0;
                        if (button.RenderTransform == null || !(button.RenderTransform is ScaleTransform))
                            button.RenderTransform = new ScaleTransform(0.8, 0.8);
                    }
                    
                    foreach (var element in decorativeElements)
                    {
                        element.Opacity = 0;
                    }
                    
                    // Create staggered animations
                    int delay = 0;
                    
                    // Animate text elements
                    foreach (var element in textElements)
                    {
                        AnimateElement(element, delay, TransitionType.SlideUp);
                        delay += 150; // Stagger by 150ms
                    }
                    
                    // Animate buttons
                    foreach (var button in buttons)
                    {
                        AnimateElementWithScale(button, delay);
                        delay += 100;
                    }
                    
                    // Animate decorative elements
                    foreach (var element in decorativeElements)
                    {
                        AnimateElement(element, delay, TransitionType.FadeOnly);
                        delay += 100;
                    }
                }
                
                // Animate the right panel
                if (RightPanel != null)
                {
                    RightPanel.Opacity = 0;
                    
                    Storyboard storyboard = new Storyboard();
                    
                    // Fade in animation
                    DoubleAnimation fadeAnimation = new DoubleAnimation
                    {
                        From = 0,
                        To = 1,
                        Duration = new Duration(TimeSpan.FromMilliseconds(800)),
                        EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                    };
                    Storyboard.SetTarget(fadeAnimation, RightPanel);
                    Storyboard.SetTargetProperty(fadeAnimation, new PropertyPath(UIElement.OpacityProperty));
                    storyboard.Children.Add(fadeAnimation);
                    
                    // Slide from right animation
                    DoubleAnimation slideAnimation = new DoubleAnimation
                    {
                        From = 50,
                        To = 0,
                        Duration = new Duration(TimeSpan.FromMilliseconds(600)),
                        EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                    };
                    
                    // Ensure there's a TranslateTransform
                    if (RightPanel.RenderTransform == null || !(RightPanel.RenderTransform is TranslateTransform))
                    {
                        RightPanel.RenderTransform = new TranslateTransform();
                    }
                    
                    Storyboard.SetTarget(slideAnimation, RightPanel);
                    Storyboard.SetTargetProperty(slideAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
                    storyboard.Children.Add(slideAnimation);
                    
                    // Start the animation
                    storyboard.Begin();
                }
                
                // Animate top nav buttons
                var navButtons = FindVisualChildren<Button>(this)
                    .Where(b => b.Content is string && 
                           (b.Content.ToString() == "HOME" || 
                            b.Content.ToString() == "LEARN" || 
                            b.Content.ToString() == "GAMES" || 
                            b.Content.ToString() == "TRANSLATER"))
                    .ToList();
                
                int navDelay = 100;
                foreach (var button in navButtons)
                {
                    button.Opacity = 0;
                    AnimateElement(button, navDelay, TransitionType.SlideFromTop);
                    navDelay += 100;
                }
                
                // Animate user profile button
                var userProfileButton = FindVisualChildren<Button>(this)
                    .FirstOrDefault(b => b.Name == "UserProfileButton" || 
                                       (b.Content is StackPanel && ((StackPanel)b.Content).Children.OfType<TextBlock>().Any(tb => tb.Text == "John Smith")));
                
                if (userProfileButton != null)
                {
                    userProfileButton.Opacity = 0;
                    AnimateElement(userProfileButton, 500, TransitionType.SlideFromTop);
                }
            }
            catch (Exception ex)
            {
                // If animation fails, just make everything visible
                System.Diagnostics.Debug.WriteLine($"Initial animation error: {ex.Message}");
                if (HomeContent != null)
                    HomeContent.Opacity = 1;
                if (RightPanel != null)
                    RightPanel.Opacity = 1;
            }
        }
        
        private void AnimateElement(UIElement element, int delayMs, TransitionType type = TransitionType.FadeOnly)
        {
            Storyboard storyboard = new Storyboard();
            
            // Fade in animation
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
            
            if (type == TransitionType.SlideUp || type == TransitionType.SlideAndFade)
            {
                // Ensure there's a TranslateTransform
                if (element.RenderTransform == null || !(element.RenderTransform is TranslateTransform))
                {
                    element.RenderTransform = new TranslateTransform(0, 30);
                }
                
                // Slide up animation
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
            else if (type == TransitionType.SlideFromRight)
            {
                // Ensure there's a TranslateTransform
                if (element.RenderTransform == null || !(element.RenderTransform is TranslateTransform))
                {
                    element.RenderTransform = new TranslateTransform(30, 0);
                }
                
                // Slide from right animation
                DoubleAnimation slideAnimation = new DoubleAnimation
                {
                    From = 30,
                    To = 0,
                    Duration = new Duration(TimeSpan.FromMilliseconds(500)),
                    BeginTime = TimeSpan.FromMilliseconds(delayMs),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };
                Storyboard.SetTarget(slideAnimation, element);
                Storyboard.SetTargetProperty(slideAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
                storyboard.Children.Add(slideAnimation);
            }
            else if (type == TransitionType.SlideFromTop)
            {
                // Ensure there's a TranslateTransform
                if (element.RenderTransform == null || !(element.RenderTransform is TranslateTransform))
                {
                    element.RenderTransform = new TranslateTransform(0, -20);
                }
                
                // Slide from top animation
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
        
        private void AnimateElementWithScale(UIElement element, int delayMs)
        {
            Storyboard storyboard = new Storyboard();
            
            // Fade in animation
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
            
            // Ensure there's a ScaleTransform
            if (element.RenderTransform == null || !(element.RenderTransform is ScaleTransform))
            {
                element.RenderTransform = new ScaleTransform(0.8, 0.8);
                element.RenderTransformOrigin = new Point(0.5, 0.5);
            }
            
            // Scale X animation
            DoubleAnimation scaleXAnimation = new DoubleAnimation
            {
                From = 0.8,
                To = 1.0,
                Duration = new Duration(TimeSpan.FromMilliseconds(500)),
                BeginTime = TimeSpan.FromMilliseconds(delayMs),
                EasingFunction = new ElasticEase { EasingMode = EasingMode.EaseOut, Oscillations = 1, Springiness = 6 }
            };
            Storyboard.SetTarget(scaleXAnimation, element);
            Storyboard.SetTargetProperty(scaleXAnimation, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleX)"));
            storyboard.Children.Add(scaleXAnimation);
            
            // Scale Y animation
            DoubleAnimation scaleYAnimation = new DoubleAnimation
            {
                From = 0.8,
                To = 1.0,
                Duration = new Duration(TimeSpan.FromMilliseconds(500)),
                BeginTime = TimeSpan.FromMilliseconds(delayMs),
                EasingFunction = new ElasticEase { EasingMode = EasingMode.EaseOut, Oscillations = 1, Springiness = 6 }
            };
            Storyboard.SetTarget(scaleYAnimation, element);
            Storyboard.SetTargetProperty(scaleYAnimation, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleY)"));
            storyboard.Children.Add(scaleYAnimation);
            
            storyboard.Begin();
        }
        
        // Helper method to find all children of a specific type
        private IEnumerable<T> FindVisualChildren<T>(DependencyObject? depObj) where T : DependencyObject
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
            // Get Started button now navigates directly to the Learn page
            LearnButton_Click(sender, e);
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Register button now navigates directly to the Learn page
            LearnButton_Click(sender, e);
        }

        private void LearnButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Check if MainFrame exists
                if (MainFrame == null)
                {
                    MessageBox.Show("Navigation frame not found.", "Navigation Error", 
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                
                // Create the Learn page
                Page.LearnPage learnPage = new Page.LearnPage();
                
                // Navigate to the Learn page
                MainFrame.Navigate(learnPage);
                
                // Apply animation transition - use slide from right for a different visual effect
                ApplyPageTransition(learnPage, TransitionType.SlideFromRight);
                
                // Hide the home content
                if (HomeContent != null)
                    HomeContent.Visibility = Visibility.Collapsed;
                
                // Find and hide the right panel
                if (RightPanel != null)
                {
                    RightPanel.Visibility = Visibility.Collapsed;
                }
                else
                {
                    // Fallback method to find the right panel if named reference doesn't work
                    var contentParentGrid = MainFrame.Parent as FrameworkElement;
                    while (contentParentGrid != null && !(contentParentGrid is Grid grid && grid.ColumnDefinitions.Count > 1))
                    {
                        contentParentGrid = contentParentGrid.Parent as FrameworkElement;
                    }
                    
                    if (contentParentGrid is Grid parentContentGrid)
                    {
                        // Find the right panel (column 1) and hide it
                        var rightPanel = parentContentGrid.Children.Cast<UIElement>()
                            .FirstOrDefault(x => Grid.GetColumn(x) == 1);
                        
                        if (rightPanel != null)
                        {
                            rightPanel.Visibility = Visibility.Collapsed;
                        }
                    }
                }
                
                // Make the main frame span both columns
                Grid? mainParentGrid = MainFrame.Parent as Grid;
                if (mainParentGrid != null)
                {
                    Grid.SetColumnSpan(mainParentGrid, 2);
                }
                
                // Show the frame
                MainFrame.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                // More detailed error handling
                string errorDetails = $"Error type: {ex.GetType().Name}\nMessage: {ex.Message}";
                if (ex.InnerException != null)
                    errorDetails += $"\nInner exception: {ex.InnerException.Message}";
                
                MessageBox.Show($"Error navigating to Learn page:\n{errorDetails}", 
                               "Navigation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToHome();
        }
        
        // Public method for navigation to home from other pages
        public void NavigateToHome()
        {
            try
            {
                // If MainFrame is empty, just show home content
                if (MainFrame == null || MainFrame.Content == null)
                {
                    if (HomeContent != null)
                        HomeContent.Visibility = Visibility.Visible;
                    
                    if (RightPanel != null)
                        RightPanel.Visibility = Visibility.Visible;
                    
                    return;
                }
                
                // Create fade-out animation for the current page
                var currentPage = MainFrame.Content as FrameworkElement;
                if (currentPage != null)
                {
                    // Create fade out animation
                    Storyboard fadeOutStoryboard = new Storyboard();
                    DoubleAnimation fadeOutAnimation = new DoubleAnimation
                    {
                        From = 1,
                        To = 0,
                        Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                        EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
                    };
                    Storyboard.SetTarget(fadeOutAnimation, currentPage);
                    Storyboard.SetTargetProperty(fadeOutAnimation, new PropertyPath(UIElement.OpacityProperty));
                    fadeOutStoryboard.Children.Add(fadeOutAnimation);
                    
                    // When animation completes, clear the frame and show home content
                    fadeOutStoryboard.Completed += (s, e) => 
                    {
                        // Clear the frame
                        MainFrame.Content = null;
                        
                        // Hide the frame
                        MainFrame.Visibility = Visibility.Collapsed;
                        
                        // Show the home content with animation
                        ShowHomeContentWithAnimation();
                    };
                    
                    // Start the fade out
                    fadeOutStoryboard.Begin();
                }
                else
                {
                    // Clear the frame
                    MainFrame.Content = null;
                    
                    // Hide the frame
                    MainFrame.Visibility = Visibility.Collapsed;
                    
                    // Show home content with animation
                    ShowHomeContentWithAnimation();
                }
            }
            catch (Exception ex)
            {
                // Fallback to non-animated version if animation fails
                System.Diagnostics.Debug.WriteLine($"Home navigation animation error: {ex.Message}");
                
                // Clear the frame
                if (MainFrame != null)
                    MainFrame.Content = null;
                
                // Hide the frame
                if (MainFrame != null)
                    MainFrame.Visibility = Visibility.Collapsed;
                
                // Restore the right panel and reset column span using the non-animated approach
                RestoreRightPanelAndLayout();
                
                // Show the home content
                if (HomeContent != null)
                    HomeContent.Visibility = Visibility.Visible;
            }
        }
        
        /// <summary>
        /// Helper method to show home content with animation
        /// </summary>
        private void ShowHomeContentWithAnimation()
        {
            try
            {
                // Make sure HomeContent is visible
                if (HomeContent != null)
                {
                    // Set initial state
                    HomeContent.Opacity = 0;
                    HomeContent.Visibility = Visibility.Visible;
                    
                    // Create fade-in animation
                    Storyboard fadeInStoryboard = new Storyboard();
                    DoubleAnimation fadeInAnimation = new DoubleAnimation
                    {
                        From = 0,
                        To = 1,
                        Duration = new Duration(TimeSpan.FromMilliseconds(300)),
                        EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                    };
                    Storyboard.SetTarget(fadeInAnimation, HomeContent);
                    Storyboard.SetTargetProperty(fadeInAnimation, new PropertyPath(UIElement.OpacityProperty));
                    fadeInStoryboard.Children.Add(fadeInAnimation);
                    
                    // Start animation
                    fadeInStoryboard.Begin();
                }
                
                // Restore right panel and layout
                RestoreRightPanelAndLayout();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Home content animation error: {ex.Message}");
                
                // Fallback to non-animated approach
                if (HomeContent != null)
                {
                    HomeContent.Opacity = 1;
                    HomeContent.Visibility = Visibility.Visible;
                }
                
                RestoreRightPanelAndLayout();
            }
        }
        
        /// <summary>
        /// Helper method to restore the right panel and layout
        /// </summary>
        private void RestoreRightPanelAndLayout()
        {
            try
            {
                // Restore the right panel and reset column span
                var parentGrid = MainFrame?.Parent as FrameworkElement;
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
                        // Animate right panel appearance
                        rightPanel.Opacity = 0;
                        rightPanel.Visibility = Visibility.Visible;
                        
                        // Create fade-in animation
                        Storyboard rightPanelStoryboard = new Storyboard();
                        DoubleAnimation rightPanelAnimation = new DoubleAnimation
                        {
                            From = 0,
                            To = 1,
                            Duration = new Duration(TimeSpan.FromMilliseconds(400)),
                            EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                        };
                        Storyboard.SetTarget(rightPanelAnimation, rightPanel);
                        Storyboard.SetTargetProperty(rightPanelAnimation, new PropertyPath(UIElement.OpacityProperty));
                        rightPanelStoryboard.Children.Add(rightPanelAnimation);
                        
                        // Start animation
                        rightPanelStoryboard.Begin();
                    }
                    
                    // Reset column span of the left content panel if MainFrame is not null
                    if (MainFrame != null && MainFrame.Parent is UIElement mainFrameParent)
                    {
                        Grid.SetColumnSpan(mainFrameParent, 1);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Right panel restoration error: {ex.Message}");
                
                // Manual fallback to show right panel
                if (RightPanel != null)
                {
                    RightPanel.Opacity = 1;
                    RightPanel.Visibility = Visibility.Visible;
                }
            }
        }

        private void GamesButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Check if MainFrame exists
                if (MainFrame == null)
                {
                    MessageBox.Show("Navigation frame not found.", "Navigation Error", 
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                
                // Create the Games page
                PolyglotEssential.Desktop.Page.GamePage gamePage = new PolyglotEssential.Desktop.Page.GamePage();
                
                // Navigate to the Games page
                MainFrame.Navigate(gamePage);
                
                // Apply animation transition
                ApplyPageTransition(gamePage, TransitionType.SlideFromRight);
                
                // Hide the home content
                if (HomeContent != null)
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
            catch (Exception ex)
            {
                // More detailed error handling
                string errorDetails = $"Error type: {ex.GetType().Name}\nMessage: {ex.Message}";
                if (ex.InnerException != null)
                    errorDetails += $"\nInner exception: {ex.InnerException.Message}";
                
                MessageBox.Show($"Error navigating to Games page:\n{errorDetails}", 
                               "Navigation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        // Add this method to navigate to TranslaterPage with UI hidden
        private void NavigateToTranslaterPageWithHiddenUI()
        {
            try
            {
                // Create the translator page with UI hidden option
                var translaterPage = new Page.TranslaterPage
                {
                    ShouldHideUI = true
                };
                
                // Navigate to the page
                MainFrame.Navigate(translaterPage);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to Translater page: {ex.Message}", 
                              "Navigation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Fix the TranslaterButton_Click method that was incorrectly replaced
        private void TranslaterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Check if MainFrame exists
                if (MainFrame == null)
                {
                    MessageBox.Show("Navigation frame not found.", "Navigation Error", 
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                
                System.Diagnostics.Debug.WriteLine("Creating TranslaterPage instance...");
                
                // Create the Translater page with detailed error handling
                Page.TranslaterPage? translaterPage = null;
                try
                {
                    // Create the translator page
                    translaterPage = new Page.TranslaterPage();
                    System.Diagnostics.Debug.WriteLine("TranslaterPage created successfully");
                    
                    // Option to hide UI if needed (set to false by default)
                    // translaterPage.ShouldHideUI = true; // Uncomment to hide UI
                }
                catch (Exception ex)
                {
                    string detailedError = $"Error type: {ex.GetType().Name}\nMessage: {ex.Message}\nStack trace: {ex.StackTrace}";
                    System.Diagnostics.Debug.WriteLine($"Error creating TranslaterPage: {detailedError}");
                    
                    MessageBox.Show($"Error creating Translater page:\n{ex.Message}\n\nPlease check the application logs for more details.", 
                                  "Page Creation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Safety check
                if (translaterPage == null)
                {
                    System.Diagnostics.Debug.WriteLine("TranslaterPage is null after creation");
                    MessageBox.Show("Failed to create Translater page.", "Page Creation Error", 
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                
                System.Diagnostics.Debug.WriteLine("Navigating to TranslaterPage...");
                
                // Navigate to the Translater page
                MainFrame.Navigate(translaterPage);
                
                // Apply animation transition with try-catch
                try
                {
                    ApplyPageTransition(translaterPage, TransitionType.SlideFromRight);
                }
                catch (Exception animEx)
                {
                    System.Diagnostics.Debug.WriteLine($"Animation error: {animEx.Message}");
                    // If animation fails, use direct transition
                    if (translaterPage != null)
                    {
                        translaterPage.Opacity = 1;
                    }
                }
                
                // Hide the home content
                if (HomeContent != null)
                {
                    HomeContent.Visibility = Visibility.Collapsed;
                }
                
                // Hide the right blue panel with detailed error handling
                try
                {
                    var parentGrid = MainFrame.Parent as FrameworkElement;
                    if (parentGrid == null)
                    {
                        System.Diagnostics.Debug.WriteLine("MainFrame.Parent is null");
                    }
                    
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
                            System.Diagnostics.Debug.WriteLine("Hidden right panel using grid column lookup");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("Could not find right panel in grid children");
                        }
                        
                        // Make the main frame span both columns
                        var mainFrameParent = MainFrame.Parent as UIElement;
                        if (mainFrameParent != null)
                        {
                            Grid.SetColumnSpan(mainFrameParent, 2);
                            System.Diagnostics.Debug.WriteLine("Set column span for MainFrame.Parent");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("MainFrame.Parent is not a UIElement");
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Could not find parent grid with columns");
                    }
                }
                catch (Exception layoutEx)
                {
                    // If layout adjustment fails, log but continue
                    System.Diagnostics.Debug.WriteLine($"Layout adjustment error: {layoutEx.Message}\n{layoutEx.StackTrace}");
                    
                    // Try a simpler approach - directly access RightPanel
                    if (RightPanel != null)
                    {
                        RightPanel.Visibility = Visibility.Collapsed;
                        System.Diagnostics.Debug.WriteLine("Hidden right panel using direct RightPanel reference");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("RightPanel is null");
                    }
                }
                
                // Show the frame
                MainFrame.Visibility = Visibility.Visible;
                System.Diagnostics.Debug.WriteLine("Navigation to TranslaterPage completed successfully");
            }
            catch (Exception ex)
            {
                // Comprehensive error handling
                string errorDetails = $"Error type: {ex.GetType().Name}\nMessage: {ex.Message}";
                if (ex.InnerException != null)
                    errorDetails += $"\nInner exception: {ex.InnerException.Message}";
                
                System.Diagnostics.Debug.WriteLine($"CRITICAL ERROR in TranslaterButton_Click: {errorDetails}\nStack trace: {ex.StackTrace}");
                
                MessageBox.Show($"Error navigating to Translater page:\n{errorDetails}", 
                               "Navigation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Call this method to open translator with hidden UI
        // You can wire this to a separate button or command as needed

        private void UserProfileButton_Click(object sender, RoutedEventArgs e)
        {
            // This button is currently disabled
            // No action will be taken when clicked
        }

        /// <summary>
        /// Applies a transition animation when navigating between pages
        /// </summary>
        private void ApplyPageTransition(System.Windows.Controls.Page page, TransitionType transitionType = TransitionType.SlideAndFade)
        {
            // Don't animate if page is null
            if (page == null)
                return;
                
            // For Direct transitions, just make the page visible without animation
            if (transitionType == TransitionType.Direct)
            {
                page.Opacity = 1;
                return;
            }
                
            try
            {
                // Set initial opacity
                page.Opacity = 0;
                
                // Set up the storyboard
                Storyboard transitionStoryboard = new Storyboard();
                
                // Create fade-in animation
                DoubleAnimation fadeAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = new Duration(TimeSpan.FromMilliseconds(400)),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };
                Storyboard.SetTarget(fadeAnimation, page);
                Storyboard.SetTargetProperty(fadeAnimation, new PropertyPath(UIElement.OpacityProperty));
                transitionStoryboard.Children.Add(fadeAnimation);
                
                // Add appropriate motion animation based on type
                if (transitionType == TransitionType.SlideAndFade || transitionType == TransitionType.SlideUp)
                {
                    // Add a TranslateTransform to the page
                    if (page.RenderTransform == null || !(page.RenderTransform is TranslateTransform))
                    {
                        page.RenderTransform = new TranslateTransform();
                    }
                    
                    // Create slide-up animation
                    DoubleAnimation slideAnimation = new DoubleAnimation
                    {
                        From = 30, // start 30 pixels down
                        To = 0,
                        Duration = new Duration(TimeSpan.FromMilliseconds(500)),
                        EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                    };
                    Storyboard.SetTarget(slideAnimation, page);
                    Storyboard.SetTargetProperty(slideAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));
                    transitionStoryboard.Children.Add(slideAnimation);
                }
                else if (transitionType == TransitionType.SlideFromRight)
                {
                    // Add a TranslateTransform to the page
                    if (page.RenderTransform == null || !(page.RenderTransform is TranslateTransform))
                    {
                        page.RenderTransform = new TranslateTransform();
                    }
                    
                    // Create slide-from-right animation
                    DoubleAnimation slideAnimation = new DoubleAnimation
                    {
                        From = 50, // start 50 pixels to the right
                        To = 0,
                        Duration = new Duration(TimeSpan.FromMilliseconds(500)),
                        EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                    };
                    Storyboard.SetTarget(slideAnimation, page);
                    Storyboard.SetTargetProperty(slideAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
                    transitionStoryboard.Children.Add(slideAnimation);
                }
                
                // Start the animation
                transitionStoryboard.Begin();
            }
            catch (Exception ex)
            {
                // If animation fails, just make the page visible
                System.Diagnostics.Debug.WriteLine($"Animation error: {ex.Message}");
                page.Opacity = 1;
            }
        }
        
        /// <summary>
        /// Enum defining different page transition types
        /// </summary>
        public enum TransitionType
        {
            SlideAndFade,
            SlideUp,
            SlideFromRight,
            FadeOnly,
            Direct, // No animations, direct page change
            SlideFromTop  // Slide in from top
        }

        // Method to clear Get Started navigation and return to home
        public void ClearGetStartedNavigation()
        {
            try
            {
                // Check if we're already on the home screen
                if (HomeContent.Visibility == Visibility.Visible)
                    return;
                
                // Use the existing home navigation with animations
                NavigateToHome();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error clearing navigation: {ex.Message}", 
                               "Navigation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Add a keyboard shortcut handler for Escape key to clear navigation
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            
            // If Escape key is pressed, clear navigation
            if (e.Key == Key.Escape)
            {
                ClearGetStartedNavigation();
            }
        }

        private void AddHoverEffectsToButtons()
        {
            try
            {
                // Find all buttons in the application
                var allButtons = FindVisualChildren<Button>(this).ToList();
                
                foreach (var button in allButtons)
                {
                    // Skip buttons that already have hover animations in their style
                    if (button.Style != null && 
                        (button.Style.Setters.OfType<EventSetter>().Any(s => s.Event.Name == "MouseEnter") ||
                         button.Style.Triggers.OfType<Trigger>().Any(t => t.Property == UIElement.IsMouseOverProperty)))
                    {
                        continue;
                    }
                    
                    // Add hover and click animations
                    button.MouseEnter += Button_MouseEnter;
                    button.MouseLeave += Button_MouseLeave;
                    button.PreviewMouseDown += Button_PreviewMouseDown;
                    button.PreviewMouseUp += Button_PreviewMouseUp;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error adding hover effects: {ex.Message}");
            }
        }
        
        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Button button)
            {
                // Create storyboard for hover effect
                Storyboard storyboard = new Storyboard();
                
                // Get current transform or create a new one
                ScaleTransform scaleTransform;
                
                if (button.RenderTransform is ScaleTransform existingScale)
                {
                    scaleTransform = existingScale;
                }
                else
                {
                    scaleTransform = new ScaleTransform(1.0, 1.0);
                    button.RenderTransform = scaleTransform;
                    button.RenderTransformOrigin = new Point(0.5, 0.5);
                }
                
                // Scale X animation
                DoubleAnimation scaleXAnimation = new DoubleAnimation
                {
                    To = 1.05,
                    Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };
                Storyboard.SetTarget(scaleXAnimation, button);
                Storyboard.SetTargetProperty(scaleXAnimation, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleX)"));
                storyboard.Children.Add(scaleXAnimation);
                
                // Scale Y animation
                DoubleAnimation scaleYAnimation = new DoubleAnimation
                {
                    To = 1.05,
                    Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };
                Storyboard.SetTarget(scaleYAnimation, button);
                Storyboard.SetTargetProperty(scaleYAnimation, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleY)"));
                storyboard.Children.Add(scaleYAnimation);
                
                // Start the animation
                storyboard.Begin();
            }
        }
        
        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Button button)
            {
                // Create storyboard for return to normal size
                Storyboard storyboard = new Storyboard();
                
                // Scale X animation
                DoubleAnimation scaleXAnimation = new DoubleAnimation
                {
                    To = 1.0,
                    Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };
                Storyboard.SetTarget(scaleXAnimation, button);
                Storyboard.SetTargetProperty(scaleXAnimation, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleX)"));
                storyboard.Children.Add(scaleXAnimation);
                
                // Scale Y animation
                DoubleAnimation scaleYAnimation = new DoubleAnimation
                {
                    To = 1.0,
                    Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };
                Storyboard.SetTarget(scaleYAnimation, button);
                Storyboard.SetTargetProperty(scaleYAnimation, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleY)"));
                storyboard.Children.Add(scaleYAnimation);
                
                // Start the animation
                storyboard.Begin();
            }
        }
        
        private void Button_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Button button)
            {
                // Create storyboard for click effect
                Storyboard storyboard = new Storyboard();
                
                // Scale X animation
                DoubleAnimation scaleXAnimation = new DoubleAnimation
                {
                    To = 0.95,
                    Duration = new Duration(TimeSpan.FromMilliseconds(100)),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };
                Storyboard.SetTarget(scaleXAnimation, button);
                Storyboard.SetTargetProperty(scaleXAnimation, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleX)"));
                storyboard.Children.Add(scaleXAnimation);
                
                // Scale Y animation
                DoubleAnimation scaleYAnimation = new DoubleAnimation
                {
                    To = 0.95,
                    Duration = new Duration(TimeSpan.FromMilliseconds(100)),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };
                Storyboard.SetTarget(scaleYAnimation, button);
                Storyboard.SetTargetProperty(scaleYAnimation, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleY)"));
                storyboard.Children.Add(scaleYAnimation);
                
                // Start the animation
                storyboard.Begin();
            }
        }
        
        private void Button_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Button button)
            {
                // Create storyboard for release effect
                Storyboard storyboard = new Storyboard();
                
                // Scale X animation
                DoubleAnimation scaleXAnimation = new DoubleAnimation
                {
                    To = 1.05, // Return to hover state
                    Duration = new Duration(TimeSpan.FromMilliseconds(100)),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };
                Storyboard.SetTarget(scaleXAnimation, button);
                Storyboard.SetTargetProperty(scaleXAnimation, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleX)"));
                storyboard.Children.Add(scaleXAnimation);
                
                // Scale Y animation
                DoubleAnimation scaleYAnimation = new DoubleAnimation
                {
                    To = 1.05, // Return to hover state
                    Duration = new Duration(TimeSpan.FromMilliseconds(100)),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };
                Storyboard.SetTarget(scaleYAnimation, button);
                Storyboard.SetTargetProperty(scaleYAnimation, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleY)"));
                storyboard.Children.Add(scaleYAnimation);
                
                // Start the animation
                storyboard.Begin();
            }
        }

        // Animation for window states
        private void AnimateWindowStateChange(WindowState oldState, WindowState newState)
        {
            // Only animate if window is visible
            if (!this.IsVisible || this.Opacity < 0.1)
                return;
                
            Storyboard stateStoryboard = new Storyboard();
            
            if (newState == WindowState.Maximized && oldState == WindowState.Normal)
            {
                // Animation for maximizing
                ScaleTransform scaleTransform = new ScaleTransform(1, 1);
                this.RenderTransform = scaleTransform;
                this.RenderTransformOrigin = new Point(0.5, 0.5);
                
                DoubleAnimation scaleXAnimation = new DoubleAnimation
                {
                    From = 0.95,
                    To = 1,
                    Duration = new Duration(TimeSpan.FromMilliseconds(300)),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };
                Storyboard.SetTarget(scaleXAnimation, this);
                Storyboard.SetTargetProperty(scaleXAnimation, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleX)"));
                stateStoryboard.Children.Add(scaleXAnimation);
                
                DoubleAnimation scaleYAnimation = new DoubleAnimation
                {
                    From = 0.95,
                    To = 1,
                    Duration = new Duration(TimeSpan.FromMilliseconds(300)),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };
                Storyboard.SetTarget(scaleYAnimation, this);
                Storyboard.SetTargetProperty(scaleYAnimation, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleY)"));
                stateStoryboard.Children.Add(scaleYAnimation);
                
                stateStoryboard.Begin();
            }
            else if (newState == WindowState.Normal && oldState == WindowState.Maximized)
            {
                // Animation for restoring
                ScaleTransform scaleTransform = new ScaleTransform(1, 1);
                this.RenderTransform = scaleTransform;
                this.RenderTransformOrigin = new Point(0.5, 0.5);
                
                DoubleAnimation scaleXAnimation = new DoubleAnimation
                {
                    From = 1.05,
                    To = 1,
                    Duration = new Duration(TimeSpan.FromMilliseconds(300)),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };
                Storyboard.SetTarget(scaleXAnimation, this);
                Storyboard.SetTargetProperty(scaleXAnimation, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleX)"));
                stateStoryboard.Children.Add(scaleXAnimation);
                
                DoubleAnimation scaleYAnimation = new DoubleAnimation
                {
                    From = 1.05,
                    To = 1,
                    Duration = new Duration(TimeSpan.FromMilliseconds(300)),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };
                Storyboard.SetTarget(scaleYAnimation, this);
                Storyboard.SetTargetProperty(scaleYAnimation, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleY)"));
                stateStoryboard.Children.Add(scaleYAnimation);
                
                stateStoryboard.Begin();
            }
            else if (newState == WindowState.Minimized)
            {
                // Animation for minimizing - this runs before minimize happens
                ScaleTransform scaleTransform = new ScaleTransform(1, 1);
                this.RenderTransform = scaleTransform;
                this.RenderTransformOrigin = new Point(0.5, 0.5);
                
                DoubleAnimation scaleXAnimation = new DoubleAnimation
                {
                    From = 1,
                    To = 0.9,
                    Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
                };
                Storyboard.SetTarget(scaleXAnimation, this);
                Storyboard.SetTargetProperty(scaleXAnimation, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleX)"));
                stateStoryboard.Children.Add(scaleXAnimation);
                
                DoubleAnimation scaleYAnimation = new DoubleAnimation
                {
                    From = 1,
                    To = 0.9,
                    Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
                };
                Storyboard.SetTarget(scaleYAnimation, this);
                Storyboard.SetTargetProperty(scaleYAnimation, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleY)"));
                stateStoryboard.Children.Add(scaleYAnimation);
                
                stateStoryboard.Begin();
            }
        }
        
        // Override to catch window state changes
        protected override void OnStateChanged(EventArgs e)
        {
            WindowState oldState = this.WindowState;
            base.OnStateChanged(e);
            AnimateWindowStateChange(oldState, this.WindowState);
        }

        private void AddParticleBackground()
        {
            try
            {
                // Create a canvas for the particles
                Canvas particleCanvas = new Canvas
                {
                    Width = this.ActualWidth,
                    Height = this.ActualHeight,
                    IsHitTestVisible = false, // Don't capture mouse events
                    Opacity = 0.5,
                };
                
                // Add the canvas to the window
                Grid? mainGrid = this.Content as Grid;
                if (mainGrid != null)
                {
                    // Insert at the bottom of the z-order
                    mainGrid.Children.Insert(0, particleCanvas);
                    Panel.SetZIndex(particleCanvas, -1);
                }
                
                // Create random particles
                Random random = new Random();
                int particleCount = 40; // Adjust for more/less particles
                
                for (int i = 0; i < particleCount; i++)
                {
                    // Create a particle
                    Ellipse particle = new Ellipse
                    {
                        Width = random.Next(3, 8),
                        Height = random.Next(3, 8),
                        Fill = new SolidColorBrush(Color.FromArgb(
                            (byte)random.Next(20, 80), // Alpha (semi-transparent)
                            (byte)random.Next(200, 240), // R
                            (byte)random.Next(200, 240), // G
                            (byte)random.Next(200, 240)  // B (light colors)
                        )),
                        Opacity = random.NextDouble() * 0.5 + 0.2, // Between 0.2 and 0.7
                    };
                    
                    // Position randomly on the canvas
                    Canvas.SetLeft(particle, random.Next(0, (int)this.ActualWidth));
                    Canvas.SetTop(particle, random.Next(0, (int)this.ActualHeight));
                    
                    // Add to canvas
                    particleCanvas.Children.Add(particle);
                    
                    // Animate the particle
                    AnimateParticle(particle, particleCanvas, random);
                }
                
                // Keep updating the canvas size when the window resizes
                this.SizeChanged += (s, args) => 
                {
                    particleCanvas.Width = args.NewSize.Width;
                    particleCanvas.Height = args.NewSize.Height;
                };
            }
            catch (Exception ex)
            {
                // Don't crash the app for decorative animations
                System.Diagnostics.Debug.WriteLine($"Particle animation error: {ex.Message}");
            }
        }
        
        private void AnimateParticle(Ellipse particle, Canvas canvas, Random random)
        {
            // Create animation storyboard
            Storyboard storyboard = new Storyboard();
            
            // Horizontal movement
            DoubleAnimation moveX = new DoubleAnimation
            {
                From = Canvas.GetLeft(particle),
                To = random.Next(-100, (int)canvas.Width + 100), // Move anywhere on canvas with some overflow
                Duration = new Duration(TimeSpan.FromSeconds(random.Next(15, 40))), // Slow movement
                RepeatBehavior = RepeatBehavior.Forever,
                AutoReverse = true,
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };
            Storyboard.SetTarget(moveX, particle);
            Storyboard.SetTargetProperty(moveX, new PropertyPath("(Canvas.Left)"));
            storyboard.Children.Add(moveX);
            
            // Vertical movement
            DoubleAnimation moveY = new DoubleAnimation
            {
                From = Canvas.GetTop(particle),
                To = random.Next(-100, (int)canvas.Height + 100), // Move anywhere on canvas with some overflow
                Duration = new Duration(TimeSpan.FromSeconds(random.Next(15, 40))), // Slow movement
                RepeatBehavior = RepeatBehavior.Forever,
                AutoReverse = true,
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };
            Storyboard.SetTarget(moveY, particle);
            Storyboard.SetTargetProperty(moveY, new PropertyPath("(Canvas.Top)"));
            storyboard.Children.Add(moveY);
            
            // Opacity pulsing
            DoubleAnimation opacityAnim = new DoubleAnimation
            {
                From = particle.Opacity,
                To = particle.Opacity / 2,
                Duration = new Duration(TimeSpan.FromSeconds(random.Next(5, 15))), // Faster than movement
                RepeatBehavior = RepeatBehavior.Forever,
                AutoReverse = true,
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };
            Storyboard.SetTarget(opacityAnim, particle);
            Storyboard.SetTargetProperty(opacityAnim, new PropertyPath(UIElement.OpacityProperty));
            storyboard.Children.Add(opacityAnim);
            
            // Start animation
            storyboard.Begin();
        }
        
        private void SetupParallaxEffect()
        {
            try
            {
                // Find the decorative elements that can have parallax
                var decorativeElements = FindVisualChildren<UIElement>(this)
                    .Where(e => (e is Ellipse || e is Path) && 
                           !(VisualTreeHelper.GetParent(e) is Canvas) && // Skip particles
                           e.Opacity < 1.0) // Most decorative elements are semi-transparent
                    .ToList();
                
                if (decorativeElements.Count == 0)
                    return;
                    
                // Store the original positions
                Dictionary<UIElement, Point> originalPositions = new Dictionary<UIElement, Point>();
                
                foreach (var element in decorativeElements)
                {
                    // Get the element's position
                    Point position = new Point();
                    
                    if (VisualTreeHelper.GetParent(element) is Canvas canvas)
                    {
                        position.X = Canvas.GetLeft(element);
                        position.Y = Canvas.GetTop(element);
                    }
                    else
                    {
                        // For other containers, use the Margin as an approximation
                        Thickness margin = new Thickness();
                        if (element is FrameworkElement frameworkElement)
                        {
                            margin = frameworkElement.Margin;
                        }
                        position.X = margin.Left;
                        position.Y = margin.Top;
                    }
                    
                    originalPositions[element] = position;
                    
                    // Ensure the element has a transform
                    if (element.RenderTransform == null || element.RenderTransform is Transform)
                    {
                        element.RenderTransform = new TranslateTransform();
                    }
                }
                
                // Add the mouse move handler
                this.MouseMove += (sender, e) =>
                {
                    // Calculate the mouse position relative to the window center
                    Point mousePos = e.GetPosition(this);
                    double centerX = this.ActualWidth / 2;
                    double centerY = this.ActualHeight / 2;
                    
                    double offsetX = (mousePos.X - centerX) / centerX; // -1 to 1
                    double offsetY = (mousePos.Y - centerY) / centerY; // -1 to 1
                    
                    // Apply parallax to each element
                    foreach (var element in decorativeElements)
                    {
                        // Calculate parallax amount (random for each element for depth effect)
                        double parallaxFactor = element.Opacity * 20; // Higher opacity = less movement
                        
                        // Apply to transform
                        if (element.RenderTransform is TranslateTransform transform)
                        {
                            transform.X = -offsetX * parallaxFactor;
                            transform.Y = -offsetY * parallaxFactor;
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                // Don't crash the app for decorative animations
                System.Diagnostics.Debug.WriteLine($"Parallax effect error: {ex.Message}");
            }
        }
        
        // Add animated notification system
        private void ShowNotification(string message, NotificationType type = NotificationType.Info)
        {
            try
            {
                // Find the main content grid
                Grid? mainGrid = this.Content as Grid;
                if (mainGrid == null)
                    return;
                
                // Create notification panel
                Border notificationBorder = new Border
                {
                    CornerRadius = new CornerRadius(8),
                    Padding = new Thickness(15, 10, 15, 10),
                    Margin = new Thickness(20),
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Top,
                    RenderTransformOrigin = new Point(1, 0), // Top-right corner
                    RenderTransform = new TranslateTransform(100, 0), // Start off-screen
                    Child = new TextBlock
                    {
                        Text = message,
                        TextWrapping = TextWrapping.Wrap,
                        Foreground = Brushes.White,
                        FontWeight = FontWeights.SemiBold,
                        MaxWidth = 300
                    }
                };
                
                // Set color based on type
                switch (type)
                {
                    case NotificationType.Success:
                        notificationBorder.Background = new SolidColorBrush(Color.FromRgb(46, 204, 113));
                        break;
                    case NotificationType.Warning:
                        notificationBorder.Background = new SolidColorBrush(Color.FromRgb(241, 196, 15));
                        break;
                    case NotificationType.Error:
                        notificationBorder.Background = new SolidColorBrush(Color.FromRgb(231, 76, 60));
                        break;
                    case NotificationType.Info:
                    default:
                        notificationBorder.Background = new SolidColorBrush(Color.FromRgb(52, 152, 219));
                        break;
                }
                
                // Add drop shadow effect
                notificationBorder.Effect = new DropShadowEffect
                {
                    Color = Colors.Black,
                    Direction = 270,
                    ShadowDepth = 5,
                    Opacity = 0.4,
                    BlurRadius = 10
                };
                
                // Add panel to grid with highest Z-index
                Panel.SetZIndex(notificationBorder, 1000);
                mainGrid.Children.Add(notificationBorder);
                
                // Animation timing
                double entranceDuration = 0.4;
                double displayDuration = 3.0;
                double exitDuration = 0.4;
                
                // Entrance animation
                Storyboard entranceStoryboard = new Storyboard();
                
                DoubleAnimation slideInAnimation = new DoubleAnimation
                {
                    From = 100,
                    To = 0,
                    Duration = new Duration(TimeSpan.FromSeconds(entranceDuration)),
                    EasingFunction = new ExponentialEase { EasingMode = EasingMode.EaseOut }
                };
                Storyboard.SetTarget(slideInAnimation, notificationBorder);
                Storyboard.SetTargetProperty(slideInAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
                entranceStoryboard.Children.Add(slideInAnimation);
                
                // Exit animation
                Storyboard exitStoryboard = new Storyboard();
                
                DoubleAnimation slideOutAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 100,
                    BeginTime = TimeSpan.FromSeconds(displayDuration),
                    Duration = new Duration(TimeSpan.FromSeconds(exitDuration)),
                    EasingFunction = new ExponentialEase { EasingMode = EasingMode.EaseIn }
                };
                Storyboard.SetTarget(slideOutAnimation, notificationBorder);
                Storyboard.SetTargetProperty(slideOutAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
                exitStoryboard.Children.Add(slideOutAnimation);
                
                DoubleAnimation fadeOutAnimation = new DoubleAnimation
                {
                    From = 1,
                    To = 0,
                    BeginTime = TimeSpan.FromSeconds(displayDuration),
                    Duration = new Duration(TimeSpan.FromSeconds(exitDuration)),
                    EasingFunction = new ExponentialEase { EasingMode = EasingMode.EaseIn }
                };
                Storyboard.SetTarget(fadeOutAnimation, notificationBorder);
                Storyboard.SetTargetProperty(fadeOutAnimation, new PropertyPath(UIElement.OpacityProperty));
                exitStoryboard.Children.Add(fadeOutAnimation);
                
                // Remove notification after animation completes
                exitStoryboard.Completed += (s, e) => 
                {
                    mainGrid.Children.Remove(notificationBorder);
                };
                
                // Start animations
                entranceStoryboard.Begin();
                exitStoryboard.Begin();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Notification error: {ex.Message}");
            }
        }
        
        // Notification types for the animated notification system
        public enum NotificationType
        {
            Info,
            Success,
            Warning,
            Error
        }

        // Create animated loading spinner
        public Border CreateLoadingSpinner(double size = 40, Brush? color = null)
        {
            // Default color if not specified
            color ??= new SolidColorBrush(Color.FromRgb(209, 67, 75)); // #D1434B
            
            // Create border that will contain the spinner
            Border spinnerContainer = new Border
            {
                Width = size,
                Height = size,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Background = Brushes.Transparent
            };
            
            // Create the ellipse for the spinner
            Ellipse spinnerEllipse = new Ellipse
            {
                Width = size,
                Height = size,
                StrokeThickness = size / 10,
                Stroke = color,
                StrokeDashArray = new DoubleCollection { 0.75, 0.25 }, // Create dash pattern
                StrokeDashCap = PenLineCap.Round
            };
            
            // Add the ellipse to the container
            spinnerContainer.Child = spinnerEllipse;
            
            // Create rotation animation
            RotateTransform rotateTransform = new RotateTransform();
            spinnerEllipse.RenderTransform = rotateTransform;
            spinnerEllipse.RenderTransformOrigin = new Point(0.5, 0.5); // Rotate around center
            
            // Rotation storyboard
            Storyboard rotationStoryboard = new Storyboard
            {
                RepeatBehavior = RepeatBehavior.Forever
            };
            
            // Create rotation animation
            DoubleAnimation rotateAnimation = new DoubleAnimation
            {
                From = 0,
                To = 360,
                Duration = new Duration(TimeSpan.FromSeconds(1.5)),
                RepeatBehavior = RepeatBehavior.Forever
            };
            Storyboard.SetTarget(rotateAnimation, spinnerEllipse);
            Storyboard.SetTargetProperty(rotateAnimation, new PropertyPath("(UIElement.RenderTransform).(RotateTransform.Angle)"));
            rotationStoryboard.Children.Add(rotateAnimation);
            
            // Start the animation
            rotationStoryboard.Begin();
            
            return spinnerContainer;
        }
        
        // Create a progress animation for loading bars
        public Border CreateProgressBar(double width = 200, double height = 10, double progress = 0)
        {
            // Main container
            Border progressContainer = new Border
            {
                Width = width,
                Height = height,
                Background = new SolidColorBrush(Color.FromRgb(240, 240, 240)),
                CornerRadius = new CornerRadius(height / 2), // Make it rounded
                Padding = new Thickness(0)
            };
            
            // Progress indicator
            Border progressIndicator = new Border
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Width = width * Math.Max(0, Math.Min(1, progress)), // Ensure between 0-100%
                Height = height,
                Background = new SolidColorBrush(Color.FromRgb(209, 67, 75)), // #D1434B
                CornerRadius = new CornerRadius(height / 2), // Make it rounded
            };
            
            // Create gradient effect on the progress indicator
            LinearGradientBrush gradientBrush = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 0)
            };
            gradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(209, 67, 75), 0.0)); // #D1434B
            gradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(220, 100, 107), 1.0)); // Lighter shade
            progressIndicator.Background = gradientBrush;
            
            // Add drop shadow effect to progress bar
            progressIndicator.Effect = new DropShadowEffect
            {
                Color = Color.FromRgb(209, 67, 75),
                Direction = 270,
                ShadowDepth = 1,
                Opacity = 0.3,
                BlurRadius = 4
            };
            
            // Add progress indicator to container
            Grid grid = new Grid();
            grid.Children.Add(progressIndicator);
            progressContainer.Child = grid;
            
            return progressContainer;
        }
        
        // Method to animate progress change
        public void AnimateProgress(Border progressBar, double newProgress, double duration = 0.5)
        {
            if (progressBar?.Child is not Grid grid || grid.Children.Count == 0 || 
                grid.Children[0] is not Border progressIndicator)
            {
                return;
            }
            
            // Get the total width
            double totalWidth = progressBar.Width;
            
            // Create animation for width change
            DoubleAnimation widthAnimation = new DoubleAnimation
            {
                To = totalWidth * Math.Max(0, Math.Min(1, newProgress)), // Constrain to 0-100%
                Duration = new Duration(TimeSpan.FromSeconds(duration)),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };
            
            // Apply the animation
            progressIndicator.BeginAnimation(FrameworkElement.WidthProperty, widthAnimation);
            
            // Add a pulse animation for visual feedback
            if (newProgress > 0)
            {
                ScaleTransform scaleTransform;
                
                // Setup transform
                if (progressIndicator.RenderTransform is ScaleTransform existingScale)
                {
                    scaleTransform = existingScale;
                }
                else
                {
                    scaleTransform = new ScaleTransform(1.0, 1.0);
                    progressIndicator.RenderTransform = scaleTransform;
                    progressIndicator.RenderTransformOrigin = new Point(0.5, 0.5);
                }
                
                // Pulse animation - Y axis only (height)
                DoubleAnimation pulseAnimation = new DoubleAnimation
                {
                    From = 1.0,
                    To = 1.1,
                    Duration = new Duration(TimeSpan.FromSeconds(0.2)),
                    AutoReverse = true
                };
                
                progressIndicator.BeginAnimation(ScaleTransform.ScaleYProperty, pulseAnimation);
            }
        }

        // Add animated focus effects to text inputs
        private void AddTextInputAnimations()
        {
            try
            {
                // Find all text inputs
                var textInputs = FindVisualChildren<TextBox>(this).Cast<UIElement>()
                                 .Concat(FindVisualChildren<PasswordBox>(this).Cast<UIElement>())
                                 .ToList();
                
                foreach (var input in textInputs)
                {
                    if (input is TextBox textBox)
                    {
                        // For TextBox
                        textBox.GotFocus += TextInput_GotFocus;
                        textBox.LostFocus += TextInput_LostFocus;
                        textBox.TextChanged += TextBox_TextChanged;
                    }
                    else if (input is PasswordBox passwordBox)
                    {
                        // For PasswordBox
                        passwordBox.GotFocus += TextInput_GotFocus;
                        passwordBox.LostFocus += TextInput_LostFocus;
                        passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Text input animation error: {ex.Message}");
            }
        }
        
        private void TextInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                // Create storyboard for focus effect
                Storyboard storyboard = new Storyboard();
                
                // Scale effect - subtle grow
                ScaleTransform scaleTransform;
                
                // Set up transform
                if (element.RenderTransform is ScaleTransform existingScale)
                {
                    scaleTransform = existingScale;
                }
                else
                {
                    scaleTransform = new ScaleTransform(1.0, 1.0);
                    element.RenderTransform = scaleTransform;
                    element.RenderTransformOrigin = new Point(0.5, 0.5);
                }
                
                // X-axis scale animation
                DoubleAnimation scaleXAnimation = new DoubleAnimation
                {
                    To = 1.02,
                    Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };
                Storyboard.SetTarget(scaleXAnimation, element);
                Storyboard.SetTargetProperty(scaleXAnimation, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleX)"));
                storyboard.Children.Add(scaleXAnimation);
                
                // Y-axis scale animation
                DoubleAnimation scaleYAnimation = new DoubleAnimation
                {
                    To = 1.02,
                    Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };
                Storyboard.SetTarget(scaleYAnimation, element);
                Storyboard.SetTargetProperty(scaleYAnimation, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleY)"));
                storyboard.Children.Add(scaleYAnimation);
                
                // Start animation
                storyboard.Begin();
            }
        }
        
        private void TextInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                // Create storyboard for lost focus effect
                Storyboard storyboard = new Storyboard();
                
                // Scale X animation - return to normal
                DoubleAnimation scaleXAnimation = new DoubleAnimation
                {
                    To = 1.0,
                    Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };
                Storyboard.SetTarget(scaleXAnimation, element);
                Storyboard.SetTargetProperty(scaleXAnimation, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleX)"));
                storyboard.Children.Add(scaleXAnimation);
                
                // Scale Y animation - return to normal
                DoubleAnimation scaleYAnimation = new DoubleAnimation
                {
                    To = 1.0,
                    Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };
                Storyboard.SetTarget(scaleYAnimation, element);
                Storyboard.SetTargetProperty(scaleYAnimation, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleY)"));
                storyboard.Children.Add(scaleYAnimation);
                
                // Start the animation
                storyboard.Begin();
            }
        }
        
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            AnimateTextChanged(sender as UIElement);
        }
        
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            AnimateTextChanged(sender as UIElement);
        }
        
        private void AnimateTextChanged(UIElement? element)
        {
            if (element == null) return;
            
            // Create storyboard for text changed effect
            Storyboard storyboard = new Storyboard();
            
            // Subtle flash effect - change opacity briefly
            DoubleAnimation opacityAnimation = new DoubleAnimation
            {
                From = 0.7,
                To = 1.0,
                Duration = new Duration(TimeSpan.FromMilliseconds(150)),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };
            Storyboard.SetTarget(opacityAnimation, element);
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(UIElement.OpacityProperty));
            storyboard.Children.Add(opacityAnimation);
            
            // Start animation
            storyboard.Begin();
        }

        /// <summary>
        /// Creates an animated accordion/expander control
        /// </summary>
        /// <param name="header">Header text</param>
        /// <param name="content">Content to show when expanded</param>
        /// <returns>The expander control</returns>
        public FrameworkElement CreateAnimatedAccordion(string header, FrameworkElement content)
        {
            // Main container grid
            Grid mainGrid = new Grid();
            mainGrid.Margin = new Thickness(5);
            
            // Create rows for header and content
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0) });
            
            // Add border with shadow effect
            Border border = new Border
            {
                Background = new SolidColorBrush(Colors.White),
                CornerRadius = new CornerRadius(8),
                Margin = new Thickness(2),
                Padding = new Thickness(10),
                Effect = new DropShadowEffect
                {
                    Color = Colors.Black,
                    Direction = 270,
                    ShadowDepth = 3,
                    Opacity = 0.2,
                    BlurRadius = 5
                }
            };
            
            // Header panel with text and icon
            StackPanel headerPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(5)
            };
            
            // Header text
            TextBlock headerText = new TextBlock
            {
                Text = header,
                FontWeight = FontWeights.SemiBold,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 16,
                Margin = new Thickness(0, 0, 10, 0)
            };
            
            // Arrow icon for expand/collapse
            Path arrowIcon = new Path
            {
                Data = Geometry.Parse("M 0,0 L 10,0 L 5,8 Z"), // Simple triangle
                Fill = new SolidColorBrush(Colors.Gray),
                Width = 10,
                Height = 8,
                Stretch = Stretch.Fill,
                VerticalAlignment = VerticalAlignment.Center,
                RenderTransformOrigin = new Point(0.5, 0.5)
            };
            
            // Initialize the arrow icon's rotation transform
            RotateTransform rotateTransform = new RotateTransform(0);
            arrowIcon.RenderTransform = rotateTransform;
            
            // Add title and icon to header panel
            headerPanel.Children.Add(headerText);
            headerPanel.Children.Add(new Decorator { Width = 10 }); // Spacer
            headerPanel.Children.Add(arrowIcon);
            
            // Content container
            Border contentBorder = new Border
            {
                Padding = new Thickness(10, 0, 10, 10),
                Child = content,
                ClipToBounds = true
            };
            
            // Set grid positions
            Grid.SetRow(headerPanel, 0);
            Grid.SetRow(contentBorder, 1);
            
            // Add elements to main grid
            mainGrid.Children.Add(border);
            mainGrid.Children.Add(headerPanel);
            mainGrid.Children.Add(contentBorder);
            
            // Track expansion state
            bool isExpanded = false;
            
            // Click handler for expand/collapse
            headerPanel.MouseLeftButtonDown += (s, e) =>
            {
                isExpanded = !isExpanded;
                
                // Create animations
                Storyboard storyboard = new Storyboard();
                
                // Animation for content height
                GridLengthAnimation heightAnimation = new GridLengthAnimation
                {
                    Duration = new Duration(TimeSpan.FromMilliseconds(300)),
                    From = mainGrid.RowDefinitions[1].Height,
                    To = isExpanded ? new GridLength(content.DesiredSize.Height, GridUnitType.Pixel) : new GridLength(0),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };
                
                // Animation for arrow rotation
                DoubleAnimation rotateAnimation = new DoubleAnimation
                {
                    Duration = new Duration(TimeSpan.FromMilliseconds(300)),
                    To = isExpanded ? 180 : 0,
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };
                
                // Set animation targets
                Storyboard.SetTarget(rotateAnimation, arrowIcon);
                Storyboard.SetTargetProperty(rotateAnimation, new PropertyPath("(UIElement.RenderTransform).(RotateTransform.Angle)"));
                
                // Add animations to storyboard
                storyboard.Children.Add(rotateAnimation);
                
                // Start animations
                storyboard.Begin();
                
                // Handle height animation separately (since GridLengthAnimation isn't a standard animation)
                mainGrid.RowDefinitions[1].Height = heightAnimation.To;
            };
            
            // Measure the content for initial sizing
            content.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            
            return mainGrid;
        }
        
        /// <summary>
        /// Grid length animation helper class for accordion animation
        /// </summary>
        public class GridLengthAnimation : AnimationTimeline
        {
            public GridLengthAnimation()
            {
            }
            
            public static readonly DependencyProperty FromProperty = DependencyProperty.Register(
                "From", typeof(GridLength), typeof(GridLengthAnimation));
                
            public GridLength From
            {
                get { return (GridLength)GetValue(FromProperty); }
                set { SetValue(FromProperty, value); }
            }
            
            public static readonly DependencyProperty ToProperty = DependencyProperty.Register(
                "To", typeof(GridLength), typeof(GridLengthAnimation));
                
            public GridLength To
            {
                get { return (GridLength)GetValue(ToProperty); }
                set { SetValue(ToProperty, value); }
            }
            
            public static readonly DependencyProperty EasingFunctionProperty = DependencyProperty.Register(
                "EasingFunction", typeof(IEasingFunction), typeof(GridLengthAnimation));
                
            public IEasingFunction EasingFunction
            {
                get { return (IEasingFunction)GetValue(EasingFunctionProperty); }
                set { SetValue(EasingFunctionProperty, value); }
            }
            
            public override Type TargetPropertyType => typeof(GridLength);
            
            protected override Freezable CreateInstanceCore()
            {
                return new GridLengthAnimation();
            }
            
            public override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
            {
                double fromVal = ((GridLength)GetValue(FromProperty)).Value;
                double toVal = ((GridLength)GetValue(ToProperty)).Value;
                
                if (animationClock.CurrentProgress == null)
                    return fromVal;
                    
                double progress = animationClock.CurrentProgress.Value;
                if (EasingFunction != null)
                    progress = EasingFunction.Ease(progress);
                    
                return new GridLength((1 - progress) * fromVal + progress * toVal, To.IsStar ? GridUnitType.Star : GridUnitType.Pixel);
            }
        }
        
        // CreateTabControl method for animated tab navigation
        public TabControl CreateAnimatedTabControl()
        {
            TabControl tabControl = new TabControl();
            tabControl.Background = Brushes.Transparent;
            tabControl.BorderThickness = new Thickness(0);
            
            // Style for TabItems with animations
            Style tabItemStyle = new Style(typeof(TabItem));
            
            // Background color triggers
            Trigger normalTrigger = new Trigger { Property = TabItem.IsSelectedProperty, Value = false };
            normalTrigger.Setters.Add(new Setter(TabItem.BackgroundProperty, new SolidColorBrush(Color.FromRgb(230, 230, 230))));
            tabItemStyle.Triggers.Add(normalTrigger);
            
            Trigger selectedTrigger = new Trigger { Property = TabItem.IsSelectedProperty, Value = true };
            selectedTrigger.Setters.Add(new Setter(TabItem.BackgroundProperty, new SolidColorBrush(Color.FromRgb(255, 255, 255))));
            tabItemStyle.Triggers.Add(selectedTrigger);
            
            // Set default style values
            tabItemStyle.Setters.Add(new Setter(TabItem.MarginProperty, new Thickness(0, 0, 4, 0)));
            tabItemStyle.Setters.Add(new Setter(TabItem.PaddingProperty, new Thickness(15, 8, 15, 8)));
            tabItemStyle.Setters.Add(new Setter(TabItem.BorderThicknessProperty, new Thickness(0)));
            tabItemStyle.Setters.Add(new Setter(TabItem.FontSizeProperty, 14.0));
            
            // Apply the style
            tabControl.ItemContainerStyle = tabItemStyle;
            
            // Add selection changed animation
            tabControl.SelectionChanged += (sender, e) =>
            {
                if (e.AddedItems.Count > 0)
                {
                    // Find the TabItem
                    TabItem? newTab = null;
                    
                    // Try to get the TabItem directly if it's in AddedItems
                    if (e.AddedItems[0] is TabItem tabItem)
                    {
                        newTab = tabItem;
                    }
                    // Otherwise try to find the container for the selected content
                    else if (tabControl.ItemContainerGenerator.ContainerFromItem(e.AddedItems[0]) is TabItem container)
                    {
                        newTab = container;
                    }
                    
                    if (newTab != null && newTab.Content is UIElement content)
                    {
                        // Fade in animation for new content
                        Storyboard storyboard = new Storyboard();
                        
                        DoubleAnimation opacityAnimation = new DoubleAnimation
                        {
                            From = 0,
                            To = 1,
                            Duration = new Duration(TimeSpan.FromMilliseconds(250)),
                            EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                        };
                        
                        Storyboard.SetTarget(opacityAnimation, content);
                        Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(UIElement.OpacityProperty));
                        storyboard.Children.Add(opacityAnimation);
                        
                        // Scale animation
                        ScaleTransform scaleTransform;
                        
                        if (content.RenderTransform is ScaleTransform existingTransform)
                        {
                            scaleTransform = existingTransform;
                        }
                        else
                        {
                            scaleTransform = new ScaleTransform(0.95, 0.95);
                            content.RenderTransform = scaleTransform;
                            content.RenderTransformOrigin = new Point(0.5, 0.5);
                        }
                        
                        DoubleAnimation scaleXAnimation = new DoubleAnimation
                        {
                            From = 0.95,
                            To = 1.0,
                            Duration = new Duration(TimeSpan.FromMilliseconds(250)),
                            EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                        };
                        
                        DoubleAnimation scaleYAnimation = new DoubleAnimation
                        {
                            From = 0.95,
                            To = 1.0,
                            Duration = new Duration(TimeSpan.FromMilliseconds(250)),
                            EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                        };
                        
                        Storyboard.SetTarget(scaleXAnimation, content);
                        Storyboard.SetTarget(scaleYAnimation, content);
                        Storyboard.SetTargetProperty(scaleXAnimation, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleX)"));
                        Storyboard.SetTargetProperty(scaleYAnimation, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleY)"));
                        
                        storyboard.Children.Add(scaleXAnimation);
                        storyboard.Children.Add(scaleYAnimation);
                        
                        // Start animation
                        storyboard.Begin();
                    }
                }
            };
            
            return tabControl;
        }

        /// <summary>
        /// Creates an animated card with hover and click effects
        /// </summary>
        /// <param name="content">Content to display in the card</param>
        /// <returns>A border containing the card with animations</returns>
        public Border CreateAnimatedCard(UIElement content)
        {
            // Main card border
            Border cardBorder = new Border
            {
                Background = new SolidColorBrush(Colors.White),
                CornerRadius = new CornerRadius(8),
                Margin = new Thickness(10),
                Padding = new Thickness(15),
                Child = content,
                Effect = new DropShadowEffect
                {
                    Color = Colors.Black,
                    Direction = 315,
                    ShadowDepth = 4,
                    Opacity = 0.2,
                    BlurRadius = 10
                }
            };
            
            // Initialize transforms
            TransformGroup transformGroup = new TransformGroup();
            ScaleTransform scaleTransform = new ScaleTransform(1, 1);
            TranslateTransform translateTransform = new TranslateTransform(0, 0);
            
            transformGroup.Children.Add(scaleTransform);
            transformGroup.Children.Add(translateTransform);
            
            cardBorder.RenderTransform = transformGroup;
            cardBorder.RenderTransformOrigin = new Point(0.5, 0.5);
            
            // Mouse enter animation
            cardBorder.MouseEnter += (s, e) =>
            {
                // Create storyboard for hover effect
                Storyboard storyboard = new Storyboard();
                
                // Scale animation
                DoubleAnimation scaleXAnimation = new DoubleAnimation
                {
                    To = 1.05,
                    Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };
                Storyboard.SetTarget(scaleXAnimation, cardBorder);
                Storyboard.SetTargetProperty(scaleXAnimation, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)"));
                
                DoubleAnimation scaleYAnimation = new DoubleAnimation
                {
                    To = 1.05,
                    Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };
                Storyboard.SetTarget(scaleYAnimation, cardBorder);
                Storyboard.SetTargetProperty(scaleYAnimation, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)"));
                
                // Translate up slightly
                DoubleAnimation translateYAnimation = new DoubleAnimation
                {
                    To = -5,
                    Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };
                Storyboard.SetTarget(translateYAnimation, cardBorder);
                Storyboard.SetTargetProperty(translateYAnimation, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[1].(TranslateTransform.Y)"));
                
                // Shadow animation - create new effect with deeper shadow
                DropShadowEffect hoverEffect = new DropShadowEffect
                {
                    Color = Colors.Black,
                    Direction = 315,
                    ShadowDepth = 8,
                    Opacity = 0.3,
                    BlurRadius = 15
                };
                
                ObjectAnimationUsingKeyFrames shadowAnimation = new ObjectAnimationUsingKeyFrames();
                DiscreteObjectKeyFrame keyFrame = new DiscreteObjectKeyFrame(hoverEffect, KeyTime.FromTimeSpan(TimeSpan.Zero));
                shadowAnimation.KeyFrames.Add(keyFrame);
                
                Storyboard.SetTarget(shadowAnimation, cardBorder);
                Storyboard.SetTargetProperty(shadowAnimation, new PropertyPath(Border.EffectProperty));
                
                // Add animations to storyboard
                storyboard.Children.Add(scaleXAnimation);
                storyboard.Children.Add(scaleYAnimation);
                storyboard.Children.Add(translateYAnimation);
                storyboard.Children.Add(shadowAnimation);
                
                // Start animation
                storyboard.Begin();
            };
            
            // Mouse leave animation
            cardBorder.MouseLeave += (s, e) =>
            {
                // Create storyboard for returning to normal
                Storyboard storyboard = new Storyboard();
                
                // Scale animation
                DoubleAnimation scaleXAnimation = new DoubleAnimation
                {
                    To = 1.0,
                    Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };
                Storyboard.SetTarget(scaleXAnimation, cardBorder);
                Storyboard.SetTargetProperty(scaleXAnimation, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)"));
                
                DoubleAnimation scaleYAnimation = new DoubleAnimation
                {
                    To = 1.0,
                    Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };
                Storyboard.SetTarget(scaleYAnimation, cardBorder);
                Storyboard.SetTargetProperty(scaleYAnimation, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)"));
                
                // Translate back to original position
                DoubleAnimation translateYAnimation = new DoubleAnimation
                {
                    To = 0,
                    Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };
                Storyboard.SetTarget(translateYAnimation, cardBorder);
                Storyboard.SetTargetProperty(translateYAnimation, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[1].(TranslateTransform.Y)"));
                
                // Shadow animation - return to original shadow
                DropShadowEffect normalEffect = new DropShadowEffect
                {
                    Color = Colors.Black,
                    Direction = 315,
                    ShadowDepth = 4,
                    Opacity = 0.2,
                    BlurRadius = 10
                };
                
                ObjectAnimationUsingKeyFrames shadowAnimation = new ObjectAnimationUsingKeyFrames();
                DiscreteObjectKeyFrame keyFrame = new DiscreteObjectKeyFrame(normalEffect, KeyTime.FromTimeSpan(TimeSpan.Zero));
                shadowAnimation.KeyFrames.Add(keyFrame);
                
                Storyboard.SetTarget(shadowAnimation, cardBorder);
                Storyboard.SetTargetProperty(shadowAnimation, new PropertyPath(Border.EffectProperty));
                
                // Add animations to storyboard
                storyboard.Children.Add(scaleXAnimation);
                storyboard.Children.Add(scaleYAnimation);
                storyboard.Children.Add(translateYAnimation);
                storyboard.Children.Add(shadowAnimation);
                
                // Start animation
                storyboard.Begin();
            };
            
            // Click animation
            cardBorder.MouseLeftButtonDown += (s, e) =>
            {
                // Create storyboard for click effect
                Storyboard storyboard = new Storyboard();
                
                // Quick scale down then up
                DoubleAnimation scaleXAnimation = new DoubleAnimation
                {
                    To = 0.98,
                    Duration = new Duration(TimeSpan.FromMilliseconds(100)),
                    AutoReverse = true,
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };
                Storyboard.SetTarget(scaleXAnimation, cardBorder);
                Storyboard.SetTargetProperty(scaleXAnimation, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)"));
                
                DoubleAnimation scaleYAnimation = new DoubleAnimation
                {
                    To = 0.98,
                    Duration = new Duration(TimeSpan.FromMilliseconds(100)),
                    AutoReverse = true,
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                };
                Storyboard.SetTarget(scaleYAnimation, cardBorder);
                Storyboard.SetTargetProperty(scaleYAnimation, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)"));
                
                // Add animations to storyboard
                storyboard.Children.Add(scaleXAnimation);
                storyboard.Children.Add(scaleYAnimation);
                
                // Start animation
                storyboard.Begin();
            };
            
            return cardBorder;
        }

        /// <summary>
        /// Adds staggered fade-in animations to list items
        /// </summary>
        /// <param name="listBox">The ListBox to animate</param>
        public void AddListItemAnimations(ListBox listBox)
        {
            // Store original items
            var itemsSource = listBox.ItemsSource;
            if (itemsSource == null) return;
            
            // Create new stack panel for items
            StackPanel itemsPanel = new StackPanel();
            listBox.ItemsPanel = new ItemsPanelTemplate(new FrameworkElementFactory(typeof(StackPanel)));
            
            // Add loaded event handler
            listBox.Loaded += (s, e) =>
            {
                // For each item in the ListBox
                int delay = 0;
                
                foreach (var item in listBox.Items)
                {
                    // Get the container for this item
                    if (listBox.ItemContainerGenerator.ContainerFromItem(item) is ListBoxItem container)
                    {
                        // Set initial opacity to 0
                        container.Opacity = 0;
                        
                        // Prepare transforms
                        TransformGroup transformGroup = new TransformGroup();
                        ScaleTransform scaleTransform = new ScaleTransform(0.9, 0.9);
                        TranslateTransform translateTransform = new TranslateTransform(0, 20);
                        
                        transformGroup.Children.Add(scaleTransform);
                        transformGroup.Children.Add(translateTransform);
                        
                        container.RenderTransform = transformGroup;
                        container.RenderTransformOrigin = new Point(0.5, 0.5);
                        
                        // Create storyboard for this item
                        Storyboard storyboard = new Storyboard();
                        
                        // Opacity animation
                        DoubleAnimation opacityAnimation = new DoubleAnimation
                        {
                            From = 0,
                            To = 1,
                            Duration = new Duration(TimeSpan.FromMilliseconds(300)),
                            EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut },
                            BeginTime = TimeSpan.FromMilliseconds(delay)
                        };
                        Storyboard.SetTarget(opacityAnimation, container);
                        Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(UIElement.OpacityProperty));
                        storyboard.Children.Add(opacityAnimation);
                        
                        // Scale animation
                        DoubleAnimation scaleXAnimation = new DoubleAnimation
                        {
                            From = 0.9,
                            To = 1.0,
                            Duration = new Duration(TimeSpan.FromMilliseconds(300)),
                            EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut },
                            BeginTime = TimeSpan.FromMilliseconds(delay)
                        };
                        Storyboard.SetTarget(scaleXAnimation, container);
                        Storyboard.SetTargetProperty(scaleXAnimation, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)"));
                        storyboard.Children.Add(scaleXAnimation);
                        
                        DoubleAnimation scaleYAnimation = new DoubleAnimation
                        {
                            From = 0.9,
                            To = 1.0,
                            Duration = new Duration(TimeSpan.FromMilliseconds(300)),
                            EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut },
                            BeginTime = TimeSpan.FromMilliseconds(delay)
                        };
                        Storyboard.SetTarget(scaleYAnimation, container);
                        Storyboard.SetTargetProperty(scaleYAnimation, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)"));
                        storyboard.Children.Add(scaleYAnimation);
                        
                        // Translate animation
                        DoubleAnimation translateYAnimation = new DoubleAnimation
                        {
                            From = 20,
                            To = 0,
                            Duration = new Duration(TimeSpan.FromMilliseconds(300)),
                            EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut },
                            BeginTime = TimeSpan.FromMilliseconds(delay)
                        };
                        Storyboard.SetTarget(translateYAnimation, container);
                        Storyboard.SetTargetProperty(translateYAnimation, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[1].(TranslateTransform.Y)"));
                        storyboard.Children.Add(translateYAnimation);
                        
                        // Start animation
                        storyboard.Begin();
                        
                        // Increase delay for next item (staggered effect)
                        delay += 50;
                    }
                }
            };
            
            // Add selection change animation
            listBox.SelectionChanged += (s, e) =>
            {
                if (e.AddedItems.Count > 0 && listBox.ItemContainerGenerator.ContainerFromItem(e.AddedItems[0]) is ListBoxItem selectedContainer)
                {
                    // Create storyboard for selection effect
                    Storyboard storyboard = new Storyboard();
                    
                    // Pulse scale animation
                    ScaleTransform? scaleTransform;
                    
                    if (selectedContainer.RenderTransform is TransformGroup group)
                    {
                        scaleTransform = group.Children.OfType<ScaleTransform>().FirstOrDefault();
                        if (scaleTransform == null)
                        {
                            scaleTransform = new ScaleTransform(1.0, 1.0);
                            group.Children.Add(scaleTransform);
                        }
                    }
                    else
                    {
                        // If no transform group exists yet
                        TransformGroup transformGroup = new TransformGroup();
                        scaleTransform = new ScaleTransform(1.0, 1.0);
                        TranslateTransform translateTransform = new TranslateTransform(0, 0);
                        
                        transformGroup.Children.Add(scaleTransform);
                        transformGroup.Children.Add(translateTransform);
                        
                        selectedContainer.RenderTransform = transformGroup;
                        selectedContainer.RenderTransformOrigin = new Point(0.5, 0.5);
                    }
                    
                    DoubleAnimation scaleXAnimation = new DoubleAnimation
                    {
                        From = 1.0,
                        To = 1.05,
                        Duration = new Duration(TimeSpan.FromMilliseconds(150)),
                        AutoReverse = true,
                        EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                    };
                    Storyboard.SetTarget(scaleXAnimation, selectedContainer);
                    Storyboard.SetTargetProperty(scaleXAnimation, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)"));
                    
                    DoubleAnimation scaleYAnimation = new DoubleAnimation
                    {
                        From = 1.0,
                        To = 1.05,
                        Duration = new Duration(TimeSpan.FromMilliseconds(150)),
                        AutoReverse = true,
                        EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                    };
                    Storyboard.SetTarget(scaleYAnimation, selectedContainer);
                    Storyboard.SetTargetProperty(scaleYAnimation, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)"));
                    
                    // Add animations to storyboard
                    storyboard.Children.Add(scaleXAnimation);
                    storyboard.Children.Add(scaleYAnimation);
                    
                    // Start animation
                    storyboard.Begin();
                }
            };
        }

        private void LeaderboardButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Check if MainFrame exists
                if (MainFrame == null)
                {
                    MessageBox.Show("Navigation frame not found.", "Navigation Error", 
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                
                // Create and navigate to the LeaderboardPage
                var leaderboardPage = new Page.LeaderboardPage();
                MainFrame.Navigate(leaderboardPage);
                
                // Apply animation transition
                ApplyPageTransition(leaderboardPage, TransitionType.SlideFromRight);
                
                // Hide home content
                if (HomeContent != null)
                    HomeContent.Visibility = Visibility.Collapsed;
                
                // Show the main frame
                MainFrame.Visibility = Visibility.Visible;
                
                // Find the right panel and hide it
                var rightPanel = this.FindName("RightPanel") as UIElement;
                if (rightPanel != null)
                    rightPanel.Visibility = Visibility.Collapsed;
                
                // Make the content take full width
                Grid? parentGrid = MainFrame.Parent as Grid;
                if (parentGrid != null)
                    Grid.SetColumnSpan(parentGrid, 2);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to leaderboard: {ex.Message}", 
                               "Navigation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                // Find the button and update its content
                if (sender is Button button)
                {
                    button.Content = "\uE922"; // Maximize icon
                }
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                // Find the button and update its content
                if (sender is Button button)
                {
                    button.Content = "\uE923"; // Restore icon
                }
            }
        }

        public void NavigateToLearnLevel()
        {
            System.Diagnostics.Debug.WriteLine("NavigateToLearnLevel: Method started.");
            if (MainFrame == null)
            {
                System.Diagnostics.Debug.WriteLine("NavigateToLearnLevel Error: MainFrame is null.");
                MessageBox.Show("Navigation frame is not initialized.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                System.Diagnostics.Debug.WriteLine("NavigateToLearnLevel: Creating LearnLevelPage...");
                // Create and navigate to the LearnLevelPage
                var learnLevelPage = new Page.LearnLevelPage();
                System.Diagnostics.Debug.WriteLine("NavigateToLearnLevel: Navigating MainFrame...");
                MainFrame.Navigate(learnLevelPage);
                System.Diagnostics.Debug.WriteLine("NavigateToLearnLevel: Navigation complete. Applying animation...");

                // Apply animation transition
                var animation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromMilliseconds(300),
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
                };

                MainFrame.BeginAnimation(OpacityProperty, animation);
                System.Diagnostics.Debug.WriteLine("NavigateToLearnLevel: Animation applied. Hiding elements...");

                // Hide home content and right panel
                if (HomeContent != null)
                {
                    HomeContent.Visibility = Visibility.Collapsed;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("NavigateToLearnLevel: HomeContent is null.");
                }

                if (RightPanel != null)
                {
                    RightPanel.Visibility = Visibility.Collapsed;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("NavigateToLearnLevel: RightPanel is null.");
                }
                System.Diagnostics.Debug.WriteLine("NavigateToLearnLevel: Elements hidden. Adjusting layout...");

                // Make MainFrame span both columns
                // Corrected: Set ColumnSpan on MainFrame itself, not its parent.
                Grid.SetColumnSpan(MainFrame, 2);
                System.Diagnostics.Debug.WriteLine("NavigateToLearnLevel: Set MainFrame ColumnSpan to 2.");
                
                // Ensure MainFrame is visible
                MainFrame.Visibility = Visibility.Visible;
                System.Diagnostics.Debug.WriteLine("NavigateToLearnLevel: MainFrame set to visible. Method complete.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"NavigateToLearnLevel Exception: {ex.ToString()}");
                MessageBox.Show($"Failed to navigate to Learn Level page.\nError: {ex.Message}", "Navigation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
