using InMyElement.Common;
using InMyElement.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class QuizGeneratorPage : Page
    {

        private NavigationHelper navigationHelper;
        private QuizViewModel _vm;
        private IDataRepository data = new SqliteRepository();

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public QuizGeneratorPage()
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

        private void numOfQuestionsSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            numOfQTextBox.Text = numOfQuestionsSlider.Value.ToString();
        }

        private void EGCheckBox1_Checked(object sender, RoutedEventArgs e)
        {
            foreach (UIElement child in ElementGroups.Children)
            {
                if (child is CheckBox && child != EGCheckBox1)
                {
                    CheckBox box = child as CheckBox;
                    box.IsChecked = true;
                }
            }
        }

        private void EGCheckBox1_Unchecked(object sender, RoutedEventArgs e)
        {
            
        }

        private void numOfQTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int value = 0;
            try
            {
                value = Convert.ToInt32(numOfQTextBox.Text);
                numOfQuestionsSlider.Value = value;
            }
            catch
            { }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string comments = "{0}-\n{1} Difficulty, Max {2} Questions\nCreated by {3}";
            comments = String.Format(comments, 
                CommentTextBox.Text, 
                ((QuizDifficulty)DifficultyLevel.SelectedIndex).ToString(), 
                Convert.ToInt32(numOfQuestionsSlider.Value), 
                _vm.UserName);

            User user = new User
                {
                    UserName = _vm.UserName,
                };
                _vm.AddUser(user);

                SavedQuiz quiz = new SavedQuiz
                {
                    UserId = _vm.CurrentUser.Id,
                    Name = QNameTextBox.Text,
                    Comments = comments,
                    NumberOfQuestions = Convert.ToInt32(numOfQuestionsSlider.Value),
                    Time = GetTime(),
                    Difficulty = DifficultyLevel.SelectedIndex,
                    IsActin = EGCheckBox2.IsChecked.Value,
                    IsAlkali = EGCheckBox3.IsChecked.Value,
                    IsAlkaliEarth = EGCheckBox4.IsChecked.Value,
                    IsHalogen = EGCheckBox5.IsChecked.Value,
                    IsLanth = EGCheckBox6.IsChecked.Value,
                    IsMetalloid = EGCheckBox7.IsChecked.Value,
                    IsNoble = EGCheckBox8.IsChecked.Value,
                    IsNonMetal = EGCheckBox9.IsChecked.Value,
                    IsPostTransMetal = EGCheckBox10.IsChecked.Value,
                    IsTransMetal = EGCheckBox11.IsChecked.Value,
                };

                _vm.AddSavedQuiz(quiz);

                if (quizToStart.IsChecked.Value)
                {
                    this.Frame.Navigate(typeof(QuizPage), quiz);                    
                }
        }

        private int GetTime()
        {
            string[] time = TimerTextBox.Text.Split(new char[] {':'});
            int totalTime;
            try
            {
                if (time.Length == 2)
                {
                    totalTime = ((60 * Convert.ToInt32(time[0])) + Convert.ToInt32(time[1]));
                }
                else
                {
                    totalTime = Convert.ToInt32(time[0]);
                }
            }
            catch
            {
                totalTime = 0;
            }

            return totalTime;
        }
    }
}
