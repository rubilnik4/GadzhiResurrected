using GadzhiModules.Modules.FilesConvertModule.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace GadzhiModules.Helpers.Wpf.Behaviors
{
    
    //public class DataGridMultiSelectionBehavior : Behavior<DataGrid>
    //{
    //    protected override void OnAttached()
    //    {
    //        base.OnAttached();
    //        if (SelectedItems != null)
    //        {
    //            AssociatedObject.SelectedItems.Clear();
    //            foreach (var item in SelectedItems)
    //            {
    //                AssociatedObject.SelectedItems.Add(item);
    //            }
    //        }
    //    }

    //    public IList SelectedItems
    //    {
    //        get { return (IList)GetValue(SelectedItemsProperty); }
    //        set { SetValue(SelectedItemsProperty, value); }
    //    }

    //    public static readonly DependencyProperty SelectedItemsProperty =
    //        DependencyProperty.Register("SelectedItems", typeof(IList), typeof(DataGridMultiSelectionBehavior), new UIPropertyMetadata(null, SelectedItemsChanged));

    //    private static void SelectedItemsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    //    {
    //        var behavior = o as DataGridMultiSelectionBehavior;
    //        if (behavior == null)
    //            return;

    //        var oldValue = e.OldValue as INotifyCollectionChanged;
    //        var newValue = e.NewValue as INotifyCollectionChanged;

    //        if (oldValue != null)
    //        {
    //            oldValue.CollectionChanged -= behavior.SourceCollectionChanged;
    //            behavior.AssociatedObject.SelectionChanged -= behavior.DataGridSelectionChanged;
    //        }
    //        if (newValue != null)
    //        {
    //            behavior.AssociatedObject?.SelectedItems.Clear();
    //            foreach (var item in (IEnumerable)newValue)
    //            {
    //                behavior.AssociatedObject.SelectedItems.Add(item);
    //            }
    //            if (behavior.AssociatedObject != null)
    //            {
    //                behavior.AssociatedObject.SelectionChanged += behavior.DataGridSelectionChanged;
    //            }

    //            newValue.CollectionChanged += behavior.SourceCollectionChanged;
    //        }
    //    }

    //    private bool _isUpdatingTarget;
    //    private bool _isUpdatingSource;

    //    void SourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    //    {
    //        if (_isUpdatingSource)
    //            return;

    //        try
    //        {
    //            _isUpdatingTarget = true;

    //            if (e.OldItems != null)
    //            {
    //                foreach (var item in e.OldItems)
    //                {
    //                    AssociatedObject.SelectedItems.Remove(item);
    //                }
    //            }

    //            if (e.NewItems != null)
    //            {
    //                foreach (var item in e.NewItems)
    //                {
    //                    AssociatedObject.SelectedItems.Add(item);
    //                }
    //            }

    //            if (e.Action == NotifyCollectionChangedAction.Reset)
    //            {
    //                AssociatedObject.SelectedItems.Clear();
    //            }
    //        }
    //        finally
    //        {
    //            _isUpdatingTarget = false;
    //        }
    //    }

    //    private void DataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
    //    {
    //        if (_isUpdatingTarget)
    //            return;

    //        var selectedItems = this.SelectedItems;
    //        if (selectedItems == null)
    //            return;

    //        try
    //        {
    //            _isUpdatingSource = true;

    //            foreach (var item in e.RemovedItems)
    //            {
    //                selectedItems.Remove(item);
    //            }

    //            foreach (var item in e.AddedItems)
    //            {
    //                selectedItems.Add(item);
    //            }
    //        }
    //        finally
    //        {
    //            _isUpdatingSource = false;
    //        }
    //    }

    //}

    //public class DataGridSelectedItemsBlendBehavior : Behavior<DataGrid>
    //{

    //    public static readonly DependencyProperty SelectedItemProperty =
    //        DependencyProperty.Register("SelectedItems", typeof(ObservableCollection<FileData>),
    //        typeof(DataGridSelectedItemsBlendBehavior),
    //        new FrameworkPropertyMetadata(null)
    //        {
    //            BindsTwoWayByDefault = true
    //        });

    //    public ObservableCollection<FileData> SelectedItems
    //    {
    //        get
    //        {
    //            return (ObservableCollection<FileData>)GetValue(SelectedItemProperty);
    //        }
    //        set
    //        {
    //            SetValue(SelectedItemProperty, value);
    //        }
    //    }

    //    protected override void OnAttached()
    //    {
    //        base.OnAttached();
    //        this.AssociatedObject.SelectionChanged += OnSelectionChanged;
    //        this.AssociatedObject.Loaded += AssociatedObject_Loaded;
    //    }

    //    void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
    //    {
    //        if (this.SelectedItems != null)
    //        {
    //            var selectedItems = this.SelectedItems.ToList();
    //            foreach (var obj in selectedItems)
    //            {
    //                var rowContainer = this.AssociatedObject.ItemContainerGenerator.ContainerFromItem(obj) as DataGridRow;
    //                if (rowContainer != null)
    //                {
    //                    rowContainer.IsSelected = true;
    //                }
    //            }
    //        }
    //    }

    //    protected override void OnDetaching()
    //    {
    //        base.OnDetaching();
    //        if (this.AssociatedObject != null)
    //        {
    //            this.AssociatedObject.SelectionChanged -= OnSelectionChanged;
    //            this.AssociatedObject.Loaded -= AssociatedObject_Loaded;
    //        }
    //    }

    //    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    //    {

    //        if (e.AddedItems != null && e.AddedItems.Count > 0 && this.SelectedItems != null)
    //        {
    //            foreach (FileData obj in e.AddedItems)
    //                this.SelectedItems.Add(obj);
    //        }

    //        if (e.RemovedItems != null && e.RemovedItems.Count > 0 && this.SelectedItems != null)
    //        {
    //            foreach (FileData obj in e.RemovedItems)
    //                this.SelectedItems.Remove(obj);
    //        }
    //    }
    //}
}
