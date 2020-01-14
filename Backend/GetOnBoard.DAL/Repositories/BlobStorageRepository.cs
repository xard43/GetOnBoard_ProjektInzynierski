using GetOnBoard.DAL.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GetOnBoard.DAL.Repositories
{
	public class BlobStorageRepository : IBlobStorageRepository
	{
		private readonly AzureStorageConfig _storageConfig;
		private CloudStorageAccount _storageAccount;
		private CloudBlobClient _blobClient;
		public CloudBlobContainer Container { get; private set; }

		public BlobStorageRepository(IOptions<AzureStorageConfig> storageConfig)
		{
			_storageConfig = storageConfig.Value;
		}

		/// <summary>
		/// Initailize BlobStorage using configuration stored in AzureStorageConfig
		/// </summary>
		/// <returns></returns>
		public Task<bool> Initialize()
		{
			_storageAccount = CloudStorageAccount.Parse(_storageConfig.ConnectionString);
			_blobClient = _storageAccount.CreateCloudBlobClient();
			Container = _blobClient.GetContainerReference(_storageConfig.FileContainerName);
			return Container.CreateIfNotExistsAsync();
		}

		public Task<IEnumerable<string>> GetNames()
		{
			throw new NotImplementedException();
		}

		public CloudBlockBlob Load(string name)
		{
			return Container.GetBlockBlobReference(name);
		}

		public Task Save(Stream fileStream, string name)
		{
			CloudBlockBlob blockBlob = Container.GetBlockBlobReference(name);
			return blockBlob.UploadFromStreamAsync(fileStream);
		}
	}
}
