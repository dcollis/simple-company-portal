using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using Portal.Database.Models;

namespace Portal.Services.Blob
{
    public class DocumentService
    {
        private const string ContainerName = "documents";
        public static string DocumentBlogConnectionString;

        /// <summary>
        /// Gets a document from the blob store and sends back the binary ready for download.
        /// </summary>
        /// <param name="filename">Name of file to be downloaded</param>
        /// <returns>Data</returns>
        public async Task<MemoryStream> GetDocument(string filename)
        {
            var cloudBlobContainer = BlobFactory.GetBlobContainer(ContainerName, DocumentBlogConnectionString);

            CloudBlockBlob blockBlob = cloudBlobContainer.GetBlockBlobReference(filename);

            MemoryStream memStream = new MemoryStream();
            await blockBlob.DownloadToStreamAsync(memStream);

            return memStream;
        }

        /// <summary>
        /// Lists X number of items from the blob store.
        /// </summary>
        /// <param name="numItems"></param>
        /// <returns></returns>
        public async Task<IList<Document>> GetLatestItems(int numItems)
        {
            var results = await GetAllDocuments();

            results.Sort((item1, item2) =>
            {
                var blob1 = item1 as CloudBlockBlob;
                var blob2 = item2 as CloudBlockBlob;

                return blob1.Properties.Created.Value.CompareTo(blob2.Properties.Created.Value);
            });

            results.Reverse();
            results = results.Take(numItems).ToList();

            return ConvertToDocuments(results);
        }

        /// <summary>
        /// Gets documents with in a reference (directory)
        /// </summary>
        /// <param name="reference"></param>
        /// <returns></returns>
        public async Task<IList<Document>> GetItems(string reference = null)
        {
            var cloudBlobContainer = BlobFactory.GetBlobContainer(ContainerName, DocumentBlogConnectionString);

            BlobContinuationToken token = null;
            var results = new List<IListBlobItem>();
            do
            {
                BlobResultSegment result;
                if (reference != null)
                {
                    var container = cloudBlobContainer.GetDirectoryReference(reference);
                    result = await container.ListBlobsSegmentedAsync(token);
                }
                else
                {
                    result = await cloudBlobContainer.ListBlobsSegmentedAsync(token);
                }
                
                results.AddRange(result.Results);
                token = result.ContinuationToken;
            } while (token != null);


            return ConvertToDocuments(results);
        }

        public async Task<IList<Document>> Search(string reference)
        {
            var allDocuments = await GetAllDocuments();

            var matches = allDocuments.Where(
                d => d.Uri.Segments.Last().StartsWith(reference, true, CultureInfo.InvariantCulture)
                ).ToList();
            return ConvertToDocuments(matches);
        }

        private async Task<List<IListBlobItem>> GetAllDocuments()
        {
            var cloudBlobContainer = BlobFactory.GetBlobContainer(ContainerName, DocumentBlogConnectionString);

            BlobContinuationToken token = null;
            var results = new List<IListBlobItem>();
            do
            {
                var response = await cloudBlobContainer.ListBlobsSegmentedAsync("", true, BlobListingDetails.None, null, token,
                    null, null);

                results.AddRange(response.Results);
                token = response.ContinuationToken;
            } while (token != null);

            return results;
        }

        /// <summary>
        /// convert blob model to document
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        private IList<Document> ConvertToDocuments(List<IListBlobItem> results)
        {
            var docs = new List<Document>();
            foreach (var result in results)
            {
                var path = result.Uri.AbsolutePath.Remove(0, 11).TrimEnd('/');

                string prefix = null;
                var name = result.Uri.Segments.Last().TrimEnd('/');
                if (path.Contains('/'))
                {
                    var index = path.LastIndexOf('/');
                    prefix = path.Substring(0, index);
                }

                var uploadedDate = (result is CloudBlockBlob blob && blob.Properties.Created.HasValue)
                    ? blob.Properties.Created.Value.UtcDateTime
                    : new DateTime();

                var doc = new Document()
                {
                    Name = name,
                    Prefix = prefix,
                    Uploaded = uploadedDate,
                    IsDirectory = result is CloudBlobDirectory
                };
                docs.Add(doc);
            }

            return docs;
        }

    }
}
