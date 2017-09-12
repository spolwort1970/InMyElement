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


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace InMyElement
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class QuizSelectionPage : Page
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


        public QuizSelectionPage()
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

        private void CreateNew_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(QuizGeneratorPage));
        }

        private void LoadSaved_Click(object sender, RoutedEventArgs e)
        {
            //show popup or something
            SavedQuizPopup.IsOpen = true;
        }

        private void VeryHard_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(QuizPage), QuizDifficulty.VeryHard);
        }

        private void Hard_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(QuizPage), QuizDifficulty.Hard);
        }

        private void Medium_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(QuizPage), QuizDifficulty.Medium);
        }

        private void Easy_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(QuizPage), QuizDifficulty.Easy);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            SavedQuizPopup.IsOpen = false;
        }

        private void QuizList_ItemClick(object sender, ItemClickEventArgs e)
        {
            //navigate to quiz
            this.Frame.Navigate(typeof(QuizPage), e.ClickedItem as SavedQuiz);
        }
    }
}
