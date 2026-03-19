using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class KnifeService : EntityService<Knife>
    {
        protected new KnifeRepository Repository { get { return base.Repository as KnifeRepository; } }

        public KnifeService(KnifeRepository repository) : base(repository)
        {
        }
    }
}