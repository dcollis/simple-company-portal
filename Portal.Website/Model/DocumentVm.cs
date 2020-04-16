using Portal.Database.Models;
using Portal.Website.Converters;

namespace Portal.Website.Model
{
    public class DocumentVm : Document
    {
        public bool IsDirectory { get; }
        public string UploadDate { get; }

        public DocumentVm(Document document)
        {
            IsDirectory = document.IsDirectory;
            this.Name = document.Name;
            this.Uploaded = document.Uploaded;
            this.UploadDate = DateToStringConverter.Convert(Uploaded);
            Prefix = document.Prefix;
        }

    }
}
