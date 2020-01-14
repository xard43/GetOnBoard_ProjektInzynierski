using Microsoft.WindowsAzure.Storage.Blob;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GetOnBoard.DAL.Repositories
{
	public interface IBlobStorageRepository
	{
		Task Save(Stream fileStream, string name);
		Task<IEnumerable<string>> GetNames();
		//	Task<CloudBlockBlob> Load(string name);
		CloudBlockBlob Load(string name);
		CloudBlobContainer Container { get; }
	}
}
