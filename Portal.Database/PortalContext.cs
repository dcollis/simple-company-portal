using Microsoft.EntityFrameworkCore;
using Portal.Database.Models;

namespace Portal.Database
{
    public class PortalContext : DbContext
    {
        public virtual DbSet<NewsArticle> NewsArticles { get; set; }

        public PortalContext(DbContextOptions options) : base(options)
        {
            
        }

        public PortalContext()
        {
            
        }
    }
}
