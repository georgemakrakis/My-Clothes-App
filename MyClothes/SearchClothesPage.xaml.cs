using System;
using System.Collections.Generic;
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

using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Windows.UI.Popups;
using Windows.Data.Json;
using System.ComponentModel;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace MyClothes
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchClothesPage : Page
    {
        //private string longitude, latitude;
        // private string CurrentConditionsTemperatureData;
        // Define the app ViewModel class
        private WeatherView.MainPageViewModel mainPageViewModel = new WeatherView.MainPageViewModel();

        public WeatherView.MainPageViewModel MainPageViewModel
        {
            get { return this.mainPageViewModel; }
        }

        public SearchClothesPage()
        {
            this.InitializeComponent();

            //FindUser();


        }
        /*#region Location-WeatherData
        public async void FindUser()
        {
            // Start the process by determining the location of the device.
            await findLocation();

            //after everything is complete show temperature
            //TemperatureTxtBlock.Text = CurrentConditionsTemperatureData;
        }

        private async Task findLocation()
        {
            var loc = new Geolocator();

            Geoposition currentLocation = null;

            loc.DesiredAccuracy = PositionAccuracy.High;
            currentLocation = await loc.GetGeopositionAsync();
            System.Diagnostics.Debug.WriteLine(loc.LocationStatus);

            if (currentLocation == null)
            {
                MessageDialog msg = new MessageDialog("Your location could not be determined. Let's pretend you are in Karlovasi.");
                await msg.ShowAsync();
                await GetData(37.7920900, 26.7049000);
            }
            else
            {
                // The users location was successfully determined, now run the WeatherUnderground query
                // to get the current and future weater conditions.

                double latitude = currentLocation.Coordinate.Point.Position.Latitude;
                double longitude = currentLocation.Coordinate.Point.Position.Longitude;
                await GetData(latitude, longitude);
            }



        }
        private async Task GetData(double latitude, double longitude)
        {
            try
            {
                using (var client = new HttpClient(new HttpClientHandler(), true))
                {
                    string q = string.Format("http://api.wunderground.com/api/bfcc608266ab1381/conditions/forecast/geolookup/q/{0},{1}.json", latitude, longitude);
                    var data = await client.GetStringAsync(q);

                    // The weather data was obtained. Process the JSON string it returned.
                    ProcessWeatherData(data.ToString());
                }
            }
            catch (Exception ex)
            {
                excep.Text = "Some Error Occured" + ex;

                pbWeather.Visibility = Visibility.Collapsed;
            }


        }
        private void ProcessWeatherData(string jsondata)
        {
            // Process the JSON string returned by the WeatherUnderground web service.

            JsonObject jsonObject = JsonObject.Parse(jsondata);

            // Get the current temperature and weather description
            string weather_description = jsonObject["current_observation"].GetObject()["weather"].GetString();
            double currentTemp = jsonObject["current_observation"].GetObject()["temp_c"].GetNumber();

            CurrentConditionsTemperature = currentTemp.ToString() + "°";


        }
        public string CurrentConditionsTemperature
        {
            get
            {
                return CurrentConditionsTemperatureData;
            }

            set
            {
                CurrentConditionsTemperatureData = value;

            }

        }
#endregion*/
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void Search_Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SearchClothesResults));
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
