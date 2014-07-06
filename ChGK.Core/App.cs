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

			CreatableTypes ()
				.EndingWith ("Deserializer")
				.AsInterfaces ()
				.RegisterAsLazySingleton ();

//			var builder = new TextProviderBuilder ();
//			Mvx.RegisterSingleton<IMvxTextProviderBuilder> (builder);
//			Mvx.RegisterSingleton<IMvxTextProvider> (builder.TextProvider);

			RegisterAppStart<ViewModels.HomeViewModel> ();
//			RegisterAppStart<ViewModels.LastAddedTournamentsViewModel> ();
		}
	}
}