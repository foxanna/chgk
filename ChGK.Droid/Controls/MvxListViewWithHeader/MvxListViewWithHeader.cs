using Android.Content;
using Android.Util;
using MvvmCross.Binding.Droid.ResourceHelpers;
using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;

namespace ChGK.Droid.Controls.MvxListViewWithHeader
{
	public class MvxListViewWithHeader : MvxListView
	{
		public MvxListViewWithHeader (Context context, IAttributeSet attrs) : base (context, attrs, null)
		{
            var headerId = MvxAttributeHelpers.ReadAttributeValue(context, attrs, MvxAndroidBindingResource.Instance.ListViewStylableGroupId,
                AndroidBindingResource.Instance.MvxListViewWithHeader_HeaderLayout);
            var footerId = MvxAttributeHelpers.ReadAttributeValue(context, attrs, MvxAndroidBindingResource.Instance.ListViewStylableGroupId,
                AndroidBindingResource.Instance.MvxListViewWithHeader_FooterLayout);
            
            var headers = GetFixedViewInfos(headerId);
            var footers = GetFixedViewInfos(footerId);

            var adapter = new MvxAdapter(context);
            adapter.ItemTemplateId = MvxAttributeHelpers.ReadListItemTemplateId(context, attrs);

            var headerAdapter = new HeaderMvxAdapter(headers, footers, adapter);
            Adapter = headerAdapter;
		}

        IList<ListView.FixedViewInfo> GetFixedViewInfos(int id)
        {
            var viewInfos = new List<ListView.FixedViewInfo>();

            var view = GetBoundView(id);

            if (view != null)
            {
                var info = new ListView.FixedViewInfo(this)
                {
                    Data = null,
                    IsSelectable = true,
                    View = view,
                };
                viewInfos.Add(info);
            }

            return viewInfos;
        }

        static View GetBoundView(int id)
        {
            if (id == 0)
                return null;

            var bindingContext = MvxAndroidBindingContextHelpers.Current();
            var view = bindingContext.BindingInflate(id, null);

            return view;
        }
	}
}