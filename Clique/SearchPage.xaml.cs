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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Clique
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchPage : Page
    {

        public SearchPage()
        {
            this.InitializeComponent();

        }

        private void getButton_Event(object sender, RoutedEventArgs e)
        {
            var toGet = mySearchBox.Text;
            var varToGet = "search";
            var target = "http://kshatriya.co.uk/dev/php/test_case/search/";

            createURI(target, varToGet, toGet);

        }

        private async void createURI(string target, string qsParameter, string qsValue)
        {
            searchProgressRing.IsActive = true;

            /*
            var uri = UriExtensions.CreateUriWithQuery(new Uri(target),
            new NameValueCollection { { qsParameter, qsValue } });

            */

            target = target + qsValue;

            var uri = new Uri(target);

            HttpClient httpClient = new HttpClient();
            var response = httpClient.GetAsync(uri).Result;

            var responseString = "";

            if (response.IsSuccessStatusCode)
            {
                responseString = await response.Content.ReadAsStringAsync();
                getSearchResults(responseString);
            }
            else
            {
                searchProgressRing.IsActive = false;
                var title = "Awww, come on Man! No search results!";
                var message = "How you uh, how you comin' on that novel you're working on? Huh? Gotta a big, uh, big stack of papers there? Gotta, gotta nice little story you're working on there? Your big novel you've been working on for 3 years? Huh? Gotta, gotta compelling protagonist? Yeah? Gotta obstacle for him to overcome? Huh? Gotta story brewing there? Working on, working on that for quite some time? Huh? (voice getting higher pitched) Yea, talking about that 3 years ago. Been working on that the whole time? Nice little narrative? Beginning, middle, and end? Some friends become enemies, some enemies become friends? At the end your main character is richer from the experience? Yeah? Yeah? No, no, you deserve some time off.";
                errorDialog(title, message);
            }

        }
        private void getSearchResults(string JSON)
        {
            /*List<Venue> myVenues = JsonConvert.DeserializeObject<List<Venue>>(JSON);
            string content = String.Empty;

            foreach (var venue in myVenues)
            {
                content += String.Format("Name: {0}, Description: {1}, Venue Type: {2},  Recommendations: {3}, Added by: {4}, ID: {5}", venue.Name, venue.Description, venue.Type, venue.Recommended, venue.Owner, venue.ID);
            }*/


            var viewModel = new VenueDataSource(JSON);
            this.DataContext = viewModel;
            searchProgressRing.IsActive = false;

            //jsonParsedTextBlock.Text = content;
        }

        private void Type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var toGet = ((ComboBoxItem)typeComboBox.SelectedItem).Content.ToString(); ;
            var varToGet = "search";
            var target = "http://kshatriya.co.uk/dev/php/test_case/search/";
            createURI(target, varToGet, toGet);
        }

        //MVVM Gridview Selection controls
        void gridviewVenues_SelectionClicked(object sender, ItemClickEventArgs e)
        {

            Venue item = e.ClickedItem as Venue;
            string itemID = item.ID;
            string itemName = item.Name;

            CreateWindow(itemID, itemName);

            /*
            var PressOn = e.OriginalSource as FrameworkElement;
            if (PressOn != null)
            {
                string elementName = PressOn.Name;
                VenuePage dPage = new VenuePage();
                dPage.selectedVenue = elementName;
                Window.Current.Content = dPage;
                CreateWindow(elementName);
            }
            */

            /*
            var selectedVenue = itemGridView.SelectedItem;
            VenuePage dPage = new VenuePage();
            dPage.selectedVenue = selectedVenue;
            Window.Current.Content = dPage;
            
            CreateWindow();

             */

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
