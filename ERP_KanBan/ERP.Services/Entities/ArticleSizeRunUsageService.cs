using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class ArticleSizeRunUsageService : EntityService<ArticleSizeRunUsage>
    {
        protected new ArticleSizeRunUsageRepository Repository { get { return base.Repository as ArticleSizeRunUsageRepository; } }

        public ArticleSizeRunUsageService(ArticleSizeRunUsageRepository repository) : base(repository)
        {
        }
    }
}