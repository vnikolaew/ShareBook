namespace ShareBook.Infrastructure.Images.Settings;
public class AzureBlobStorageSettings
{
	public string AccountName { get; set; }
	public string AccountKey { get; set; }
	public string EndpointSuffix { get; set; }	
	
	public string ContainerName { get; set; }	
	
	public string ConnectionString
		=> $"DefaultEndpointsProtocol=https;{nameof(AccountName)}={AccountName};{nameof(AccountKey)}={AccountKey};{nameof(EndpointSuffix)}={EndpointSuffix}";
}