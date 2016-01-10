using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using System.Windows;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.ComponentModel;
using Windows.Phone.UI.Input;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace MyClothes
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainMenuPage : Page
    {
        public MainMenuPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
    
      
        private void Add_new_Clothes_Click(object sender, RoutedEventArgs e)
        {
            //navigate sto InsertClothes
            Frame.Navigate(typeof(CameraPage));
        }

        private void Search_Button_Click(object sender, RoutedEventArgs e)
        {
            // navigate sto page gia search 
            Frame.Navigate(typeof(SearchClothesPage));
        }


        private void View_Collection_Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CollectionViewer));
        }

        private void View_Favourite_Button_cCick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(FavouriteViewer));
        }

        private void Make_Combination_Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CombinationPage));
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

      

    

      
    }
}
