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
using Clique.DataModel;

// The Grouped Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234231

namespace Clique
{

    public sealed partial class LoginFormPage : Page
    {

        public LoginFormPage()
        {
            this.InitializeComponent();

        }
        private void Login_Event(object sender, RoutedEventArgs e)
        {
            string username = myUsernameBox.Text;
            string password = myPasswordBox.Text;

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
                var target = "http://apps.kshatriya.co.uk/test_case/auth.php";

                createURI(target, varToGetUsername, varToGetPassword, username, password);
            }
        }

        private async void createURI(string target, string qsParameterUsername, string qsParameterPassword, string qsValueUsername, string qsValuePassword)
        {
            searchProgressRing.IsActive = true;

            var uri = UriExtensions.CreateUriWithQuery(new Uri(target),
            new NameValueCollection { { qsParameterUsername, qsValueUsername } },
            new NameValueCollection { { qsParameterPassword, qsValuePassword } });

            HttpClient httpClient = new HttpClient();
            string responseBodyAsText = await httpClient.GetStringAsync(uri);

            getSearchResults(responseBodyAsText, qsParameterUsername, qsParameterPassword);

        }

        private void getSearchResults(string JSON, string username, string password)
        {

            if(JSON != "")
            {

                searchProgressRing.IsActive = false;

                var loginData = new UserDataSource(JSON, username, password);

                string title = "Login Successful";

                loginDialog(title, username);
            }

        }

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
            string message = "Your user credentials for " + messageDetails + " are confirmed, to successfully login go back to the previous page. Your details will now be displayed and used to access the Clique Service.";
            int commands = 1;
            Dialog.standardDialog(title, message, commands, sender);

        }
    }
}