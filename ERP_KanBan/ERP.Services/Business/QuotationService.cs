using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Services.Bases;

namespace ERP.Services.Business
{
    public class QuotationService : BusinessService
    {
        private ERP.Services.Business.Entities.QuotationService Quotation { get; set; }
        public QuotationService(
            ERP.Services.Business.Entities.QuotationService quotationService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Quotation = quotationService;
        }

        public ERP.Models.Views.QuotationGroup Get(int id, int localeId)
        {
            var group = new ERP.Models.Views.QuotationGroup { };
            var quotation = Quotation.Get(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
            if (quotation != null)
            {
                group.Quotation = quotation;
                group.QuotationHistory = Quotation.Get(i => i.LocaleId == quotation.LocaleId && 
                                                       i.BrandCodeId == quotation.BrandCodeId && 
                                                       i.StyleNo == quotation.StyleNo && 
                                                       i.ArticleNo == quotation.ArticleNo).ToList();
            }
            return group;
        }

        public ERP.Models.Views.QuotationGroup Save(ERP.Models.Views.Quotation item)
        {
            UnitOfWork.BeginTransaction();
            try
            {
                var _item = Quotation.Get(i => i.LocaleId == item.LocaleId && i.Id == item.Id).FirstOrDefault();
                if (_item != null)
                {
                    item = Quotation.Update(item);
                }
                else
                {
                    item = Quotation.Create(item);
                }
                UnitOfWork.Commit();
                return Get((int)item.Id, (int)item.LocaleId);
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        public void Remove(ERP.Models.Views.Quotation item)
        {
            UnitOfWork.BeginTransaction();
            try
            {
                Quotation.Remove(item);
                UnitOfWork.Commit();
            }
            catch
            {
                UnitOfWork.Rollback();
            }
        }

        public List<ERP.Models.Views.Quotation> SaveBatch(List<ERP.Models.Views.Quotation> items)
        {
            UnitOfWork.BeginTransaction();
            try
            {
                Quotation.CreateRange(items);
                UnitOfWork.Commit();

                return items;
            }
            catch(Exception e)
            {
                UnitOfWork.Rollback();
                throw;
            }
        }
        public void RemoveBatch(List<ERP.Models.Views.Quotation> items)
        {
            UnitOfWork.BeginTransaction();
            try
            {
                Quotation.RemoveRange(items);
                UnitOfWork.Commit();
            }
            catch
            {
                UnitOfWork.Rollback();
            }
        }
    }
}