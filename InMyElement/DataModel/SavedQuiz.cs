/*******************************************************************************\
Class: SavedQuiz
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
    public class SavedQuiz
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Comments { get; set; }
        public int UserId { get; set; }
        public int NumberOfQuestions { get; set; }
        public int Time { get; set; }
        public int Difficulty { get; set; }
        public bool IsAlkali { get; set; }
        public bool IsAlkaliEarth { get; set; }
        public bool IsTransMetal { get; set; }
        public bool IsMetalloid { get; set; }
        public bool IsPostTransMetal { get; set; }
        public bool IsNonMetal { get; set; }
        public bool IsHalogen { get; set; }
        public bool IsLanth { get; set; }
        public bool IsActin { get; set; }
        public bool IsNoble { get; set; }
        public bool IncludeUserGen { get; set; }
    }
}
