using System;
using System.Collections.Generic;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.Binding.Droid.ResourceHelpers;

namespace ChGK.Droid.Controls.MvxListViewWithHeader
{
	public class MvxListViewWithHeader : MvxListView
	{
		public MvxListViewWithHeader (Context context, IAttributeSet attrs) : base (context, attrs, null)
		{
			ApplyAttributes (context, attrs);

			var headers = GetHeaders ();
			var footers = GetFooters ();

			IMvxAdapter adapter = new MvxAdapter (context);

			var itemTemplateId = MvxAttributeHelpers.ReadListItemTemplateId (context, attrs);
			adapter.ItemTemplateId = itemTemplateId;

			IMvxAdapter headerAdapter = new HeaderMvxAdapter (headers, footers, adapter);

			Adapter = headerAdapter;
		}

		int _headerId, _footerId;

		void ApplyAttributes (Context c, IAttributeSet attrs)
		{
			_headerId = MvxAttributeHelpers.ReadAttributeValue (c, attrs, MvxAndroidBindingResource.Instance.ListViewStylableGroupId, 
				AndroidBindingResource.Instance.MvxListViewWithHeader_HeaderLayout);
			_footerId = MvxAttributeHelpers.ReadAttributeValue (c, attrs, MvxAndroidBindingResource.Instance.ListViewStylableGroupId, 
				AndroidBindingResource.Instance.MvxListViewWithHeader_FooterLayout);
		}

		View GetBoundView (int id)
		{
			if (id == 0)
				return null;

			IMvxAndroidBindingContext bindingContext = MvxAndroidBindingContextHelpers.Current ();
			var view = bindingContext.BindingInflate (id, null);

			return view;
		}

		IList<ListView.FixedViewInfo> GetFixedViewInfos (int id)
		{
			var viewInfos = new List<ListView.FixedViewInfo> ();

			View view = GetBoundView (id);

			if (view != null) {
				var info = new ListView.FixedViewInfo (this) {
					Data = null,
					IsSelectable = true,
					View = view,
				};
				viewInfos.Add (info);
			}

			return viewInfos;
		}

		IList<ListView.FixedViewInfo> GetFooters ()
		{
			return GetFixedViewInfos (_footerId);
		}

		IList<ListView.FixedViewInfo> GetHeaders ()
		{
			return GetFixedViewInfos (_headerId);
		}
	}
}

