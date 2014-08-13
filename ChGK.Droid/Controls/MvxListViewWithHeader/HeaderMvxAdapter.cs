using System.Collections.Generic;
using Android.Widget;
using Cirrious.MvvmCross.Binding.Droid.Views;

namespace ChGK.Droid.Controls.MvxListViewWithHeader
{
	//http://blog.masterdevs.com/headers-and-footers-on-an-mvxlistview/
	public class HeaderMvxAdapter : HeaderViewListAdapter, IMvxAdapter
	{
		readonly IMvxAdapter _adapter;

		public HeaderMvxAdapter (IList<ListView.FixedViewInfo> headers, IMvxAdapter adapter)
			: this (headers, new List<ListView.FixedViewInfo> (), adapter)
		{
		}

		public HeaderMvxAdapter (IList<ListView.FixedViewInfo> headers, IList<ListView.FixedViewInfo> footers, IMvxAdapter adapter)
			: base (headers, footers, adapter)
		{
			_adapter = adapter;
		}

		public int DropDownItemTemplateId {
			get { return _adapter.DropDownItemTemplateId; }
			set { _adapter.DropDownItemTemplateId = value; }
		}

		public object GetRawItem (int position)
		{
			return _adapter.GetRawItem (position - HeadersCount);
		}

		public int GetPosition (object value)
		{
			return _adapter.GetPosition (value);
		}

		public int SimpleViewLayoutId {
			get {
				return _adapter.SimpleViewLayoutId;
			}
			set {
				_adapter.SimpleViewLayoutId = value;
			}
		}

		public System.Collections.IEnumerable ItemsSource {
			get {
				return _adapter.ItemsSource;
			}
			set {
				_adapter.ItemsSource = value;
			}
		}

		public int ItemTemplateId {
			get {
				return _adapter.ItemTemplateId;
			}
			set {
				_adapter.ItemTemplateId = value;
			}
		}

		public Android.Views.View GetDropDownView (int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			return _adapter.GetDropDownView (position, convertView, parent);
		}

		public override bool IsEnabled (int position)
		{
            return (position > HeadersCount - 1) ? base.IsEnabled(position) : false;
		}
	}
}

