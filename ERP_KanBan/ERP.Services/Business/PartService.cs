using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Services.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERP.Models.Views;
using ERP.Services.Business.Entities;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ERP.Services.Business
{
    public class PartService : BusinessService
    {
        private ERP.Services.Business.Entities.PartService Part { get; set; }
        public PartService(
            ERP.Services.Business.Entities.PartService partService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Part = partService;
        }

        public ERP.Models.Views.Part Get(int id, int localeId)
        {
            return Part.Get().Where(i => i.LocaleId == localeId && i.Id == id).FirstOrDefault();
        }
        public IEnumerable<ERP.Models.Views.Part> GetAllParts(int localeId)
        {
            return Part.Get().Where(i => i.LocaleId == localeId).OrderBy(i => i.PartNo).ToList();
        }
        public ERP.Models.Views.Part Save(Part item)
        {
            if (item != null)
            {
                try
                {
                    UnitOfWork.BeginTransaction();
                    {
                        var _part = Part.Get().Where(i => i.LocaleId == item.LocaleId && i.Id == item.Id).FirstOrDefault();

                        if (_part != null)
                        {
                            item.Id = _part.Id;
                            item.LocaleId = _part.LocaleId;
                            item = Part.Update(item);
                        }
                        else
                        {
                            item = Part.Create(item);
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

            return Get((int)item.Id, (int)item.LocaleId);
        }
       
        public void Remove(Part item)
        {
            try
            {
                UnitOfWork.BeginTransaction();
                Part.Remove(item);
                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }

        public void CopyPart(string ids, int locale, string user)
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
                    var _parts = Part.Get().Where(i => _ids.Contains(i.Id) && i.LocaleId == _localeId).ToList();

                    _parts.ForEach(i =>
                    {
                        // convert knife
                        i.Id = 0;
                        i.LocaleId = locale;
                        i.ModifyUserName = user;

                        this.Save(i);
                    });
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<ERP.Models.Views.Part> GetPartNo(string partNo, int localeId)
        {
            return Part.Get().Where(i => i.PartNo.ToLower() == partNo.ToLower() && i.LocaleId == localeId).ToList();
        }
    }
}
