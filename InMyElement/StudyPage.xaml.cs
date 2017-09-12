using InMyElement.Common;
using InMyElement.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class StudyPage : Page
    {

        private NavigationHelper navigationHelper;
        private IDataRepository data = new SqliteRepository();
        private StudyViewModel _vm;
        private Button selectedElement;

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public StudyPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;

            _vm = new StudyViewModel(data);
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

        private void ElementButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedElement!=null)
            {
                DeselectElement(selectedElement);
            }
            selectedElement = sender as Button;
            SelectElement(selectedElement);

            StudyElement element = selectedElement.Content as StudyElement;
            DisplayInfo(Convert.ToInt32(element.Number));
        }

        private void DisplayInfo(int elementNumber)
        {
            this.Frame.Navigate(typeof(ElementDetailPage), elementNumber);
        }

        private void ElementButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Button elementButton = sender as Button;
            SelectElement(elementButton);
        }

        private void ElementButton_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Button elementButton = sender as Button;
            if (elementButton != selectedElement)
            {
                DeselectElement(elementButton);
            }
        }

        private void SelectElement(Button elementButton)
        {
            StudyElement element = elementButton.Content as StudyElement;
            element.ElementBackground = element.BorderStroke;
            element.ElementTextColor = new SolidColorBrush(Colors.Black);
        }

        private void DeselectElement(Button elementButton)
        {
            StudyElement element = elementButton.Content as StudyElement;
            element.ElementBackground = new SolidColorBrush(Colors.Black);
            element.ElementTextColor = new SolidColorBrush(Colors.White);
        }
    }
}
