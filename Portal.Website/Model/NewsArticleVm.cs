using Portal.Database.Models;
using Portal.Website.Converters;

namespace Portal.Website.Model
{
    public class NewsArticleVm : NewsArticle
    {
        public string PublishedString { get; set; }
        public string UpdatedString { get; set; }

        public NewsArticleVm(NewsArticle newsArticle)
        {
            this.NewsArticleId = newsArticle.NewsArticleId;
            this.Title = newsArticle.Title;
            this.Body = newsArticle.Body;
            this.Published = newsArticle.Published;
            this.Updated = newsArticle.Updated;
            this.PublishedString = DateToStringConverter.Convert(Published);
            if (Updated != null) this.UpdatedString = DateToStringConverter.Convert(Updated.Value);
        }
    }
}
