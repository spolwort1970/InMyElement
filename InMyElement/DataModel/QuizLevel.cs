/*******************************************************************************\
Class: QuizLevel
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
    public class QuizLevel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Difficulty { get; set; }
        public string Points{ get; set; }
    }
}
