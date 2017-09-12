using InMyElement.Common;
using InMyElement.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace InMyElement
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class QuizPage : Page
    {

        private NavigationHelper navigationHelper;
        private QuizViewModel _vm;
        private IDataRepository data = new SqliteRepository();
        private List<string> SelectedAnswers = new List<string>();
        private DispatcherTimer dispatcherTimer;
        int timesTicked;
        int timeLimit = 0;
        int selectedIndex = 0;

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public QuizPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;

            _vm = new QuizViewModel(data);
            _vm.Initialize();
            DataContext = _vm;

            
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            QuizDifficulty selected = QuizDifficulty.Medium;
            if (e.NavigationParameter != null && e.NavigationParameter is QuizDifficulty)
            {
                selected = (QuizDifficulty)e.NavigationParameter;
            }
            else if (e.NavigationParameter != null && e.NavigationParameter is SavedQuiz)
            {
                SavedQuiz quiz= (SavedQuiz)e.NavigationParameter;
                _vm.SetQuizFromSaved(quiz);
                LoadFirstQuestion();

                if (quiz.Time != null && quiz.Time > 0)
                {
                    timeLimit = quiz.Time;
                    SetupTimer();
                }

                return;
            }

            _vm.SetDifficulty(selected);
            LoadFirstQuestion();            
        }

        async private void LoadFirstQuestion()
        {
            selectedIndex = 0;

            if (_vm.QuizQuestions == null || _vm.QuizQuestions.Count < 1)
            {
                await Task.Delay(500);
            }
            SetQuestion();
        }

        private void SetupTimer()
        {
            TimerLabel.Visibility = Windows.UI.Xaml.Visibility.Visible;
            TimerValue.Visibility = Windows.UI.Xaml.Visibility.Visible;
            dispatcherTimer = new DispatcherTimer();
            timesTicked = 0;

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        void dispatcherTimer_Tick(object sender, object e)
        {
            if (timesTicked<timeLimit)
            {
                TimerValue.Text = (timeLimit- timesTicked).ToString();
                timesTicked++;
            }
            else
            {
                GradeQuiz();
            }
            
        }
       

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void SetQuestion()
        {
            if(selectedIndex==0)
            {
                PreviousQuestion.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                NextQuestion.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            else if (selectedIndex== _vm.QuizQuestions.Count-1)
            {
                PreviousQuestion.Visibility = Windows.UI.Xaml.Visibility.Visible;
                NextQuestion.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
            else
            {
                PreviousQuestion.Visibility = Windows.UI.Xaml.Visibility.Visible;
                NextQuestion.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }

            QuestionNumber.Text = "Question " + (selectedIndex + 1);
            TotalNumber.Text = "of " + _vm.QuizQuestions.Count;

            QuizQuestion question= _vm.QuizQuestions[selectedIndex];
            Question.Text = question.Question;
            AnswerA.Content = question.AnswerA;
            AnswerB.Content = question.AnswerB;
            AnswerC.Content = question.AnswerC;
            AnswerD.Content = question.AnswerD;

            if(SelectedAnswers.Count>selectedIndex)
            {
                switch(SelectedAnswers[selectedIndex])
                {
                    case "A":
                        AnswerA.IsChecked = true;
                        break;
                    case "B":
                        AnswerB.IsChecked = true;
                        break;
                    case "C":
                        AnswerC.IsChecked = true;
                        break;
                    case "D":
                        AnswerD.IsChecked = true;
                        break;
                    default:
                        AnswerA.IsChecked = true;
                        AnswerA.IsChecked = false;
                        break;
                }
            }
        }

        private void A_Click(object sender, RoutedEventArgs e)
        {
            SetAnswer("A");
            if (selectedIndex < _vm.QuizQuestions.Count - 1)
            {
                selectedIndex++;
                SetQuestion();
            }
        }

        private void B_Click(object sender, RoutedEventArgs e)
        {
            SetAnswer("B");
            if (selectedIndex < _vm.QuizQuestions.Count - 1)
            {
                selectedIndex++;
                SetQuestion();
            }
        }

        private void C_Click(object sender, RoutedEventArgs e)
        {
            SetAnswer("C");
            if (selectedIndex < _vm.QuizQuestions.Count - 1)
            {
                selectedIndex++;
                SetQuestion();
            }
        }

        private void D_Click(object sender, RoutedEventArgs e)
        {
            SetAnswer("D");
            if (selectedIndex < _vm.QuizQuestions.Count - 1)
           {
                selectedIndex++;
            SetQuestion();
            }
        }

        private void SetAnswer(string answer)
        {
            while (SelectedAnswers.Count < _vm.QuizQuestions.Count)
            {
                SelectedAnswers.Add("");
            }
            SelectedAnswers[selectedIndex] = answer;
        }

        private void TakeAgain_Click(object sender, RoutedEventArgs e)
        {
            ScorePopup.IsOpen = false;
            SelectedAnswers.Clear();
            selectedIndex = 0;
            SetQuestion();
            if (timeLimit>0)
            {
                SetupTimer();
            }
        }

        private int CheckAnswers()
        {
            int score = 0;
            for (int i = 0; i < _vm.QuizQuestions.Count; i++)
            {
                string correct = _vm.QuizQuestions[i].CorrectAnswer.ToUpper();
                if (SelectedAnswers.Count > i && SelectedAnswers[i].ToUpper() == correct)
                {
                    score++;
                }
            }
            return score;
        }

        private void Grade_Click(object sender, RoutedEventArgs e)
        {
            GradeQuiz();
        }

        private void GradeQuiz()
        {
            if (dispatcherTimer != null)
            {
                dispatcherTimer.Stop();
            }
            UserName.Text = _vm.UserName;
            
            int correct= CheckAnswers();
            Score.Text = correct.ToString();
            SaveScore(correct);
            ScorePopup.IsOpen = true;
        }

        private void SaveScore(int numberCorrect)
        {
            User user = new User
            {
                UserName = _vm.UserName,
            };
            _vm.AddUser(user);

            QuizScore score = new QuizScore
            {
                UserId = _vm.CurrentUser.Id,
                NumberOfQuestions=_vm.QuizQuestions.Count,
                NumberCorrect=numberCorrect
            };
            _vm.AddScore(score);
        }

        private void ClosePopup_Click(object sender, RoutedEventArgs e)
        {
            ScorePopup.IsOpen = false;
        }

        private void NextQuestion_Click(object sender, RoutedEventArgs e)
        {
            selectedIndex++;
            SetQuestion();
        }

        private void PreviousQuestion_Click(object sender, RoutedEventArgs e)
        {
            selectedIndex--;
            SetQuestion();
        }
    }
}
