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
using Clique.DataModel;
using System.Collections.ObjectModel;
using Kshatriya_Clique.Assets;
using System.Net.Http;
using Clique.Assets;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Clique
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AllVenuesPage : Page
    {

        private ObservableCollection<Venue> _venuesViewModel = new ObservableCollection<Venue>();

        public ObservableCollection<Venue> VenuesViewModel
        {
            get { return this._venuesViewModel; }
        }

        public AllVenuesPage()
        {
            this.InitializeComponent();
            var toGet = "all";
            var varToGet = "search";
            var target = "http://kshatriya.co.uk/dev/php/test_case/search.php";

            searchProgressRing.IsActive = true;

            createURI(target, varToGet, toGet);
        }

        private async void createURI(string target, string qsParameter, string qsValue)
        {
            /*
            var uri = UriExtensions.CreateUriWithQuery(new Uri(target),
            new NameValueCollection { { qsParameter, qsValue } });

            HttpClient httpClient = new HttpClient();
            string responseBodyAsText = await httpClient.GetStringAsync(uri);

            getSearchResults(responseBodyAsText);
            */

            var client = new HttpClient();

            var uri = UriExtensions.CreateUriWithQuery(new Uri(target),
            new NameValueCollection { { qsParameter, qsValue } });

            // call sync
            var response = client.GetAsync(uri).Result;
            var responseString = "";

            //checking if request was successful
            if (response.IsSuccessStatusCode)
            {
                responseString = await response.Content.ReadAsStringAsync();
                getSearchResults(responseString);
            }
            else
            {
                searchProgressRing.IsActive = false;
                var title = "Error with Application";
                var message = "It's not you, it's me! Unfortuantely there is an error connecting with the Clique Service";
                errorDialog(title, message);
            }
        }

        private void getSearchResults(string JSON)
        {
            //updates the GUI with the received web data
            var viewModel = new VenueDataSource(JSON);
            this.DataContext = viewModel;
            searchProgressRing.IsActive = false;

        }

        void gridviewVenues_SelectionClicked(object sender, ItemClickEventArgs e)
        {
            //getting the venue ID and sending the user to the detailed venue page
            Venue item = e.ClickedItem as Venue;
            string itemID = item.ID;
            string itemName = item.Name;

            CreateWindow(itemID, itemName);


        }

        private async void CreateWindow(object qStringID, object qStringName)
        {
            int minWidth = 400;
            int minHeight = 400;
            string page = "VenuePage";
            await WindowingHelper.CreateNewWindow(qStringID, qStringName, minWidth, minHeight, page);
        }

        private async void errorDialog(string title, string messageDetails)
        {
            object sender = null;
            string message = messageDetails;
            int commands = 1;
            Dialog.standardDialog(title, message, commands, sender);

        }

    }
}
