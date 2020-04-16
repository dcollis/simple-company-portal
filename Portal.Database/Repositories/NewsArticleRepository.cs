using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Portal.Database.Models;

namespace Portal.Database.Repositories
{
    public class NewsArticleRepository
    {
        private readonly PortalContext _portalContext;

        public NewsArticleRepository(PortalContext portalContext)
        {
            _portalContext = portalContext;
        }

        public IList<NewsArticle> GetNewsArticlePreviews(int page, int pageSize)
        {
            var numToSkip = (page - 1) * pageSize;
            var articles = _portalContext.NewsArticles.OrderByDescending(a => a.Published).Skip(numToSkip).Take(pageSize).ToList();
            return articles;
        }

        public NewsArticle GetNewsArticle(int newsArticleId)
        {
            var article = _portalContext.NewsArticles.FirstOrDefault(a => a.NewsArticleId == newsArticleId);
            return article;
        }

        public void AddArticle(NewsArticle article)
        {
            _portalContext.NewsArticles.Add(article);
            _portalContext.SaveChanges();
        }

        public int GetTotalNumArticles()
        {
            return _portalContext.NewsArticles.Count();
        }
    }
}
