using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Portal.Services.Blob
{
    public class ProfilePhotoService
    {
        private const string ContainerName = "profilephotos";
        public static string PhotoBlobConnectionString;

        public async Task<MemoryStream> GetPhoto(string emailaddress)
        {
            var cloudBlobContainer = BlobFactory.GetBlobContainer(ContainerName, PhotoBlobConnectionString);

            CloudBlockBlob blockBlob = cloudBlobContainer.GetBlockBlobReference(emailaddress.ToLower() + ".jpg");
            if (!await blockBlob.ExistsAsync())
            {
                return null;
            }
            MemoryStream memStream = new MemoryStream();
            await blockBlob.DownloadToStreamAsync(memStream);

            return memStream;
        }
    }
}
