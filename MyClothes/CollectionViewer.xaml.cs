using MyClothes.Local_Database;
using MyClothes.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace MyClothes
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CollectionViewer : Page
    {
        ObservableCollection<Clothes_ID> DB_List = new ObservableCollection<Clothes_ID>();

        public CollectionViewer()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void Detail_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Delete_Button_CLick(object sender, RoutedEventArgs e)
        {

        }      

        private void Favourite_Button_CLick(object sender, RoutedEventArgs e)
        {

        }

        private void AddContact_Click(object sender, RoutedEventArgs e)
        {
            ReadClothesList db_clothes = new ReadClothesList();
            DB_List = db_clothes.GetAllClothes();//Get all DB contacts   
            //if (DB_List.Count > 0)
            //{
            //    Btn_Delete.IsEnabled = true;
            //}
            listBoxobj.ItemsSource = DB_List.OrderByDescending(i => i.id).ToList();//Binding DB data to LISTBOX and Latest contact ID can Display first. 
        }

        private void listBoxobj_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


    }
}
