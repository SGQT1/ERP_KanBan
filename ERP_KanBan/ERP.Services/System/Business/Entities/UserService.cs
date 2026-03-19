using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;
using System.IO;
using System;
using System.Configuration;

namespace ERP.Services.Business.Entities
{
    public class UserService : BusinessService
    {
        private Services.Entities.UserService User { get; }
        public IConfiguration Configuration { get; }
        public UserService(
            IConfiguration iConfig,
            Services.Entities.UserService userService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            User = userService;
            Configuration = iConfig;
        }
        public IQueryable<Models.Views.User> Get()
        {
            return User.Get().Select(i => new Models.Views.User
            {
                Id = i.ID,
                Password = i.PWD,
                Agent = i.AGENT,
                Email = i.EMAIL,
                Remark = i.TEMP1,
                ChineseName = i.NameTw,
                EnglishName = i.NameEn,
                POTeam = i.POTeam,
                Sign = i.SIGN,
                Validate = i.Validate == true ? true : false,
            });
        }

        public Models.Views.User Create(Models.Views.User item)
        {
            var _item = User.Create(Build(item));
            SavePhoto(_item.SIGN, item.SignPhoto);

            return Get().Where(i => i.Id == _item.ID).FirstOrDefault();
        }
        public Models.Views.User Update(Models.Views.User item)
        {
            var _item = User.Update(Build(item));
            SavePhoto(_item.SIGN, item.SignPhoto);

            return Get().Where(i => i.Id == _item.ID).FirstOrDefault();
        }
        public void Remove(Models.Views.User item)
        {
            User.Remove(Build(item));
        }
        private Models.Entities.users Build(Models.Views.User item)
        {
            var sign = (item.SignPhoto != null && item.SignPhoto.Length > 0) ? PhotoRootPath() + "/users/" + "_" + item.Id.ToLower() + "_user.jpg" : "";
            return new Models.Entities.users()
            {
                ID = item.Id,
                PWD = item.Password,
                AGENT = item.Agent,
                EMAIL = item.Email,
                TEMP1 = item.Remark,
                NameTw = item.ChineseName,
                NameEn = item.EnglishName,
                POTeam = item.POTeam,
                // SIGN = item.Sign,
                SIGN = (item.SignPhoto != null && item.SignPhoto.Length > 0) ? PhotoRootPath() + "/users/" + "_" + item.Id.ToLower() + "_user.jpg" : "",
                // Photo1URL = (item.Photo1Photo != null && item.Photo1Photo.Length > 0) ? PhotoRootPath() + "/style/" + item.LocaleId + "_" + item.Id + "_style01.jpg" : "",
                Validate = item.Validate == true ? true : false,
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