using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Portal.Services
{
    public static class BlobFactory
    {
        public static CloudBlobContainer GetBlobContainer(string containerName, string connectionString)
        {
            string storageConnection = connectionString;
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(storageConnection);
            CloudBlobClient blobClient = cloudStorageAccount.CreateCloudBlobClient();

            CloudBlobContainer cloudBlobContainer = blobClient.GetContainerReference(containerName);
            return cloudBlobContainer;
        }
    }
}
