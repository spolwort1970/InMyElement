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
    public sealed partial class MainPage : Page
    {
        private NavigationHelper navigationHelper;
        private IDataRepository data = new SqliteRepository();
        private ViewModel _vm;
        private List<string> learnMode;
        private List<string> gameMode;
        private List<string> quizMode;

        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public MainPage()
        {
            this.InitializeComponent();
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
            _vm = new ViewModel(data);
            _vm.Initialize();
            //cache the main page so that the random element doesn't change

            //prevents vm getting favorites updated
            //this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            
            DataContext = _vm;
           
            InitializeData();
            
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

        private void InitializeData()
        {
            learnMode = new List<string>();
            learnMode.Add("/Assets/Mode/periodic_table.png");
           
            
          
            gameMode = new List<string>();
            gameMode.Add("/Assets/Mode/puzzle.png");
        
            
          
            quizMode = new List<string>();
            quizMode.Add("/Assets/Mode/quiz.png");

            learnModeCollectionViewSource.Source = learnMode;
            quizModeCollectionViewSource.Source = quizMode;
            gameModeCollectionViewSource.Source = gameMode;
        }

        private void LearnGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem.Equals("/Assets/Mode/periodic_table.png"))
            {
                this.Frame.Navigate(typeof(StudyPage));
            }
        }

        private void GameGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem.Equals("/Assets/Mode/puzzle.png"))
            {
                this.Frame.Navigate(typeof(PuzzlePage));
            }
        }

        private void QuizGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem.Equals("/Assets/Mode/quiz.png"))
            {
                this.Frame.Navigate(typeof(QuizSelectionPage));
            }

        }

        private void FavList_ItemClick(object sender, ItemClickEventArgs e)
        {
            Element element = e.ClickedItem as Element;
            this.Frame.Navigate(typeof(ElementDetailPage), element.AtomicNumber);
        }
    }
}
