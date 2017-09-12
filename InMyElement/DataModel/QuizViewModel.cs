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
    public class QuizViewModel : INotifyPropertyChanged
    {
          IDataRepository _data;

        public QuizViewModel(IDataRepository data)
        {
            _data = data;
            //Used for binding on XAML puzzle elemnt / validate
            QuizQuestions = new ObservableCollection<QuizQuestion>();
          
           
        }

        async public void Initialize()
        {
            //load tables from sqldb
            //QuizQuestions = await _data.LoadQuizQuestion();
            Users = await _data.LoadUser();
            UserName = await Windows.System.UserProfile.UserInformation.GetDisplayNameAsync();
            Users = await _data.LoadUser();
            CurrentUser = Users.Where(s => (s.UserName == this.UserName)).FirstOrDefault();
            SavedQuizes = await _data.LoadSavedQuiz();
            QuizScore = await _data.LoadQuizScore();
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

        private ObservableCollection<QuizQuestion> quizQuestions;
        public ObservableCollection<QuizQuestion> QuizQuestions
        {
            get { return quizQuestions; }
            set
            {
                quizQuestions = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<SavedQuiz> savedQuizes;
        public ObservableCollection<SavedQuiz> SavedQuizes
        {
            get { return savedQuizes; }
            set
            {
                savedQuizes = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<QuizScore> quizScore;
        public ObservableCollection<QuizScore> QuizScore
        {
            get { return quizScore; }
            set
            {
                quizScore = value;
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

        async public void SetDifficulty(QuizDifficulty level)
        {
           QuizQuestions= await _data.LoadQuizQuestion(level);
        }

        async public void SetQuizFromSaved(SavedQuiz quiz)
        {
            QuizQuestions = await _data.LoadQuizQuestion(quiz);
        }

        internal void AddUser(User user)
        {
            if (Users.Where(s => (s.UserName == user.UserName)).FirstOrDefault() == null)
            {
                _data.Add(user);
                CurrentUser = Users.Where(s => (s.UserName == this.UserName)).FirstOrDefault();
                if (CurrentUser.Id==0)
                {
                    CurrentUser.Id++;
                }
            }
        }

        internal void AddSavedQuiz(SavedQuiz quiz)
        {
            _data.Add(quiz);
        }

        internal void AddScore(QuizScore score)
        {
            _data.Add(score);
        }
        
    }
}
