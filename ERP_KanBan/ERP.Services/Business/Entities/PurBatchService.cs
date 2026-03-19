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
    public class PurBatchService : BusinessService
    {
        private ERP.Services.Entities.PurBatchService PurBatch { get; set; }

        public PurBatchService(
            ERP.Services.Entities.PurBatchService purBatchService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            PurBatch = purBatchService;
        }
        public IQueryable<Models.Views.PurBatch> Get()
        {
            return PurBatch.Get().Select(i => new Models.Views.PurBatch
            {
                Id = i.Id,
                LocaleId = i.LocaleId,
                BatchNo = i.BatchNo,
                BatchDate = i.BatchDate,
                ModifyUserName = i.ModifyUserName,
                LastUpdateTime = i.LastUpdateTime,
                RefLocaleId = i.RefLocaleId,
            });
        }
        public Models.Views.PurBatch Create(Models.Views.PurBatch item)
        {
            var _item = PurBatch.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.PurBatch Update(Models.Views.PurBatch item)
        {
            var _item = PurBatch.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.PurBatch item)
        {
            PurBatch.Remove(Build(item));
        }
        private Models.Entities.PurBatch Build(Models.Views.PurBatch item)
        {
            return new Models.Entities.PurBatch()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                BatchNo = item.BatchNo,
                BatchDate = item.BatchDate,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime,
                RefLocaleId = item.RefLocaleId,
            };
        }

    }
}