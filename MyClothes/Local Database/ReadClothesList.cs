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

        public string GetSeasonData()
        {
            db_helper.ReadClothesSeason(1);
            return db_helper.result_select;
        }
        public string GetKindData()
        {
            db_helper.ReadClothesKind(1);
            return db_helper.result_select;
        }
        public string GetCategoryData()
        {
            db_helper.ReadClothesCategory(1);
            return db_helper.result_select;
        }
        public ObservableCollection<Clothes_ID> GetAllClothes()
        {
            return db_helper.ReadClothes();
        }
    }
}
