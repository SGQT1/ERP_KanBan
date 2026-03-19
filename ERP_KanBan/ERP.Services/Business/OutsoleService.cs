using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP.Data.Utilities;
using ERP.Services.Bases;

namespace ERP.Services.Business
{
    public class OutsoleService : BusinessService
    {
        private ERP.Services.Business.Entities.OutsoleService Outsole { get; set; }
        private ERP.Services.Business.Entities.OutsoleItemService OutsoleItem { get; set; }
        private ERP.Services.Business.Entities.CodeItemService CodeItem { get; set; }

        private ERP.Services.Business.CacheService Cache { get; set; }
        public OutsoleService(
            ERP.Services.Business.Entities.OutsoleService outsoleService,
            ERP.Services.Business.Entities.OutsoleItemService outsoleItemService,
            ERP.Services.Business.Entities.CodeItemService codeItemService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Outsole = outsoleService;
            OutsoleItem = outsoleItemService;
            CodeItem = codeItemService;
        }

        public ERP.Models.Views.OutsoleGroup Get(int id, int localeId)
        {
            var group = new ERP.Models.Views.OutsoleGroup { };
            var outsole = Outsole.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
            if (outsole != null)
            {
                group.Outsole = outsole;
                group.OutsoleItem = OutsoleItem.Get().Where(i => i.OutsoleId == id && i.LocaleId == localeId).OrderBy(i => i.ShoeSizeSortKey).ToList();
            }
            return group;
        }

        public ERP.Models.Views.OutsoleGroup Save(ERP.Models.Views.OutsoleGroup item)
        {
            var outsole = item.Outsole;
            var outsoleItem = item.OutsoleItem.ToList();

            if (outsole != null)
            {
                try
                {
                    UnitOfWork.BeginTransaction();

                    //Outsole
                    {
                        var _knife = Outsole.Get().Where(i => i.LocaleId == outsole.LocaleId && i.Id == outsole.Id).FirstOrDefault();

                        if (_knife != null)
                        {
                            outsole.Id = _knife.Id;
                            outsole.LocaleId = _knife.LocaleId;
                            outsole = Outsole.Update(outsole);
                        }
                        else
                        {
                            outsole = Outsole.Create(outsole);
                        }
                    }

                    //Outsole Item
                    {
                        if (outsole.Id != 0)
                        {
                            outsoleItem.ForEach(i => i.OutsoleId = outsole.Id);

                            OutsoleItem.RemoveRange(i => i.OutsoleId == outsole.Id && i.LocaleId == outsole.LocaleId);
                            OutsoleItem.CreateRange(outsoleItem);
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

            return Get((int)outsole.Id, (int)outsole.LocaleId);
        }

        public void Remove(ERP.Models.Views.OutsoleGroup item)
        {
            var outsole = item.Outsole;
            UnitOfWork.BeginTransaction();
            try
            {
                OutsoleItem.RemoveRange(i => i.OutsoleId == outsole.Id && i.LocaleId == outsole.LocaleId);
                Outsole.Remove(outsole);

                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }

        public void CopyOutsole(string ids, int locale, string user)
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
                    var _codeType = new string[] { "02" };
                    var _outsoles = Outsole.Get().Where(i => _ids.Contains(i.Id) && i.LocaleId == _localeId).ToList();
                    var _outsoleItems = OutsoleItem.Get().Where(i => _ids.Contains(i.OutsoleId) && i.LocaleId == _localeId).ToList();
                    var _codeItems = CodeItem.Get().Where(i => _codeType.Contains(i.CodeType) && i.LocaleId == locale).ToList();

                    _outsoles.ForEach(i =>
                    {
                        var group = new ERP.Models.Views.OutsoleGroup();
                        // convert Outsole items
                        var _items = _outsoleItems.Where(oi => oi.OutsoleId == i.Id && oi.LocaleId == i.LocaleId)
                            .Select(i => new ERP.Models.Views.OutsoleItem
                            {
                                Id = 0,
                                LocaleId = locale,
                                OutsoleId = i.OutsoleId,
                                ShoeSize = i.ShoeSize,
                                ShoeSizeSuffix = i.ShoeSizeSuffix,
                                ShoeSizeSortKey = i.ShoeSizeSortKey,
                                Qty = i.Qty,
                                MadeDate = i.MadeDate,
                                Cost = i.Cost,
                                Map2EVASize = i.Map2EVASize,
                                Map2MDSize = i.Map2MDSize
                            })
                            .ToList();

                        // convert knife
                        i.Id = 0;
                        i.LocaleId = locale;
                        i.ModifyUserName = user;
                        i.MoneyCodeId = i.MoneyCode == null ? 0 : _codeItems.Where(c => c.NameTW == i.MoneyCode && c.CodeType == "02").Max(c => c.Id);
                        i.OwnerCustomerId = null;

                        group.Outsole = i;
                        group.OutsoleItem = _items;

                        this.Save(group);
                    });
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    
        public IEnumerable<ERP.Models.Views.Outsole> GetOutsoleNo (string OutsoleNo, int localeId) {
            return Outsole.Get().Where(i => i.OutsoleNo.ToLower() == OutsoleNo.ToLower() && i.LocaleId == localeId).ToList();
        }
    }
}