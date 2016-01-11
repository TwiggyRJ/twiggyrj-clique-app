using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Clique.Assets
{
    public static class WindowingHelper
    {

        public class Payload
        {
            public object qStringID { get; set; }
            public object qStringName{ get; set; }
        }

        public static async Task CreateNewWindow(object qString, int minWidth, int minHeight, string page)
        {
            var newCoreAppView = CoreApplication.CreateNewView();
            var appView = ApplicationView.GetForCurrentView();
            await newCoreAppView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, async () =>
            {
                var window = Window.Current;
                var newAppView = ApplicationView.GetForCurrentView();

#if WINDOWS_UAP
                newAppView.SetPreferredMinSize(new Windows.Foundation.Size(minWidth, minHeight));
#endif
                var frame = new Frame();
                window.Content = frame;
                frame.Navigate(typeof(VenuePage), qString);
                window.Activate();

                await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newAppView.Id, ViewSizePreference.UseMore, appView.Id, ViewSizePreference.Default);

#if WINDOWS_UAP
                var success = newAppView.TryResizeView(new Windows.Foundation.Size(minWidth, minHeight));
#endif
            });
        }

        public static async Task CreateNewWindow(int minWidth, int minHeight, string page)
        {
            var newCoreAppView = CoreApplication.CreateNewView();
            var appView = ApplicationView.GetForCurrentView();
            await newCoreAppView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, async () =>
            {
                var window = Window.Current;
                var newAppView = ApplicationView.GetForCurrentView();

#if WINDOWS_UAP
                newAppView.SetPreferredMinSize(new Windows.Foundation.Size(minWidth, minHeight));
#endif
                var frame = new Frame();
                window.Content = frame;
                if(page == "LoginFormPage")
                {
                    frame.Navigate(typeof(LoginFormPage));
                }
                window.Activate();

                await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newAppView.Id, ViewSizePreference.UseMore, appView.Id, ViewSizePreference.Default);

#if WINDOWS_UAP
                var success = newAppView.TryResizeView(new Windows.Foundation.Size(minWidth, minHeight));
#endif
            });
        }

        public static async Task CreateNewWindow(object qStringID, object qStringName, int minWidth, int minHeight, string page)
        {
            var newCoreAppView = CoreApplication.CreateNewView();
            var appView = ApplicationView.GetForCurrentView();

            Payload payload = new Payload();
            payload.qStringID = qStringID;
            payload.qStringName = qStringName;

            await newCoreAppView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, async () =>
            {
                var window = Window.Current;
                var newAppView = ApplicationView.GetForCurrentView();


#if WINDOWS_UAP
                newAppView.SetPreferredMinSize(new Windows.Foundation.Size(minWidth, minHeight));
#endif
                var frame = new Frame();
                window.Content = frame;
                frame.Navigate(typeof(VenuePage), payload);
                window.Activate();

                await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newAppView.Id, ViewSizePreference.UseMore, appView.Id, ViewSizePreference.Default);

#if WINDOWS_UAP
                var success = newAppView.TryResizeView(new Windows.Foundation.Size(minWidth, minHeight));
#endif
            });
        }
    }
}
