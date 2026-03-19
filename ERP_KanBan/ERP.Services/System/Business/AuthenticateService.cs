using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Services.Bases;
using ERP.Services.Business.Entities;

namespace ERP.Services.Business
{
    public class AuthenticateService : BusinessService
    {
        private ERP.Services.Business.Entities.UserService User { get; set; }
        private ERP.Services.Business.Entities.CodeItemService CodeItem { get; set; }
        public AuthenticateService(
            ERP.Services.Business.Entities.UserService userService,
            ERP.Services.Business.Entities.CodeItemService codeItemService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            User = userService;
            CodeItem = codeItemService;
        }

        public TokenGroup Authenticate(string account, string password, int localeId)
        {
            try
            {
                var user = User.Get().Where(i => i.Id.ToLower() == account.ToLower() && i.Password == password && i.Validate == true).FirstOrDefault();
                if (user == null)
                {
                    return null;
                }

                var brands = CodeItem.Get().Where(i => i.LocaleId == localeId && i.CodeType == "25" && i.Disable == false)
                    // .Where(i =>
                    //     i.LocaleId == localeId &&
                    //     (i.NameTW == "PUMA" ||
                    //         i.NameTW == "Mizuno" ||
                    //         i.NameTW == "NB" ||
                    //         i.NameTW == "NEW BALANCE" ||
                    //         i.NameTW == "APOS" ||
                    //         i.NameTW == "HUTCH" ||
                    //         i.NameTW == "IRON STEEL" ||
                    //         i.NameTW == "ON RUNNING" ||
                    //         i.NameTW == "PATRONI" ||
                    //         i.NameTW == "LE COQ SPORTIF" ||
                    //         i.NameTW == "UMBRO" ||
                    //         i.NameTW == "QIAODAN"||
                    //         i.NameTW == "DESCENTE")
                    // )
                    .Select(i => i.Id).ToList();
                return new TokenGroup()
                {
                    Token = DateTime.Now.AddHours(8).Ticks.ToString(),
                    User = new User { Id = user.Id.ToUpper(), Email = user.Email, EnglishName = user.EnglishName, ChineseName = user.ChineseName, POTeam = user.POTeam },
                    Brands = brands
                };
            }
            catch (Exception e)
            {
                Console.WriteLine("Authenticate:" + e.StackTrace);
                throw e;
            }

        }
    }
}