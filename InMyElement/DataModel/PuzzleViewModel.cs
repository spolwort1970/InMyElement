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
    public class PuzzleViewModel : INotifyPropertyChanged
    {
        IDataRepository _data;
        private int correctAnswers;




        public PuzzleViewModel(IDataRepository data)
        {
            _data = data;
            //Keeps track of Elements placed on board
            ElementOnBoard = new ObservableCollection<Element>();
            //Used for binding on XAML puzzle elemnt / validate
            PuzzleElements = new ObservableCollection<Element>();

            
        }

        public PuzzleViewModel()
        {

        }

        async public void Initialize()
        {
            //load tables from sqldb
            Elements = await _data.LoadElementByName();
          
            PuzzleElements = await _data.LoadElement();
            PuzzleScores = await _data.LoadPuzzleScore();

            UserName = await Windows.System.UserProfile.UserInformation.GetDisplayNameAsync();
            Users = await _data.LoadUser();
            CurrentUser = Users.Where(s => (s.UserName == this.UserName)).FirstOrDefault();
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

        private ObservableCollection<Element> puzzleElements;
        public ObservableCollection<Element> PuzzleElements
        {
            get { return puzzleElements; }
            set
            {
                puzzleElements = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<PuzzleScore> puzzleScores;
        public ObservableCollection<PuzzleScore> PuzzleScores
        {
            get { return puzzleScores; }
            set
            {
                puzzleScores = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<Element> elementOnBoard;
        public ObservableCollection<Element> ElementOnBoard
        {
            get { return elementOnBoard; }
            set
            {
                elementOnBoard = value;
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

        public void SwitchElement(Element element)
        {
            if (Elements.Contains(element))
            {
                Elements.Remove(element);
                ElementOnBoard.Add(element);



            }


        }
        public void SwitchElement(string symbol)
        {
            foreach (Element element in ElementOnBoard.ToList())
            {
                if (symbol.Equals(element.Symbol))
                {
                    Elements.Add(element);
                    ElementOnBoard.Remove(element);
                }
            }


        }
        public void CorrectAnswers()
        {
            correctAnswers++;
            RaisePropertyChanged();
        }
        public int GetCorrectAnswers()
        {
            RaisePropertyChanged();
            return correctAnswers;
        }

        async public void ResetElements()
        {
            ElementOnBoard.Clear();
            correctAnswers = 0;
            Elements = await _data.LoadElementByName();
        }

        internal void AddPuzzleScore(PuzzleScore puzzleScore)
        {
            _data.Add(puzzleScore);
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
}






    
}
