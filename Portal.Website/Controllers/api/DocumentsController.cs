using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Portal.Services.Blob;
using Portal.Website.Model;

namespace Portal.Website.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly DocumentService _documentService;

        public DocumentsController(DocumentService documentService)
        {
            this._documentService = documentService;
        }

        [HttpGet("getdocuments")]
        public IList<DocumentVm> Get(string path)
        {
            var docPath = HttpUtility.UrlDecode(path);
            var items = Task.Run(async () => await _documentService.GetItems(docPath)).Result.Select(d => new DocumentVm(d)).ToList();
            items.RemoveAll(i => i.Name.Equals("thumbs.db", StringComparison.InvariantCultureIgnoreCase));
            return items;
        }

        [HttpGet("getlimiteddocuments")]
        public IList<DocumentVm> Get(int numItems)
        {
            var items = Task.Run(async () => await _documentService.GetLatestItems(numItems)).Result
                .Select(d => new DocumentVm(d)).ToList();

            items.RemoveAll(i => i.Name.Equals("thumbs.db", StringComparison.InvariantCultureIgnoreCase));
            return items;
        }

        [HttpGet("download")]
        public async Task<Stream> Download(string file)
        {
            var docPath = HttpUtility.UrlDecode(file);
            var docStream = await _documentService.GetDocument(docPath);
            docStream.Position = 0;
            
            var filename = docPath;
            if (filename.Contains("/"))
            {
                filename = filename.Substring(filename.LastIndexOf('/') + 1);
            }
            Response.Headers.Add("Content-disposition", $"inline; filename=\"{ HttpUtility.UrlEncode(filename).Replace('+', ' ') }\"");
            return docStream;
        }

        [HttpGet("search")]
        public async Task<IList<DocumentVm>> Search(string pattern)
        {
            string encodedPattern = Uri.EscapeUriString(pattern);
            var items = (await _documentService.Search(encodedPattern)).Select(d => new DocumentVm(d)).ToList();

            items.RemoveAll(i => i.Name.Equals("thumbs.db", StringComparison.InvariantCultureIgnoreCase));
            return items;
        }
    }
}
