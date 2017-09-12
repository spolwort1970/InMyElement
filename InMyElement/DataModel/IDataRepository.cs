/*******************************************************************************\
Class: IDataRepository
Created by: Shaun Cobb
Date: 3/25/14
 * *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace InMyElement.DataModel
{
    public interface IDataRepository
    {
        //tasks to get datatables into objects
        Task<ObservableCollection<Element>> LoadElement();
        Task<ObservableCollection<Element>> LoadElementByName();
        Task<ObservableCollection<ElementProperty>> LoadElementProperty();
        Task<ObservableCollection<Property>> LoadProperty();
        Task<ObservableCollection<QuizLevel>> LoadQuizLevel();
        Task<ObservableCollection<QuizQuestion>> LoadQuizQuestion();
        Task<ObservableCollection<QuizQuestion>> LoadQuizQuestion(QuizDifficulty difficulty);
        Task<ObservableCollection<QuizQuestion>> LoadQuizQuestion(SavedQuiz quiz);
        Task<ObservableCollection<SavedQuiz>> LoadSavedQuiz();
        Task<ObservableCollection<Series>> LoadSeries();
        Task<ObservableCollection<User>> LoadUser();
        Task<ObservableCollection<UserFavorite>> LoadUserFavorite();
        Task<ObservableCollection<UserNote>> LoadUserNote();
        Task<ObservableCollection<UserQuizQuestion>> LoadUserQuizQuestion();
        Task<ObservableCollection<PuzzleScore>> LoadPuzzleScore();
        Task<ObservableCollection<QuizScore>> LoadQuizScore();
        
        //Add to db tasks
        Task Add(Element element);
        Task Add(ElementProperty elementProperty);
        Task Add(Property property);
        Task Add(QuizLevel quizlevel);
        Task Add(QuizQuestion quizQuestion);
        Task Add(SavedQuiz savedQuiz);
        Task Add(Series series);
        Task Add(User user);
        Task Add(UserFavorite userFavorite);
        Task Add(UserNote userNote);
        Task Add(UserQuizQuestion userQuizQuestion);
        Task Add(PuzzleScore puzzleScore);
        Task Add(QuizScore quizScore);

        //Remove from db tasks
        Task Remove(Element element);
        Task Remove(ElementProperty elementProperty);
        Task Remove(Property property);
        Task Remove(QuizLevel quizlevel);
        Task Remove(QuizQuestion quizQuestion);
        Task Remove(SavedQuiz savedQuiz);
        Task Remove(Series seriesSingular);
        Task Remove(User user);
        Task Remove(UserFavorite userFavorite);
        Task Remove(UserNote userNote);
        Task Remove(UserQuizQuestion userQuizQuestion);
        Task Remove(PuzzleScore puzzleScore);
        Task Remove(QuizScore quizScore);

        //Update Database
        Task Update(Element element);
        Task Update(ElementProperty elementProperty);
        Task Update(Property property);
        Task Update(QuizLevel quizlevel);
        Task Update(QuizQuestion quizQuestion);
        Task Update(SavedQuiz savedQuiz);
        Task Update(Series seriesSingular);
        Task Update(User user);
        Task Update(UserFavorite userFavorite);
        Task Update(UserNote userNote);
        Task Update(UserQuizQuestion userQuizQuestion);
        Task Update(PuzzleScore puzzleScore);
        Task Update(QuizScore quizScore);

        //get random Element
        Task<ObservableCollection<Element>> LoadRandomElement();
    }
}
