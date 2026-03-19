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
    public class LabelArticleService : BusinessService
    {
        public IConfiguration Configuration { get; }
        private Services.Entities.LabelArticleService LabelArticle { get; }

        public LabelArticleService(
            IConfiguration iConfig,
            Services.Entities.LabelArticleService labelArticleService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.Configuration = iConfig;
            this.LabelArticle = labelArticleService;
        }
        public IQueryable<Models.Views.LabelArticle> Get()
        {
            return LabelArticle.Get().Select(i => new Models.Views.LabelArticle
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                ArticleId = i.ArticleId,
                ArticleNo = i.ArticleNo,
                ArticleName = i.ArticleName,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                LabelArticle01 = i.LabelArticle01,
                LabelArticle01PhotoURL = i.LabelArticle01PhotoURL,
                LabelArticle02 = i.LabelArticle02,
                LabelArticle02PhotoURL = i.LabelArticle02PhotoURL,
                LabelArticle03 = i.LabelArticle03,
                LabelArticle03PhotoURL = i.LabelArticle03PhotoURL,
                LabelArticle04 = i.LabelArticle04,
                LabelArticle04PhotoURL = i.LabelArticle04PhotoURL,
                LabelArticle05 = i.LabelArticle05,
                LabelArticle05PhotoURL = i.LabelArticle05PhotoURL,
                LabelArticle06 = i.LabelArticle06,
                LabelArticle06PhotoURL = i.LabelArticle06PhotoURL,
                LabelArticle07 = i.LabelArticle07,
                LabelArticle07PhotoURL = i.LabelArticle07PhotoURL,
                LabelArticle08 = i.LabelArticle08,
                LabelArticle08PhotoURL = i.LabelArticle08PhotoURL,
                LabelArticle09 = i.LabelArticle09,
                LabelArticle09PhotoURL = i.LabelArticle09PhotoURL,
            });
        }
        public Models.Views.LabelArticle Create(Models.Views.LabelArticle item)
        {
            var _item = LabelArticle.Create(Build(item));
            // 暫時
            SavePhoto(_item.LabelArticle01PhotoURL, item.LabelArticle01Photo);
            SavePhoto(_item.LabelArticle02PhotoURL, item.LabelArticle02Photo);
            SavePhoto(_item.LabelArticle03PhotoURL, item.LabelArticle03Photo);
            SavePhoto(_item.LabelArticle04PhotoURL, item.LabelArticle04Photo);
            SavePhoto(_item.LabelArticle05PhotoURL, item.LabelArticle05Photo);
            SavePhoto(_item.LabelArticle06PhotoURL, item.LabelArticle06Photo);
            SavePhoto(_item.LabelArticle07PhotoURL, item.LabelArticle07Photo);
            SavePhoto(_item.LabelArticle08PhotoURL, item.LabelArticle08Photo);
            SavePhoto(_item.LabelArticle09PhotoURL, item.LabelArticle09Photo);
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.LabelArticle Update(Models.Views.LabelArticle item)
        {
            var _item = LabelArticle.Update(Build(item));
            SavePhoto(_item.LabelArticle01PhotoURL, item.LabelArticle01Photo);
            SavePhoto(_item.LabelArticle02PhotoURL, item.LabelArticle02Photo);
            SavePhoto(_item.LabelArticle03PhotoURL, item.LabelArticle03Photo);
            SavePhoto(_item.LabelArticle04PhotoURL, item.LabelArticle04Photo);
            SavePhoto(_item.LabelArticle05PhotoURL, item.LabelArticle05Photo);
            SavePhoto(_item.LabelArticle06PhotoURL, item.LabelArticle06Photo);
            SavePhoto(_item.LabelArticle07PhotoURL, item.LabelArticle07Photo);
            SavePhoto(_item.LabelArticle08PhotoURL, item.LabelArticle08Photo);
            SavePhoto(_item.LabelArticle09PhotoURL, item.LabelArticle09Photo);
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.LabelArticle item)
        {
            LabelArticle.Remove(Build(item));
        }
        public void RemoveRange(Expression<Func<ERP.Models.Entities.LabelArticle, bool>> predicate)
        {
            LabelArticle.RemoveRange(predicate);
        }
        //for update, transfer view model to entity
        private Models.Entities.LabelArticle Build(Models.Views.LabelArticle item)
        {
            return new Models.Entities.LabelArticle()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                ArticleId = item.ArticleId,
                ArticleNo = item.ArticleNo,
                ArticleName = item.ArticleName,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                LabelArticle01 = item.LabelArticle01,
                LabelArticle01PhotoURL = (item.LabelArticle01Photo != null && item.LabelArticle01Photo.Length > 0) ? PhotoRootPath() + "/LabelArticle/" + item.LocaleId + "_" + item.ArticleId + "_LabelArticle01.jpg" : "",
                LabelArticle02 = item.LabelArticle02,
                LabelArticle02PhotoURL = (item.LabelArticle02Photo != null && item.LabelArticle02Photo.Length > 0) ? PhotoRootPath() + "/LabelArticle/" + item.LocaleId + "_" + item.ArticleId + "_LabelArticle02.jpg" : "",
                LabelArticle03 = item.LabelArticle03,
                LabelArticle03PhotoURL = (item.LabelArticle03Photo != null && item.LabelArticle03Photo.Length > 0) ? PhotoRootPath() + "/LabelArticle/" + item.LocaleId + "_" + item.ArticleId + "_LabelArticle03.jpg" : "",
                LabelArticle04 = item.LabelArticle04,
                LabelArticle04PhotoURL = (item.LabelArticle04Photo != null && item.LabelArticle04Photo.Length > 0) ? PhotoRootPath() + "/LabelArticle/" + item.LocaleId + "_" + item.ArticleId + "_LabelArticle04.jpg" : "",
                LabelArticle05 = item.LabelArticle05,
                LabelArticle05PhotoURL = (item.LabelArticle05Photo != null && item.LabelArticle05Photo.Length > 0) ? PhotoRootPath() + "/LabelArticle/" + item.LocaleId + "_" + item.ArticleId + "_LabelArticle05.jpg" : "",
                LabelArticle06 = item.LabelArticle06,
                LabelArticle06PhotoURL = (item.LabelArticle06Photo != null && item.LabelArticle06Photo.Length > 0) ? PhotoRootPath() + "/LabelArticle/" + item.LocaleId + "_" + item.ArticleId + "_LabelArticle06.jpg" : "",
                LabelArticle07 = item.LabelArticle07,
                LabelArticle07PhotoURL = (item.LabelArticle07Photo != null && item.LabelArticle07Photo.Length > 0) ? PhotoRootPath() + "/LabelArticle/" + item.LocaleId + "_" + item.ArticleId + "_LabelArticle07.jpg" : "",
                LabelArticle08 = item.LabelArticle08,
                LabelArticle08PhotoURL = (item.LabelArticle08Photo != null && item.LabelArticle08Photo.Length > 0) ? PhotoRootPath() + "/LabelArticle/" + item.LocaleId + "_" + item.ArticleId + "_LabelArticle08.jpg" : "",
                LabelArticle09 = item.LabelArticle09,
                LabelArticle09PhotoURL = (item.LabelArticle09Photo != null && item.LabelArticle09Photo.Length > 0) ? PhotoRootPath() + "/LabelArticle/" + item.LocaleId + "_" + item.ArticleId + "_LabelArticle09.jpg" : "",
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