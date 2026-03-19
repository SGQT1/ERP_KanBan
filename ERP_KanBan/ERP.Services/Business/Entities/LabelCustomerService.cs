using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;
using Microsoft.Extensions.Configuration;

namespace ERP.Services.Business.Entities
{
    public class LabelCustomerService : BusinessService
    {
        public IConfiguration Configuration { get; }
        private Services.Entities.LabelCustomerService LabelCustomer { get; }

        public LabelCustomerService(
            IConfiguration iConfig,
            Services.Entities.LabelCustomerService labelCustomerService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.Configuration = iConfig;
            this.LabelCustomer = labelCustomerService;
        }
        public IQueryable<Models.Views.LabelCustomer> Get()
        {
            return LabelCustomer.Get().Select(i => new Models.Views.LabelCustomer
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                CustomerId = i.CustomerId,
                CustomerName = i.CustomerName,
                BrandCodeId = i.BrandCodeId,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                LabelCustomer01 = i.LabelCustomer01,
                LabelCustomer01PhotoURL = i.LabelCustomer01PhotoURL,
                LabelCustomer02 = i.LabelCustomer02,
                LabelCustomer02PhotoURL = i.LabelCustomer02PhotoURL,
                LabelCustomer03 = i.LabelCustomer03,
                LabelCustomer03PhotoURL = i.LabelCustomer03PhotoURL,
                LabelCustomer04 = i.LabelCustomer04,
                LabelCustomer04PhotoURL = i.LabelCustomer04PhotoURL,
                LabelCustomer05 = i.LabelCustomer05,
                LabelCustomer05PhotoURL = i.LabelCustomer05PhotoURL,
                LabelCustomer06 = i.LabelCustomer06,
                LabelCustomer06PhotoURL = i.LabelCustomer06PhotoURL,
                LabelCustomer07 = i.LabelCustomer07,
                LabelCustomer07PhotoURL = i.LabelCustomer07PhotoURL,
                LabelCustomer08 = i.LabelCustomer08,
                LabelCustomer08PhotoURL = i.LabelCustomer08PhotoURL,
                LabelCustomer09 = i.LabelCustomer09,
                LabelCustomer09PhotoURL = i.LabelCustomer09PhotoURL,
                LabelCustomer10 = i.LabelCustomer10,
                LabelCustomer10PhotoURL = i.LabelCustomer10PhotoURL,
                LabelCustomer11 = i.LabelCustomer11,
                LabelCustomer11PhotoURL = i.LabelCustomer11PhotoURL,
                LabelCustomer12 = i.LabelCustomer12,
                LabelCustomer12PhotoURL = i.LabelCustomer12PhotoURL,
                LabelCustomer13 = i.LabelCustomer13,
                LabelCustomer13PhotoURL = i.LabelCustomer13PhotoURL,
                LabelCustomer14 = i.LabelCustomer14,
                LabelCustomer14PhotoURL = i.LabelCustomer14PhotoURL,
                LabelCustomer15 = i.LabelCustomer15,
                LabelCustomer15PhotoURL = i.LabelCustomer15PhotoURL,
                PackDesc = i.PackDesc,
                PackDescPhotoURL = i.PackDescPhotoURL,
                PackDescEng = i.PackDescEng,
                PackDescEngPhotoURL = i.PackDescEngPhotoURL
            });
        }
        public Models.Views.LabelCustomer Create(Models.Views.LabelCustomer item)
        {
            var _item = LabelCustomer.Create(Build(item));

            // 暫時
            SavePhoto(_item.LabelCustomer01PhotoURL, item.LabelCustomer01Photo);
            SavePhoto(_item.LabelCustomer02PhotoURL, item.LabelCustomer02Photo);
            SavePhoto(_item.LabelCustomer03PhotoURL, item.LabelCustomer03Photo);
            SavePhoto(_item.LabelCustomer04PhotoURL, item.LabelCustomer04Photo);
            SavePhoto(_item.LabelCustomer05PhotoURL, item.LabelCustomer05Photo);
            SavePhoto(_item.LabelCustomer06PhotoURL, item.LabelCustomer06Photo);
            SavePhoto(_item.LabelCustomer07PhotoURL, item.LabelCustomer07Photo);
            SavePhoto(_item.LabelCustomer08PhotoURL, item.LabelCustomer08Photo);
            SavePhoto(_item.LabelCustomer09PhotoURL, item.LabelCustomer09Photo);
            SavePhoto(_item.LabelCustomer10PhotoURL, item.LabelCustomer10Photo);
            SavePhoto(_item.LabelCustomer11PhotoURL, item.LabelCustomer11Photo);
            SavePhoto(_item.LabelCustomer12PhotoURL, item.LabelCustomer12Photo);
            SavePhoto(_item.LabelCustomer13PhotoURL, item.LabelCustomer13Photo);
            SavePhoto(_item.LabelCustomer14PhotoURL, item.LabelCustomer14Photo);
            SavePhoto(_item.LabelCustomer15PhotoURL, item.LabelCustomer15Photo);

            SavePhoto(_item.PackDescPhotoURL, item.PackDescPhoto);
            SavePhoto(_item.PackDescEngPhotoURL, item.PackDescEngPhoto);

            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.LabelCustomer Update(Models.Views.LabelCustomer item)
        {
            var _item = LabelCustomer.Update(Build(item));
            // 暫時
            SavePhoto(_item.LabelCustomer01PhotoURL, item.LabelCustomer01Photo);
            SavePhoto(_item.LabelCustomer02PhotoURL, item.LabelCustomer02Photo);
            SavePhoto(_item.LabelCustomer03PhotoURL, item.LabelCustomer03Photo);
            SavePhoto(_item.LabelCustomer04PhotoURL, item.LabelCustomer04Photo);
            SavePhoto(_item.LabelCustomer05PhotoURL, item.LabelCustomer05Photo);
            SavePhoto(_item.LabelCustomer06PhotoURL, item.LabelCustomer06Photo);
            SavePhoto(_item.LabelCustomer07PhotoURL, item.LabelCustomer07Photo);
            SavePhoto(_item.LabelCustomer08PhotoURL, item.LabelCustomer08Photo);
            SavePhoto(_item.LabelCustomer09PhotoURL, item.LabelCustomer09Photo);
            SavePhoto(_item.LabelCustomer10PhotoURL, item.LabelCustomer10Photo);
            SavePhoto(_item.LabelCustomer11PhotoURL, item.LabelCustomer11Photo);
            SavePhoto(_item.LabelCustomer12PhotoURL, item.LabelCustomer12Photo);
            SavePhoto(_item.LabelCustomer13PhotoURL, item.LabelCustomer13Photo);
            SavePhoto(_item.LabelCustomer14PhotoURL, item.LabelCustomer14Photo);
            SavePhoto(_item.LabelCustomer15PhotoURL, item.LabelCustomer15Photo);

            SavePhoto(_item.PackDescPhotoURL, item.PackDescPhoto);
            SavePhoto(_item.PackDescEngPhotoURL, item.PackDescEngPhoto);

            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.LabelCustomer item)
        {
            LabelCustomer.Remove(Build(item));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.LabelCustomer, bool>> predicate)
        {
            LabelCustomer.RemoveRange(predicate);
        }
        //for update, transfer view model to entity
        private Models.Entities.LabelCustomer Build(Models.Views.LabelCustomer item)
        {
            return new Models.Entities.LabelCustomer()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                CustomerId = item.CustomerId,
                CustomerName = item.CustomerName,
                BrandCodeId = item.BrandCodeId,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                LabelCustomer01 = item.LabelCustomer01,
                LabelCustomer01PhotoURL = (item.LabelCustomer01Photo != null && item.LabelCustomer01Photo.Length > 0) ? PhotoRootPath() + "/LabelCustomer/" + item.LocaleId + "_" + item.CustomerId + "_LabelCustomer01.jpg" : "",
                LabelCustomer02 = item.LabelCustomer02,
                LabelCustomer02PhotoURL = (item.LabelCustomer02Photo != null && item.LabelCustomer02Photo.Length > 0) ? PhotoRootPath() + "/LabelCustomer/" + item.LocaleId + "_" + item.CustomerId + "_LabelCustomer02.jpg" : "",
                LabelCustomer03 = item.LabelCustomer03,
                LabelCustomer03PhotoURL = (item.LabelCustomer03Photo != null && item.LabelCustomer03Photo.Length > 0) ? PhotoRootPath() + "/LabelCustomer/" + item.LocaleId + "_" + item.CustomerId + "_LabelCustomer03.jpg" : "",
                LabelCustomer04 = item.LabelCustomer04,
                LabelCustomer04PhotoURL = (item.LabelCustomer04Photo != null && item.LabelCustomer04Photo.Length > 0) ? PhotoRootPath() + "/LabelCustomer/" + item.LocaleId + "_" + item.CustomerId + "_LabelCustomer04.jpg" : "",
                LabelCustomer05 = item.LabelCustomer05,
                LabelCustomer05PhotoURL = (item.LabelCustomer05Photo != null && item.LabelCustomer05Photo.Length > 0) ? PhotoRootPath() + "/LabelCustomer/" + item.LocaleId + "_" + item.CustomerId + "_LabelCustomer05.jpg" : "",
                LabelCustomer06 = item.LabelCustomer06,
                LabelCustomer06PhotoURL = (item.LabelCustomer06Photo != null && item.LabelCustomer06Photo.Length > 0) ? PhotoRootPath() + "/LabelCustomer/" + item.LocaleId + "_" + item.CustomerId + "_LabelCustomer06.jpg" : "",
                LabelCustomer07 = item.LabelCustomer07,
                LabelCustomer07PhotoURL = (item.LabelCustomer07Photo != null && item.LabelCustomer07Photo.Length > 0) ? PhotoRootPath() + "/LabelCustomer/" + item.LocaleId + "_" + item.CustomerId + "_LabelCustomer07.jpg" : "",
                LabelCustomer08 = item.LabelCustomer08,
                LabelCustomer08PhotoURL = (item.LabelCustomer08Photo != null && item.LabelCustomer08Photo.Length > 0) ? PhotoRootPath() + "/LabelCustomer/" + item.LocaleId + "_" + item.CustomerId + "_LabelCustomer08.jpg" : "",
                LabelCustomer09 = item.LabelCustomer09,
                LabelCustomer09PhotoURL = (item.LabelCustomer09Photo != null && item.LabelCustomer09Photo.Length > 0) ? PhotoRootPath() + "/LabelCustomer/" + item.LocaleId + "_" + item.CustomerId + "_LabelCustomer09.jpg" : "",
                LabelCustomer10 = item.LabelCustomer10,
                LabelCustomer10PhotoURL = (item.LabelCustomer10Photo != null && item.LabelCustomer10Photo.Length > 0) ? PhotoRootPath() + "/LabelCustomer/" + item.LocaleId + "_" + item.CustomerId + "_LabelCustomer10.jpg" : "",
                LabelCustomer11 = item.LabelCustomer11,
                LabelCustomer11PhotoURL = (item.LabelCustomer11Photo != null && item.LabelCustomer11Photo.Length > 0) ? PhotoRootPath() + "/LabelCustomer/" + item.LocaleId + "_" + item.CustomerId + "_LabelCustomer11.jpg" : "",
                LabelCustomer12 = item.LabelCustomer12,
                LabelCustomer12PhotoURL = (item.LabelCustomer12Photo != null && item.LabelCustomer12Photo.Length > 0) ? PhotoRootPath() + "/LabelCustomer/" + item.LocaleId + "_" + item.CustomerId + "_LabelCustomer12.jpg" : "",
                LabelCustomer13 = item.LabelCustomer13,
                LabelCustomer13PhotoURL = (item.LabelCustomer13Photo != null && item.LabelCustomer13Photo.Length > 0) ? PhotoRootPath() + "/LabelCustomer/" + item.LocaleId + "_" + item.CustomerId + "_LabelCustomer13.jpg" : "",
                LabelCustomer14 = item.LabelCustomer14,
                LabelCustomer14PhotoURL = (item.LabelCustomer14Photo != null && item.LabelCustomer14Photo.Length > 0) ? PhotoRootPath() + "/LabelCustomer/" + item.LocaleId + "_" + item.CustomerId + "_LabelCustomer14.jpg" : "",
                LabelCustomer15 = item.LabelCustomer15,
                LabelCustomer15PhotoURL = (item.LabelCustomer15Photo != null && item.LabelCustomer15Photo.Length > 0) ? PhotoRootPath() + "/LabelCustomer/" + item.LocaleId + "_" + item.CustomerId + "_LabelCustomer15.jpg" : "",
                PackDesc = item.PackDesc,
                PackDescPhotoURL = (item.PackDescPhoto != null && item.PackDescPhoto.Length > 0) ? PhotoRootPath() + "/LabelCustomer/" + item.LocaleId + "_" + item.CustomerId + "_PackDescPhoto.jpg" : "",
                PackDescEng = item.PackDescEng,
                PackDescEngPhotoURL = (item.PackDescEngPhoto != null && item.PackDescEngPhoto.Length > 0) ? PhotoRootPath() + "/LabelCustomer/" + item.LocaleId + "_" + item.CustomerId + "_PackDescEng.jpg" : "",
            };
        }

        public string GetPhoto(string path)
        {
            string base64Str = "";
            try
            {
                if (path != null && path.Length > 0)
                {
                    path = PathMap(path);

                    if (File.Exists(path))
                    {
                        var bytes = File.ReadAllBytes(path);
                        base64Str = "data:image/bmp;base64," + Convert.ToBase64String(bytes);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return base64Str;
        }
        public void SavePhoto(string path, string image)
        {
            if (path == null || image == null || (path.Length == 0 && image.Length == 0)) return;

            byte[] _imageBytes;
            var base64Content = image.Split(',')[1];

            try
            {
                path = PathMap(path);

                _imageBytes = Convert.FromBase64String(base64Content);
                System.IO.File.WriteAllBytes(path, _imageBytes);//檔案時體化    
            }
            catch (Exception e)
            {
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
        //         path = path.Replace(disk, host);
        //         path = path.Replace(@"/", @"\");
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
    }
}