using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class KnifeItemService : EntityService<KnifeItem>
    {
        protected new KnifeItemRepository Repository { get { return base.Repository as KnifeItemRepository; } }

        public KnifeItemService(KnifeItemRepository repository) : base(repository)
        {
        }
    }
}