using ChGK.Core.Services;

namespace ChGK.Core.ViewModels
{
	public class AboutViewModel : MenuItemViewModel
	{
		public AboutViewModel (IAppInfoProvider appInfoProvider)
		{
			Title = "О приложении";
			CopyrightUrl = "http://db.chgk.info/copyright";
			SomeTitle = "База Вопросов Интернет-клуба \"Что? Где? Когда?\" http://db.chgk.info";

			Version = "Версия " + appInfoProvider.AppVersion;
		}

		public string SomeTitle {
			get;
			set;
		}

		public string CopyrightUrl {
			get;
			set;
		}

		public string Version {
			get;
			set;
		}

		#region implemented abstract members of MenuItemViewModel

		public override System.Threading.Tasks.Task Refresh ()
		{
			throw new System.NotImplementedException ();
		}

		#endregion
	}
}

