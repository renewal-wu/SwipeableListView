using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SwipeableListView.Controls
{
    public class SwipeableListViewItem : ListViewItem
    {
        public static readonly DependencyProperty LeftSwipeContentProperty =
            DependencyProperty.Register("LeftSwipeContent", typeof(object), typeof(SwipeableListViewItem), new PropertyMetadata(null));

        /// <summary>
        /// 左滑的 Content
        /// </summary>
        public object LeftSwipeContent
        {
            get { return GetValue(LeftSwipeContentProperty); }
            set { SetValue(LeftSwipeContentProperty, value); }
        }

        public static readonly DependencyProperty LeftSwipeContentTemplateProperty =
            DependencyProperty.Register("LeftSwipeContentTemplate", typeof(DataTemplate), typeof(SwipeableListViewItem), new PropertyMetadata(null));

        /// <summary>
        /// 左滑的 ContentTemplate
        /// </summary>
        public DataTemplate LeftSwipeContentTemplate
        {
            get { return (DataTemplate)GetValue(LeftSwipeContentTemplateProperty); }
            set { SetValue(LeftSwipeContentTemplateProperty, value); }
        }

        public static readonly DependencyProperty RightSwipeContentProperty =
            DependencyProperty.Register("RightSwipeContent", typeof(object), typeof(SwipeableListViewItem), new PropertyMetadata(null));

        /// <summary>
        /// 右滑的 Content
        /// </summary>
        public object RightSwipeContent
        {
            get { return GetValue(RightSwipeContentProperty); }
            set { SetValue(RightSwipeContentProperty, value); }
        }

        public static readonly DependencyProperty RightSwipeContentTemplateProperty =
            DependencyProperty.Register("RightSwipeContentTemplate", typeof(DataTemplate), typeof(SwipeableListViewItem), new PropertyMetadata(null));

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

        private ContentPresenter LeftContentPresenter { get; set; }
        private ContentPresenter RightContentPresenter { get; set; }

        private static double swipeValidOpacityLimit = 0.8;
        private static double validTranslateXLimit = 20;
        private double SwipeTranslateXDenominator
        {
            get
            {
                return ActualWidth / 3;
            }
        }

        public SwipeableListViewItem()
        {
            this.DefaultStyleKey = typeof(SwipeableListViewItem);
            bindingSwipeItem();
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            LeftContentPresenter = base.GetTemplateChild("LeftContentPresenter") as ContentPresenter;
            RightContentPresenter = base.GetTemplateChild("RightContentPresenter") as ContentPresenter;
        }

        private void bindingSwipeItem()
        {
            ManipulationDelta += ListViewItem_ManipulationDelta;
            ManipulationCompleted += ListViewItem_ManipulationCompleted;
        }

        private void ListViewItem_ManipulationDelta(object sender, Windows.UI.Xaml.Input.ManipulationDeltaRoutedEventArgs e)
        {
            SwipeableListViewItem listViewItem = sender as SwipeableListViewItem;
            if (listViewItem != null)
            {
                double listViewItemTotalWidth = listViewItem.ActualWidth;
                var movementPercentageDenominator = this.SwipeTranslateXDenominator;

                double movementY = Math.Abs(e.Cumulative.Translation.Y);
                double movementX = e.Cumulative.Translation.X;
                double movementPercentage = Math.Min(1.0, Math.Abs(movementX) / movementPercentageDenominator);

                Debug.WriteLine("movement Y: " + movementY.ToString());

                if (e.IsInertial == false)
                {
                    if (movementX >= validTranslateXLimit)
                    {
                        SetVisualState(SwipeableListViewItemMode.RightSwipeMode);
                        HideLeftContentPresenter();
                        SetRightContentPresenterOpacity(movementPercentage);
                        e.Handled = true;
                    }
                    else if (movementX <= -(validTranslateXLimit))
                    {
                        SetVisualState(SwipeableListViewItemMode.LeftSwipeMode);
                        HideRightContentPresenter();
                        SetLeftContentPresenterOpacity(movementPercentage);
                        e.Handled = true;
                    }
                    else
                    {
                        SetVisualState(SwipeableListViewItemMode.NoSwipe);
                        HideLeftContentPresenter();
                        HideRightContentPresenter();
                    }
                }
            }
        }

        private void ListViewItem_ManipulationCompleted(object sender, Windows.UI.Xaml.Input.ManipulationCompletedRoutedEventArgs e)
        {
            SwipeableListViewItem listViewItem = sender as SwipeableListViewItem;
            if (listViewItem != null)
            {
                listViewItem.CheckSwipeStatus();
                listViewItem.HideLeftContentPresenter();
                listViewItem.HideRightContentPresenter();
            }
        }

        private void HideLeftContentPresenter()
        {
            SetLeftContentPresenterOpacity(0.0);
        }

        private void HideRightContentPresenter()
        {
            SetRightContentPresenterOpacity(0.0);
        }

        private void SetLeftContentPresenterOpacity(double targetOpacity)
        {
            if (LeftContentPresenter != null)
            {
                SetUIElementOpacity(LeftContentPresenter, targetOpacity);
                LeftContentPresenter.Tag = targetOpacity >= swipeValidOpacityLimit;
            }
        }

        private void SetRightContentPresenterOpacity(double targetOpacity)
        {
            if (RightContentPresenter != null)
            {
                SetUIElementOpacity(RightContentPresenter, targetOpacity);
                RightContentPresenter.Tag = targetOpacity >= swipeValidOpacityLimit;
            }
        }

        private void SetUIElementOpacity(UIElement targetElement, double targetOpacity)
        {
            targetElement.Opacity = targetOpacity;
        }

        private void CheckSwipeStatus()
        {
            if (LeftContentPresenter != null && LeftContentPresenter.Tag is bool && (bool)LeftContentPresenter.Tag == true)
            {
                this.IsHitTestVisible = false;

                if (LeftSwiped != null)
                {
                    LeftSwiped(this, EventArgs.Empty);
                }

                this.IsHitTestVisible = true;
            }
            else if (RightContentPresenter != null && RightContentPresenter.Tag is bool && (bool)RightContentPresenter.Tag == true)
            {
                this.IsHitTestVisible = false;

                if (RightSwiped != null)
                {
                    RightSwiped(this, EventArgs.Empty);
                }

                this.IsHitTestVisible = true;
            }
        }

        private void SetVisualState(SwipeableListViewItemMode mode)
        {
            VisualStateManager.GoToState(this, mode.ToString(), true);
        }
    }
}
