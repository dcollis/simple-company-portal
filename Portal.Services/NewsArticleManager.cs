using System.Collections.Generic;
using Portal.Database.Models;
using Portal.Database.Repositories;

namespace Portal.Services
{
    public class NewsArticleManager
    {
        private readonly NewsArticleRepository _newsArticleRepository;

        public NewsArticleManager(NewsArticleRepository newsArticleRepository)
        {
            this._newsArticleRepository = newsArticleRepository;
        }

        public IList<NewsArticle> GetNewsArticlePreviews(int page, int pageSize)
        {
            var articles = _newsArticleRepository.GetNewsArticlePreviews(page, pageSize);
            foreach (var article in articles)
            {
                // truncate the body to 200 chars and last space
                if (article.Body.Length <= 200) continue;

                article.Body = article.Body.Substring(0, 200);
                article.Body = article.Body.Substring(0, article.Body.LastIndexOf(' ')) + "...";
            }

            return articles;
        }

        public NewsArticle GetArticle(int newsArticleId)
        {
            var article = _newsArticleRepository.GetNewsArticle(newsArticleId);
            return article;
        }

        public void SaveArticle(NewsArticle article)
        {
            _newsArticleRepository.AddArticle(article);
        }

        public int GetTotalNumArticles()
        {
            return _newsArticleRepository.GetTotalNumArticles();
        }
    }
}
