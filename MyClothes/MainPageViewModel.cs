using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Devices.Geolocation;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Imaging;

//
// The ViewModel class
//
// This class contains the code that runs the web queries, and updates
// the databound values which will update the XAML controls.
//


namespace WeatherView
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        // Databound variables
        public string CurrentConditionsTemperatureData;
        public string CurrentConditionsDetailsData;
        public BitmapImage CurrentConditionsImageData;
        private string Forecast1TextData;
        public BitmapImage Forecast1ImageData;
        public string Forecast2TextData;
        public BitmapImage Forecast2ImageData;
        public int TemperatureDelta1Data;
        public int TemperatureDelta2Data;
        public bool ProgressVisibleData = true;


        public event PropertyChangedEventHandler PropertyChanged;

        public MainPageViewModel()
        {

            // Set some default values for images.

            CurrentConditionsImage = new BitmapImage(new System.Uri("ms-appx:///Assets/kirkland.jpg"));

            // Start the internet queries that will update the XAML controls
            FindUser();



        }


        private void WhenThingsDontWorkOut()
        {
            // When the internet queries fail, display an error message.
            CurrentConditionsTemperature = ";-(";
            CurrentConditionsDetails = "Sorry, things didn't quite go to plan!";
            Forecast1Text = "";
            Forecast2Text = "";
            Forecast1Image = new BitmapImage(new System.Uri("ms-appx:///Assets/kirkland.jpg")); ;
            Forecast2Image = new BitmapImage(new System.Uri("ms-appx:///Assets/kirkland.jpg")); ;

            TemperatureDelta1 = 999;
            TemperatureDelta2 = 999;

            ProgressVisible = false;

        }


        private void WhenSomeThingsDontWorkOut()
        {


            ProgressVisible = false;
        }




        public async void FindUser()
        {
            // Start the process by determining the location of the device.
            await findLocation();
        }

        private async Task findLocation()
        {
            var loc = new Geolocator();

            Geoposition currentLocation = null;

            try
            {

                loc.DesiredAccuracy = PositionAccuracy.High;
                currentLocation = await loc.GetGeopositionAsync();
                System.Diagnostics.Debug.WriteLine(loc.LocationStatus);
            }

            catch (TaskCanceledException)
            {
                WhenThingsDontWorkOut();
            }


            catch (System.UnauthorizedAccessException)
            {
                WhenThingsDontWorkOut();
            }

            if (currentLocation == null)
            {
                MessageDialog msg = new MessageDialog("Your location could not be determined. Let's pretend you are in Karlovasi.");
                await msg.ShowAsync();
                await WeatherUndergroundQuery(122, 47);
            }
            else
            {
                // The users location was successfully determined, now run the WeatherUnderground query
                // to get the current and future weater conditions.

                double latitude = currentLocation.Coordinate.Point.Position.Latitude;
                double longitude = currentLocation.Coordinate.Point.Position.Longitude;
                await WeatherUndergroundQuery(latitude, longitude);
            }

        }



        public async Task WeatherUndergroundQuery(double latitude, double longitude)
        {
            using (var client = new HttpClient(new HttpClientHandler(), true))
            {
                string q = string.Format("http://api.wunderground.com/api/bfcc608266ab1381/conditions/forecast/geolookup/q/{0},{1}.json", latitude, longitude);
                var data = await client.GetStringAsync(q);

                // The weather data was obtained. Process the JSON string it returned.
                ProcessWeatherData(data.ToString());
            }
        }


        private async void ProcessWeatherData(string jsondata)
        {
            // Process the JSON string returned by the WeatherUnderground web service.

            JsonObject jsonObject = JsonObject.Parse(jsondata);

            // Get the current temperature and weather description
            string weather_description = jsonObject["current_observation"].GetObject()["weather"].GetString();
            double currentTemp = jsonObject["current_observation"].GetObject()["temp_c"].GetNumber(); // or use temp_c for Celcius 
            CurrentConditionsDetails = jsonObject["current_observation"].GetObject()["display_location"].GetObject()["full"].GetString() + ".  " + weather_description + ".";
            CurrentConditionsTemperature = currentTemp.ToString() + "°";

            // Get images and description text for the forecasts
            JsonArray jsonResults = jsonObject["forecast"].GetObject()["txt_forecast"].GetObject()["forecastday"].GetArray();
            Forecast1Image = BitmapFromString(jsonResults[0].GetObject()["icon_url"].GetString());
            Forecast1Text = jsonResults[0].GetObject()["title"].GetString();
            Forecast2Image = BitmapFromString(jsonResults[2].GetObject()["icon_url"].GetString());
            Forecast2Text = jsonResults[2].GetObject()["title"].GetString();

            // Get temperature for the first two forecasts to define the colors used in their XAML control
            JsonArray jsonResults2 = jsonObject["forecast"].GetObject()["simpleforecast"].GetObject()["forecastday"].GetArray();
            int forecast1temp = Convert.ToInt16(jsonResults2[0].GetObject()["high"].GetObject()["fahrenheit"].GetString());
            int forecast2temp = Convert.ToInt16(jsonResults2[2].GetObject()["high"].GetObject()["fahrenheit"].GetString());

            TemperatureDelta1 = ((int)currentTemp - forecast1temp);
            TemperatureDelta2 = ((int)currentTemp - forecast2temp);

            try
            {
                // Query Bing to get an image related to the weather description
                await BingQuery(weather_description);
            }
            catch
            {
                WhenSomeThingsDontWorkOut();
            }

        }

        public async Task BingQuery(string query)
        {
            // Query the Bing search engine to return an image related to the input string
            using (var client = new HttpClient(new HttpClientHandler(), true))
            {

                var bingKey = "<your Bing key here>";
                var authentication = string.Format("{0}:{1}", string.Empty, bingKey);
                var encodedKey = Convert.ToBase64String(Encoding.UTF8.GetBytes(authentication));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedKey);
                string q = string.Format("https://api.datamarket.azure.com/Data.ashx/Bing/Search/v1/Image?Query=%27{0}%27&Adult=%27strict%27&$top=3&$format=JSON", query);
                var data = await client.GetStringAsync(q);

                // Bing query returned results, which now need processed
                ProcessSearch(data);
            }
        }


        private void ProcessSearch(string jsondata)
        {
            // Process the Bing return string and display the resulting image
            JsonObject jsonObject = JsonObject.Parse(jsondata);
            JsonArray jsonResults = jsonObject["d"].GetObject()["results"].GetArray();
            string weather_image = jsonResults[0].GetObject()["MediaUrl"].GetString();
            CurrentConditionsImage = BitmapFromString(weather_image);
            ProgressVisible = false;
        }



        private BitmapImage BitmapFromString(string weather_image)
        {
            // Helper function to create a bitmap from a string containing a URI
            if (weather_image == null) return null;
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.UriSource = new Uri(weather_image);
            return bitmapImage;
        }


        protected void OnPropertyChanged(string name)
        {
            // Handle the property changes which occur when a databound
            // variable is changed.
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }


        // Methods to handle the databound variables, and trigger Property Changed event

        public string CurrentConditionsTemperature
        {
            get
            {
                return CurrentConditionsTemperatureData;
            }

            set
            {
                CurrentConditionsTemperatureData = value;
                OnPropertyChanged("CurrentConditionsTemperature");
            }

        }

        public string CurrentConditionsDetails
        {
            get
            {
                return CurrentConditionsDetailsData;
            }
            set
            {
                CurrentConditionsDetailsData = value;
                OnPropertyChanged("CurrentConditionsDetails");
            }
        }

        public BitmapImage CurrentConditionsImage
        {
            get
            {
                return CurrentConditionsImageData;
            }
            set
            {
                CurrentConditionsImageData = value;
                OnPropertyChanged("CurrentConditionsImage");
            }
        }

        public string Forecast1Text
        {
            get
            {
                return Forecast1TextData;
            }

            set
            {
                Forecast1TextData = value;
                OnPropertyChanged("Forecast1Text");
            }
        }

        public BitmapImage Forecast1Image
        {
            get
            {
                return Forecast1ImageData;
            }

            set
            {
                Forecast1ImageData = value;
                OnPropertyChanged("Forecast1Image");
            }
        }

        public string Forecast2Text
        {
            get
            {
                return Forecast2TextData;
            }

            set
            {
                Forecast2TextData = value;
                OnPropertyChanged("Forecast2Text");
            }
        }

        public BitmapImage Forecast2Image
        {
            get
            {
                return Forecast2ImageData;
            }

            set
            {
                Forecast2ImageData = value;
                OnPropertyChanged("Forecast2Image");
            }

        }

        public int TemperatureDelta1
        {
            get
            {
                return TemperatureDelta1Data;
            }

            set
            {
                TemperatureDelta1Data = value;
                OnPropertyChanged("TemperatureDelta1");
            }
        }

        public int TemperatureDelta2
        {
            get
            {
                return TemperatureDelta2Data;
            }

            set
            {
                TemperatureDelta2Data = value;
                OnPropertyChanged("TemperatureDelta2");
            }
        }

        public bool ProgressVisible
        {
            get
            {
                return ProgressVisibleData;
            }

            set
            {
                ProgressVisibleData = value;
                OnPropertyChanged("ProgressVisible");
            }
        }

    }
}
