# PolyglotEssential

A modern language learning application designed to help users learn multiple languages through an intuitive interface and effective learning methods.

![PolyglotEssential Main Screen](docs/images/main-screen.png)

## What This Project Does

PolyglotEssential is a comprehensive language learning platform that helps users master new languages through a variety of interactive features:

- **Vocabulary Building**: Learn words and phrases through a flashcard system that uses spaced repetition for optimal retention
- **Language Practice**: Test your knowledge with interactive exercises including word matching, translation challenges, and comprehension tests
- **Progress Tracking**: Advanced analytics to monitor your learning journey and identify areas for improvement
- **Gamification**: Earn points, badges, and compete on leaderboards to stay motivated
- **Translation Tools**: Instantly translate words and phrases with contextual information and examples

The application currently focuses on English-Uzbek language pairs but is designed with a scalable architecture to support multiple languages in the future.

## Screenshots

> **Note**: The images below are placeholders. Replace them with actual screenshots of your application once available.

<div align="center">
  <img src="docs/images/login-screen.png" alt="Login Screen" width="400" />
  <p><em>User Authentication Screen</em></p>
  
  <img src="docs/images/learn-words.png" alt="Learn Words" width="800" />
  <p><em>Word Learning Interface</em></p>
  
  <img src="docs/images/translation.png" alt="Translation" width="800" />
  <p><em>Translation Tool</em></p>
  
  <img src="docs/images/leaderboard.png" alt="Leaderboard" width="600" />
  <p><em>Leaderboard and Gamification</em></p>
</div>

## Features

### User Authentication
- Secure login and registration system
- User profile management
- Progress persistence across sessions

### Word Learning
- Interactive flashcard system based on spaced repetition principles
- Pronunciation guides and audio support
- Context-based examples for better understanding

### Word Matching
- Multiple game modes to test vocabulary
- Difficulty levels from beginner to advanced
- Real-time feedback and scoring

### Translation Tools
- Bidirectional translation between supported languages
- Examples showing words used in context
- Save favorite translations for future reference

### Progress Tracking
- Detailed statistics on learning progress
- Visualization of strong and weak areas
- Custom learning recommendations

### Leaderboards
- Compete with friends and other learners
- Daily and weekly challenges
- Achievement badges and milestones

## Installation

### Prerequisites
- .NET 8.0 or higher
- Windows 8.0 or higher

### Steps
1. Clone the repository:
   ```
   git clone https://github.com/yourusername/PolyglotEssential.git
   ```

2. Navigate to the project directory:
   ```
   cd PolyglotEssential
   ```

3. Build the solution:
   ```
   dotnet build
   ```

4. Run the application:
   ```
   dotnet run --project PolyglotEssential/PolyglotEssential.Desktop.csproj
   ```

## Project Structure

The project follows a clean architecture approach with separation of concerns:

- **PolyglotEssential.Desktop**: WPF UI application with MVVM pattern
  - Pages for different learning activities
  - Custom controls and UI components
  - Animation and transition effects for enhanced user experience
  
- **PolyglotEssential.Service**: Business logic and services
  - Language learning algorithms
  - Authentication and user management
  - Data processing and analysis
  
- **PolyglotEssential.DataAccess**: Data access layer
  - Database interactions
  - External API integrations
  - Caching mechanisms
  
- **PolyglotEssential.Domain**: Domain models and entities
  - Language vocabulary models
  - User profile models
  - Learning progress models

## Usage

1. Register for a new account or login with existing credentials
2. Navigate through different learning modules:
   - Learn new words through interactive flashcards
   - Practice with word matching exercises
   - Use the translator for immediate translations
   - Check your progress and standing on leaderboards
3. Set daily learning goals and track your progress
4. Challenge friends and compete on leaderboards to stay motivated

## Technology Stack

- **Frontend**: WPF (Windows Presentation Foundation)
- **Backend**: .NET 8.0
- **UI Framework**: MaterialDesign
- **Architecture**: Clean Architecture pattern with MVVM
- **Database**: Entity Framework Core
- **Animations**: Custom WPF animations for smooth transitions

## Future Enhancements

- Support for additional languages
- Mobile application versions (iOS and Android)
- Advanced learning analytics with AI-powered recommendations
- Speech recognition and pronunciation assessment
- Community features for collaborative learning
- Offline mode for learning without internet connection

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Acknowledgments

- Thanks to all contributors who have helped build this application
- Special thanks to the language learning community for feedback and suggestions
- Inspired by modern language learning approaches and cognitive science research 