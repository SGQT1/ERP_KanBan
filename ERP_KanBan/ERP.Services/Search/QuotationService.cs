using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Services.Bases;
using ERP.Services.Business.Entities;

namespace ERP.Services.Search
{
    public class QuotationService : SearchService
    {
        private ERP.Services.Business.Entities.QuotationService Quotation { get; set; }
        public QuotationService(
            ERP.Services.Business.Entities.QuotationService quotationService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Quotation = quotationService;
        }
        public IQueryable<Models.Views.Quotation> GetProductQuotation(string predicate)
        {
            return Quotation.Get(predicate);
        }
    }
}