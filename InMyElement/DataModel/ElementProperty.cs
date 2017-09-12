/*******************************************************************************\
Class: ElementProperty
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
    public class ElementProperty
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int  AtomicNumber { get; set; }
        public int PropertyId { get; set; }
        public string Value { get; set; }
       
    }
}
