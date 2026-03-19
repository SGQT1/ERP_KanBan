using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Services.Bases;
using ERP.Services.Business.Entities;

namespace ERP.Services.Business
{
    public class PackInvoiceService : BusinessService
    {
        private Services.Business.Entities.PackInoviceService PackInovice { get; }

        public PackInvoiceService(
            Services.Business.Entities.PackInoviceService packInovice,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            PackInovice = packInovice;
        }
        public List<Models.Views.PackInvoice> SavePackInvoice(List<Models.Views.PackInvoice> packInvoices)
        {
            try
            {
                UnitOfWork.BeginTransaction();

                if (packInvoices != null)
                {

                    var createItems = packInvoices.Where(i => i.Id == null).ToList();
                    PackInovice.CreateRange(createItems);

                    var updateItems = packInvoices.Where(i => i.Id != null).ToList();
                    PackInovice.UpdateRange(updateItems);
                }

                UnitOfWork.Commit();
                return packInvoices;
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }

        public void ReomvePackInvoice(List<Models.Views.PackInvoice> packInvoices)
        {

            try
            {
                UnitOfWork.BeginTransaction();
                if (packInvoices != null)
                {
                    var localeId = packInvoices.Where(i => i.Id != null).Select(i => (int)i.LocaleId).FirstOrDefault();
                    var removeItems = packInvoices.Where(i => i.Id != null).Select(i => (decimal)i.Id).ToList();

                    PackInovice.RemoveRange(removeItems, localeId);
                }

                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }

    }
}