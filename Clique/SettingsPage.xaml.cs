using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Clique.Assets;
using Kshatriya_Clique.Assets;
using System.Net.Http;
using Clique.Data;
using Clique.DataModel;
using System.Net.Http.Headers;
using System.Text;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Clique
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();

            Storage storage = new Storage();

            string sDataKey = "userDetails";
            string uDataKey = "usernameDetails";
            string pDataKey = "passwordDetails";

            string userData = storage.LoadSettings(sDataKey);
            string usernameDetails = storage.LoadSettings(uDataKey);
            string passwordDetails = storage.LoadSettings(pDataKey);
            
            if (userData != "Null")
            {

                var viewModel = new UserDataSource(userData, usernameDetails, passwordDetails);

                LoginForm.Visibility = Visibility.Collapsed;
                UserData.Visibility = Visibility.Visible;
                AccountSettingsPanel.Visibility = Visibility.Visible;

                this.DataContext = viewModel;

            }

        }

        private void Login_Event(object sender, RoutedEventArgs e)
        {
            string username = myUsernameBox.Text;
            string password = myPasswordBox.Password;
            string eventStarted = "login";

            object dummysender = null;

            if (username == "")
            {
                usernameDialog(dummysender);
            }
            else if (username == null)
            {
                usernameDialog(dummysender);
            }
            else
            {
                var varToGetPassword = "password";
                var varToGetUsername = "username";
                var target = "http://www.kshatriya.co.uk/dev/php/test_case/auth.php";

                createURI(target, varToGetUsername, varToGetPassword, username, password, eventStarted);
            }
        }

        private void Register_Event(object sender, RoutedEventArgs e)
        {
            string username = myRegUsernameBox.Text;
            string name = myRegNameBox.Text;
            string password = myRegPasswordBox.Password;
            string eventStarted = "register";

            object dummysender = null;

            if (username == "")
            {
                usernameDialog(dummysender);
            }
            else if (username == null)
            {
                usernameDialog(dummysender);
            }
            else
            {
                var varToGetPassword = "password";
                var varToGetUsername = "username";
                var varToGetName = "name";
                var target = "http://www.kshatriya.co.uk/dev/php/test_case/newuser.php";

                createURI(target, varToGetUsername, varToGetPassword, varToGetName, username, password, name, eventStarted);
            }
        }

        private async void createURI(string target, string qsParameterUsername, string qsParameterPassword, string qsParameterName, string qsValueUsername, string qsValuePassword, string qsValueName, string eventStarted)
        {
            loginProgressRing.IsActive = true;

            var postData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(qsParameterName, qsValueName),
                new KeyValuePair<string, string>("action", "add")
            };

            var content = new FormUrlEncodedContent(postData);

            var client = new HttpClient();

            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", qsValueUsername, qsValuePassword)));

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            // call sync
            var response = client.PostAsync("http://kshatriya.co.uk/dev/php/test_case/newuser.php", content).Result;
            if (response.IsSuccessStatusCode)
            {
                target = "http://kshatriya.co.uk/dev/php/test_case/auth.php";
                createURI(target, qsParameterUsername, qsParameterPassword, qsValueUsername, qsValuePassword, eventStarted);
            }
            else
            {
                loginProgressRing.IsActive = false;
                var title = "Error with Authentication";
                var message = "Unable to register";
                errorDialog(title, message);
            }

        }

        private async void createURI(string target, string qsParameterUsername, string qsParameterPassword, string qsValueUsername, string qsValuePassword, string eventStarted)
        {
            loginProgressRing.IsActive = true;
            
            var uri = UriExtensions.CreateUriWithQuery(new Uri(target),
            new NameValueCollection { { "action", "get" } });

            /*
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(qsValueUsername, qsValuePassword);
            var responseString = "";
            var response = httpClient.GetAsync(uri).Result;
            string responseBodyAsText = await httpClient.GetStringAsync(target);
            */

            var client = new HttpClient();
            var byteArray = Encoding.UTF8.GetBytes(qsValueUsername + ":" + qsValuePassword);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            // call sync
            var response = client.GetAsync(uri).Result;
            var responseString = "";
            
            if (response.IsSuccessStatusCode)
            {
                responseString = await response.Content.ReadAsStringAsync();
                getSearchResults(responseString, qsValueUsername, qsValuePassword, eventStarted);
            }
            else
            {
                loginProgressRing.IsActive = false;
                var title = "Error with Authentication";
                var message = "Unable to login, Username or Password was incorrect";
                errorDialog(title, message);  
            }

        }

        private void getSearchResults(string JSON, string username, string password, string eventStarted)
        {

            if (JSON != "")
            {

                loginProgressRing.IsActive = false;

                var loginData = new UserDataSource(JSON, username, password);

                if(eventStarted == "register")
                {
                    string title = "Registration Successful";

                    LoginForm.Visibility = Visibility.Collapsed;
                    UserData.Visibility = Visibility.Visible;
                    AccountSettingsPanel.Visibility = Visibility.Visible;

                    this.DataContext = loginData;

                    registerDialog(title, username);
                }
                else if(eventStarted == "login")
                {
                    string title = "Login Successful";

                    LoginForm.Visibility = Visibility.Collapsed;
                    UserData.Visibility = Visibility.Visible;
                    AccountSettingsPanel.Visibility = Visibility.Visible;

                    this.DataContext = loginData;

                    loginDialog(title, username);
                }
                
            }

        }




        /* Dialog Boxes for various messages */

        private async void usernameDialog(object sender)
        {
            string title = "No Username";
            string message = "You have not entered your Username! How do you expect me to log you in!?";
            int commands = 1;
            Dialog.standardDialog(title, message, commands, sender);

        }
        private async void loginDialog(string title, string messageDetails)
        {
            object sender = null;
            string template = "Your user credentials for {0} are confirmed, we will store your details to allow unguided login when you open the app. Your details will now be displayed and used to access the Clique Service.";
            string message = string.Format(template, messageDetails);
            int commands = 1;
            Dialog.standardDialog(title, message, commands, sender);

        }
        private async void registerDialog(string title, string messageDetails)
        {
            object sender = null;
            string message = "Your user credentials have been registered with Clique, we will store your details to allow unguided login when you open the app. Your details will now be displayed and used to access the Clique Service.";
            int commands = 1;
            Dialog.standardDialog(title, message, commands, sender);

        }
        private async void errorDialog(string title, string messageDetails)
        {
            object sender = null;
            string message = messageDetails;
            int commands = 1;
            Dialog.standardDialog(title, message, commands, sender);

        }

        private void Login_Off_Event(object sender, RoutedEventArgs e)
        {

            string defaultValue = "Null";

            Storage storage = new Storage();

            string sDataKey = "userDetails";
            string uDataKey = "usernameDetails";
            string pDataKey = "passwordDetails";

            storage.SaveSettings(sDataKey, defaultValue);
            storage.SaveSettings(uDataKey, defaultValue);
            storage.SaveSettings(pDataKey, defaultValue);

            UserData.Visibility = Visibility.Collapsed;
            LoginForm.Visibility = Visibility.Visible;
            AccountSettingsPanel.Visibility = Visibility.Collapsed;

        }

        private async void NewAvatar_Event(object sender, RoutedEventArgs e)
        {
            var filePicker = new FileOpenPicker();
            filePicker.FileTypeFilter.Add(".jpg");
            filePicker.ViewMode = PickerViewMode.Thumbnail;
            filePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            filePicker.SettingsIdentifier = "picker1";
            filePicker.CommitButtonText = "Select";

            var file = await filePicker.PickSingleFileAsync();

            Storage storage = new Storage();

            string uDataKey = "usernameDetails";
            string pDataKey = "passwordDetails";

            string usernameDetails = storage.LoadSettings(uDataKey);
            string passwordDetails = storage.LoadSettings(pDataKey);

            /* File upload */

            byte[] fileBytes = null;
            using (IRandomAccessStreamWithContentType stream = await file.OpenReadAsync())
            {
                fileBytes = new byte[stream.Size];
                using (DataReader reader = new DataReader(stream))
                {
                    await reader.LoadAsync((uint)stream.Size);
                    reader.ReadBytes(fileBytes);
                }
            }

            var qsParameterUsername = "username";
            var qsParameterPassword = "password";
            var eventStarted = "avatarChanged";

            var content = new ByteArrayContent(fileBytes);

            var client = new HttpClient();

            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", usernameDetails, passwordDetails)));

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            // call sync
            var response = client.PostAsync("http://apps.kshatriya.co.uk/test_case/newavatar.php", content).Result;
            if (response.IsSuccessStatusCode)
            {
                var target = "http://kshatriya.co.uk/dev/php/test_case/auth.php";
                createURI(target, qsParameterUsername, qsParameterPassword, usernameDetails, passwordDetails, eventStarted);
            }
            else
            {
                loginProgressRing.IsActive = false;
                var title = "Error with Authentication";
                var message = "Unable to register";
                errorDialog(title, message);
            }

            
        }
    }
}
