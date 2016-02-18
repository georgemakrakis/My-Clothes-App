using MyClothes.Local_Database;
using MyClothes.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Activation;
using Windows.Storage;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace MyClothes
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CollectionViewer : Page
    {
        ObservableCollection<Clothes_ID> DB_List = new ObservableCollection<Clothes_ID>();

        private CoreApplicationView view;
        private string ImagePath;

        public CollectionViewer()
        {
            this.InitializeComponent();
            view = CoreApplication.GetCurrentView();
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
            

        }

        private  async void viewActivated(CoreApplicationView sender, IActivatedEventArgs args1)
        {
            FileOpenPickerContinuationEventArgs args = args1 as FileOpenPickerContinuationEventArgs;

            if (args != null)
            {
                if (args.Files.Count == 0) return;

                view.Activated -= viewActivated;
                StorageFile storageFile = args.Files[0];
                var stream = await storageFile.OpenAsync(Windows.Storage.FileAccessMode.Read);
                var bitmapImage = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
                await bitmapImage.SetSourceAsync(stream);
                var decoder = await Windows.Graphics.Imaging.BitmapDecoder.CreateAsync(stream);
                img.Source = bitmapImage;

                //PictureTags.Text = storageFile.Name;
                //PassPictureData.picture_view_source2 = storageFile.Name;
                var obj = App.Current as App;
                obj.exam2 = storageFile.Name;

                //getting data from DB 
                ReadClothesList db_clothes = new ReadClothesList();
                SeasonTxtBlck.Text = db_clothes.GetSeasonData();
                CategoryTxtBlck.Text = db_clothes.GetCategoryData();
                KindTxtBlck.Text = db_clothes.GetKindData();               

            }
        }

        private void listBoxobj_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            //ReadClothesList db_clothes = new ReadClothesList();
            //DB_List = db_clothes.GetAllClothes();//Get all DB contacts   
            ////if (DB_List.Count > 0)
            ////{
            ////    Btn_Delete.IsEnabled = true;
            ////}
            //listBoxobj.ItemsSource = DB_List.OrderByDescending(i => i.id).ToList();//Binding DB data to LISTBOX and Latest contact ID can Display first. 

            ImagePath = string.Empty;
            FileOpenPicker filePicker = new FileOpenPicker();
            filePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            filePicker.ViewMode = PickerViewMode.Thumbnail;

            // Filter to include a sample subset of file types
            filePicker.FileTypeFilter.Clear();
            filePicker.FileTypeFilter.Add(".bmp");
            filePicker.FileTypeFilter.Add(".png");
            filePicker.FileTypeFilter.Add(".jpeg");
            filePicker.FileTypeFilter.Add(".jpg");

            filePicker.PickSingleFileAndContinue();
            view.Activated += viewActivated;
           
        }
    }
}
