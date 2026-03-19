using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Diamond.DataSource.Extensions;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class PartService : BusinessService
    {
        private Services.Entities.PartService Part { get; }

        public PartService(
            Services.Entities.PartService partService,
            UnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.Part = partService;
        }
        public IQueryable<Models.Views.Part> Get()
        {
            return Part.Get().Select(i => new Models.Views.Part
            {
                Id = i.Id,
                PartNo = i.PartNo,
                PartNameTw = i.PartNameTw,
                PartNameEn = i.PartNameEn,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                LocaleId = i.LocaleId,
            });
        }
        public IQueryable<Models.Views.Part> GetCopyPart(string predicate, int localeId)
        {
            var notIn = Part.Get().Where(i => i.LocaleId == localeId).Select(i => i.PartNo).Distinct();
            var parts = Part.Get()
                .Where(new ParsingConfig { CustomTypeProvider = new DynamicLinqProvider() }, predicate)
                .Where(i => !notIn.Contains(i.PartNo))
                .Select(i => new Models.Views.Part
                {
                    Id = i.Id,
                    PartNo = i.PartNo,
                    PartNameTw = i.PartNameTw,
                    PartNameEn = i.PartNameEn,
                    ModifyUserName = i.ModifyUserName,
                    LastUpdateTime = i.LastUpdateTime,
                    LocaleId = i.LocaleId,
                })
                .ToList()
                .AsQueryable();
            return parts;
        }
        public Models.Views.Part Create(Models.Views.Part item)
        {
            var _item = Part.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.Part Update(Models.Views.Part item)
        {
            var _item = Part.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.Part item)
        {
            Part.Remove(Build(item));
        }
        //for update, transfer view model to entity
        private Models.Entities.Part Build(Models.Views.Part item)
        {
            return new Models.Entities.Part()
            {
                Id = item.Id,
                PartNo = item.PartNo,
                PartNameTw = item.PartNameTw,
                PartNameEn = item.PartNameEn,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                LocaleId = item.LocaleId,
            };
        }

    }
}