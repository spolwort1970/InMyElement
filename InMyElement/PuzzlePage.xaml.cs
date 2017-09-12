using InMyElement.Common;
using InMyElement.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace InMyElement
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PuzzlePage : Page
    {
        private NavigationHelper navigationHelper;
        private IDataRepository data = new SqliteRepository();
        private PuzzleViewModel _vm;
        private TextBlock destinationElement;
        private Element sendingElement;
        private DispatcherTimer dispatcherTimer;
        int timesTicked;
        private bool gameInProgress;
     
       
        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public PuzzlePage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;

            _vm = new PuzzleViewModel(data);
            _vm.Initialize();
            


            DataContext = _vm;
        

            destinationElement = new TextBlock();
            sendingElement = new Element();

           
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
        public void DispatcherTimerSetup()
        {
            timesTicked = 1;
          
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
            
        }
        void dispatcherTimer_Tick(object sender, object e)
        {
            TimerLog.Text = timesTicked.ToString();       
            timesTicked++;
        }
       

       
        private void xElements_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            var item = e.Items.FirstOrDefault();
            if (item == null)
                return;

            e.Data.Properties.Add("item", item);
            e.Data.Properties.Add("gridSource", sender);
        }

        private void xElements_Drop(object sender, DragEventArgs e)
        {
            PuzzleElement PuzzleElementControl = new PuzzleElement();
            PuzzleElementControl = (PuzzleElement)sender;
            //checks to see if the destination box has been set
            if (destinationElement.Text.Equals(null))
            {
                return;
            }
            //gets sender gridsource
            object gridSource;
            e.Data.Properties.TryGetValue("gridSource", out gridSource);

            //verify if sender grid is same as destination
            if (gridSource == sender)
                return;

            object sourceItem;
            //pulls sent object checks for null
            e.Data.Properties.TryGetValue("item", out sourceItem);
            if (sourceItem == null)
                return;
            //set sent object to Element
            sendingElement = (Element)sourceItem;
            //Perform switch in ViewModel, removes element from list and adds to an onboard list
            _vm.SwitchElement(sendingElement);

            //rotate element 360 degrees
            FlipElement360(PuzzleElementControl);
            //change element text to droppped element
            destinationElement.Text = sendingElement.Symbol;
            //disables drop on already dropped elemnt
            destinationElement.AllowDrop = false;
            PuzzleElementControl.AllowDrop = false;
            PuzzleElementControl.ElementBackground = PuzzleElementControl.BorderStroke;
            PuzzleElementControl.ElementTextColor = new SolidColorBrush(Colors.Black);
            
        }

        private static void FlipElement360(UserControl PuzzleElementControl)
        {
            PlaneProjection plane = new PlaneProjection();
            PuzzleElementControl.Projection = plane;
            //sets text of the box to the elemnt passed in
            Storyboard board = new Storyboard();
            var timeline = new DoubleAnimationUsingKeyFrames();
            Storyboard.SetTarget(timeline, PuzzleElementControl.Projection);
            Storyboard.SetTargetProperty(timeline, "RotationY");
            var frame = new EasingDoubleKeyFrame() { KeyTime = TimeSpan.FromSeconds(1), Value = 360, EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseOut } };
            timeline.KeyFrames.Add(frame);
            board.Children.Add(timeline);
            board.Begin();
        }




        private void ElementDragLeave(object sender, DragEventArgs e)
        {
            destinationElement = new TextBlock();
        }

        private void ElementDragOver(object sender, DragEventArgs e)
        {
            destinationElement = (TextBlock)e.OriginalSource;
        }

        private void RemoveElement(object sender, DoubleTappedRoutedEventArgs e)
        {
            PuzzleElement PuzzleElementControl = new PuzzleElement();
            PuzzleElementControl = (PuzzleElement)sender;

            TextBlock textBlock = new TextBlock();
            textBlock = (TextBlock)e.OriginalSource;

            //if textBlock Contains INT do nothing
            if (Regex.IsMatch(textBlock.Text, "[0-9]"))
            {
                return;
            }
            else
            {
                string symbol = textBlock.Text;
                _vm.SwitchElement(symbol);
                textBlock.Text = DefaultTextName(textBlock.Name);
               
                FlipElement360(PuzzleElementControl);
                textBlock.Foreground = new SolidColorBrush(Colors.White);
                PuzzleElementControl.ElementBackground = new SolidColorBrush(Colors.Black);

                textBlock.AllowDrop = true;
                PuzzleElementControl.AllowDrop = true;
            }
        }

        private string DefaultTextName(string defaultName)
        {
            return defaultName.Trim(new Char[] { '_' });
        }

        public void ValidateElements()
        {
           
            List<UserControl> PuzzleElementControls = new List<UserControl>();
            PuzzleElementControls = GetPuzzleElementControls();

            foreach (PuzzleElement control in PuzzleElementControls)
            {
                if (DefaultTextName(control.Name).Equals(control.Text))
                {
                    control.BorderStroke = new SolidColorBrush(Colors.Green);
                    control.BorderStrokeThickness = 5;
                    _vm.CorrectAnswers();
                }
                else
                {
                    control.BorderStroke = new SolidColorBrush(Colors.DarkRed);
                    control.BorderStrokeThickness = 5;
                }
            }
        }

        public void ResetElementsGUI()
        {

            List<UserControl> PuzzleElementControls = new List<UserControl>();
            PuzzleElementControls = GetPuzzleElementControls();

            foreach (PuzzleElement control in PuzzleElementControls)
            {
                control.Text = DefaultTextName(control.ElementName.ToString());
                //added this line to test it out- CZ
                control.ResetToDefault();
                //control.BorderStroke = null;
                //    control.BorderStrokeThickness = 0;
                    control.AllowDrop = true;
                 
             
            }
        }

        public List<UserControl> GetPuzzleElementControls()
        {
            List<UserControl> PuzzleElementControls = new List<UserControl>();

            foreach (UIElement child in ElementsGrid.Children)
            {
                if (child is PuzzleElement)
                {
                    PuzzleElement element = child as PuzzleElement;
                    PuzzleElementControls.Add(element);
                    
                }
            }

            return PuzzleElementControls;
        }

        private void Validate_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Stop();//stop timer
            ValidateElements();
            //ends game
            Validate.IsEnabled = false;
            gameInProgress = false;
            xElementGrid.CanDragItems = false;
            xUserName.Text = _vm.UserName;
            xPuzzleTime.Text = TimerLog.Text + " Seconds";
            int correct = _vm.GetCorrectAnswers();
            CorrectAmount.Text = correct.ToString() + " Element(s) Correct";

            /**
            User user = new User
            {
                UserName = "TestName",
                PuzzleFastTime = TimerLog.Text,
                PuzzleNumCorrect = _vm.GetCorrectAnswers()

            };
            _vm.AddUser(user);
             */
            SaveScore(correct, Convert.ToInt32(TimerLog.Text));
            GameOverPopup.IsOpen = true;

        }


        private void SaveScore(int numberCorrect, int time)
        {
            User user = new User
            {
                UserName = _vm.UserName,
            };
            _vm.AddUser(user);

            PuzzleScore score = new PuzzleScore
            {
                UserId = _vm.CurrentUser.Id,
                TimeInSecs= time,
                NumberCorrect = numberCorrect
            };
            _vm.AddPuzzleScore(score);
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            //ToDO add code to ask user that they want to terminate current game session

            NewGamePopup.IsOpen = false;
            ResetElementsGUI();
            _vm.ResetElements();

            //set game in progress
            Validate.IsEnabled = true;
            gameInProgress = true;
            //close app bar
            BottomAppBar.IsOpen = false;
            //make elements draggable
            xElementGrid.CanDragItems = true;
            //start timer
            dispatcherTimer = new DispatcherTimer();
            DispatcherTimerSetup();
        }

        private void Instructions_Click(object sender, RoutedEventArgs e)
        {
            //close app bar
            BottomAppBar.IsOpen = false;
            //Open instructions popup
            InstructionsPopup.IsOpen = true;

        }

        private void CloseInstructions_Click(object sender, RoutedEventArgs e)
        {
            //open app bar
            BottomAppBar.IsOpen = true;
            //Open instructions popup
            InstructionsPopup.IsOpen = false;

        }

        private void CloseGamePopup_Click(object sender, RoutedEventArgs e)
        {
            //open app bar
            BottomAppBar.IsOpen = true;
            GameOverPopup.IsOpen = false;
            

        }

     
    }
}
