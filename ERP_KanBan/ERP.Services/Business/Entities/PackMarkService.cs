using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Services.Bases;
using Microsoft.Extensions.Configuration;

namespace ERP.Services.Business.Entities
{
    public class PackMarkService : BusinessService
    {
        public IConfiguration Configuration { get; }
        private Services.Business.Entities.OrdersService Orders { get; }
        private Services.Entities.OrdersPLPhotoService OrdersPLPhoto { get; }
        private Services.Entities.OrdersPLService OrdersPL { get; }
        private Services.Entities.CustomerService Customer { get; }
        public PackMarkService(
            IConfiguration iConfig,
            Services.Business.Entities.OrdersService ordersService,
            Services.Entities.OrdersPLPhotoService ordersPLPhotoService,
            Services.Entities.OrdersPLService ordersPLService,
            Services.Entities.CustomerService customerService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Orders = ordersService;
            OrdersPLPhoto = ordersPLPhotoService;
            OrdersPL = ordersPLService;
            Customer = customerService;

            Configuration = iConfig;
        }
        public IQueryable<Models.Views.PackMark> Get()
        {
            var packMark = (
                from o in Orders.Get()
                join pl in OrdersPL.Get() on new { OrdersId = o.Id, OrderNo = o.OrderNo } equals
                                             new { OrdersId = (decimal)pl.RefOrdersId, OrderNo = pl.OrderNo}
                join m in OrdersPLPhoto.Get() on new { OrdersId = pl.RefOrdersId, LocaleId = pl.LocaleId, RefLocaleId = pl.RefLocaleId, Edition = pl.Edition } equals
                                                 new { OrdersId = (decimal?)m.RefOrdersId, LocaleId = m.LocaleId, RefLocaleId = (decimal?)m.RefLocaleId, Edition = m.Edition }

                select new Models.Views.PackMark
                {
                    Id = m.Id,
                    LocaleId = m.LocaleId,
                    RefLocaleId = m.RefLocaleId,
                    RefOrdersId = m.RefOrdersId,
                    Edition = m.Edition,
                    MarkPhotoURL = m.MarkPhotoURL,
                    MarkDesc = m.MarkDesc,
                    SideMarkPhotoURL = m.SideMarkPhotoURL,
                    SideMarkDesc = m.SideMarkDesc,
                    Add1PhotoURL = m.Add1PhotoURL,
                    Add1Desc = m.Add1Desc,
                    Remark = m.Remark,
                    MarkTitle = m.MarkTitle,
                    SubMarkTitle = m.SubMarkTitle,
                    Add1Title = m.Add1Title,
                    DeliveryAddress = m.DeliveryAddress,
                    LacosteTitle = m.LacosteTitle,

                    RefOrderNo = o.OrderNo,
                    RefCompanyId = o.CompanyId,
                    // RefCompany
                    RefCustomerId = o.CustomerId,
                    // RefCustomer
                    RefBrandCodeId = (decimal)o.BrandCodeId,
                    RefBrand = o.Brand
                }
            );
            return packMark;
        }
        
        public Models.Views.PackMark Create(Models.Views.PackMark packMark)
        {
            var _packMark = OrdersPLPhoto.Create(Build(packMark));
            SaveMarkPhoto(_packMark.MarkPhotoURL, packMark.MarkPhoto);
            SaveMarkPhoto(_packMark.SideMarkPhotoURL, packMark.SideMarkPhoto);
            SaveMarkPhoto(_packMark.Add1PhotoURL, packMark.Add1Photo);
            return Get().Where(i => i.Id == _packMark.Id).FirstOrDefault();
        }
        public Models.Views.PackMark Update(Models.Views.PackMark packMark)
        {
            var _packMark = OrdersPLPhoto.Update(Build(packMark));
            SaveMarkPhoto(_packMark.MarkPhotoURL, packMark.MarkPhoto);
            SaveMarkPhoto(_packMark.SideMarkPhotoURL, packMark.SideMarkPhoto);
            SaveMarkPhoto(_packMark.Add1PhotoURL, packMark.Add1Photo);
            return Get().Where(i => i.Id == _packMark.Id).FirstOrDefault();
        }
        public void Remove(Models.Views.PackMark packMark)
        {
            OrdersPLPhoto.Remove(Build(packMark));
        }
        private Models.Entities.OrdersPLPhoto Build(Models.Views.PackMark item)
        {
            return new Models.Entities.OrdersPLPhoto
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                RefLocaleId = item.RefLocaleId,
                RefOrdersId = item.RefOrdersId,
                Edition = item.Edition,
                MarkPhotoURL = (item.MarkPhoto != null && item.MarkPhoto.Length > 0) ? PhotoRootPath()+"/OrdersPLPhoto/" + item.LocaleId + "_" + item.RefOrdersId + "_" + item.Edition + "_Mark.jpg" : "",
                MarkDesc = item.MarkDesc,
                SideMarkPhotoURL = (item.SideMarkPhoto != null && item.SideMarkPhoto.Length > 0 )? PhotoRootPath()+"/OrdersPLPhoto/" + item.LocaleId + "_" + item.RefOrdersId + "_" + item.Edition + "_SideMark.jpg" : "",
                SideMarkDesc = item.SideMarkDesc,
                Add1PhotoURL = (item.Add1Photo != null && item.Add1Photo.Length > 0) ? PhotoRootPath()+"/OrdersPLPhoto/" + item.LocaleId + "_" + item.RefOrdersId + "_" + item.Edition + "_Add1.jpg" : "",
                Add1Desc = item.Add1Desc,
                Remark = item.Remark,
                MarkTitle = item.MarkTitle,
                SubMarkTitle = item.SubMarkTitle,
                Add1Title = item.Add1Title,
                DeliveryAddress = item.DeliveryAddress,
                LacosteTitle = item.LacosteTitle,
            };
        }

        public Models.Views.PackMark GetSimMark(Models.Views.PackMark parkMark) {
            var simMark = (
                from o in Orders.Get()
                join c in Customer.Get() on new { CustomerId = o.CustomerId, LocaleId = o.LocaleId } equals
                                             new { CustomerId = (decimal)c.Id, LocaleId = c.LocaleId }
                join pl in OrdersPL.Get() on new { OrdersId = o.Id, LocaleId = o.LocaleId, OrderNo = o.OrderNo } equals
                                             new { OrdersId = (decimal)pl.RefOrdersId, LocaleId = pl.LocaleId , OrderNo = pl.OrderNo}
                join m in OrdersPLPhoto.Get() on new { OrdersId = pl.RefOrdersId, LocaleId = pl.LocaleId, RefLocaleId = pl.RefLocaleId, Edition = pl.Edition } equals
                                                 new { OrdersId = (decimal?)m.RefOrdersId, LocaleId = m.LocaleId, RefLocaleId = (decimal?)m.RefLocaleId, Edition = m.Edition }
                where  o.CustomerId == parkMark.RefCustomerId && 
                       o.LocaleId == parkMark.LocaleId && 
                       ( m.MarkTitle.Trim().Length > 0 || m.SubMarkTitle.Trim().Length > 0)&& 
                       o.OrderNo != parkMark.RefOrderNo &&
                       m.Id < parkMark.Id
                // where  o.Customer == parkMark.RefCustomer && o.LocaleId == parkMark.LocaleId && m.MarkTitle.Trim().Length > 0 && o.OrderNo != parkMark.RefOrderNo
                select new Models.Views.PackMark
                {
                    Id = m.Id,
                    LocaleId = m.LocaleId,
                    RefLocaleId = m.RefLocaleId,
                    RefOrdersId = m.RefOrdersId,
                    Edition = m.Edition,
                    MarkPhotoURL = m.MarkPhotoURL,
                    MarkDesc = m.MarkDesc,
                    SideMarkPhotoURL = m.SideMarkPhotoURL,
                    SideMarkDesc = m.SideMarkDesc,
                    Add1PhotoURL = m.Add1PhotoURL,
                    Add1Desc = m.Add1Desc,
                    Remark = m.Remark,
                    MarkTitle = m.MarkTitle,
                    SubMarkTitle = m.SubMarkTitle,
                    Add1Title = m.Add1Title,
                    // DeliveryAddress = m.DeliveryAddress,
                    DeliveryAddress = c.CompanyAddress,
                    LacosteTitle = m.LacosteTitle,

                    RefOrderNo = o.OrderNo,
                    RefCompanyId = o.CompanyId,
                    // RefCompany
                    RefCustomerId = o.CustomerId,
                    // RefCustomer
                    RefBrandCodeId = (decimal)o.BrandCodeId,
                    RefBrand = o.Brand
                }
            ).OrderByDescending(i => i.Id).Take(100).FirstOrDefault();

            return simMark;
        }

        public string GetMarkPhoto(string path)
        {
            string base64Str = "";
            try
            {
                if (path != null && path.Length > 0)
                {
                    path = PathMap(path);

                    if (File.Exists(path))
                    {
                        var bytes = File.ReadAllBytes(path); Console.WriteLine("131:GetMarkPhoto: Length"+bytes.Length);
                        base64Str = "data:image/bmp;base64," + Convert.ToBase64String(bytes);
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("137:GetMarkPhoto:"+e);
                throw e;
            }

            return base64Str;
        }
        public void SaveMarkPhoto(string path, string image)
        {
            if (path == null || image == null || (path.Length == 0 && image.Length == 0)) return;

            byte[] _imageBytes;
            var base64Content = image.Split(',')[1];
            
            try
            {
                path = PathMap(path);
                Console.WriteLine("154:SaveMarkPhoto:"+path);

                _imageBytes = Convert.FromBase64String(base64Content);
                System.IO.File.WriteAllBytes(path, _imageBytes);//檔案時體化    
            }
            catch(Exception e)
            {
                Console.WriteLine("161:SaveMarkPhoto:"+e);
                throw e;
            }
        }
        // private string PathMap(string path)
        // {
        //     if (path != null && path.Length > 0)
        //     {
        //         var companySetting = Configuration.GetSection(Environment.GetEnvironmentVariable("ERP_ENVIRONMENT"));
        //         var disk = companySetting.GetValue<string>("PhotoDisk");
        //         var host = companySetting.GetValue<string>("PhotoHost");
        //         path = path.Replace(disk,host);
        //         path = path.Replace(@"/",@"\");
        //         Console.WriteLine("171:Convert PathMap:"+path);
        //     }
        //     return path;
        // }
        private string PathMap(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                var companySetting = Configuration.GetSection(Environment.GetEnvironmentVariable("ERP_ENVIRONMENT"));
                var disk = companySetting.GetValue<string>("PhotoDisk");
                var host = companySetting.GetValue<string>("PhotoHost");

                // 使用 Regex 忽略大小寫取代 disk 為 host
                path = Regex.Replace(path, Regex.Escape(disk), host, RegexOptions.IgnoreCase);

                // 統一路徑符號為 Windows 的形式
                path = path.Replace("/", "\\");
            }
            // Console.WriteLine(path);
            return path;
        }

        private string PhotoRootPath()
        {
            var companySetting = Configuration.GetSection(Environment.GetEnvironmentVariable("ERP_ENVIRONMENT"));
            var rootPaht = companySetting.GetValue<string>("PhotoDisk");
            return rootPaht;
        }

        public void UpdateByPackPlan(Models.Views.PackPlan packPlan)
        {
            var order = Orders.Get().Where(i => i.Id == packPlan.RefOrdersId && i.OrderNo == packPlan.OrderNo).FirstOrDefault();
            if (order != null)
            {
                var mark = Get().Where(i => i.RefOrdersId == packPlan.RefOrdersId && i.Edition == packPlan.Edition && i.LocaleId == packPlan.LocaleId && i.RefOrderNo == packPlan.OrderNo).FirstOrDefault();
                if (mark == null)
                {
                    // 檢查同一個客戶的訂單，把該訂單的外箱抬頭當預設值匯入
                    // var simOrder = Orders.Get().Where(i => i.LocaleId == order.LocaleId && i.BrandCodeId == order.BrandCodeId && i.CustomerId == order.CustomerId).OrderByDescending(i => i.KeyInDate).FirstOrDefault();
                    // var simMark = Get().Where(i => i.RefOrdersId == simOrder.Id && i.RefOrderNo == simOrder.OrderNo && i.LocaleId == simOrder.LocaleId).FirstOrDefault();
                    var simMark = (
                        from o in Orders.Get()
                        join c in Customer.Get() on new { CustomerId = o.CustomerId, LocaleId = o.LocaleId } equals new { CustomerId = (decimal)c.Id, LocaleId = c.LocaleId }
                        join pl in OrdersPL.Get() on new { OrdersId = o.Id, LocaleId = o.LocaleId, OrderNo = o.OrderNo } equals
                                                    new { OrdersId = (decimal)pl.RefOrdersId, LocaleId = pl.LocaleId , OrderNo = pl.OrderNo}
                        join m in OrdersPLPhoto.Get() on new { OrdersId = pl.RefOrdersId, LocaleId = pl.LocaleId, RefLocaleId = pl.RefLocaleId, Edition = pl.Edition } equals
                                                        new { OrdersId = (decimal?)m.RefOrdersId, LocaleId = m.LocaleId, RefLocaleId = (decimal?)m.RefLocaleId, Edition = m.Edition }
                        where  o.CustomerId == order.CustomerId && o.LocaleId == order.LocaleId && ( m.MarkTitle.Trim().Length > 0 || m.SubMarkTitle.Trim().Length > 0) && o.OrderNo != order.OrderNo
                        select new Models.Views.PackMark
                        {
                            Id = m.Id,
                            LocaleId = m.LocaleId,
                            RefLocaleId = m.RefLocaleId,
                            RefOrdersId = m.RefOrdersId,
                            Edition = m.Edition,
                            MarkPhotoURL = m.MarkPhotoURL,
                            MarkDesc = m.MarkDesc,
                            SideMarkPhotoURL = m.SideMarkPhotoURL,
                            SideMarkDesc = m.SideMarkDesc,
                            Add1PhotoURL = m.Add1PhotoURL,
                            Add1Desc = m.Add1Desc,
                            Remark = m.Remark,
                            MarkTitle = m.MarkTitle,
                            SubMarkTitle = m.SubMarkTitle,
                            Add1Title = m.Add1Title,
                            DeliveryAddress = c.CompanyAddress,
                            LacosteTitle = m.LacosteTitle,
                        }
                    ).OrderByDescending(i => i.Id).Take(100).FirstOrDefault();
                    
                    var packMark = new ERP.Models.Views.PackMark()
                    {
                        LocaleId = packPlan.LocaleId,
                        RefLocaleId = (decimal)packPlan.RefLocaleId,
                        RefOrdersId = (decimal)packPlan.RefOrdersId,
                        Edition = packPlan.Edition,
                        MarkTitle = simMark == null ? "" : simMark.MarkTitle,
                        MarkDesc = order.Mark,
                        SubMarkTitle = simMark == null ? "" : simMark.SubMarkTitle,
                        SideMarkDesc = order.SideMark,
                        Add1Desc = simMark == null ? "" : simMark.Add1Desc,
                        DeliveryAddress = simMark == null ? "" : simMark.DeliveryAddress,
                    };
                    Create(packMark);

                }
            }
        }
        public void RemoveByPackPlan(Models.Views.PackPlan packPlan) {
            var mark = Get().Where(i => i.RefOrdersId == packPlan.RefOrdersId && i.Edition == packPlan.Edition && i.LocaleId == packPlan.LocaleId && i.RefOrderNo == packPlan.OrderNo).FirstOrDefault();
                if (mark != null)
                {
                    Remove(mark);
                }
        }
    }
}