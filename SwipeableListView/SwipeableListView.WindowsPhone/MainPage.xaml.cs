using SwipeableListView.Controls;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SwipeableListView
{
    public sealed partial class MainPage : Page
    {
        MainPageViewModel viewModel = new MainPageViewModel();

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.DataContext = viewModel;
        }

        private void DemoSwipeableListView_LeftSwiped(object sender, System.EventArgs e)
        {
            var sourceItem = sender as SwipeableListViewItem;
            if (sourceItem != null && viewModel.DemoItemsSource != null && viewModel.DemoItemsSource.Contains(sourceItem.Content as string))
            {
                string sourceItemContent = sourceItem.Content as string;
                var itemIndex = viewModel.DemoItemsSource.IndexOf(sourceItemContent);
                viewModel.DemoItemsSource.Insert(itemIndex, sourceItemContent);
            }
        }
    }
}
