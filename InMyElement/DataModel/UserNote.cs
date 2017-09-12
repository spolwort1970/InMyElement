/*******************************************************************************\
Class: UserNote
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
    public class UserNote
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int AtomicNumber { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Note { get; set; }

    }
}
