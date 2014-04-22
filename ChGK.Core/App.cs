using Cirrious.CrossCore.IoC;

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
				
			//RegisterAppStart<ViewModels.FirstViewModel> ();
		}
	}
}