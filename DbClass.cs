using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClothes
{

    public class UserDataContext : DataContext
    {
        public static string DBConnectionString = @"isostore:/Databases.sdf";
        public UserDataContext(string connectionString)
            : base(connectionString)
        { }
        public Table<User> User
        {
            get
            {
                return this.GetTable<User>();
            }
        }

        internal void SubmitChanges()
        {
            throw new NotImplementedException();
        }
    }

    [Table]
    public class User
    {
        [Column(IsDbGenerated = true, IsPrimaryKey = true)]
        public int ID
        {
            get;
            set;
        }
        [Column]
        public string user_name
        {
            get;
            set;
        }
        [Column]
        public string user_pass
        {
            get;
            set;
        }
    }

    [Table]
    public class Clothes_user_ID
    {
        [Column(IsDbGenerated = true, IsPrimaryKey = true)]
        public int ID
        {
            get;
            set;
        }
        [Column]
        public int season
        {
            get;
            set;
        }
        [Column]
        public float category
        {
            get;
            set;
        }
        [Column]
        public float kind
        {
            get;
            set;
        }
    }


}


