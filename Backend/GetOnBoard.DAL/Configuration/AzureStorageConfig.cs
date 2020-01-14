namespace GetOnBoard.DAL.Configuration
{
	/// <summary>
	/// Class contains information about Azure Storage configuration
	/// </summary>
	public class AzureStorageConfig
	{
		public string ConnectionString { get; set; }

		public string FileContainerName { get; set; }
	}
}
