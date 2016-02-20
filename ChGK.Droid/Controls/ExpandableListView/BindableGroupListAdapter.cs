using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Android.Content;
using Android.Views;
using Android.Widget;
using Java.Lang;
using MvvmCross.Binding.Droid.Views;

namespace ChGK.Droid.Controls
{
    public class BindableGroupListAdapter : MvxAdapter
    {
        private int _groupTemplateId;
        private IEnumerable _itemsSource;

        public BindableGroupListAdapter(Context context) : base(context)
        {
        }

        public int GroupTemplateId
        {
            get { return _groupTemplateId; }
            set
            {
                if (_groupTemplateId == value)
                    return;
                _groupTemplateId = value;

                // since the template has changed then let's force the list to redisplay by firing NotifyDataSetChanged()
                if (ItemsSource != null)
                    NotifyDataSetChanged();
            }
        }

        private new void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            FlattenAndSetSource(_itemsSource);
        }

        private void FlattenAndSetSource(IEnumerable value)
        {
            var list = value.Cast<object>();
            var flattened = list.SelectMany(FlattenHierachy);
            base.SetItemsSource(flattened.ToList());
        }

        protected override void SetItemsSource(IEnumerable value)
        {
            if (_itemsSource == value)
                return;
            var existingObservable = _itemsSource as INotifyCollectionChanged;
            if (existingObservable != null)
                existingObservable.CollectionChanged -= OnItemsSourceCollectionChanged;

            _itemsSource = value;

            var newObservable = _itemsSource as INotifyCollectionChanged;
            if (newObservable != null)
                newObservable.CollectionChanged += OnItemsSourceCollectionChanged;

            if (value != null)
            {
                FlattenAndSetSource(value);
            }
            else
                base.SetItemsSource(null);
        }

        private IEnumerable<object> FlattenHierachy(object group)
        {
            yield return new FlatItem {IsGroup = true, Item = group};
            var items = group as IEnumerable;
            if (items != null)
                foreach (var d in items)
                    yield return new FlatItem {IsGroup = false, Item = d};
        }

        protected override View GetBindableView(View convertView, object source, int templateId)
        {
            var item = (FlatItem) source;
            if (item.IsGroup)
                return base.GetBindableView(convertView, item.Item, GroupTemplateId);
            return base.GetBindableView(convertView, item.Item, ItemTemplateId);
        }

        public class FlatItem
        {
            public bool IsGroup;
            public object Item;
        }
    }

    public class BindableExpandableListAdapter : MvxAdapter, IExpandableListAdapter
    {
        private int _groupTemplateId;
        private IList _itemsSource;

        public BindableExpandableListAdapter(Context context)
            : base(context)
        {
        }

        public int GroupTemplateId
        {
            get { return _groupTemplateId; }
            set
            {
                if (_groupTemplateId == value)
                    return;
                _groupTemplateId = value;

                // since the template has changed then let's force the list to redisplay by firing NotifyDataSetChanged()
                if (ItemsSource != null)
                    NotifyDataSetChanged();
            }
        }

        public int GroupCount => (_itemsSource != null ? _itemsSource.Count : 0);

        public void OnGroupExpanded(int groupPosition)
        {
            // do nothing
        }

        public void OnGroupCollapsed(int groupPosition)
        {
            // do nothing
        }

        public bool IsChildSelectable(int groupPosition, int childPosition)
        {
            return true;
        }

        public View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            var item = _itemsSource[groupPosition];
            return GetBindableView(convertView, item, GroupTemplateId);
        }

        public long GetGroupId(int groupPosition)
        {
            return groupPosition;
        }

        public Object GetGroup(int groupPosition)
        {
            return null;
        }

        public long GetCombinedGroupId(long groupId)
        {
            return groupId;
        }

        public long GetCombinedChildId(long groupId, long childId)
        {
            return childId;
        }

        public View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView,
            ViewGroup parent)
        {
            var sublist = ((_itemsSource[groupPosition]) as IEnumerable)?.Cast<object>().ToList();

            var item = sublist?[childPosition];
            return GetBindableView(convertView, item, ItemTemplateId);
        }

        public int GetChildrenCount(int groupPosition)
        {
            return (((_itemsSource[groupPosition]) as IEnumerable)?.Cast<object>().ToList().Count).GetValueOrDefault();
        }

        public long GetChildId(int groupPosition, int childPosition)
        {
            return childPosition;
        }

        public Object GetChild(int groupPosition, int childPosition)
        {
            return null;
        }

        protected override void SetItemsSource(IEnumerable value)
        {
            MvvmCross.Platform.Mvx.Trace("Setting itemssource");
            if (_itemsSource == value)
                return;
            var existingObservable = _itemsSource as INotifyCollectionChanged;
            if (existingObservable != null)
                existingObservable.CollectionChanged -= OnItemsSourceCollectionChanged;

            _itemsSource = value as IList;

            var newObservable = _itemsSource as INotifyCollectionChanged;
            if (newObservable != null)
                newObservable.CollectionChanged += OnItemsSourceCollectionChanged;

            base.SetItemsSource(_itemsSource);
        }

        public object GetRawItem(int groupPosition, int position)
        {
            return ((_itemsSource[groupPosition]) as IEnumerable)?.Cast<object>().ToList()[position];
        }
    }
}