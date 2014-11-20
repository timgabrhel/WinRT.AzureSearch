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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
using AdventureWorksSearch.ViewModels;

namespace AdventureWorksSearch
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Flyout sortFlyout;
        private Flyout filterFlyout;

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
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private void AppBarSort_Click(object sender, RoutedEventArgs e)
        {
            // Ensure we have an app bar
            if (BottomAppBar == null) return;

            // Get the button just clicked
            var sortButton = sender as AppBarButton;
            if (sortButton == null) return;

            // Get the attached flyout
            sortFlyout = (Flyout)Resources["SortFlyout"];
            if (sortFlyout == null) return;

            // Close the app bar before opening the flyout
            sortFlyout.Opening += delegate(object o, object o1)
            {
                if (BottomAppBar != null && BottomAppBar.Visibility == Visibility.Visible)
                {
                    BottomAppBar.Visibility = Visibility.Collapsed;
                }
            };

            // Show the app bar after the flyout closes
            sortFlyout.Closed += delegate(object o, object o1)
            {
                if (BottomAppBar != null && BottomAppBar.Visibility == Visibility.Collapsed)
                {
                    BottomAppBar.Visibility = Visibility.Visible;
                }
            };

            var grid = sortFlyout.Content as Grid;
            if (grid == null) return;
            grid.Tapped += delegate(object o, TappedRoutedEventArgs args)
            {
                var transparentGrid = args.OriginalSource as Grid;
                if (transparentGrid != null)
                {
                    sortFlyout.Hide();
                }
            };

            // Use the ShowAt() method on the flyout to specify where exactly the flyout should be located
            sortFlyout.ShowAt(BottomAppBar);
        }

        private void AppBarFilter_Click(object sender, RoutedEventArgs e)
        {
            // Ensure we have an app bar
            if (BottomAppBar == null) return;

            // Get the button just clicked
            var filterButton = sender as AppBarButton;
            if (filterButton == null) return;

            // Get the attached flyout
            filterFlyout = (Flyout)Resources["FilterFlyout"];
            if (filterFlyout == null) return;

            // Close the app bar before opening the flyout
            filterFlyout.Opening += delegate(object o, object o1)
            {
                if (BottomAppBar != null && BottomAppBar.Visibility == Visibility.Visible)
                {
                    BottomAppBar.Visibility = Visibility.Collapsed;
                }
            };

            // Show the app bar after the flyout closes
            filterFlyout.Closed += delegate(object o, object o1)
            {
                if (BottomAppBar != null && BottomAppBar.Visibility == Visibility.Collapsed)
                {
                    BottomAppBar.Visibility = Visibility.Visible;
                }

                (DataContext as MainViewModel).SearchCommand.Execute(null);
            };

            // Use the ShowAt() method on the flyout to specify where exactly the flyout should be located
            filterFlyout.ShowAt(BottomAppBar);
        }

        private void Sort_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            var mainViewModel = DataContext as MainViewModel;
            if (mainViewModel == null) return;

            mainViewModel.SortCommand.Execute(button.Tag);

            if (sortFlyout != null)
            {
                sortFlyout.Hide();
            }
        }
    }
}
