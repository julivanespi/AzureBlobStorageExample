using System.IO;
using System.Threading.Tasks;
using AzureBlobStorageExample.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzureBlobStorageExample.Services
{
    public interface IBlobService
    {
        Task GetFile(string fileName);
    }

    public class BlobService : IBlobService
    {

        private readonly IConfiguration _config;
        private readonly BlobStorage _blobStorage;

        public BlobService(IConfiguration config, BlobStorage blobStorage)
        {
            _config = config;
            _blobStorage = blobStorage;
        }
        public async Task GetFile(string fileName)
        {
            MemoryStream ms = new MemoryStream();
            if(CloudStorageAccount.TryParse(_blobStorage.ConnectionString, out CloudStorageAccount storageAccount))
            {
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(_blobStorage.ContainerName);
                if(await container.ExistsAsync())
                {
                    CloudBlob file = container.GetBlobReference("funny_password.jpg");
                    if(await file.ExistsAsync())
                    {
                        await file.DownloadToStreamAsync(ms);  
                        Stream blobStream = file.OpenReadAsync().Result;  
                        return File(blobStream, file.Properties.ContentType, file.Name);
                    }
                }
            }

            throw new System.NotImplementedException();
        }
    }
}