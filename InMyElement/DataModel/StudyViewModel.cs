/*******************************************************************************\
Class: ViewModel
Created by: Shaun Cobb
Date: 3/25/14
 * *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace InMyElement.DataModel
{
    public class StudyViewModel : INotifyPropertyChanged
    {
        IDataRepository _data;

        public StudyViewModel(IDataRepository data)
        {
            _data = data;
        }

        async public void Initialize()
        {
            Elements = await _data.LoadElement();
            GetColorsAndFormat();
            UserName = await Windows.System.UserProfile.UserInformation.GetDisplayNameAsync();
            Users = await _data.LoadUser();
            UserNotes = await _data.LoadUserNote();
            UserFavorites = await _data.LoadUserFavorite();
            CurrentUser = Users.Where(s => (s.UserName == this.UserName)).FirstOrDefault();
        }

        private ObservableCollection<Element> elements;
        public ObservableCollection<Element> Elements
        {
            get { return elements; }
            set
            {
                elements = value;
                RaisePropertyChanged();
            }
        }

        private string userName;
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                RaisePropertyChanged();
            }
        }

        private User currentUser;
        public User CurrentUser
        {
            get { return currentUser; }
            set
            {
                currentUser = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<User> users;
        public ObservableCollection<User> Users
        {
            get { return users; }
            set
            {
                users = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<UserNote> userNotes;
        public ObservableCollection<UserNote> UserNotes
        {
            get { return userNotes; }
            set
            {
                userNotes = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<UserFavorite> userFavorites;
        public ObservableCollection<UserFavorite> UserFavorites
        {
            get { return userFavorites; }
            set
            {
                userFavorites = value;
                RaisePropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(
            [CallerMemberName] string caller = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
            
        }

        internal void AddUser(User user)
        {
            if (Users.Where(s => (s.UserName == user.UserName)).FirstOrDefault() == null)
            {
                _data.Add(user);
                CurrentUser = Users.Where(s => (s.UserName == this.UserName)).FirstOrDefault();
                if (CurrentUser.Id == 0)
                {
                    CurrentUser.Id++;
                }
            }
        }

        internal void AddNote(UserNote note)
        {
            if (UserNotes.Where(s => (s.UserId == note.UserId && s.AtomicNumber == note.AtomicNumber)).FirstOrDefault() == null)
            {
                _data.Add(note);
            }
            else
            {
                _data.Update(note);
            }
        }

        internal void AddFavorite(UserFavorite favorite)
        {
            if (UserFavorites.Where(s => (s.UserId == favorite.UserId && s.AtomicNumber == favorite.AtomicNumber)).FirstOrDefault() == null)
            {
                _data.Add(favorite);
            }
            else
            {
                _data.Update(favorite);
            }
        }

        public UserNote GetNote(int elementNumber)
        {
            if (CurrentUser!= null)
            {
                return UserNotes.Where(s => (s.UserId == CurrentUser.Id && s.AtomicNumber == elementNumber)).FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        public UserFavorite GetFavorite(int elementNumber)
        {
            if (CurrentUser != null)
            {
                return UserFavorites.Where(s => (s.UserId == CurrentUser.Id && s.AtomicNumber == elementNumber)).FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        private void GetColorsAndFormat()
        {
            foreach (Element element in Elements)
            {
                GroupColorMatching.GetColor(element);
                try
                {
                    double temp = Convert.ToDouble(element.AtomicWeight);
                    element.AtomicWeight = String.Format("{0:0.00}", temp);
                }
                catch { }
            }
        }
    }
}
