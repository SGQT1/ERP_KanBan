using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Diamond.DataSource.Extensions;
using ERP.Data.Utilities;
using ERP.Services.Bases;

namespace ERP.Services.Business.Entities
{
    public class RDPOService : BusinessService
    {
        private ERP.Services.Entities.ProjectPOService ProjectPO { get; set; }
        public RDPOService(
            ERP.Services.Entities.ProjectPOService projectPOService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            ProjectPO = projectPOService;
        }
        public IQueryable<Models.Views.RDPO> Get()
        {
            var result = ProjectPO.Get()
                .Select(i => new Models.Views.RDPO
                {
                    Id = i.Id,
                    LocaleId = i.LocaleId,
                    ProjectPODate = i.ProjectPODate,
                    Type = i.Type,
                    Remark = i.Remark,
                    ModifyUserName = i.ModifyUserName,
                    LastUpdateTime = i.LastUpdateTime
                });

            return result;
        }
        public Models.Views.RDPO Create(Models.Views.RDPO item)
        {
            var _item = ProjectPO.Create(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public Models.Views.RDPO Update(Models.Views.RDPO item)
        {
            var _item = ProjectPO.Update(Build(item));
            return Get().Where(i => i.Id == _item.Id && i.LocaleId == _item.LocaleId).FirstOrDefault();
        }
        public void Remove(Models.Views.RDPO item)
        {
            ProjectPO.Remove(Build(item));
        }
        private Models.Entities.ProjectPO Build(Models.Views.RDPO item)
        {
            return new Models.Entities.ProjectPO()
            {
                Id = item.Id,
                LocaleId = item.LocaleId,
                ProjectPODate = item.ProjectPODate,
                Type = item.Type,
                Remark = item.Remark,
                ModifyUserName = item.ModifyUserName,
                LastUpdateTime = item.LastUpdateTime
            };
        }


    }
}