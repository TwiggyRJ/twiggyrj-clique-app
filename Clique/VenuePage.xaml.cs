using Clique;
using Kshatriya_Clique.Assets;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Collections.ObjectModel;
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
using Clique.DataModel;
using Clique.Assets;
using Windows.UI;
using Windows.UI.ViewManagement;
using static Clique.Assets.WindowingHelper;

// The Grouped Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234231

namespace Clique
{

    public sealed partial class VenuePage : Page
    {

        public string selectedVenue { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)

        {

            Payload passedParameter = e.Parameter as Payload;

            string itemID = passedParameter.qStringID as string;
            string itemName = passedParameter.qStringName as string;

            var target = "http://kshatriya.co.uk/dev/php/test_case/search/";

            createURI(target, passedParameter, itemID, itemName);

        }


        public VenuePage()
        {
            
            this.InitializeComponent();


            var kshatriyaPink = Color.FromArgb(0, 194, 24, 91);
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = kshatriyaPink;
            titleBar.ForegroundColor = Colors.White;
            titleBar.ButtonBackgroundColor = kshatriyaPink;
            titleBar.ButtonForegroundColor = Colors.White;
        }

        private async void createURI(string target, object stuffing, string qsValueID, string qsValueName)
        {

            searchProgressRing.IsActive = true;

            var targetVenue = target + qsValueName;

            var uri = new Uri(targetVenue);

            HttpClient httpClient = new HttpClient();
            var response = httpClient.GetAsync(uri).Result;

            var responseString = "";

            if (response.IsSuccessStatusCode)
            {
                responseString = await response.Content.ReadAsStringAsync();
                createURI(target, qsValueID, responseString);
            }
            else
            {
                searchProgressRing.IsActive = false;
                var title = "Error with Application";
                var message = "It's not you, it's me! Unfortuantely there is an error connecting with the Clique Service";
                errorDialog(title, message);
            }

        }

        private async void createURI(string target, string qsValueID, string JSON)
        {

            var targetReviews = target + qsValueID;

            var uri = new Uri(targetReviews);

            HttpClient httpClient = new HttpClient();
            var response = httpClient.GetAsync(uri).Result;

            var responseString = "";

            if (response.IsSuccessStatusCode)
            {
                responseString = await response.Content.ReadAsStringAsync();
                getSearchResults(JSON, responseString);
            }
            else
            {
                searchProgressRing.IsActive = false;
                var title = "Error with Application";
                var message = "He didn't get out of the cock-a-doodie car!";
                errorDialog(title, message);
            }

        }

        private void getSearchResults(string JSON, string JSONReview)
        {

            var viewModelVenues = new VenueDataSource(JSON, JSONReview);
            this.DataContext = viewModelVenues;
            searchProgressRing.IsActive = false;

        }


        private async void errorDialog(string title, string messageDetails)
        {
            object sender = null;
            string message = messageDetails;
            int commands = 1;
            Dialog.standardDialog(title, message, commands, sender);

        }

        /*
        private async void createURI(string target, string targetReviews, string qsParameter, string qsValue)
        {

            searchProgressRing.IsActive = true;

            var uri = UriExtensions.CreateUriWithQuery(new Uri(target),
            new NameValueCollection { { qsParameter, qsValue } });

            HttpClient httpClient = new HttpClient();
            string responseBodyAsText = await httpClient.GetStringAsync(uri);

            createreviewURI(targetReviews, qsParameter, qsValue, responseBodyAsText);

        }

        private async void createreviewURI(string target, string qsParameter, string qsValue, string JSON)
        {
            var uri = UriExtensions.CreateUriWithQuery(new Uri(target),
            new NameValueCollection { { qsParameter, qsValue } });

            HttpClient httpClient = new HttpClient();
            string responseBodyAsText = await httpClient.GetStringAsync(uri);

            getSearchResults(JSON, responseBodyAsText);

        }

        private void getSearchResults(string JSON, string JSONReview)
        {
            
            var viewModelVenues = new VenueDataSource(JSON, JSONReview);
            this.DataContext = viewModelVenues;
            searchProgressRing.IsActive = false;

        }

        */
    }
}
