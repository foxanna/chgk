using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore;
using ChGK.Core.DbChGKInfo;
using ChGK.Core.NetworkService;
using Cirrious.MvvmCross.Localization;

namespace ChGK.Core
{
	public class App : Cirrious.MvvmCross.ViewModels.MvxApplication
	{
		public override void Initialize ()
		{
			CreatableTypes ()
                .EndingWith ("Service")
                .AsInterfaces ()
                .RegisterAsLazySingleton ();

			RegisterAppStart<ViewModels.HomeViewModel> ();
		}
	}
}