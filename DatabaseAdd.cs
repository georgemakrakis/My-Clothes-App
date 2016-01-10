using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClothes
{
    class DatabaseAdd
    {
  
        public void AddUser(String Username, String Password)
        {
            using (UserDataContext context = new UserDataContext(UserDataContext.DBConnectionString))
            {
                User du = new User();
                du.user_name = Username;
                du.user_pass = Password;
                context.User.InsertOnSubmit(du);
                context.SubmitChanges();
            }
        }
    }
}
