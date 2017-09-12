
/*******************************************************************************\
Class: UserQuizQuestion
Created by: Shaun Cobb
Date: 3/25/14
 * *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace InMyElement.DataModel
{
    public class UserQuizQuestion
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Question { get; set; }
        public string CorrectAnswer { get; set; }
        public string AnswerA { get; set; }
        public string AnswerB { get; set; }
        public string AnswerC { get; set; }
        public string AnswerD { get; set; }
        public int ElementSeries { get; set; }
        public int Difficulty { get; set; }
        public int UserId { get; set; }

    }
}
