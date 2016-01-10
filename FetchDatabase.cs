using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClothes
{
    class User
    {
           
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
    }
    class FetchDatabase
    {
        public IList<User> GetAllUsers()
        {
            IList<User> list = null;
            using (UserDataContext context = new UserDataContext(UserDataContext.DBConnectionString))
            {
                IQueryable<User> query = from c in context.User select c;
                list = query.ToList();
            }
            return list;
        }
        public List<User> getUser()
        {
            IList<User> usrs = this.GetAllUsers();
            List<User> allmsgs = new List<User>();
            foreach (User m in usrs)
            {
                User n = new User();
                n.id = m.ID.ToString();
                n.name = m.user_name;
                n.email = m.user_pass;
                allmsgs.Add(n);
            }
            return allmsgs;
        }
    }
    }

