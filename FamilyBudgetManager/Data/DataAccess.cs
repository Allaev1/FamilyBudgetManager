//This code was generated by a tool.
//Changes to this file will be lost if the code is regenerated.
// See the blog post here for help on using the generated code: http://erikej.blogspot.dk/2014/10/database-first-with-sqlite-in-universal.html
using SQLite;
using System;

namespace FamilyBudgetManager.Data
{
    public class SQLiteDb
    {
        string _path;
        public SQLiteDb(string path)
        {
            _path = path;
        }
        
         public void Create()
        {
            using (SQLiteConnection db = new SQLiteConnection(_path))
            {
                db.CreateTable<MoneyActions>();
                db.CreateTable<Categories>();
            }
        }
    }

    public partial class MoneyActions
    {
        [PrimaryKey, AutoIncrement]
        public Int64 ID { get; set; }
        
        [NotNull]
        public Int64 Type { get; set; }
        
        [NotNull]
        public String Category { get; set; }
        
        [NotNull]
        public String Date { get; set; }
        
        [NotNull]
        public Double Sum { get; set; }
        
        public String Note { get; set; }
        
    }

    public partial class Categories
    {
        [PrimaryKey,AutoIncrement]
        public Int64 ID { get; set; }

        [NotNull]
        public Int64 Type { get; set; }

        [NotNull]
        public String CategoryName { get; set; }

        public String Description { get; set; }
    }
    
}
