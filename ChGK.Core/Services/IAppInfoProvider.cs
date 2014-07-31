namespace ChGK.Core.Services
{
	public interface IAppInfoProvider
	{
		string AppVersion { get; }

		string AppName { get; }
	}
}

