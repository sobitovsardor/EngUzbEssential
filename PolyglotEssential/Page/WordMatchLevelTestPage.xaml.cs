using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace PolyglotEssential.Page
{
    /// <summary>
    /// Interaction logic for WordMatchLevelTestPage.xaml
    /// </summary>
    public partial class WordMatchLevelTestPage : System.Windows.Controls.Page
    {
        private DispatcherTimer timer;
        private int totalSeconds = 5;
        private int remainingSeconds;
        private int currentQuestionIndex = 0;

        // Demo data structure for questions/answers
        private class Question
        {
            public string Word { get; set; }
            public string[] Answers { get; set; } // 4 answers
            public int CorrectIndex { get; set; }
        }
        private List<Question> questions = new List<Question>
        {
            new Question { Word = "angry", Answers = new[]{ "tag, pastgi qism", "yetib kelmoq, kelmoq", "qo'rqan, cho'chigan", "jahli chiqqan, badjahl" }, CorrectIndex = 3 },
            new Question { Word = "happy", Answers = new[]{ "baxtli", "g'amgin", "yaxshi", "yomon" }, CorrectIndex = 0 },
            new Question { Word = "fast", Answers = new[]{ "tez", "sekin", "katta", "kichik" }, CorrectIndex = 0 },
            // Add more questions as needed
        };

        public WordMatchLevelTestPage()
        {
            InitializeComponent();
            ShowQuestion(0);
            StartTimer();
        }

        private void StartTimer()
        {
            remainingSeconds = totalSeconds;
            UpdateTimerUI();

            if (timer == null)
            {
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += Timer_Tick;
            }
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (remainingSeconds > 0)
            {
                remainingSeconds--;
                UpdateTimerUI();
            }
            else
            {
                timer.Stop();
                // Optionally handle timer end (e.g., auto-submit, show message)
                NavigationService?.Navigate(new PolyglotEssential.Desktop.Page.WordMatchLevelTestResultPage());
            }
        }

        private void UpdateTimerUI()
        {
            TimerText.Text = remainingSeconds.ToString();
            double percent = (double)remainingSeconds / totalSeconds;
            TimerArc.Data = CreateArcGeometry(percent);
        }

        private void ShowQuestion(int index)
        {
            if (index < 0 || index >= questions.Count) return;
            var q = questions[index];
            WordText.Text = q.Word;
            QuestionNumberText.Text = $"Question: {index + 1}/{questions.Count}";
            AnswerButtonA.Content = "A)   " + q.Answers[0];
            AnswerButtonB.Content = "B)   " + q.Answers[1];
            AnswerButtonC.Content = "C)   " + q.Answers[2];
            AnswerButtonD.Content = "D)   " + q.Answers[3];
            // Do not reset timer here; just ensure it's running
            if (timer != null && !timer.IsEnabled)
                timer.Start();
        }

        private void AnswerButton_Click(object sender, RoutedEventArgs e)
        {
            // Stop timer immediately so it doesn't update during transition

            // Optionally: check if answer is correct, give feedback, etc.
            currentQuestionIndex++;
            if (currentQuestionIndex < questions.Count)
            {
                ShowQuestion(currentQuestionIndex);
            }
            else
            {
                // Navigate to result page
                NavigationService?.Navigate(new PolyglotEssential.Desktop.Page.WordMatchLevelTestResultPage());
            }
        }

        // Creates a PathGeometry for a circular arc based on percent (0.0 to 1.0)
        private Geometry CreateArcGeometry(double percent)
        {
            double angle = 360 * percent;
            double radius = 30; // half of ellipse width/height
            double centerX = 30;
            double centerY = 30;
            double startAngle = -90; // start at top
            double endAngle = startAngle + angle;

            Point startPoint = PointOnCircle(centerX, centerY, radius, startAngle);
            Point endPoint = PointOnCircle(centerX, centerY, radius, endAngle);
            bool isLargeArc = angle > 180;

            var arc = new PathFigure
            {
                StartPoint = startPoint,
                IsClosed = false
            };
            arc.Segments.Add(new ArcSegment
            {
                Point = endPoint,
                Size = new Size(radius, radius),
                IsLargeArc = isLargeArc,
                SweepDirection = SweepDirection.Clockwise
            });
            return new PathGeometry(new[] { arc });
        }

        private Point PointOnCircle(double cx, double cy, double r, double angleDegrees)
        {
            double angleRad = Math.PI * angleDegrees / 180.0;
            return new Point(
                cx + r * Math.Cos(angleRad),
                cy + r * Math.Sin(angleRad)
            );
        }
    }
}
