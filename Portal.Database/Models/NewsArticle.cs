using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Portal.Database.Models
{
    [Table("NewsArticle")]
    public class NewsArticle
    {
        public int NewsArticleId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime Published { get; set; }
        public DateTime? Updated { get; set; }
    }
}
