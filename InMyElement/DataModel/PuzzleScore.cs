﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMyElement.DataModel
{
    public class PuzzleScore
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int TimeInSecs { get; set; }
        public int NumberCorrect { get; set; }
        public int UserId { get; set; }
    }
}
