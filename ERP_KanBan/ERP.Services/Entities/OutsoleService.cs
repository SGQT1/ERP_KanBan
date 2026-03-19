using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class OutsoleService : EntityService<Outsole>
    {
        protected new OutsoleRepository Repository { get { return base.Repository as OutsoleRepository; } }

        public OutsoleService(OutsoleRepository repository) : base(repository)
        {
        }
    }
}