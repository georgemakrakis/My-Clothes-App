using MyClothes.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClothes
{
    class DatabaseHelperClass
    {
        SQLiteConnection MyConn;
        public string result_select = "";
        public async Task<bool> onCreate(string db_path)
        {
            try
            {
                if (!CheckFileExists(db_path).Result)
                {
                    using (MyConn = new SQLiteConnection(db_path))
                    {
                        MyConn.CreateTable<Clothes_ID>();
                    }

                }
                return true;
            }
            catch
            {
                return false;
            }

        }
        private async Task<bool> CheckFileExists(string filename)
        {
            try
            {
                var store = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFileAsync(filename);
                return true;
            }
            catch
            {
                return false;
            }

        }
        public Clothes_ID ReadClothesSeason(int clothesId)
        {
            //string p_path = PassPictureData.picture_view_source2;
            var obj = App.Current as App;
            string p_path = obj.exam2;

            using (var MyConn = new SQLiteConnection(App.db_path))
            {
                var existingId = MyConn.Query<Clothes_ID>("select season,kind,category from Clothes_ID where picture_path ='" + p_path+"'").FirstOrDefault();
                result_select = existingId.season.ToString();
                return existingId;
            }
        }
        public Clothes_ID ReadClothesKind(int clothesId)
        {
            //string p_path = PassPictureData.picture_view_source2;
            var obj = App.Current as App;
            string p_path = obj.exam2;

            using (var MyConn = new SQLiteConnection(App.db_path))
            {
                var existingId = MyConn.Query<Clothes_ID>("select season,kind,category from Clothes_ID where picture_path ='" + p_path + "'").FirstOrDefault();
                result_select = existingId.kind.ToString();
                return existingId;
            }
        }
        public Clothes_ID ReadClothesCategory(int clothesId)
        {
            //string p_path = PassPictureData.picture_view_source2;
            var obj = App.Current as App;
            string p_path = obj.exam2;

            using (var MyConn = new SQLiteConnection(App.db_path))
            {
                var existingId = MyConn.Query<Clothes_ID>("select season,kind,category from Clothes_ID where picture_path ='" + p_path + "'").FirstOrDefault();
                result_select = existingId.category.ToString();
                return existingId;
            }
        }
        public ObservableCollection<Clothes_ID> ReadClothes()
        {
            using(var MyConn=new SQLiteConnection(App.db_path))
            {
                List<Clothes_ID> myCollection = MyConn.Table<Clothes_ID>().ToList<Clothes_ID>();
                ObservableCollection<Clothes_ID> clothesList = new ObservableCollection<Clothes_ID>(myCollection);
                return clothesList;
            }
        }
        public void Insert(Clothes_ID newcontact)
        {
            using (var MyConn = new SQLiteConnection(App.db_path))
            {
                MyConn.RunInTransaction(() =>
                {
                    MyConn.Insert(newcontact);
                });
            }
        }
    }
}
