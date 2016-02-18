using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClothes.Model
{
    public class Clothes_ID
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int id { get; set; }
        public string season { get; set; }
        public string category { get; set; }
        public string kind { get; set; }
        public string picture_path { get; set; }
        public Clothes_ID()
        {

        }
        public Clothes_ID(string season, string category, string kind,string picture_path)
        { 
            this.season = season;
            this.category = category;
            this.kind = kind;
            this.picture_path = picture_path;
        }
    }
}
