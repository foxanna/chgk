using Android.Widget;
using Android.Content;
using Android.Util;
using System.Collections;
using System.Windows.Input;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Binding.Droid.ResourceHelpers;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Attributes;

namespace ChGK.Droid.Controls
{
	public class BindableGroupListView : MvxListView
	{
		public BindableGroupListView (Context context, IAttributeSet attrs)
			: this (context, attrs, new BindableGroupListAdapter (context))
		{
		}

		public BindableGroupListView (Context context, IAttributeSet attrs, BindableGroupListAdapter adapter)
			: base (context, attrs, adapter)
		{
			var groupTemplateId = MvxAttributeHelpers.ReadAttributeValue (context, attrs,
				                      MvxAndroidBindingResource.Instance
				.ListViewStylableGroupId,
				                      AndroidBindingResource.Instance
				.BindableListGroupItemTemplateId);
			adapter.GroupTemplateId = groupTemplateId;
		}

		public ICommand GroupClick { get; set; }

		protected override void ExecuteCommandOnItem (ICommand command, int position)
		{
			var item = Adapter.GetRawItem (position);
			if (item == null)
				return;
			var flatItem = (BindableGroupListAdapter.FlatItem)item;

			if (flatItem.IsGroup)
				command = GroupClick;

			if (command == null)
				return;

			if (!command.CanExecute (flatItem.Item))
				return;

			command.Execute (flatItem.Item);
		}
	}

	public class BindableExpandableListView : ExpandableListView
	{
		public BindableExpandableListView (Context context, IAttributeSet attrs)
			: base (context, attrs)
		{
            var expandableAdapter = new BindableExpandableListAdapter(context);
			
            var groupTemplateId = MvxAttributeHelpers.ReadAttributeValue (context, attrs,
				                      MvxAndroidBindingResource.Instance
				.ListViewStylableGroupId,
				                      AndroidBindingResource.Instance
				.BindableListGroupItemTemplateId);

			var itemTemplateId = MvxAttributeHelpers.ReadListItemTemplateId (context, attrs);
            
            expandableAdapter.GroupTemplateId = groupTemplateId;
            expandableAdapter.ItemTemplateId = itemTemplateId;

            SetAdapter(expandableAdapter);

            InitHeaders(context, attrs);
            InitFooters(context, attrs);            
		}

        void InitFooters(Context context, IAttributeSet attrs)
        {
            var footerId = MvxAttributeHelpers.ReadAttributeValue(context, attrs, MvxAndroidBindingResource.Instance.ListViewStylableGroupId,
                            AndroidBindingResource.Instance.MvxListViewWithHeader_FooterLayout);

            if (footerId != 0)
            {
                var bindingContext = MvxAndroidBindingContextHelpers.Current();
                var view = bindingContext.BindingInflate(footerId, null);

                AddFooterView(view, null, false);
            }
        }

        void InitHeaders(Context context, IAttributeSet attrs)
        {
            var headerId = MvxAttributeHelpers.ReadAttributeValue(context, attrs, MvxAndroidBindingResource.Instance.ListViewStylableGroupId,
                AndroidBindingResource.Instance.MvxListViewWithHeader_HeaderLayout);

            if (headerId != 0)
            {
                var bindingContext = MvxAndroidBindingContextHelpers.Current();
                var view = bindingContext.BindingInflate(headerId, null);

                AddHeaderView(view, null, false);
            }
        }

		// An expandableListView has ExpandableListAdapter as propertyname, but Adapter still exists but is always null.
        protected BindableExpandableListAdapter ThisAdapter
        {
            get { return ExpandableListAdapter as BindableExpandableListAdapter; }
		}

		[MvxSetToNullAfterBinding]
		public virtual IEnumerable ItemsSource {
			get { return ThisAdapter.ItemsSource; }
			set { ThisAdapter.ItemsSource = value; }
		}

		public int ItemTemplateId {
			get { return ThisAdapter.ItemTemplateId; }
			set { ThisAdapter.ItemTemplateId = value; }
		}

		private ICommand _itemClick;

		public new ICommand ItemClick {
			get { return _itemClick; }
			set {
				_itemClick = value;
				if (_itemClick != null)
					EnsureItemClickOverloaded ();
			}
		}

		//		public ICommand GroupClick { get; set; }

		private bool _itemClickOverloaded = false;

		private void EnsureItemClickOverloaded ()
		{
			if (_itemClickOverloaded)
				return;

			_itemClickOverloaded = true;
			base.ChildClick +=
				(sender, args) => ExecuteCommandOnItem (this.ItemClick, args.GroupPosition, args.ChildPosition);
		}

		protected virtual void ExecuteCommandOnItem (ICommand command, int groupPosition, int position)
		{
			if (command == null)
				return;

			var item = ThisAdapter.GetRawItem (groupPosition, position);
			if (item == null)
				return;

			if (!command.CanExecute (item))
				return;

			command.Execute (item);
		}
	}
}

