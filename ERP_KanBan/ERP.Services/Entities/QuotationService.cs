using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Entities
{
    public class QuotationService : EntityService<Quotation>
    {
        public QuotationService(QuotationRepository repository) : base(repository)
        {
        }
    }
}