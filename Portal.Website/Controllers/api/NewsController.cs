using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.Database.Models;
using Portal.Services;
using Portal.Website.Model;

namespace Portal.Website.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly NewsArticleManager _newsArticleManager;

        public NewsController(NewsArticleManager newsArticleManager)
        {
            _newsArticleManager = newsArticleManager;
        }

        [HttpGet("getarticles")]
        public IList<NewsArticleVm> Index(int page, int pageSize)
        {
            return _newsArticleManager.GetNewsArticlePreviews(page, pageSize)
                .Select(a => new NewsArticleVm(a))
                .ToList();
        }

        [HttpGet("article")]
        public NewsArticleVm GetArticle(int newsArticleId)
        {
            Console.WriteLine(User);
            var article = _newsArticleManager.GetArticle(newsArticleId);
            return new NewsArticleVm(article);
        }

        [HttpGet("getnumarticles")]
        public int GetNumArticles()
        {
            return _newsArticleManager.GetTotalNumArticles();
        }

        [HttpPost("create")]
        [Authorize(Roles = "Author")]
        public void Create([FromBody]NewsArticle article)
        {
            var user = User;
            article.Published = DateTime.Now;
            _newsArticleManager.SaveArticle(article);
        }
    }
}
