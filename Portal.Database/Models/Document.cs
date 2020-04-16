using System;

namespace Portal.Database.Models
{
    public class Document
    {
        public string Name { get; set; }
        public DateTime Uploaded { get; set; }

        public string Prefix { get; set; }
        public bool IsDirectory { get; set; }
    }
}
