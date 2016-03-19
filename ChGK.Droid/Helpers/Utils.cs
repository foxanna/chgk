using Android.Widget;

namespace ChGK.Droid.Helpers
{
    public static class Utils
    {
        public static void CollapseAllGroups(this ExpandableListView listView)
        {
            for (var i = 0; i < listView?.ExpandableListAdapter?.GroupCount; i++)
                listView.CollapseGroup(i);
        }
    }
}