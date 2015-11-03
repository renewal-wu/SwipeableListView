using System;
using System.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace SwipeableListView.Controls
{
    public class SwipeableListView : ListView
    {
        public static readonly DependencyProperty LeftSwipeContentProperty =
            DependencyProperty.Register("LeftSwipeContent", typeof(object), typeof(SwipeableListView), new PropertyMetadata(null));

        /// <summary>
        /// 左滑的 Content
        /// </summary>
        public object LeftSwipeContent
        {
            get { return GetValue(LeftSwipeContentProperty); }
            set { SetValue(LeftSwipeContentProperty, value); }
        }

        public static readonly DependencyProperty LeftSwipeContentTemplateProperty =
            DependencyProperty.Register("LeftSwipeContentTemplate", typeof(DataTemplate), typeof(SwipeableListView), new PropertyMetadata(null));

        /// <summary>
        /// 左滑的 ContentTemplate
        /// </summary>
        public DataTemplate LeftSwipeContentTemplate
        {
            get { return (DataTemplate)GetValue(LeftSwipeContentTemplateProperty); }
            set { SetValue(LeftSwipeContentTemplateProperty, value); }
        }

        public static readonly DependencyProperty RightSwipeContentProperty =
            DependencyProperty.Register("RightSwipeContent", typeof(object), typeof(SwipeableListView), new PropertyMetadata(null));

        /// <summary>
        /// 右滑的 Content
        /// </summary>
        public object RightSwipeContent
        {
            get { return GetValue(RightSwipeContentProperty); }
            set { SetValue(RightSwipeContentProperty, value); }
        }

        public static readonly DependencyProperty RightSwipeContentTemplateProperty =
            DependencyProperty.Register("RightSwipeContentTemplate", typeof(DataTemplate), typeof(SwipeableListView), new PropertyMetadata(null));

        /// <summary>
        /// 右滑的 ContentTemplate
        /// </summary>
        public DataTemplate RightSwipeContentTemplate
        {
            get { return (DataTemplate)GetValue(RightSwipeContentTemplateProperty); }
            set { SetValue(RightSwipeContentTemplateProperty, value); }
        }

        public event EventHandler LeftSwiped;
        public event EventHandler RightSwiped;

        private SolidColorBrush whiteBrush = new SolidColorBrush(Colors.White);
        private ScrollViewer scrollViewer;
        private double verticalTotal = 0;

        public SwipeableListView()
        {
            this.DefaultStyleKey = typeof(ListView);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            scrollViewer = base.GetTemplateChild("ScrollViewer") as ScrollViewer;
            if (scrollViewer != null)
            {
                scrollViewer.CancelDirectManipulations();
            }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            SwipeableListViewItem ListViewItem = new SwipeableListViewItem()
            {
                LeftSwipeContent = LeftSwipeContent,
                LeftSwipeContentTemplate = LeftSwipeContentTemplate,
                RightSwipeContent = RightSwipeContent,
                RightSwipeContentTemplate = RightSwipeContentTemplate
            };

            return ListViewItem;
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            bindingSwipeItem(element);
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);
            unbindingSwipeItem(element);
        }

        private void bindingSwipeItem(DependencyObject element)
        {
            SwipeableListViewItem listViewItem = element as SwipeableListViewItem;
            if (listViewItem != null)
            {
                listViewItem.LeftSwiped += ListViewItem_LeftSwiped;
                listViewItem.RightSwiped += ListViewItem_RightSwiped;
            }
        }

        private void unbindingSwipeItem(DependencyObject element)
        {
            SwipeableListViewItem listViewItem = element as SwipeableListViewItem;
            if (listViewItem != null)
            {
                listViewItem.LeftSwiped -= ListViewItem_LeftSwiped;
                listViewItem.RightSwiped -= ListViewItem_RightSwiped;
            }
        }

        private void ListViewItem_LeftSwiped(object sender, EventArgs e)
        {
            if (LeftSwiped != null)
            {
                LeftSwiped(sender, e);
            }
        }

        private void ListViewItem_RightSwiped(object sender, EventArgs e)
        {
            var sourceListViewItem = sender as SwipeableListViewItem;
            if (sourceListViewItem != null)
            {
                var targetRemoveItem = this.ItemFromContainer(sourceListViewItem);
                RemoveItem(targetRemoveItem);
            }

            if (RightSwiped != null)
            {
                RightSwiped(sender, e);
            }
        }

        private void RemoveItem(object sourceItem)
        {
            if (sourceItem != null)
            {
                IList currentItems = this.ItemsSource as IList;
                if (currentItems != null)
                {
                    currentItems.Remove(sourceItem);
                }
            }
        }
    }
}
