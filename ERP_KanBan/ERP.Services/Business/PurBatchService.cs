using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERP.Models.Views;
using ERP.Services.Business.Entities;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Diamond.DataSource.Extensions;

namespace ERP.Services.Business
{
    public class PurBatchService : BusinessService
    {
        private ERP.Services.Business.Entities.PurBatchService PurBatch { get; set; }
        private ERP.Services.Business.Entities.PurOrdersItemService PurOrdersItem { get; set; }
        private ERP.Services.Entities.OrdersService Orders { get; set; }
        public PurBatchService(
            ERP.Services.Business.Entities.PurBatchService purBatchService,
            ERP.Services.Business.Entities.PurOrdersItemService purOrdersItemService,
            ERP.Services.Entities.OrdersService ordersService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            PurBatch = purBatchService;
            PurOrdersItem = purOrdersItemService;
            Orders = ordersService;
        }
        public IQueryable<Models.Views.PurBatch> GetWithItem(string predicate)
        {
            var result = (
                from p in PurBatch.Get()
                join pi in PurOrdersItem.Get() on new { PurId = p.Id, LocaleId = p.LocaleId } equals new { PurId = pi.PurBatchId, LocaleId = pi.LocaleId } into apiGRP
                from pi in apiGRP.DefaultIfEmpty()
                select new
                {
                    Id = p.Id,
                    LocaleId = p.LocaleId,                   
                    OrderNo = pi.OrderNo,
                    CompanyId = pi.CompanyId,
                    BatchDate = p.BatchDate,
                    BatchNo = p.BatchNo,
                    RefLocaleId = p.RefLocaleId,
                    ModifyUserName = p.ModifyUserName,
                    LastUpdateTime = p.LastUpdateTime,
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .Select(i => new ERP.Models.Views.PurBatch
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                BatchNo = i.BatchNo,
                BatchDate = i.BatchDate,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                RefLocaleId = i.RefLocaleId,
            })
            .Distinct();
            return result;
        }
        public ERP.Models.Views.PurBatchGroup Get(int id, int localeId)
        {
            var group = new ERP.Models.Views.PurBatchGroup { };
            var purBatch = PurBatch.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();

            if (purBatch != null)
            {
                group.PurBatch = purBatch;
                group.PurOrdersItem = PurOrdersItem.Get().Where(i => i.PurBatchId == purBatch.Id && i.LocaleId == purBatch.LocaleId).ToList();
            }
            return group;
        }
        public ERP.Models.Views.PurBatchGroup Save(PurBatchGroup item)
        {
            var purBatch = item.PurBatch;
            var purOrdersItems = item.PurOrdersItem.Distinct().ToList();

            if (purBatch != null)
            {
                try
                {
                    UnitOfWork.BeginTransaction();
                    {
                        var _purBatch = PurBatch.Get().Where(i => i.Id == purBatch.Id && i.LocaleId == purBatch.LocaleId).FirstOrDefault();

                        if (_purBatch != null)
                        {
                            purBatch.Id = _purBatch.Id;
                            purBatch.LocaleId = _purBatch.LocaleId;
                            purBatch = PurBatch.Update(purBatch);
                        }
                        else
                        {
                            purBatch = PurBatch.Create(purBatch);
                        }
                    }

                    //Vendor Item
                    {
                        if (purBatch.Id != 0)
                        {
                            purOrdersItems.ForEach(i => i.PurBatchId = purBatch.Id);

                            PurOrdersItem.RemoveRange(i => i.PurBatchId == purBatch.Id && i.LocaleId == purBatch.LocaleId);
                            PurOrdersItem.CreateRange(purOrdersItems);
                        }
                    }
                    UnitOfWork.Commit();

                }
                catch (Exception e)
                {
                    UnitOfWork.Rollback();
                    throw e;
                }
            }
            return Get((int)purBatch.Id, (int)purBatch.LocaleId);
        }
        public void Remove(PurBatchGroup item)
        {
            var purBatch = item.PurBatch;
            UnitOfWork.BeginTransaction();
            try
            {
                PurOrdersItem.RemoveRange(i => i.PurBatchId == purBatch.Id && i.LocaleId == purBatch.LocaleId);
                PurBatch.Remove(purBatch);

                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
    
        public IQueryable<Models.Views.PurOrdersItem> BuildPurOrdersItemForOrders(string predicate, string[] filters) {
            
            var extendPredicate = "";
            if (filters != null && filters.Length > 0)
            {
                //condition
                var tmpFilters = filters.Where(i => i.Contains("LocaleId")).ToArray();
                extendPredicate = tmpFilters.Length > 0 ? String.Join(" && ", tmpFilters) : "1=1";
            }

            var purOrdersId = PurOrdersItem.Get().Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, extendPredicate).Select(i => i.OrdersId);
            var result = Orders.Get()
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .Where(i => !purOrdersId.Contains(i.Id))
                .Select(i => new Models.Views.PurOrdersItem {
                    Id = 0,
                    LocaleId = i.LocaleId,
                    PurBatchId = 0,
                    OrdersId = i.Id,
                    ModifyUserName = "",
                    LastUpdateTime = DateTime.Now,
                    OrderNo = i.OrderNo,
                    OrderQty = i.OrderQty,
                    CompanyId = i.CompanyId,
                    ArticleNo = i.ArticleNo,
                    StyleNo = i.StyleNo,
                    CSD = i.CSD,
                    ETD = i.ETD,
                    LCSD = i.LCSD,
                    OrderType = i.OrderType,
                    ProductType = i.ProductType,
                    RefLocaleId = i.LocaleId,
                    Status = i.Status,
                    doMRP = i.doMRP,
                    CustomerOrderNo = i.CustomerOrderNo,
                    OWD = i.OWD,
                    ShoeName = i.ShoeName,
                });

            
            return result;
        }
    }
}
