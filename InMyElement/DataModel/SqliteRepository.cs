/*******************************************************************************\
Class: SqliteDataRepository
Created by: Shaun Cobb
Date: 3/25/14
 * *****************************************************************************/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;

namespace InMyElement.DataModel
{
    public class SqliteRepository : IDataRepository
    {
        private static readonly string dbPath =
            Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "inmyelement.sqlite");

        ObservableCollection<Element> elements;
        ObservableCollection<ElementProperty> elementProperties;
        ObservableCollection<Property> properties;
        ObservableCollection<QuizLevel> quizLevels;
        ObservableCollection<QuizQuestion> quizQuestions;
        ObservableCollection<SavedQuiz> savedQuizzes;
        ObservableCollection<Series> series;
        ObservableCollection<User> users;
        ObservableCollection<UserFavorite> userFavorites;
        ObservableCollection<UserNote> userNotes;
        ObservableCollection<UserQuizQuestion> userQuizQuestions;
        ObservableCollection<Element> randomElements;
        ObservableCollection<PuzzleScore> puzzleScores;
        ObservableCollection<QuizScore> quizScores;

        private int randomElementNumber;
        private Random rnd;



        public SqliteRepository()
        {
            //Initialize();
            CopyDatabase();
            rnd = new Random(Convert.ToInt32(DateTime.Today.Date.ToString("ddMMyyyy")));
            //randomElementNumber = rnd.Next(0, 117);
            randomElementNumber = 21;
        }

        private void Initialize()
        {
            using ( var db = new SQLite.SQLiteConnection( dbPath) )
            {
                db.CreateTable<Element>();
                db.CreateTable<ElementProperty>();
                db.CreateTable<Property>();
                db.CreateTable<QuizLevel>();
                db.CreateTable<QuizQuestion>();
                db.CreateTable<SavedQuiz>();
                db.CreateTable<Series>();
                db.CreateTable<User>();
                db.CreateTable<UserFavorite>();
                db.CreateTable<UserNote>();
                db.CreateTable<UserQuizQuestion>();
                
                

                //Note: This is a simple init scenario
                //if (db.ExecuteScalar<int>("Select count(1) From Element") == 0)
                //{
                //    db.RunInTransaction(() =>
                //        {
                //            db.Insert(new Element()
                //            {
                //                AtomicNumber = 1,
                //                ElementSeries = 1,
                //                Description = "This is hyrogenBitch",
                //                Name = "Hydrogen"
                //            });
                //            db.Insert(new Element()
                //            {
                //                AtomicNumber = 2,
                //                ElementSeries = 2,
                //                Description = "This is Helium Bitch",
                //                Name = "Helium"
                //            });
                //        });
                //}
                //else
                //{
                //    LoadElement();

                //}
            }
        }

        public async Task<ObservableCollection<Element>> LoadElement()
        {
         
            var connection = new SQLite.SQLiteAsyncConnection(dbPath);

            elements = new ObservableCollection<Element>(await connection.QueryAsync<Element>("Select * FROM Element"));
            return elements;
        }
        public async Task<ObservableCollection<Element>> LoadElementByName()
        {

            var connection = new SQLite.SQLiteAsyncConnection(dbPath);

            elements = new ObservableCollection<Element>(await connection.QueryAsync<Element>("Select * FROM Element ORDER BY Name"));
            return elements;
        }
        public async Task<ObservableCollection<ElementProperty>> LoadElementProperty()
        {
          
            var connection = new SQLite.SQLiteAsyncConnection(dbPath);

            elementProperties= new ObservableCollection<ElementProperty>(await connection.QueryAsync<ElementProperty>("Select * FROM ElementProperty"));
            return elementProperties;
        }

        public async Task<ObservableCollection<Property>> LoadProperty()
        {

            var connection = new SQLite.SQLiteAsyncConnection(dbPath);

            properties = new ObservableCollection<Property>(await connection.QueryAsync<Property>("Select * FROM Property"));
            return properties;
        }

        public async Task<ObservableCollection<QuizLevel>> LoadQuizLevel()
        {

            var connection = new SQLite.SQLiteAsyncConnection(dbPath);

            quizLevels = new ObservableCollection<QuizLevel>(await connection.QueryAsync<QuizLevel>("Select * FROM QuizLevel"));
            return quizLevels;
        }
        public async Task<ObservableCollection<QuizQuestion>> LoadQuizQuestion()
        {

            var connection = new SQLite.SQLiteAsyncConnection(dbPath);

            quizQuestions = new ObservableCollection<QuizQuestion>(await connection.QueryAsync<QuizQuestion>("Select * FROM QuizQuestion"));
            return quizQuestions;
        }
        public async Task<ObservableCollection<QuizQuestion>> LoadQuizQuestion(QuizDifficulty difficulty)
        {
            string query="Select * FROM QuizQuestion WHERE Difficulty {0} ORDER BY RANDOM() LIMIT 30";
            switch (difficulty)
            {
                case QuizDifficulty.Easy:
                    query = String.Format(query, "= 0");
                    break;
                default:
                case QuizDifficulty.Medium:
                    query = String.Format(query, "in (0,1)");
                    break;
                case QuizDifficulty.Hard:
                    query = String.Format(query, "in (0,1,2)");
                    break;
                case QuizDifficulty.VeryHard:
                    query = String.Format(query, "in (0,1,2,3)");
                    break;
            }
            var connection = new SQLite.SQLiteAsyncConnection(dbPath);

            quizQuestions = new ObservableCollection<QuizQuestion>(await connection.QueryAsync<QuizQuestion>(query));
            return quizQuestions;
        }
        public async Task<ObservableCollection<QuizQuestion>> LoadQuizQuestion(SavedQuiz quiz)
        {
            string query = "Select * FROM QuizQuestion WHERE ElementCategory in ({0}) AND Difficulty {1} ORDER BY RANDOM() LIMIT {2}";            
            string categories = "";

            if (quiz.IsActin)
            {
                categories += Convert.ToInt32(ElementGroup.Actinoid);
            }
            if (quiz.IsAlkali)
            {
                if (categories!="")
                {
                    categories += ",";
                }
                categories += Convert.ToInt32(ElementGroup.AlkaliMetal);
            }
            if (quiz.IsAlkaliEarth)
            {
                if (categories != "")
                {
                    categories += ",";
                }
                categories += Convert.ToInt32(ElementGroup.AlkalineEarth);
            }
            if (quiz.IsHalogen)
            {
                if (categories != "")
                {
                    categories += ",";
                }
                categories += Convert.ToInt32(ElementGroup.Halogen);
            }
            if (quiz.IsLanth)
            {
                if (categories != "")
                {
                    categories += ",";
                }
                categories += Convert.ToInt32(ElementGroup.Lanthanoid);
            }
            if (quiz.IsMetalloid)
            {
                if (categories != "")
                {
                    categories += ",";
                }
                categories += Convert.ToInt32(ElementGroup.Metalloid);
            }
            if (quiz.IsNoble)
            {
                if (categories != "")
                {
                    categories += ",";
                }
                categories += Convert.ToInt32(ElementGroup.Noble);
            }
            if (quiz.IsNonMetal)
            {
                if (categories != "")
                {
                    categories += ",";
                }
                categories += Convert.ToInt32(ElementGroup.NonMetal);
            }
            if (quiz.IsPostTransMetal)
            {
                if (categories != "")
                {
                    categories += ",";
                }
                categories += Convert.ToInt32(ElementGroup.PostTransitionMetal);
            }
            if (quiz.IsTransMetal)
            {
                if (categories != "")
                {
                    categories += ",";
                }
                categories += Convert.ToInt32(ElementGroup.TransitionMetal);
            }

            string difficulty;
            switch (quiz.Difficulty)
            {
                case 0:
                    difficulty = "= 0";
                    break;
                default:
                case 1:
                    difficulty = "in (0,1)";
                    break;
                case 2:
                    difficulty = "in (0,1,2)";
                    break;
                case 3:
                    difficulty =  "in (0,1,2,3)";
                    break;
            }

            query = String.Format(query, categories, difficulty, quiz.NumberOfQuestions);

            var connection = new SQLite.SQLiteAsyncConnection(dbPath);

            quizQuestions = new ObservableCollection<QuizQuestion>(await connection.QueryAsync<QuizQuestion>(query));
            return quizQuestions;
        }
        public async Task<ObservableCollection<SavedQuiz>> LoadSavedQuiz()
        {

            var connection = new SQLite.SQLiteAsyncConnection(dbPath);

            savedQuizzes = new ObservableCollection<SavedQuiz>(await connection.QueryAsync<SavedQuiz>("Select * FROM SavedQuiz"));
            return savedQuizzes;
        }

        public async Task<ObservableCollection<Series>> LoadSeries()
        {
      
            var connection = new SQLite.SQLiteAsyncConnection(dbPath);

            series = new ObservableCollection<Series>(await connection.QueryAsync<Series>("Select * FROM Series"));
            return series;
        }
        public async Task<ObservableCollection<User>> LoadUser()
        {

            var connection = new SQLite.SQLiteAsyncConnection(dbPath);

            users = new ObservableCollection<User>(await connection.QueryAsync<User>("Select * FROM User"));
            return users;
        }
        public async Task<ObservableCollection<UserFavorite>> LoadUserFavorite()
        {

            var connection = new SQLite.SQLiteAsyncConnection(dbPath);

            userFavorites = new ObservableCollection<UserFavorite>(await connection.QueryAsync<UserFavorite>("Select * FROM UserFavorite"));
            return userFavorites;
        }
        public async Task<ObservableCollection<UserNote>> LoadUserNote()
        {

            var connection = new SQLite.SQLiteAsyncConnection(dbPath);

            userNotes = new ObservableCollection<UserNote>(await connection.QueryAsync<UserNote>("Select * FROM UserNote"));
            return userNotes;
        }
        public async Task<ObservableCollection<UserQuizQuestion>> LoadUserQuizQuestion()
        {

            var connection = new SQLite.SQLiteAsyncConnection(dbPath);

            userQuizQuestions = new ObservableCollection<UserQuizQuestion>(await connection.QueryAsync<UserQuizQuestion>("Select * FROM UserQuizQuestion"));
            return userQuizQuestions;
        }
        public async Task<ObservableCollection<PuzzleScore>> LoadPuzzleScore()
        {

            var connection = new SQLite.SQLiteAsyncConnection(dbPath);

            puzzleScores = new ObservableCollection<PuzzleScore>(await connection.QueryAsync<PuzzleScore>("Select * FROM PuzzleScore"));
            return puzzleScores;
        }
        public async Task<ObservableCollection<QuizScore>> LoadQuizScore()
        {

            var connection = new SQLite.SQLiteAsyncConnection(dbPath);

            quizScores = new ObservableCollection<QuizScore>(await connection.QueryAsync<QuizScore>("Select * FROM QuizScore"));
            return quizScores;
        }
        //public async Task<ObservableCollection<Series>> GetSeriesById( int number)
        //{
        //    var list = new ObservableCollection<Series>();
        //    var connection = new SQLite.SQLiteAsyncConnection(dbPath);

        //    series = new ObservableCollection<Series>(await connection.QueryAsync<Series>("Select * FROM Series where id = ?", number));
        //    return series;
        //}


        //add to db tasks
        public Task Add(Element element)
        {
            elements.Add(element);
            var connection = new SQLite.SQLiteAsyncConnection(dbPath);
            return connection.InsertAsync(element);
        }
        public Task Add(ElementProperty elementProperty)
        {
            elementProperties.Add(elementProperty);
            var connection = new SQLite.SQLiteAsyncConnection(dbPath);
            return connection.InsertAsync(elementProperty);
        }
        public Task Add(Property property)
        {
            properties.Add(property);
            var connection = new SQLite.SQLiteAsyncConnection(dbPath);
            return connection.InsertAsync(property);
        }
        public Task Add(QuizLevel quizLevel)
        {
            quizLevels.Add( quizLevel);
            var connection = new SQLite.SQLiteAsyncConnection(dbPath);
            return connection.InsertAsync(quizLevel);
        }
        public Task Add(QuizQuestion quizQuestion)
        {
            quizQuestions.Add(quizQuestion);
            var connection = new SQLite.SQLiteAsyncConnection(dbPath);
            return connection.InsertAsync(quizQuestion);
        }
        public Task Add(SavedQuiz savedQuiz)
        {
            savedQuizzes.Add(savedQuiz);
            var connection = new SQLite.SQLiteAsyncConnection(dbPath);
            return connection.InsertAsync(savedQuiz);
        }
        public Task Add(Series seriesSingular)
        {
            series.Add(seriesSingular);
            var connection = new SQLite.SQLiteAsyncConnection(dbPath);
            return connection.InsertAsync(seriesSingular);
        }
        public Task Add(User user)
        {
            users.Add(user);
            var connection = new SQLite.SQLiteAsyncConnection(dbPath);
            return connection.InsertAsync(user);
        }
        public Task Add( UserFavorite userFavorite)
        {
            userFavorites.Add(userFavorite);
            var connection = new SQLite.SQLiteAsyncConnection(dbPath);
            return connection.InsertAsync(userFavorite);
        }
        public Task Add(UserNote userNote)
        {
            userNotes.Add(userNote);
            var connection = new SQLite.SQLiteAsyncConnection(dbPath);
            return connection.InsertAsync(userNote);
        }
        public Task Add(UserQuizQuestion userQuizQuestion)
        {
            userQuizQuestions.Add(userQuizQuestion);
            var connection = new SQLite.SQLiteAsyncConnection(dbPath);
            return connection.InsertAsync( userQuizQuestion );
        }
        public Task Add(PuzzleScore puzzleScore)
        {
            puzzleScores.Add(puzzleScore);
            var connection = new SQLite.SQLiteAsyncConnection(dbPath);
            return connection.InsertAsync(puzzleScore);
        }
        public Task Add(QuizScore quizScore)
        {
            quizScores.Add(quizScore);
            var connection = new SQLite.SQLiteAsyncConnection(dbPath);
            return connection.InsertAsync(quizScore);
        }

        //db remove methods
        public Task Remove(Element element)
        {
            elements.Remove(element);
            var connection = new SQLite.SQLiteAsyncConnection(dbPath);
            return connection.DeleteAsync(element);
        }
        public Task Remove(ElementProperty elementProperty)
        {
            elementProperties.Remove(elementProperty);
            var connection = new SQLite.SQLiteAsyncConnection(dbPath);
            return connection.DeleteAsync(elementProperty);
        }
        public Task Remove(Property property)
        {
            properties.Remove(property);
            var connection = new SQLite.SQLiteAsyncConnection(dbPath);
            return connection.DeleteAsync(property);
        }
        public Task Remove(QuizLevel quizLevel)
        {
            quizLevels.Remove(quizLevel);
            var connection = new SQLite.SQLiteAsyncConnection(dbPath);
            return connection.DeleteAsync(quizLevel);
        }
        public Task Remove(QuizQuestion quizQuestion)
        {
            quizQuestions.Remove(quizQuestion);
            var connection = new SQLite.SQLiteAsyncConnection(dbPath);
            return connection.DeleteAsync(quizQuestion);
        }
        public Task Remove(SavedQuiz savedQuiz)
        {
            savedQuizzes.Remove(savedQuiz);
            var connection = new SQLite.SQLiteAsyncConnection(dbPath);
            return connection.DeleteAsync(savedQuiz);
        }
        public Task Remove(Series seriesSingular)
        {
            series.Remove(seriesSingular);
            var connection = new SQLite.SQLiteAsyncConnection(dbPath);
            return connection.DeleteAsync(seriesSingular);
        }
        public Task Remove(User user)
        {
            users.Remove(user);
            var connection = new SQLite.SQLiteAsyncConnection(dbPath);
            return connection.DeleteAsync(user);
        }
        public Task Remove(UserFavorite userFavorite)
        {
            userFavorites.Remove(userFavorite);
            var connection = new SQLite.SQLiteAsyncConnection(dbPath);
            return connection.DeleteAsync(userFavorite);
        }
        public Task Remove(UserNote userNote)
        {
            userNotes.Remove(userNote);
            var connection = new SQLite.SQLiteAsyncConnection(dbPath);
            return connection.DeleteAsync(userNote);
        }
        public Task Remove(UserQuizQuestion userQuizQuestion)
        {
            userQuizQuestions.Remove(userQuizQuestion);
            var connection = new SQLite.SQLiteAsyncConnection(dbPath);
            return connection.DeleteAsync(userQuizQuestion);
        }
        public Task Remove(PuzzleScore puzzleScore)
        {
            puzzleScores.Remove(puzzleScore);
            var connection = new SQLite.SQLiteAsyncConnection(dbPath);
            return connection.DeleteAsync(puzzleScore);
        }
        public Task Remove(QuizScore quizScore)
        {
            quizScores.Remove(quizScore);
            var connection = new SQLite.SQLiteAsyncConnection(dbPath);
            return connection.DeleteAsync(quizScore);
        }

        //Update Tasks for datatables

        public Task Update(Element element)
        {
            var oldElement = elements.FirstOrDefault(e => e.AtomicNumber == element.AtomicNumber);
            if (oldElement == null)
            {
                throw new System.ArgumentException("Element not found.");
            }
            elements.Remove(oldElement);
            elements.Add(element);
            var connection = new SQLite.SQLiteAsyncConnection(dbPath);
            return connection.UpdateAsync(element);
        }

        public Task Update(ElementProperty elementProperty)
        {
            var oldElementProperty = elementProperties.FirstOrDefault(ep => ep.Id == elementProperty.Id);
            if (oldElementProperty == null)
            {
                throw new System.ArgumentException("ElementProperty not found.");
            }
            elementProperties.Remove(oldElementProperty);
            elementProperties.Add(elementProperty);
            var connection = new SQLite.SQLiteAsyncConnection(dbPath);
            return connection.UpdateAsync(elementProperty);
        }
        public Task Update(Property property)
        {
            var oldProperty = properties.FirstOrDefault(p => p.Id == property.Id);
            if (oldProperty == null)
            {
                throw new System.ArgumentException("Property not found.");
            }
            properties.Remove(oldProperty);
            properties.Add(property);
            var connection = new SQLite.SQLiteAsyncConnection(dbPath);
            return connection.UpdateAsync(property);
        }
        public Task Update(QuizLevel quizLevel)
        {
            var oldQuizLevel = quizLevels.FirstOrDefault(ql => ql.Id == quizLevel.Id);
            if (oldQuizLevel == null)
            {
                throw new System.ArgumentException("Quiz Level not found.");
            }
            quizLevels.Remove(oldQuizLevel);
            quizLevels.Add(quizLevel);
            var connection = new SQLite.SQLiteAsyncConnection(dbPath);
            return connection.UpdateAsync(quizLevel);
        }
        public Task Update(QuizQuestion quizQuestion)
        {
            var oldQuizQuestion = quizQuestions.FirstOrDefault(qq => qq.Id == quizQuestion.Id);
            if (oldQuizQuestion == null)
            {
                throw new System.ArgumentException("Quiz Level not found.");
            }
            quizQuestions.Remove(oldQuizQuestion);
            quizQuestions.Add(quizQuestion);
            var connection = new SQLite.SQLiteAsyncConnection(dbPath);
            return connection.UpdateAsync(quizQuestion);
        }
        public Task Update(SavedQuiz savedQuiz)
        {
            var oldSavedQuiz = savedQuizzes.FirstOrDefault(sq => sq.Id == savedQuiz.Id);
            if (oldSavedQuiz == null)
            {
                throw new System.ArgumentException("Saved Quiz not found.");
            }
            savedQuizzes.Remove(oldSavedQuiz);
            savedQuizzes.Add(savedQuiz);
            var connection = new SQLite.SQLiteAsyncConnection(dbPath);
            return connection.UpdateAsync(savedQuiz);
        }

        public Task Update(Series seriesSingular)
        {
            var oldSeriesSingular = series.FirstOrDefault(ss => ss.Id == seriesSingular.Id);
            if (oldSeriesSingular == null)
            {
                throw new System.ArgumentException("Series not found.");
            }
            series.Remove(oldSeriesSingular);
            series.Add(seriesSingular);
            var connection = new SQLite.SQLiteAsyncConnection(dbPath);
            return connection.UpdateAsync(seriesSingular);
        }
        public Task Update(User user)
        {
            var oldUser = users.FirstOrDefault(u => u.Id == user.Id);
            if (oldUser == null)
            {
                throw new System.ArgumentException("User not found.");
            }
            users.Remove(oldUser);
            users.Add(user);
            var connection = new SQLite.SQLiteAsyncConnection(dbPath);
            return connection.UpdateAsync(user);
        }
        public Task Update(UserFavorite userFavorite)
        {
            var oldUserFavorites = userFavorites.FirstOrDefault(uf => uf.Id == userFavorite.Id);
            if (oldUserFavorites == null)
            {
                throw new System.ArgumentException("UserFavorites not found.");
            }
            userFavorites.Remove(oldUserFavorites);
            userFavorites.Add(userFavorite);
            var connection = new SQLite.SQLiteAsyncConnection(dbPath);
            return connection.UpdateAsync(userFavorite);
        }
        public Task Update(UserNote userNote)
        {
            var oldUserNote = userNotes.FirstOrDefault(un => un.Id == userNote.Id);
            if (oldUserNote == null)
            {
                throw new System.ArgumentException("User Note not found.");
            }
            userNotes.Remove(oldUserNote);
            userNotes.Add(userNote);
            var connection = new SQLite.SQLiteAsyncConnection(dbPath);
            return connection.UpdateAsync(userNote);
        }
        public Task Update(UserQuizQuestion userQuizQuestion)
        {
            var oldUserQuizQuestion = userQuizQuestions.FirstOrDefault(uqq => uqq.Id == userQuizQuestion.Id);
            if (oldUserQuizQuestion == null)
            {
                throw new System.ArgumentException("User Quiz Question not found.");
            }
            userQuizQuestions.Remove(oldUserQuizQuestion);
            userQuizQuestions.Add(userQuizQuestion);
            var connection = new SQLite.SQLiteAsyncConnection(dbPath);
            return connection.UpdateAsync(userQuizQuestions);
        }
        public Task Update(PuzzleScore puzzleScore)
        {
            var oldPuzzleScore = puzzleScores.FirstOrDefault(e => e.Id == puzzleScore.Id);
            if (oldPuzzleScore == null)
            {
                throw new System.ArgumentException("PuzzleScore not found.");
            }
            puzzleScores.Remove(oldPuzzleScore);
            puzzleScores.Add(puzzleScore);
            var connection = new SQLite.SQLiteAsyncConnection(dbPath);
            return connection.UpdateAsync(puzzleScore);
        }
        public Task Update(QuizScore quizScore)
        {
            var oldQuizScore = quizScores.FirstOrDefault(e => e.Id == quizScore.Id);
            if (oldQuizScore == null)
            {
                throw new System.ArgumentException("QuizScore not found.");
            }
            quizScores.Remove(oldQuizScore);
            quizScores.Add(quizScore);
            var connection = new SQLite.SQLiteAsyncConnection(dbPath);
            return connection.UpdateAsync(quizScore);
        }

        public async Task<ObservableCollection<Element>> LoadRandomElement()
        {

            var connection = new SQLite.SQLiteAsyncConnection(dbPath);

            randomElements = new ObservableCollection<Element>(await connection.QueryAsync<Element>("Select * FROM Element Where AtomicNumber = ?", randomElementNumber+1));
            return randomElements;
        }
        private async Task CopyDatabase()
        {
            bool isDatabaseExisting = false;

            try
            {
                StorageFile storageFile = await ApplicationData.Current.LocalFolder.GetFileAsync("inmyelement.sqlite");
                isDatabaseExisting = true;
            }
            catch
            {
                isDatabaseExisting = false;
            }

            if (!isDatabaseExisting)
            {
                StorageFile databaseFile = await Package.Current.InstalledLocation.GetFileAsync("inmyelement.sqlite");
                await databaseFile.CopyAsync(ApplicationData.Current.LocalFolder);
            }
        }

    }
}
