using MyClothes.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClothes.Local_Database
{
    class ReadClothesList
    {
        DatabaseHelperClass db_helper = new DatabaseHelperClass();
        public ObservableCollection<Clothes_ID> GetAllClothes()
        {
            return db_helper.ReadClothes();
        }
    }
}
