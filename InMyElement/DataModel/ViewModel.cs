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
    public class ViewModel : INotifyPropertyChanged
    {
        IDataRepository _data;

        public ViewModel(IDataRepository data)
        {
            _data = data; 
        }

        async public void Initialize()
        {
            Elements = await _data.LoadElement();
            RandomElements = await _data.LoadRandomElement();
            PuzzleScores = await _data.LoadPuzzleScore();
            QuizScores = await _data.LoadQuizScore();
            UserName = await Windows.System.UserProfile.UserInformation.GetDisplayNameAsync();
            Users = await _data.LoadUser();
            UserFavorites = await _data.LoadUserFavorite();
            CurrentUser = Users.Where(s => (s.UserName == this.UserName)).FirstOrDefault();
            GetFavorites();
            TrimAndFormatScores();
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

        private ObservableCollection<Element> randomElements;
        public ObservableCollection<Element> RandomElements {
            get { return randomElements; }
            set
            {
                randomElements = value;
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

        private ObservableCollection<QuizScore> quizScores;
        public ObservableCollection<QuizScore> QuizScores
        {
            get { return quizScores; }
            set
            {
                quizScores = value;
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

        private ObservableCollection<Element> myFavorites;
        public ObservableCollection<Element> MyFavorites
        {
            get { return myFavorites; }
            set
            {
                myFavorites = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<HighScore> puzzleHighScores;
        public ObservableCollection<HighScore> PuzzleHighScores
        {
            get { return puzzleHighScores; }
            set
            {
                puzzleHighScores = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<HighScore> quizHighScores;
        public ObservableCollection<HighScore> QuizHighScores
        {
            get { return quizHighScores; }
            set
            {
                quizHighScores = value;
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

        private void GetFavorites()
        {
            List<UserFavorite> favorites = UserFavorites.Where(s => (s.UserId == CurrentUser.Id && s.IsFavorite == true)).ToList();
            MyFavorites = new ObservableCollection<Element>();
            foreach (UserFavorite favorite in favorites)
            {
                Element element = Elements.Where(s => (s.AtomicNumber == favorite.AtomicNumber)).FirstOrDefault();
                if (MyFavorites.Contains(element) == false)
                {
                    GroupColorMatching.GetColor(element);
                    MyFavorites.Add(element);
                }
            }
            if (MyFavorites.Count==0)
            {
                MyFavorites.Add(new Element
                {
                    AtomicNumber= 0,
                    Symbol="",
                    Name = "No Favorite Elements Yet!",
                    GroupColor= GroupColorMatching.Default
                });
            }
        }

        private void TrimAndFormatScores()
        {
            PuzzleHighScores = new ObservableCollection<HighScore>();
            PuzzleScores.OrderBy(s => s.NumberCorrect);
            int maxIndex = 3;
            if (PuzzleScores.Count < maxIndex)
            {
                maxIndex = PuzzleScores.Count;
            }
            for (int i=0; i<maxIndex; i++)
            {
                User player = Users.Where(s => (s.Id == PuzzleScores[i].UserId)).FirstOrDefault();
                PuzzleHighScores.Add(new HighScore
                {
                    UserName= player.UserName,
                    Message= PuzzleScores[i].NumberCorrect + " in "+ PuzzleScores[i].TimeInSecs + " seconds"
                });
            }

            if (PuzzleScores.Count == 0)
            {
                PuzzleHighScores.Add(new HighScore
                {
                    Message = "No High Scores yet!",
                });
            }

            QuizHighScores = new ObservableCollection<HighScore>();
            QuizScores.OrderBy(s => (s.NumberCorrect/s.NumberOfQuestions));
            maxIndex = 3;
            if (QuizScores.Count < maxIndex)
            {
                maxIndex = QuizScores.Count;
            }
            for (int i=0; i<maxIndex; i++)
            {
                User player= Users.Where(s => (s.Id == QuizScores[i].UserId)).FirstOrDefault();
                QuizHighScores.Add(new HighScore
                {
                    UserName= player.UserName,
                    Message= QuizScores[i].NumberCorrect + " out of "+ QuizScores[i].NumberOfQuestions
                });
            }

            if (QuizHighScores.Count == 0)
            {
                QuizHighScores.Add(new HighScore
                {
                    Message = "No High Scores yet!",
                });
            }
        }
    }
}
