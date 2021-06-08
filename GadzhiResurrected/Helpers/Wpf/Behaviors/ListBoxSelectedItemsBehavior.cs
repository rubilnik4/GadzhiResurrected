using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace GadzhiResurrected.Helpers.Wpf.Behaviors
{
    public class ListBoxSelectedItemsBehavior : Behavior<ListBox>
    {
        protected override void OnAttached()
        {
            AssociatedObject.SelectionChanged += AssociatedObjectSelectionChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.SelectionChanged -= AssociatedObjectSelectionChanged;
        }

        private void AssociatedObjectSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var array = new object[AssociatedObject.SelectedItems.Count];
            AssociatedObject.SelectedItems.CopyTo(array, 0);
            SelectedItems = array;
        }

        private static readonly DependencyProperty _selectedItemsProperty =
            DependencyProperty.Register(nameof(SelectedItems), typeof(IList), typeof(ListBoxSelectedItemsBehavior),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public IList SelectedItems
        {
            get => (IList)GetValue(_selectedItemsProperty);
            set => SetValue(_selectedItemsProperty, value);
        }
    }
}
