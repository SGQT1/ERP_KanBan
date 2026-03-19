using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class ArticleService : EntityService<Article>
    {
        protected new ArticleRepository Repository { get { return base.Repository as ArticleRepository; } }

        public ArticleService(ArticleRepository repository) : base(repository)
        {
        }
    }
}