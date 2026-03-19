using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class POService : BusinessService
    {
        private ERP.Services.Entities.POService PO { get; set; }
        private ERP.Services.Entities.VendorService Vendor { get; set; }

        public POService(
            ERP.Services.Entities.POService poService,
            ERP.Services.Entities.VendorService vendorService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            PO = poService;
            Vendor = vendorService;
        }
        public IQueryable<Models.Views.PO> Get()
        {
            return (
                from p in PO.Get()
                join v in Vendor.Get() on new { VendorId = p.VendorId, LocaleId = p.LocaleId } equals new { VendorId = v.Id, LocaleId = v.LocaleId }
                select new Models.Views.PO
                {
                    Id = p.Id,
                    LocaleId = p.LocaleId,
                    PODate = p.PODate,
                    BatchNo = p.BatchNo,
                    SeqId = p.SeqId,
                    VendorId = p.VendorId,
                    IsShowSizeRun = p.IsShowSizeRun,
                    ShowAlternateType = p.ShowAlternateType,
                    VendorETD = p.VendorETD.Date,
                    IsAllowPartial = p.IsAllowPartial,
                    Remark = p.Remark,
                    ModifyUserName = p.ModifyUserName,
                    LastUpdateTime = p.LastUpdateTime,
                    PhotoURLDescTw = p.PhotoURLDescTw,
                    PhotoURL = p.PhotoURL,
                    POTeam = p.POTeam,
                    Vendor = v.ShortNameTw,
                    PONo = p.BatchNo + '-' + p.SeqId
                }
            );
        }

        public IQueryable<Models.Views.PO> GetWithOutVendor()
        {
            return (
                from p in PO.Get()
                join v in Vendor.Get() on new { VendorId = p.VendorId, LocaleId = p.LocaleId } equals new { VendorId = v.Id, LocaleId = v.LocaleId } into vGRP
                from v in vGRP.DefaultIfEmpty()
                select new Models.Views.PO
                {
                    Id = p.Id,
                    LocaleId = p.LocaleId,
                    PODate = p.PODate,
                    BatchNo = p.BatchNo,
                    SeqId = p.SeqId,
                    VendorId = p.VendorId,
                    IsShowSizeRun = p.IsShowSizeRun,
                    ShowAlternateType = p.ShowAlternateType,
                    VendorETD = p.VendorETD.Date,
                    IsAllowPartial = p.IsAllowPartial,
                    Remark = p.Remark,
                    ModifyUserName = p.ModifyUserName,
                    LastUpdateTime = p.LastUpdateTime,
                    PhotoURLDescTw = p.PhotoURLDescTw,
                    PhotoURL = p.PhotoURL,
                    POTeam = p.POTeam,
                    Vendor = v.ShortNameTw,
                    PONo = p.BatchNo + '-' + p.SeqId
                }
            );
        }

        public Models.Views.PO Create(Models.Views.PO item)
        {
            var _item = PO.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.PO Update(Models.Views.PO item)
        {
            var _item = PO.Update(Build(item));

            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.PO item)
        {
            PO.Remove(Build(item));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.PO, bool>> predicate)
        {
            PO.RemoveRange(predicate);
        }
        //for update, transfer view model to entity
        private Models.Entities.PO Build(Models.Views.PO item)
        {
            return new Models.Entities.PO()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                PODate = item.PODate,
                BatchNo = item.BatchNo,
                SeqId = item.SeqId,
                VendorId = item.VendorId,
                IsShowSizeRun = item.IsShowSizeRun,
                ShowAlternateType = item.ShowAlternateType,
                VendorETD = item.VendorETD.Date,
                IsAllowPartial = item.IsAllowPartial,
                Remark = item.Remark,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                PhotoURLDescTw = item.PhotoURLDescTw,
                PhotoURL = item.PhotoURL,
                POTeam = item.POTeam,
            };
        }
    
        public void CreateRange(IEnumerable<Models.Views.PO> items)
        {
            PO.CreateRange(BuildRange(items));
        }

        public void UpdateRange(int localeId, IEnumerable<decimal> poIds, Models.Views.PO item)
        {
            PO.UpdateRange(
                i => poIds.Contains(i.Id) && i.LocaleId == localeId,
                // u => new Models.Entities.PO {
                //     ModifyUserName = item.ModifyUserName,
                //     POTeam = item.POTeam,
                //     Remark = item.Remark,
                //     LastUpdateTime = DateTime.Now,
                //     IsShowSizeRun = item.IsShowSizeRun
                // }
                u => u.SetProperty(p => p.ModifyUserName, v => item.ModifyUserName).SetProperty(p => p.POTeam, v => item.POTeam).SetProperty(p => p.Remark, v => item.Remark).SetProperty(p => p.LastUpdateTime, v => DateTime.Now).SetProperty(p => p.IsShowSizeRun, v => item.IsShowSizeRun)
            );
        }
        private IEnumerable<ERP.Models.Entities.PO> BuildRange(IEnumerable<Models.Views.PO> items)
        {
            return items.Select(item => new ERP.Models.Entities.PO
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                PODate = item.PODate,
                BatchNo = item.BatchNo,
                SeqId = item.SeqId,
                VendorId = item.VendorId,
                IsShowSizeRun = item.IsShowSizeRun,
                ShowAlternateType = item.ShowAlternateType,
                VendorETD = item.VendorETD.Date,
                IsAllowPartial = item.IsAllowPartial,
                Remark = item.Remark,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                PhotoURLDescTw = item.PhotoURLDescTw,
                PhotoURL = item.PhotoURL,
                POTeam = item.POTeam,
            });
        }
    }
}