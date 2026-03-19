using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP.Data.Utilities;
using ERP.Services.Bases;

namespace ERP.Services.Business
{
    public class VendorService : BusinessService
    {
        private ERP.Services.Business.Entities.VendorService Vendor { get; set; }
        private ERP.Services.Business.Entities.VendorItemService VendorItem { get; set; }
        private ERP.Services.Business.Entities.CodeItemService CodeItem { get; set; }
        private ERP.Services.Business.Entities.POService PO { get; set; }
        private ERP.Services.Business.Entities.MaterialQuotService MaterialQuot { get; set; }
        private ERP.Services.Business.Entities.RDPOItemService ProjectPOItem { get; set; }

        private ERP.Services.Business.CacheService Cache { get; set; }
        public VendorService(
            ERP.Services.Business.Entities.VendorService vendorService,
            ERP.Services.Business.Entities.VendorItemService vendorItemService,
            ERP.Services.Business.Entities.CodeItemService codeItemService,
            ERP.Services.Business.Entities.POService poService,
            ERP.Services.Business.Entities.MaterialQuotService materialQuotService,
            ERP.Services.Business.Entities.RDPOItemService projectPOItemService,

            ERP.Services.Business.CacheService cacheService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Vendor = vendorService;
            VendorItem = vendorItemService;
            CodeItem = codeItemService;
            PO = poService;
            MaterialQuot = materialQuotService;
            ProjectPOItem = projectPOItemService;

            Cache = cacheService;
        }

        public ERP.Models.Views.VendorGroup Get(int id, int localeId)
        {
            var group = new ERP.Models.Views.VendorGroup { };
            var vendor = Vendor.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
            if (vendor != null)
            {
                group.Vendor = vendor;
                group.VendorItem = VendorItem.Get().Where(i => i.VendorId == id && i.LocaleId == localeId).ToList();
                group.VendorUseFor = new Models.Views.VendorUseFor
                {
                    POCount = PO.Get().Where(i => i.VendorId == vendor.Id && i.LocaleId == vendor.LocaleId).Count(),
                    QuotationCount = MaterialQuot.Get().Where(i => i.VendorId == vendor.Id && i.LocaleId == vendor.LocaleId).Count(),
                    ProjectPOItemCount = ProjectPOItem.Get().Where(i => i.VendorId == vendor.Id && i.LocaleId == vendor.LocaleId).Count(),
                };
            }
            return group;
        }

        public ERP.Models.Views.VendorGroup Save(ERP.Models.Views.VendorGroup item)
        {
            var vendor = item.Vendor;
            var vendorItem = item.VendorItem.ToList();

            if (vendor != null)
            {
                try
                {
                    UnitOfWork.BeginTransaction();

                    //Vendor
                    {
                        var _vendor = Vendor.Get().Where(i => i.LocaleId == vendor.LocaleId && i.Id == vendor.Id).FirstOrDefault();

                        if (_vendor != null)
                        {
                            vendor.Id = _vendor.Id;
                            vendor.LocaleId = _vendor.LocaleId;
                            vendor = Vendor.Update(vendor);
                        }
                        else
                        {
                            vendor = Vendor.Create(vendor);
                        }
                    }

                    //Vendor Item
                    {
                        if (vendor.Id != 0)
                        {
                            vendorItem.ForEach(i => i.VendorId = vendor.Id);

                            VendorItem.RemoveRange(i => i.VendorId == vendor.Id && i.LocaleId == vendor.LocaleId);
                            VendorItem.CreateRange(vendorItem);
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
            // Cache.LoadMaterialCache((int)vendor.LocaleId);
            return Get((int)vendor.Id, (int)vendor.LocaleId);

        }

        public void Remove(ERP.Models.Views.VendorGroup item)
        {
            var vendor = item.Vendor;
            try
            {
                UnitOfWork.BeginTransaction();

                VendorItem.RemoveRange(i => i.VendorId == vendor.Id && i.LocaleId == vendor.LocaleId);
                Vendor.Remove(vendor);

                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
            // Cache.LoadMaterialCache((int)vendor.LocaleId);
        }

        public void CopyVendors(string ids, int locale, string user)
        {
            var items = ids.Split(',').ToList();

            try
            {
                var _ids = new List<decimal>();
                var _localeId = 0;

                items.ForEach(i =>
                {
                    _ids.Add(Convert.ToInt16(i.Split('_')[0]));
                    _localeId = Convert.ToInt16(i.Split('_')[1]);
                });

                if (_ids.Count > 0)
                {
                    var _codeType = new string[] { "10", "01", "07", "23", "09", "02" };
                    var _vendors = Vendor.Get().Where(i => _ids.Contains(i.Id) && i.LocaleId == _localeId).ToList();
                    var _vendorItems = VendorItem.Get().Where(i => _ids.Contains(i.VendorId) && i.LocaleId == _localeId).ToList();
                    var _codeItems = CodeItem.Get().Where(i => _codeType.Contains(i.CodeType) && i.LocaleId == locale).ToList();

                    _vendors.ForEach(i =>
                    {
                        var group = new ERP.Models.Views.VendorGroup();
                        // convert vendor items
                        var _items = _vendorItems.Where(vi => vi.VendorId == i.Id && vi.LocaleId == i.LocaleId)
                            .Select(i => new ERP.Models.Views.VendorItem
                            {
                                Id = 0,
                                ModifyUserName = user,
                                LocaleId = locale,
                                VendorId = 0,
                                BankName = i.BankName,
                                AccountName = i.AccountName,
                                AccountNo = i.AccountNo,
                                BankAddress = i.BankAddress,
                                MoneyCodeId = _codeItems.Where(c => c.NameTW == i.MoneyCode && c.CodeType == "02").Max(c => c.Id),
                            })
                            .ToList();

                        // convert vendor
                        i.Id = 0;
                        i.LocaleId = locale;
                        i.ModifyUserName = user;
                        i.TypeCodeId = i.TypeCodeId == null || i.TypeCodeId == 0 || _codeItems.Where(c => c.NameTW == i.VendorType && c.CodeType == "23").Count() == 0 ? 0 : _codeItems.Where(c => c.NameTW == i.VendorType && c.CodeType == "23").Max(c => c.Id);
                        i.CountryCodeId = i.CountryCodeId == null || i.CountryCodeId == 0 || _codeItems.Where(c => c.NameTW == i.CountryCode && c.CodeType == "01").Count() == 0 ? 0 : _codeItems.Where(c => c.NameTW == i.CountryCode && c.CodeType == "01").Max(c => c.Id);
                        i.AreaCodeId = i.AreaCodeId == null || i.AreaCodeId == 0 || _codeItems.Where(c => c.NameTW == i.AreaCode && c.CodeType == "07").Count() == 0 ? 0 : _codeItems.Where(c => c.NameTW == i.AreaCode && c.CodeType == "07").Max(c => c.Id);
                        i.TaxCodeId = i.TaxCodeId == null || i.TaxCodeId == 0 || _codeItems.Where(c => c.NameTW == i.TaxCode && c.CodeType == "09").Count() == 0 ? 0 : _codeItems.Where(c => c.NameTW == i.TaxCode && c.CodeType == "09").Max(c => c.Id);
                        i.DollarCodeId = i.DollarCodeId == null || i.DollarCodeId == 0 || _codeItems.Where(c => c.NameTW == i.DollarCode && c.CodeType == "02").Count() == 0 ? 0 : _codeItems.Where(c => c.NameTW == i.DollarCode && c.CodeType == "02").Max(c => c.Id);
                        i.PaymentCodeId = i.PaymentCodeId == null || i.PaymentCodeId == 0 || _codeItems.Where(c => c.NameTW == i.PaymentCode && c.CodeType == "02").Count() == 0 ? 0 : _codeItems.Where(c => c.NameTW == i.PaymentCode && c.CodeType == "10").Max(c => c.Id);

                        group.Vendor = i;
                        group.VendorItem = _items;

                        this.Save(group);
                    });
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public IEnumerable<ERP.Models.Views.Vendor> GetVendor(string vendor, int localeId)
        {
            return Vendor.Get().Where(i => i.NameTw.ToLower() == vendor.ToLower() && i.LocaleId == localeId).ToList();
        }
    }
}