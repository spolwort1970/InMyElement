/*******************************************************************************\
Class: User
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
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PuzzleFastTime { get; set; }
        public float QuizHighPpm { get; set; }
        public float QuizHighScore { get; set; }
        public int PuzzleNumCorrect { get; set; }
    }
}
