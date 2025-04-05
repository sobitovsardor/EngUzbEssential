using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Linq;

namespace EngUzbEssential.Page
{
    /// <summary>
    /// Interaction logic for TranslaterPage.xaml
    /// </summary>
    public partial class TranslaterPage : System.Windows.Controls.Page
    {
        // Demo dictionary for English to Uzbek translations (would be replaced with a proper database/API in a real app)
        private Dictionary<string, string> englishToUzbek = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "hello", "salom" },
            { "goodbye", "xayr" },
            { "thank you", "rahmat" },
            { "yes", "ha" },
            { "no", "yo'q" },
            { "please", "iltimos" },
            { "sorry", "kechirasiz" },
            { "how are you", "qalaysiz" },
            { "good morning", "xayrli tong" },
            { "good evening", "xayrli kech" },
            { "welcome", "xush kelibsiz" },
            { "friend", "do'st" },
            { "water", "suv" },
            { "food", "ovqat" },
            { "language", "til" },
            { "book", "kitob" },
            { "student", "talaba" },
            { "teacher", "o'qituvchi" },
            { "school", "maktab" },
            { "learn", "o'rganmoq" }
        };

        // Demo dictionary for Uzbek to English translations
        private Dictionary<string, string> uzbekToEnglish = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "salom", "hello" },
            { "xayr", "goodbye" },
            { "rahmat", "thank you" },
            { "ha", "yes" },
            { "yo'q", "no" },
            { "iltimos", "please" },
            { "kechirasiz", "sorry" },
            { "qalaysiz", "how are you" },
            { "xayrli tong", "good morning" },
            { "xayrli kech", "good evening" },
            { "xush kelibsiz", "welcome" },
            { "do'st", "friend" },
            { "suv", "water" },
            { "ovqat", "food" },
            { "til", "language" },
            { "kitob", "book" },
            { "talaba", "student" },
            { "o'qituvchi", "teacher" },
            { "maktab", "school" },
            { "o'rganmoq", "learn" }
        };

        // Track translation history
        private List<TranslationItem> translationHistory = new List<TranslationItem>();
        
        // Track favorite translations
        private List<TranslationItem> translationFavorites = new List<TranslationItem>();
        
        // Maximum allowed characters for translation
        private const int MaxCharacters = 500;
        
        // Flag to avoid text change triggers during programmatic changes
        private bool isUpdatingTextProgrammatically = false;

        // Add property to control initial UI visibility
        public bool ShouldHideUI { get; set; } = false;

        public TranslaterPage()
        {
            try
            {
                InitializeComponent();
            
                // We must attach to the Loaded event here in the constructor
                // to ensure it runs after all XAML elements are created
                this.Loaded += TranslaterPage_Loaded;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during TranslaterPage initialization: {ex.Message}\n\nStack trace: {ex.StackTrace}", 
                               "Critical Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void TranslaterPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // Initialize UI elements in the correct order
                SetupPlaceholderText();
                UpdateCharacterCount();
                
                // No need to set sidebar states since we've hidden the sidebar
                
                // Check if we need to hide UI elements on startup
                // To use this feature, you can add a query parameter or set a property before navigation
                if (ShouldHideUI)
                {
                    // This will hide all UI except the essential elements
                    HideNonEssentialUI();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during TranslaterPage load: {ex.Message}", 
                              "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void SetupPlaceholderText()
        {
            // Add placeholder text behavior to input textbox
            if (InputTextBox != null)
            {
                InputTextBox.GotFocus += (s, e) => 
                {
                    if (InputTextBox.Text == InputTextBox.Tag?.ToString())
                    {
                        isUpdatingTextProgrammatically = true;
                        InputTextBox.Text = string.Empty;
                        InputTextBox.Foreground = Brushes.Black;
                        isUpdatingTextProgrammatically = false;
                    }
                };
                
                // Initialize with placeholder
                if (string.IsNullOrEmpty(InputTextBox.Text) && InputTextBox.Tag != null)
                {
                    InputTextBox.Text = InputTextBox.Tag.ToString();
                    InputTextBox.Foreground = Brushes.Gray;
                }
            }
            
            // Dictionary search box code removed since the sidebar is hidden
        }
        
        private void InputTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (InputTextBox == null)
                    return;
            
                // Restore placeholder text if empty
                if (string.IsNullOrEmpty(InputTextBox.Text) && InputTextBox.Tag != null)
                {
                    isUpdatingTextProgrammatically = true;
                    InputTextBox.Text = InputTextBox.Tag.ToString();
                    InputTextBox.Foreground = Brushes.Gray;
                    isUpdatingTextProgrammatically = false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in InputTextBox_LostFocus: {ex.Message}");
            }
        }
        
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get the main window
                if (Application.Current.MainWindow is MainWindow mainWindow)
                {
                    // Use the existing method to navigate home
                    mainWindow.NavigateToHome();
                }
                else
                {
                    // Fallback if we can't get the main window
                    MessageBox.Show("Could not navigate back to home.", "Navigation Error", 
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                // Show error if navigation fails
                MessageBox.Show($"Error navigating back: {ex.Message}", "Navigation Error", 
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // Early initialization/load: OutputTextBox may not be initialized yet
                if (OutputTextBox == null)
                {
                    System.Diagnostics.Debug.WriteLine("LanguageComboBox_SelectionChanged: OutputTextBox is null");
                    return;
                }
                
                // Clear the output when language selection changes
                OutputTextBox.Text = string.Empty;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in LanguageComboBox_SelectionChanged: {ex.Message}");
            }
        }
        
        private void SwapLanguages_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (FromLanguageComboBox == null || ToLanguageComboBox == null || 
                    InputTextBox == null || OutputTextBox == null)
                    return;
            
                // Swap selected languages
                int fromIndex = FromLanguageComboBox.SelectedIndex;
                int toIndex = ToLanguageComboBox.SelectedIndex;
                
                FromLanguageComboBox.SelectedIndex = toIndex;
                ToLanguageComboBox.SelectedIndex = fromIndex;
                
                // Swap text in input and output boxes (if both have text)
                if (!string.IsNullOrEmpty(OutputTextBox.Text) && 
                    InputTextBox.Text != InputTextBox.Tag?.ToString())
                {
                    string inputText = InputTextBox.Text;
                    
                    isUpdatingTextProgrammatically = true;
                    InputTextBox.Text = OutputTextBox.Text;
                    OutputTextBox.Text = inputText;
                    isUpdatingTextProgrammatically = false;
                    
                    // Update character count
                    UpdateCharacterCount();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in SwapLanguages_Click: {ex.Message}");
            }
        }
        
        private void InputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!isUpdatingTextProgrammatically)
            {
                // Update character count
                UpdateCharacterCount();
                
                // Clear output when input changes
                OutputTextBox.Text = string.Empty;
            }
        }
        
        private void UpdateCharacterCount()
        {
            // Check if UI elements exist before using them
            if (InputTextBox == null || CharacterCountTextBlock == null)
                return;
            
            // Don't count placeholder text
            int length = (InputTextBox.Text == InputTextBox.Tag?.ToString()) 
                        ? 0 
                        : InputTextBox.Text?.Length ?? 0;
            
            // Update the character count display
            CharacterCountTextBlock.Text = $"{length}/{MaxCharacters} characters";
            
            // Warn user if approaching or exceeding limit
            if (length > MaxCharacters * 0.9)
            {
                CharacterCountTextBlock.Foreground = length > MaxCharacters 
                    ? Brushes.Red 
                    : new SolidColorBrush(Color.FromRgb(255, 165, 0)); // Orange
            }
            else
            {
                CharacterCountTextBlock.Foreground = new SolidColorBrush(Color.FromRgb(102, 106, 122)); // Default gray
            }
            
            // If over limit, trim the text
            if (length > MaxCharacters && InputTextBox.Text != InputTextBox.Tag?.ToString())
            {
                isUpdatingTextProgrammatically = true;
                InputTextBox.Text = InputTextBox.Text?.Substring(0, MaxCharacters);
                InputTextBox.CaretIndex = MaxCharacters;
                isUpdatingTextProgrammatically = false;
                
                // Update count after trim
                CharacterCountTextBlock.Text = $"{MaxCharacters}/{MaxCharacters} characters";
            }
        }
        
        private void ClearInput_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (InputTextBox == null || OutputTextBox == null || CharacterCountTextBlock == null)
                    return;
            
                // Clear the input and set to placeholder
                isUpdatingTextProgrammatically = true;
                if (InputTextBox.Tag != null)
                {
                    InputTextBox.Text = InputTextBox.Tag.ToString();
                    InputTextBox.Foreground = Brushes.Gray;
                }
                else
                {
                    InputTextBox.Text = string.Empty;
                }
                isUpdatingTextProgrammatically = false;
                
                // Clear the output
                OutputTextBox.Text = string.Empty;
                
                // Reset character count
                UpdateCharacterCount();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in ClearInput_Click: {ex.Message}");
            }
        }
        
        private void TranslateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Verify all controls exist
                if (InputTextBox == null || OutputTextBox == null || 
                    FromLanguageComboBox == null || ToLanguageComboBox == null)
                {
                    MessageBox.Show("UI controls not properly initialized. Please try again.", 
                                  "Translation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                
                // Don't translate placeholder text
                if (InputTextBox.Tag != null && InputTextBox.Text == InputTextBox.Tag.ToString())
                {
                    MessageBox.Show("Please enter text to translate.", "Translation Error", 
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                
                // Check if exceeds character limit
                if (InputTextBox.Text != null && InputTextBox.Text.Length > MaxCharacters)
                {
                    MessageBox.Show($"Text exceeds the maximum of {MaxCharacters} characters. Please shorten your text.", 
                                  "Translation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                
                // Get selected languages
                var fromLanguageItem = FromLanguageComboBox.SelectedItem as ComboBoxItem;
                var toLanguageItem = ToLanguageComboBox.SelectedItem as ComboBoxItem;
                
                string? fromLanguage = fromLanguageItem?.Content?.ToString();
                string? toLanguage = toLanguageItem?.Content?.ToString();
                
                if (string.IsNullOrEmpty(fromLanguage) || string.IsNullOrEmpty(toLanguage))
                {
                    MessageBox.Show("Please select languages for translation.", "Translation Error", 
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                
                // Get the text to translate
                string textToTranslate = InputTextBox.Text?.Trim() ?? string.Empty;
                if (string.IsNullOrEmpty(textToTranslate))
                {
                    MessageBox.Show("Please enter text to translate.", "Translation Error", 
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                
                // Perform the translation
                string translatedText = Translate(textToTranslate, fromLanguage, toLanguage);
                
                // Display the translation
                OutputTextBox.Text = translatedText;
                
                // Add to history
                AddToHistory(textToTranslate, translatedText, fromLanguage, toLanguage);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during translation: {ex.Message}", 
                               "Translation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Diagnostics.Debug.WriteLine($"Error in TranslateButton_Click: {ex.Message}\n{ex.StackTrace}");
            }
        }
        
        private string Translate(string text, string fromLanguage, string toLanguage)
        {
            // Simple word-by-word translation demo
            // In a real app, this would call a translation API or service
            
            string result = "";
            
            // Select the appropriate dictionary based on translation direction
            Dictionary<string, string> translationDict;
            
            if (fromLanguage == "English" && toLanguage == "Uzbek")
            {
                translationDict = englishToUzbek;
            }
            else if (fromLanguage == "Uzbek" && toLanguage == "English")
            {
                translationDict = uzbekToEnglish;
            }
            else
            {
                // Same language, just return the original text
                return text;
            }
            
            // For this demo, we'll do a very simple translation
            // by looking up each word individually
            
            // First, try exact phrase match
            if (translationDict.TryGetValue(text.ToLower(), out string exactMatch))
            {
                return exactMatch;
            }
            
            // If no exact match, try word by word
            string[] words = text.Split(new[] { ' ', ',', '.', '!', '?', ';', ':', '-', '\n', '\r', '\t' }, 
                                      StringSplitOptions.RemoveEmptyEntries);
            
            foreach (string word in words)
            {
                if (translationDict.TryGetValue(word.ToLower(), out string translation))
                {
                    result += translation + " ";
                }
                else
                {
                    // If word not found in dictionary, keep the original
                    result += word + " ";
                }
            }
            
            return result.Trim();
        }
        
        private void CopyOutput_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(OutputTextBox.Text))
            {
                // Copy to clipboard
                Clipboard.SetText(OutputTextBox.Text);
                
                // Show brief notification
                MessageBox.Show("Translation copied to clipboard.", "Copy Successful", 
                              MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        
        private void AddToFavorites_Click(object sender, RoutedEventArgs e)
        {
            // Check if there's text to save
            if (string.IsNullOrEmpty(OutputTextBox.Text) || 
                InputTextBox.Text == InputTextBox.Tag.ToString())
            {
                MessageBox.Show("Please translate some text before adding to favorites.", 
                              "Favorites", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            
            // Get selected languages
            var fromLanguageItem = FromLanguageComboBox.SelectedItem as ComboBoxItem;
            var toLanguageItem = ToLanguageComboBox.SelectedItem as ComboBoxItem;
            
            string? fromLanguage = fromLanguageItem?.Content?.ToString();
            string? toLanguage = toLanguageItem?.Content?.ToString();
            
            if (string.IsNullOrEmpty(fromLanguage) || string.IsNullOrEmpty(toLanguage))
            {
                MessageBox.Show("Something went wrong. Language selection is invalid.", 
                              "Favorites", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            // Create a new favorite item
            TranslationItem favoriteItem = new TranslationItem
            {
                SourceText = InputTextBox.Text,
                TranslatedText = OutputTextBox.Text,
                FromLanguage = fromLanguage,
                ToLanguage = toLanguage,
                Timestamp = DateTime.Now
            };
            
            // Check if this translation is already in favorites
            bool isDuplicate = translationFavorites.Any(f => 
                f.SourceText == favoriteItem.SourceText && 
                f.TranslatedText == favoriteItem.TranslatedText &&
                f.FromLanguage == favoriteItem.FromLanguage &&
                f.ToLanguage == favoriteItem.ToLanguage);
            
            if (!isDuplicate)
            {
                // Add to favorites
                translationFavorites.Insert(0, favoriteItem);
                
                // Update the UI
                UpdateFavoritesUI();
                
                // Show confirmation
                MessageBox.Show("Translation added to favorites.", "Favorites", 
                              MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("This translation is already in your favorites.", 
                              "Favorites", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        
        private void AddToHistory(string sourceText, string translatedText, string fromLanguage, string toLanguage)
        {
            // Create a new history item
            TranslationItem historyItem = new TranslationItem
            {
                SourceText = sourceText,
                TranslatedText = translatedText,
                FromLanguage = fromLanguage,
                ToLanguage = toLanguage,
                Timestamp = DateTime.Now
            };
            
            // Add to history (at the beginning)
            translationHistory.Insert(0, historyItem);
            
            // Limit history size to prevent memory issues (keep last 20 items)
            if (translationHistory.Count > 20)
            {
                translationHistory.RemoveAt(translationHistory.Count - 1);
            }
            
            // Update the UI
            UpdateHistoryUI();
        }
        
        private void UpdateHistoryUI()
        {
            try
            {
                // Clear the list
                if (HistoryList != null)
                {
                    // Remove all items except the default message (first item)
                    while (HistoryList.Children.Count > 1)
                    {
                        HistoryList.Children.RemoveAt(1);
                    }
                    
                    // Hide the default message if we have history items
                    if (translationHistory.Count > 0 && HistoryList.Children.Count > 0)
                    {
                        HistoryList.Children[0].Visibility = Visibility.Collapsed;
                    }
                    else if (HistoryList.Children.Count > 0)
                    {
                        HistoryList.Children[0].Visibility = Visibility.Visible;
                    }
                    
                    // Add each history item to the UI
                    foreach (var item in translationHistory)
                    {
                        HistoryList.Children.Add(CreateTranslationItemUI(item));
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in UpdateHistoryUI: {ex.Message}");
            }
        }
        
        private void UpdateFavoritesUI()
        {
            try
            {
                // Clear the list
                if (FavoritesList != null)
                {
                    // Remove all items except the default message (first item)
                    while (FavoritesList.Children.Count > 1)
                    {
                        FavoritesList.Children.RemoveAt(1);
                    }
                    
                    // Hide the default message if we have favorite items
                    if (translationFavorites.Count > 0 && FavoritesList.Children.Count > 0)
                    {
                        FavoritesList.Children[0].Visibility = Visibility.Collapsed;
                    }
                    else if (FavoritesList.Children.Count > 0)
                    {
                        FavoritesList.Children[0].Visibility = Visibility.Visible;
                    }
                    
                    // Add each favorite item to the UI
                    foreach (var item in translationFavorites)
                    {
                        FavoritesList.Children.Add(CreateTranslationItemUI(item, true));
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in UpdateFavoritesUI: {ex.Message}");
            }
        }
        
        private Border CreateTranslationItemUI(TranslationItem item, bool isFavorite = false)
        {
            // Main container for the item
            Border itemBorder = new Border
            {
                BorderThickness = new Thickness(1),
                BorderBrush = new SolidColorBrush(Color.FromRgb(224, 224, 224)), // #E0E0E0
                Margin = new Thickness(0, 0, 0, 8),
                Padding = new Thickness(10),
                CornerRadius = new CornerRadius(3)
            };
            
            // Create a stack panel for the content
            StackPanel itemContent = new StackPanel();
            
            // Language info
            TextBlock languageInfo = new TextBlock
            {
                Text = $"{item.FromLanguage} → {item.ToLanguage}",
                FontSize = 12,
                Foreground = new SolidColorBrush(Color.FromRgb(102, 106, 122)), // #666A7A
            };
            
            // Source text - safely handle potential null values
            string sourceDisplayText = item.SourceText;
            if (sourceDisplayText.Length > 50)
            {
                sourceDisplayText = sourceDisplayText.Substring(0, 47) + "...";
            }
            
            TextBlock sourceText = new TextBlock
            {
                Text = sourceDisplayText,
                FontWeight = FontWeights.SemiBold,
                Margin = new Thickness(0, 5, 0, 5)
            };
            
            // Translated text - safely handle potential null values
            string translatedDisplayText = item.TranslatedText;
            if (translatedDisplayText.Length > 50)
            {
                translatedDisplayText = translatedDisplayText.Substring(0, 47) + "...";
            }
            
            TextBlock translatedText = new TextBlock
            {
                Text = translatedDisplayText,
                Foreground = new SolidColorBrush(Color.FromRgb(63, 81, 181)) // #3F51B5
            };
            
            // Add elements to the stack panel
            itemContent.Children.Add(languageInfo);
            itemContent.Children.Add(sourceText);
            itemContent.Children.Add(translatedText);
            
            // For favorites, add a remove button
            if (isFavorite)
            {
                Button removeButton = new Button
                {
                    Content = "Remove",
                    Padding = new Thickness(8, 3, 8, 3),
                    Margin = new Thickness(0, 5, 0, 0),
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Background = Brushes.Transparent,
                    Foreground = new SolidColorBrush(Color.FromRgb(255, 87, 34)), // #FF5722
                    BorderThickness = new Thickness(0)
                };
                
                // Handle remove action
                removeButton.Click += (s, e) => 
                {
                    translationFavorites.Remove(item);
                    UpdateFavoritesUI();
                };
                
                itemContent.Children.Add(removeButton);
            }
            
            // Make the item clickable to load the translation
            itemBorder.MouseLeftButtonDown += (s, e) => 
            {
                // Set the input and output text
                isUpdatingTextProgrammatically = true;
                
                InputTextBox.Text = item.SourceText;
                InputTextBox.Foreground = Brushes.Black;
                
                OutputTextBox.Text = item.TranslatedText;
                
                // Set the language dropdowns
                for (int i = 0; i < FromLanguageComboBox.Items.Count; i++)
                {
                    var comboItem = FromLanguageComboBox.Items[i] as ComboBoxItem;
                    string? itemContent = comboItem?.Content?.ToString();
                    if (itemContent == item.FromLanguage)
                    {
                        FromLanguageComboBox.SelectedIndex = i;
                        break;
                    }
                }
                
                for (int i = 0; i < ToLanguageComboBox.Items.Count; i++)
                {
                    var comboItem = ToLanguageComboBox.Items[i] as ComboBoxItem;
                    string? itemContent = comboItem?.Content?.ToString();
                    if (itemContent == item.ToLanguage)
                    {
                        ToLanguageComboBox.SelectedIndex = i;
                        break;
                    }
                }
                
                isUpdatingTextProgrammatically = false;
                
                // Update the character count
                UpdateCharacterCount();
            };
            
            // Add hover effect
            itemBorder.MouseEnter += (s, e) => 
            {
                itemBorder.Background = new SolidColorBrush(Color.FromRgb(245, 245, 245));
                itemBorder.Cursor = Cursors.Hand;
            };
            
            itemBorder.MouseLeave += (s, e) => 
            {
                itemBorder.Background = Brushes.Transparent;
            };
            
            // Set the content
            itemBorder.Child = itemContent;
            
            return itemBorder;
        }
        
        private void ClearHistory_Click(object sender, MouseButtonEventArgs e)
        {
            // Since history panel is removed, this method can be empty or simplified
        }
        
        private void ClearFavorites_Click(object sender, MouseButtonEventArgs e)
        {
            // Since favorites panel is removed, this method can be empty or simplified
        }
        
        private void SidebarTab_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is RadioButton radioButton && 
                    HistoryContent != null && 
                    FavoritesContent != null)
                {
                    // Get the tag to identify which tab was selected
                    string? tag = radioButton.Tag?.ToString();
                    
                    // Show the appropriate content based on the selected tab
                    if (tag == "History")
                    {
                        HistoryContent.Visibility = Visibility.Visible;
                        FavoritesContent.Visibility = Visibility.Collapsed;
                        
                        // Update history list
                        UpdateHistoryUI();
                    }
                    else if (tag == "Favorites")
                    {
                        HistoryContent.Visibility = Visibility.Collapsed;
                        FavoritesContent.Visibility = Visibility.Visible;
                        
                        // Update favorites list
                        UpdateFavoritesUI();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in SidebarTab_Checked: {ex.Message}");
            }
        }

        // Add handlers for sidebar toggle
        private void SidebarToggle_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                // When sidebar is opened, update the appropriate content
                if (HistoryTab != null && HistoryTab.IsChecked == true)
                {
                    UpdateHistoryUI();
                }
                else if (FavoritesTab != null && FavoritesTab.IsChecked == true)
                {
                    UpdateFavoritesUI();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in SidebarToggle_Checked: {ex.Message}");
            }
        }

        private void SidebarToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            // Nothing special needed when sidebar is closed
        }

        /// <summary>
        /// Toggles the visibility of the entire UI or specific sections
        /// </summary>
        /// <param name="section">Optional section name: "all", "main", "sidebar", or null to toggle everything</param>
        /// <param name="isVisible">Whether to make the section visible or hidden</param>
        public void ToggleUIVisibility(string section = "all", bool isVisible = false)
        {
            try
            {
                // Set the visibility state
                Visibility visibilityState = isVisible ? Visibility.Visible : Visibility.Collapsed;
                
                // Handle different sections
                switch (section.ToLower())
                {
                    case "main":
                        // Only hide/show the main translator area
                        var mainContent = this.GetTemplateChild("MainContent") as FrameworkElement;
                        if (mainContent != null)
                            mainContent.Visibility = visibilityState;
                        break;
                        
                    case "sidebar":
                        // Only hide/show the sidebar
                        var sidebarContent = this.GetTemplateChild("SidebarContent") as FrameworkElement;
                        if (sidebarContent != null)
                            sidebarContent.Visibility = visibilityState;
                        break;
                        
                    case "all":
                    default:
                        // Apply to all content - find elements in the visual tree
                        var mainTranslatorArea = FindVisualChild<Border>(this, "MainTranslatorArea");
                        var sidebar = FindVisualChild<Border>(this, "SidebarArea");
                        
                        if (mainTranslatorArea != null)
                            mainTranslatorArea.Visibility = visibilityState;
                            
                        if (sidebar != null)
                            sidebar.Visibility = visibilityState;
                        break;
                }
            }
            catch (Exception ex)
            {
                // Log error or handle exception
                Console.WriteLine($"Error toggling UI visibility: {ex.Message}");
            }
        }

        /// <summary>
        /// Helper method to find a child of the specified type in the visual tree
        /// </summary>
        private T FindVisualChild<T>(DependencyObject parent, string name = null) where T : FrameworkElement
        {
            if (parent == null) return null;
            
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                
                // If the child is the type we're looking for and has the right name (if specified)
                if (child is T typedChild && (string.IsNullOrEmpty(name) || typedChild.Name == name))
                    return typedChild;
                    
                // Search children of this child
                T result = FindVisualChild<T>(child, name); 
                if (result != null)
                    return result;
            }
            
            return null;
        }

        /// <summary>
        /// Hides all UI elements
        /// </summary>
        public void HideUI()
        {
            ToggleUIVisibility("all", false);
        }

        /// <summary>
        /// Shows all UI elements
        /// </summary>
        public void ShowUI()
        {
            ToggleUIVisibility("all", true);
        }

        // Hide non-essential UI elements but keep essential ones visible
        public void HideNonEssentialUI()
        {
            try 
            {
                // We no longer need to reference MainTranslatorArea as it's been replaced
                // with a different layout in the updated Google-style UI
                
                // We can still hide other UI elements if needed
                if (CharacterCountTextBlock != null)
                    CharacterCountTextBlock.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in HideNonEssentialUI: {ex.Message}");
            }
        }
    }
    
    // Class to represent a translation item for history and favorites
    public class TranslationItem
    {
        public required string SourceText { get; set; }
        public required string TranslatedText { get; set; }
        public required string FromLanguage { get; set; }
        public required string ToLanguage { get; set; }
        public DateTime Timestamp { get; set; }
    }
} 