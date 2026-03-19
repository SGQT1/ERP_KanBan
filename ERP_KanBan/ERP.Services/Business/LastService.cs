using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP.Data.Utilities;
using ERP.Services.Bases;

namespace ERP.Services.Business
{
    public class LastService : BusinessService
    {
        private ERP.Services.Business.Entities.LastService Last { get; set; }
        private ERP.Services.Business.Entities.LastItemService LastItem { get; set; }
        private ERP.Services.Business.Entities.CodeItemService CodeItem { get; set; }

        private ERP.Services.Business.CacheService Cache { get; set; }
        public LastService(
            ERP.Services.Business.Entities.LastService lastService,
            ERP.Services.Business.Entities.LastItemService lastItemService,
            ERP.Services.Business.Entities.CodeItemService codeItemService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Last = lastService;
            LastItem = lastItemService;
            CodeItem = codeItemService;
        }

        public ERP.Models.Views.LastGroup Get(int id, int localeId)
        {
            var group = new ERP.Models.Views.LastGroup { };
            var last = Last.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
            if (last != null)
            {
                group.Last = last;
                group.LastItem = LastItem.Get().Where(i => i.LastId == id && i.LocaleId == localeId).OrderBy(i => i.ShoeSizeSortKey).ToList();
            }
            return group;
        }

        public ERP.Models.Views.LastGroup Save(ERP.Models.Views.LastGroup item)
        {
            var last = item.Last;
            var lastItem = item.LastItem.ToList();

            if (last != null)
            {
                try
                {
                    UnitOfWork.BeginTransaction();

                    //Knife
                    {
                        var _last = Last.Get().Where(i => i.LocaleId == last.LocaleId && i.Id == last.Id).FirstOrDefault();

                        if (_last != null)
                        {
                            last.Id = _last.Id;
                            last.LocaleId = _last.LocaleId;
                            last = Last.Update(last);
                        }
                        else
                        {
                            last = Last.Create(last);
                        }
                    }

                    //Knife Item
                    {
                        if (last.Id != 0)
                        {
                            lastItem.ForEach(i => i.LastId = last.Id);

                            LastItem.RemoveRange(i => i.LastId == last.Id && i.LocaleId == last.LocaleId);
                            LastItem.CreateRange(lastItem);
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

            return Get((int)last.Id, (int)last.LocaleId);
        }

        public void Remove(ERP.Models.Views.LastGroup item)
        {
            var last = item.Last;
            UnitOfWork.BeginTransaction();
            try
            {
                LastItem.RemoveRange(i => i.LastId == last.Id && i.LocaleId == last.LocaleId);
                Last.Remove(last);

                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }

        public void CopyLast(string ids, int locale, string user)
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
                    var _lasts = Last.Get().Where(i => _ids.Contains(i.Id) && i.LocaleId == _localeId).ToList();
                    var _lastItems = LastItem.Get().Where(i => _ids.Contains(i.LastId) && i.LocaleId == _localeId).ToList();
                    var _codeItems = CodeItem.Get().Where(i => _codeType.Contains(i.CodeType) && i.LocaleId == locale).ToList();

                    _lasts.ForEach(i =>
                    {
                        var group = new ERP.Models.Views.LastGroup();
                        // convert Last items
                        var _items = _lastItems.Where(ki => ki.LastId == i.Id && ki.LocaleId == i.LocaleId)
                            .Select(i => new ERP.Models.Views.LastItem
                            {
                                Id = 0,
                                LocaleId = locale,
                                LastId = i.LastId,
                                ShoeSize = i.ShoeSize,
                                ShoeSizeSuffix = i.ShoeSizeSuffix,
                                ShoeSizeSortKey = i.ShoeSizeSortKey,
                                Qty = i.Qty,
                                MadeDate = i.MadeDate,
                                Cost = i.Cost,
                            })
                            .ToList();

                        // convert Last
                        i.Id = 0;
                        i.LocaleId = locale;
                        i.ModifyUserName = user;
                        i.MoneyCodeId = i.MoneyCode == null ? 0 : _codeItems.Where(c => c.NameTW == i.MoneyCode && c.CodeType == "02").Max(c => c.Id);
                        i.OwnerCustomerId = null;

                        group.Last = i;
                        group.LastItem = _items;

                        this.Save(group);
                    });
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    
        public IEnumerable<ERP.Models.Views.Last> GetLastNo (string lastNo, int localeId) {
            return Last.Get().Where(i => i.LastNo.ToLower() == lastNo.ToLower() && i.LocaleId == localeId).ToList();
        }
    }
}