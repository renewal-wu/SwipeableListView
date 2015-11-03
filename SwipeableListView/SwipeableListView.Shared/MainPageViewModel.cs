using System;
using System.Collections.ObjectModel;

namespace SwipeableListView
{
    public class MainPageViewModel
    {
        public ObservableCollection<string> DemoItemsSource { get; set; }

        public MainPageViewModel()
        {
            DemoItemsSource = new ObservableCollection<string>();

            for (int i = 0; i < 100; i++)
            {
                DemoItemsSource.Add("Demo string " + i.ToString());
            }
        }
    }
}
