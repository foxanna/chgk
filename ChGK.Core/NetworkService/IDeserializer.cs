using System.Threading.Tasks;

namespace ChGK.Core.NetworkService
{
	public interface IDeserializer<T>
	{
		Task<T> Deserialize (string some);
	}
}

