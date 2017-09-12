/*******************************************************************************\
Class: Series
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
    public class Series
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Description { get; set; }
    }
}
