using MyClothes.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Popups;
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
    public sealed partial class InsertClothes : Page
    {
        public InsertClothes()
        {
            this.InitializeComponent();
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            Frame frame = Window.Current.Content as Frame;
            if (frame == null)
            {
                return;
            }

            if (frame.CanGoBack)
            {
                frame.GoBack();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void Cancel_Button_click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private async void Insert_Button_Click(object sender, RoutedEventArgs e)
        {
            DatabaseHelperClass db_helper = new DatabaseHelperClass();
            //Creating object for DatabaseHelperClass.cs

            //Create a unique ID
            /*Guid guid = Guid.NewGuid();
            string str = guid.ToString();
            int unique = Convert.ToInt32(str);
            */

            //Converting ComboBoxItem selected item to string 
            string season = ((ComboBoxItem)ComboBoxSeason.SelectedItem).Content.ToString();
            string category= ((ComboBoxItem)ComboBoxCategory.SelectedItem).Content.ToString();
            string kind = ((ComboBoxItem)ComboBoxKind.SelectedItem).Content.ToString();
            System.Diagnostics.Debug.WriteLine(season + " " + category + " " + kind);
            if (season.Equals("None") || category.Equals("None") || kind.Equals("None") )//All three fields must me filled
            {
                MessageDialog messageDialog = new MessageDialog("Please fill three fields");
                //Combobox should not be none   
                await messageDialog.ShowAsync();
            }
            else
            {
                //Calling insert function from helper class
                string pass_file_name = "";
                ///pass_file_name = PassPictureData.picture_source;
                var obj = App.Current as App;
                pass_file_name = obj.exam;
                db_helper.Insert(new Clothes_ID(season, category, kind,obj.exam));
                MessageDialog messageDialog = new MessageDialog("Insert Successfull");
                await messageDialog.ShowAsync();

                Frame.Navigate(typeof(MainMenuPage));

            }
        }

        public void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
            tb.GotFocus -= TextBox_GotFocus;
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
