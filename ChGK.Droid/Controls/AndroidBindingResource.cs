using System;
using MvvmCross.Binding.Droid.ResourceHelpers;
using MvvmCross.Platform.Droid;

namespace ChGK.Droid.Controls
{
	// https://github.com/hlogmans/MvvmCross.DeapExtensions/blob/master/src/DeapExtensions.Binding.Droid/AndroidBindingResource.cs
	public class AndroidBindingResource
	{
		public static readonly AndroidBindingResource Instance = new AndroidBindingResource ();

		private AndroidBindingResource ()
		{
			var setup = MvvmCross.Platform.Mvx.Resolve<IMvxAndroidGlobals> ();
			var resourceTypeName = setup.ExecutableNamespace + ".Resource";
			Type resourceType = setup.ExecutableAssembly.GetType (resourceTypeName);
			if (resourceType == null)
				throw new Exception ("Unable to find resource type - " + resourceTypeName);
			try {
				BindableListGroupItemTemplateId =
					(int)
					resourceType.GetNestedType ("Styleable")
						.GetField ("MvxListView_GroupItemTemplate")
						.GetValue (null);
				MvxListViewWithHeader_HeaderLayout =
					(int)
					resourceType.GetNestedType ("Styleable")
						.GetField ("MvxListView_header")
						.GetValue (null);
				MvxListViewWithHeader_FooterLayout =
					(int)
					resourceType.GetNestedType ("Styleable")
						.GetField ("MvxListView_footer")
						.GetValue (null);
			} catch (Exception) {
				throw new Exception (
					"Error finding resource ids for MvvmCross.DeapBinding - please make sure ResourcesToCopy are linked into the executable");
			}
		}

		public int BindableListGroupItemTemplateId { get; private set; }

		public int MvxListViewWithHeader_HeaderLayout { get; private set; }

		public int MvxListViewWithHeader_FooterLayout { get; private set; }
	}

    internal static class Mvx
    {
    }
}

