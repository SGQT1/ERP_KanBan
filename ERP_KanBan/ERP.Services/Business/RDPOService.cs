using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Diamond.DataSource.Extensions;
using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Models.Views.Common;
using ERP.Services.Bases;
using ERP.Services.Business.Entities;
using Newtonsoft.Json;
using NPOI.HSSF.Record;

namespace ERP.Services.Business
{
    public class RDPOService : BusinessService
    {
        private ERP.Services.Entities.VendorService Vendor { get; set; }
        private ERP.Services.Entities.CompanyService Company { get; set; }
        private ERP.Services.Entities.MaterialService Material { get; set; }
        private ERP.Services.Business.Entities.CodeItemService CodeItem { get; set; }
        private ERP.Services.Business.Entities.RDPOService RDPO { get; set; }
        private ERP.Services.Business.Entities.RDPOItemService RDPOItem { get; set; }
        private ERP.Services.Entities.ProjectPOItemService ProjectPOItem { get; set; }

        public RDPOService
        (
            ERP.Services.Entities.MaterialService materialService,
            ERP.Services.Entities.VendorService vendorService,
            ERP.Services.Entities.CompanyService companyService,
            ERP.Services.Business.Entities.CodeItemService codeItemService,
            ERP.Services.Business.Entities.RDPOService rdPOService,
            ERP.Services.Business.Entities.RDPOItemService rdPOItemService,
            ERP.Services.Entities.ProjectPOItemService projectPOItemService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Material = materialService;
            Vendor = vendorService;
            Company = companyService;
            CodeItem = codeItemService;
            RDPOItem = rdPOItemService;
            RDPO = rdPOService;
            ProjectPOItem = projectPOItemService;
        }

        public ERP.Models.Views.RDPOGroup Get(int id, int localeId)
        {
            var group = new ERP.Models.Views.RDPOGroup { };
            var rdPO = RDPO.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
            var rdPOItem = RDPOItem.Get().Where(i => i.ProjectPOId == id && i.LocaleId == localeId).OrderByDescending(i => i.IsCFM).ThenBy(i => i.SeqNo).ToList();

            if (rdPO != null)
            {
                group.RDPO = rdPO;
                group.RDPOItem = rdPOItem;
            }

            return group;
        }

        public ERP.Models.Views.RDPOGroup Save(RDPOGroup group)
        {
            var rdPO = group.RDPO;
            var rdPOItems = group.RDPOItem.ToList();
            var companies = Company.Get().Select(i => new { i.Id, i.CompanyNo }).ToList();
            try
            {
                UnitOfWork.BeginTransaction();
                if (rdPO != null)
                {
                    //Plan
                    {
                        var _rdPO = RDPO.Get().Where(i => i.LocaleId == rdPO.LocaleId && i.Id == rdPO.Id).FirstOrDefault();
                        if (_rdPO == null)
                        {
                            rdPO = RDPO.Create(rdPO);
                        }
                        else
                        {
                            rdPO.Id = _rdPO.Id;
                            rdPO.LocaleId = _rdPO.LocaleId;
                            rdPO = RDPO.Update(rdPO);
                        }
                    }
                    //items
                    {
                        if (rdPO.Id != 0)
                        {
                            RDPOItem.RemoveRange(i => i.ProjectPOId == rdPO.Id && i.LocaleId == rdPO.LocaleId);

                            var maxSeq = rdPOItems.Max(i => i.SeqNo);
                            rdPOItems.ForEach(poi =>
                            {
                                poi.ProjectPOId = rdPO.Id;
                                poi.ProjectPODate = rdPO.ProjectPODate;
                                poi.ModifyUserName = rdPO.ModifyUserName;
                                poi.LastUpdateTime = (DateTime)rdPO.LastUpdateTime;
                                poi.LocaleId = rdPO.LocaleId;
                                poi.Amount = poi.PlanQty * poi.PayUnitPrice + poi.ExtraAmount;

                                if (poi.SeqNo == 0)
                                {
                                    maxSeq += 1;
                                    poi.SeqNo = maxSeq;
                                }

                                if (poi.ProjectPONo == null || poi.ProjectPONo.Length == 0)
                                {
                                    var companyNo = companies.Where(i => i.Id == poi.RefLocaleId).Max(i => i.CompanyNo);
                                    poi.ProjectPONo = $"{companyNo}{((DateTime)poi.ProjectPODate):yyyyMMdd}-{poi.ProjectPOId.ToString().Substring(poi.ProjectPOId.ToString().Length - 2)}-{poi.SeqNo:0000}";

                                }
                            });

                            RDPOItem.CreateRange(rdPOItems);
                        }
                    }
                }
                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
            return Get((int)rdPO.Id, (int)rdPO.LocaleId);
        }

        public void Remove(RDPOGroup group)
        {
            var rdPO = group.RDPO;
            var rdPOItems = group.RDPOItem.ToList();

            UnitOfWork.BeginTransaction();
            try
            {
                RDPOItem.RemoveRange(i => i.ProjectPOId == rdPO.Id && i.LocaleId == rdPO.LocaleId);
                RDPO.Remove(rdPO);

                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        public IQueryable<ERP.Models.Views.RDPO> GetConfirmRDPO(string predicate, string[] filter)
        {
            var poItems = ProjectPOItem.Get().GroupBy(i => new { i.ProjectPOId, i.LocaleId }).Select(i => new { ProjectPOId = i.Key.ProjectPOId, LocaleId = i.Key.LocaleId, Records = i.Count() });
            var noComfirms = ProjectPOItem.Get().Where(i => i.IsCFM == 0).GroupBy(i => new { i.ProjectPOId, i.LocaleId }).Select(i => new { ProjectPOId = i.Key.ProjectPOId, LocaleId = i.Key.LocaleId, Records = i.Count() });
            var result = (
                from p in RDPOItem.Get()
                join c in noComfirms on new { ProjectPOId = p.ProjectPOId, LocaleId = p.LocaleId } equals new { ProjectPOId = c.ProjectPOId, LocaleId = c.LocaleId } into cGRP
                from c in cGRP.DefaultIfEmpty()
                join pi in poItems on new { ProjectPOId = p.ProjectPOId, LocaleId = p.LocaleId } equals new { ProjectPOId = pi.ProjectPOId, LocaleId = pi.LocaleId } into piGRP
                from pi in piGRP.DefaultIfEmpty()
                select new
                {
                    ProjectPOId = p.ProjectPOId,
                    LocaleId = p.LocaleId,
                    ProjectPODate = p.ProjectPODate,
                    Type = p.Type,
                    StyleNo = p.StyleNo,
                    ProjectPONo = p.ProjectPONo,
                    MaterialName = p.MaterialNameTw,
                    VendorId = p.VendorId,
                    Purchaser = p.ModifyUserName,
                    IsCFM = p.IsCFM,
                    Vendor = p.VendorNameTw,
                    NotApplyCount = (int?)c.Records ?? 0,
                    POCount = (int?)pi.Records ?? 0,
                }
            )
            .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
            .GroupBy(i => new { i.ProjectPOId, i.LocaleId, i.ProjectPODate, i.Type, i.NotApplyCount, i.POCount })
            .Select(i => new Models.Views.RDPO
            {
                Id = i.Key.ProjectPOId,
                LocaleId = i.Key.LocaleId,
                ProjectPODate = (DateTime)i.Key.ProjectPODate,
                Type = (int)i.Key.Type,
                NotApplyCount = i.Key.NotApplyCount,
                POCount = i.Key.POCount,
            })
            .ToList();
            return result.AsQueryable();

        }
        public IEnumerable<ERP.Models.Views.RDPOItem> GetConfirmRDPOItem(int rdPOId, int localeId)
        {
            var rdPOItem = RDPOItem.Get().Where(i => i.ProjectPOId == rdPOId && i.LocaleId == localeId).OrderByDescending(i => i.IsCFM).ThenBy(i => i.SeqNo).ToList();

            return rdPOItem;

        }
        public List<ERP.Models.Views.RDPOItem> ConfirmRDPOItem(List<ERP.Models.Views.RDPOItem> items)
        {
            return items;
        }

    }
}
