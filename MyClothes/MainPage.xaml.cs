﻿using makrakis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Windows.UI.Popups;
using System.Linq;
using Windows.Security.Credentials;
using MyClothes.Helpers;
using Facebook;
using Windows.ApplicationModel.Activation;
using Windows.Security.Authentication.Web;
using System.Diagnostics;
using System.Text.RegularExpressions;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace MyClothes
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public string AccessToken;
        public DateTime TokenExpiry;

        private async void Login()
        {
            //Facebook app id
            var clientId = "451251791732276";
            //Facebook permissions
            var scope = "public_profile, email";

            var redirectUri = WebAuthenticationBroker.GetCurrentApplicationCallbackUri().ToString();
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = clientId,
                redirect_uri = redirectUri,
                response_type = "token",
                scope = scope
            });
            Debug.WriteLine(redirectUri);
            Uri startUri = loginUrl;
            Uri endUri = new Uri(redirectUri, UriKind.Absolute);

            
            WebAuthenticationBroker.AuthenticateAndContinue(startUri, endUri, null, WebAuthenticationOptions.None);

        }
        public async void ContinueWebAuthentication(WebAuthenticationBrokerContinuationEventArgs args)
        {
            await ParseAuthenticationResult(args.WebAuthenticationResult);
        }
        public async Task ParseAuthenticationResult(WebAuthenticationResult result)
        {
            switch (result.ResponseStatus)
            {
                case WebAuthenticationStatus.ErrorHttp:
                    Debug.WriteLine("Error");
                    break;
                case WebAuthenticationStatus.Success:
                    var pattern = string.Format("{0}#access_token={1}&expires_in={2}", WebAuthenticationBroker.GetCurrentApplicationCallbackUri(), "(?<access_token>.+)", "(?<expires_in>.+)");
                    var match = Regex.Match(result.ResponseData, pattern);

                    var access_token = match.Groups["access_token"];
                    var expires_in = match.Groups["expires_in"];

                    AccessToken = access_token.Value;
                    TokenExpiry = DateTime.Now.AddSeconds(double.Parse(expires_in.Value));

                    break;
                case WebAuthenticationStatus.UserCancel:
                    Debug.WriteLine("Operation aborted");
                    break;
                default:
                    break;
            }
        }
        private async Task ShowUserInfo()
        {
            FacebookClient client = new FacebookClient(AccessToken);
            dynamic user = await client.GetTaskAsync("me");
            User1.Text = user.name;
        }
        //IfUsed
        #region AzuremobileServiceFbLogin
        /*
        private MobileServiceCollection<TodoItem, TodoItem> items;
        private IMobileServiceTable<TodoItem> todoTable = App.MobileService.GetTable<TodoItem>();

        private MobileServiceUser user;

        // Define a method that performs the authentication process
        // using a Facebook sign-in. 
        private async System.Threading.Tasks.Task<bool> Authenticate()
        {
            string message;
            bool success = false;
            try
            {
                // Sign-in using Facebook authentication.
                user = await App.MobileService.LoginAsync(MobileServiceAuthenticationProvider.Facebook);
                message =string.Format("You are now signed in - {0}", user.UserId);

                success = true;
            }
            catch (InvalidOperationException)
            {
                message = "You must log in. Login Required";
            }

            var dialog = new MessageDialog(message);
            dialog.Commands.Add(new UICommand("OK"));
            await dialog.ShowAsync();
            return success;
        }*/
        /*
        private async void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            // Login the user and then load data from the mobile app.
            if (await Authenticate())
            {
                // Hide the login button and load items from the mobile app.
                ButtonLogin.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                //await InitLocalStoreAsync(); //offline sync support.
                await RefreshTodoItems();
            }

            MessageDialog msg = new MessageDialog("Logged in");
            msg.Commands.Add(new UICommand("OK"));
            await msg.ShowAsync();

            Frame.Navigate(typeof(MainMenuPage));
        }*/
        #endregion //IufUSed
        #region TodoItems
        /*
        private async Task InsertTodoItem(TodoItem todoItem)
        {
            // This code inserts a new TodoItem into the database. When the operation completes
            // and Mobile App backend has assigned an Id, the item is added to the CollectionView.
            await todoTable.InsertAsync(todoItem);
            items.Add(todoItem);

            //await SyncAsync(); // offline sync
        }

        private async Task RefreshTodoItems()
        {
            MobileServiceInvalidOperationException exception = null;
            try
            {
                // This code refreshes the entries in the list view by querying the TodoItems table.
                // The query excludes completed TodoItems.
                items = await todoTable
                    .Where(todoItem => todoItem.Complete == false)
                    .ToCollectionAsync();
            }
            catch (MobileServiceInvalidOperationException e)
            {
                exception = e;
            }

            if (exception != null)
            {
                await new MessageDialog(exception.Message, "Error").ShowAsync();
            }
            else
            {

            }
        }

        private async Task UpdateCheckedTodoItem(TodoItem item)
        {
            // This code takes a freshly completed TodoItem and updates the database. When the service 
            // responds, the item is removed from the list.
            await todoTable.UpdateAsync(item);
            items.Remove(item);
            //ListItems.Focus(Windows.UI.Xaml.FocusState.Unfocused);

            //await SyncAsync(); // offline sync
        }
        */
        #endregion

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        /*protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }*/
        private void Sign_in_ButtonClick(object sender, RoutedEventArgs e)
        {

            //elegxos gia ta stoixeia oy user

            // kateftheian navigation sto menu 
            Frame.Navigate(typeof(MainMenuPage));
        }

        private void Register_ButtonClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RegistrationPage));
        }

        private void FbLogin_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }

        Uri _logoutUrl;
        private void FbLogout_Click(object sender, RoutedEventArgs e)
        {
                        


        }

       
    }
}