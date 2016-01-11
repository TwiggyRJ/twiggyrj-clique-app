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
    public sealed partial class AddVenuePage : Page
    {
        public AddVenuePage()
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


            }

        }
    }
}
