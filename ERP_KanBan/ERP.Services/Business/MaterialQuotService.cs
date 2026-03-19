using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Models.Views;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Services.Business
{
    public class MaterialQuotService : BusinessService
    {
        private ERP.Services.Business.Entities.MaterialService Material { get; set; }
        private ERP.Services.Business.Entities.MaterialQuotService MaterialQuot { get; set; }
        private ERP.Services.Business.Entities.VendorService Vendor { get; set; }
        public MaterialQuotService(
            ERP.Services.Business.Entities.MaterialService materialService,
            ERP.Services.Business.Entities.MaterialQuotService materialQuotService,
            ERP.Services.Business.Entities.VendorService vendorService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Material = materialService;
            MaterialQuot = materialQuotService;
            Vendor = vendorService;
        }

        public IQueryable<Models.Views.MaterialQuot> Get(int materialId, int localeId)
        {
            return MaterialQuot.Get().Where(i => i.MaterialId == materialId && i.LocaleId == localeId);
        }
        public Models.Views.MaterialQuotGroup GetMaterialQuotGroup(int materialId, int localeId)
        {
            var group = new ERP.Models.Views.MaterialQuotGroup { };
            var material = Material.Get().Where(i => i.Id == materialId && i.LocaleId == localeId).FirstOrDefault();
            if (material != null)
            {
                group.Material = material;
                group.MaterialQuot = MaterialQuot.Get().Where(i => i.MaterialId == materialId && i.LocaleId == localeId).OrderBy(i => i.EffectiveDate).ToList();
            }
            return group;
        }
        public ERP.Models.Views.MaterialQuotGroup Save(MaterialQuotGroup item)
        {
            var material = item.Material;
            var materialQutoItem = item.MaterialQuot.ToList();
            try
            {
                UnitOfWork.BeginTransaction();
                if (materialQutoItem.Count() > 0)
                {
                    // get Vendor
                    var vIds = materialQutoItem.Select(i => i.VendorId).ToList();
                    var vendors = Vendor.Get().Where(i => i.LocaleId == material.LocaleId && vIds.Contains(i.Id)).Select(i => new { i.Id, i.LocaleId, i.TaxCodeId, i.DollarCodeId, i.ShortNameTw }).ToList();
                    // update vendor info
                    materialQutoItem.ForEach(i =>
                    {
                        var vendor = vendors.Where(v => v.Id == i.VendorId && v.LocaleId == i.LocaleId).FirstOrDefault();
                        if (vendor != null)
                        {
                            i.VendorShortNameTw = i.VendorShortNameTw.Length == 0 ? vendor.ShortNameTw : i.VendorShortNameTw;
                            i.ProcessMethod = i.ProcessMethod == null ? (int?)vendor.TaxCodeId: i.ProcessMethod;
                            i.DollarCodeId = i.DollarCodeId == null ? (decimal)vendor.DollarCodeId: i.DollarCodeId;
                        }
                    });

                    MaterialQuot.RemoveRange(i => i.MaterialId == material.Id && i.LocaleId == material.LocaleId);
                    MaterialQuot.CreateRange(materialQutoItem);
                }

                // 在報價的時候更新材料的報價類型
                {
                    Material.UpdateInspect(material);
                }
                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }

            return GetMaterialQuotGroup((int)material.Id, (int)material.LocaleId);

        }

        public void Remove(MaterialQuotGroup item)
        {
            var material = item.Material;
            UnitOfWork.BeginTransaction();
            try
            {
                MaterialQuot.RemoveRange(i => i.MaterialId == material.Id && i.LocaleId == material.LocaleId);

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
