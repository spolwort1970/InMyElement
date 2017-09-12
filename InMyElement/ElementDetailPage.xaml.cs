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
using System.Threading.Tasks;

// The Item Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234232

namespace InMyElement
{
    /// <summary>
    /// A page that displays details for a single item within a group while allowing gestures to
    /// flip through other items belonging to the same group.
    /// </summary>
    public sealed partial class ElementDetailPage : Page
    {
        private NavigationHelper navigationHelper;
        private IDataRepository data = new SqliteRepository();
        private StudyViewModel _vm;

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public ElementDetailPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;

            _vm = new StudyViewModel(data);
            _vm.Initialize();


            DataContext = _vm;
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        async private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            int selected = 0;
            if (e.NavigationParameter != null && e.NavigationParameter is int)
            {
                selected = (int)e.NavigationParameter - 1;
            }

            if (_vm.Elements == null || _vm.Elements.Count < 118)
            {
                await Task.Delay(500);
            }

            if (_vm.UserFavorites == null || _vm.UserNotes == null)
            {
                await Task.Delay(500);
            }

            ElementViewer.SelectedIndex = selected;

            if (selected == 0)
            {
                LoadNote();
                LoadFavorite();
            }
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


        private void Favorite_Click(object sender, RoutedEventArgs e)
        {
            int index = ElementViewer.SelectedIndex + 1;
            UserFavorite favorite = _vm.GetFavorite(index);
            if (favorite != null)
            {
                if (favorite.IsFavorite)
                {
                    favorite.IsFavorite = false;
                }
                else
                {
                    favorite.IsFavorite = true;
                }
                
            }
            else
            {
                User user = new User
                {
                    UserName = _vm.UserName,
                };
                _vm.AddUser(user);

                favorite = new UserFavorite
                {
                    AtomicNumber = index,
                    UserId = _vm.CurrentUser.Id,
                    IsFavorite=true
                };

            }
            _vm.AddFavorite(favorite);
            LoadFavorite();
        }

        private void AddNote_Click(object sender, RoutedEventArgs e)
        {
            NotePopup.IsOpen = true;
        }

        private void ElementViewer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NotePopup.IsOpen)
            {
                if (e.RemovedItems.Count > 0)
                {
                    Element lastElement = e.RemovedItems[0] as Element;
                    SaveThisNote(lastElement.AtomicNumber);
                }
            }

            if (ElementViewer.SelectedIndex > -1)
            {
                LoadNote();
                LoadFavorite();
            }
        }

        private void CloseNote_Click(object sender, RoutedEventArgs e)
        {
            NotePopup.IsOpen = false;
        }

        private void SaveNote_Click(object sender, RoutedEventArgs e)
        {
            SaveThisNote(ElementViewer.SelectedIndex + 1);
            NotePopup.IsOpen = false;
        }

        private void SaveThisNote(int index)
        {
            UserNote note = _vm.GetNote(index);
            if (note != null)
            {
                note.Note = ElementNote.Text;
                note.ModifiedDate = DateTime.Today;
            }
            else
            {
                User user = new User
                {
                    UserName = _vm.UserName,
                };
                _vm.AddUser(user);

                note = new UserNote
                {
                    AtomicNumber = index,
                    UserId = _vm.CurrentUser.Id,
                    CreatedDate = DateTime.Today,
                    Note = ElementNote.Text,
                };

            }
            _vm.AddNote(note);
        }

        private void LoadNote()
        {
            UserNote note = _vm.GetNote(ElementViewer.SelectedIndex + 1);
            if (note != null)
            {
                ElementNote.Text = note.Note;
            }
            else
            {
                ElementNote.Text = "";
            }
        }

        private void LoadFavorite()
        {
            UserFavorite favorite = _vm.GetFavorite(ElementViewer.SelectedIndex + 1);
            if (favorite != null && favorite.IsFavorite==true)
            {
                Favorite.Icon = new SymbolIcon(Symbol.UnFavorite);
                Favorite.Label = "Unfavorite";
            }
            else
            {
                Favorite.Icon = new SymbolIcon(Symbol.Favorite);
                Favorite.Label = "Mark as Favorite";
            }
        }
    }
}
