using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class OutsoleTCService : EntityService<OutsoleTC>
    {
        protected new OutsoleTCRepository Repository { get { return base.Repository as OutsoleTCRepository; } }

        public OutsoleTCService(OutsoleTCRepository repository) : base(repository)
        {
        }
    }
}