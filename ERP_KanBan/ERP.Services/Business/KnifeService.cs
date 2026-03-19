using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP.Data.Utilities;
using ERP.Services.Bases;

namespace ERP.Services.Business
{
    public class KnifeService : BusinessService
    {
        private ERP.Services.Business.Entities.KnifeService Knife { get; set; }
        private ERP.Services.Business.Entities.KnifeItemService KnifeItem { get; set; }
        private ERP.Services.Business.Entities.CodeItemService CodeItem { get; set; }

        private ERP.Services.Business.CacheService Cache { get; set; }
        public KnifeService(
            ERP.Services.Business.Entities.KnifeService knifeService,
            ERP.Services.Business.Entities.KnifeItemService knifeItemService,
            ERP.Services.Business.Entities.CodeItemService codeItemService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Knife = knifeService;
            KnifeItem = knifeItemService;
            CodeItem = codeItemService;
        }

        public ERP.Models.Views.KnifeGroup Get(int id, int localeId)
        {
            var group = new ERP.Models.Views.KnifeGroup { };
            var knife = Knife.Get().Where(i => i.Id == id && i.LocaleId == localeId).FirstOrDefault();
            if (knife != null)
            {
                group.Knife = knife;
                group.KnifeItem = KnifeItem.Get().Where(i => i.KnifeId == id && i.LocaleId == localeId).OrderBy(i => i.ShoeSizeSortKey).ToList();
            }
            return group;
        }

        public ERP.Models.Views.KnifeGroup Save(ERP.Models.Views.KnifeGroup item)
        {
            var knife = item.Knife;
            var knifeItem = item.KnifeItem.ToList();

            if (knife != null)
            {
                try
                {
                    UnitOfWork.BeginTransaction();

                    //Knife
                    {
                        var _knife = Knife.Get().Where(i => i.LocaleId == knife.LocaleId && i.Id == knife.Id).FirstOrDefault();

                        if (_knife != null)
                        {
                            knife.Id = _knife.Id;
                            knife.LocaleId = _knife.LocaleId;
                            knife = Knife.Update(knife);
                        }
                        else
                        {
                            knife = Knife.Create(knife);
                        }
                    }

                    //Knife Item
                    {
                        if (knife.Id != 0)
                        {
                            knifeItem.ForEach(i => i.KnifeId = knife.Id);

                            KnifeItem.RemoveRange(i => i.KnifeId == knife.Id && i.LocaleId == knife.LocaleId);
                            KnifeItem.CreateRange(knifeItem);
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

            return Get((int)knife.Id, (int)knife.LocaleId);
        }

        public void Remove(ERP.Models.Views.KnifeGroup item)
        {
            var knife = item.Knife;
            UnitOfWork.BeginTransaction();
            try
            {
                KnifeItem.RemoveRange(i => i.KnifeId == knife.Id && i.LocaleId == knife.LocaleId);
                Knife.Remove(knife);

                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }

        public void CopyKnife(string ids, int locale, string user)
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
                    var _knifes = Knife.Get().Where(i => _ids.Contains(i.Id) && i.LocaleId == _localeId).ToList();
                    var _knifeItems = KnifeItem.Get().Where(i => _ids.Contains(i.KnifeId) && i.LocaleId == _localeId).ToList();
                    var _codeItems = CodeItem.Get().Where(i => _codeType.Contains(i.CodeType) && i.LocaleId == locale).ToList();

                    _knifes.ForEach(i =>
                    {
                        var group = new ERP.Models.Views.KnifeGroup();
                        // convert knife items
                        var _items = _knifeItems.Where(ki => ki.KnifeId == i.Id && ki.LocaleId == i.LocaleId)
                            .Select(i => new ERP.Models.Views.KnifeItem
                            {
                                Id = 0,
                                LocaleId = locale,
                                KnifeId = i.KnifeId,
                                ShoeSize = i.ShoeSize,
                                ShoeSizeSuffix = i.ShoeSizeSuffix,
                                ShoeSizeSortKey = i.ShoeSizeSortKey,
                                Qty = i.Qty,
                                MadeDate = i.MadeDate,
                                Cost = i.Cost,
                            })
                            .ToList();

                        // convert knife
                        i.Id = 0;
                        i.LocaleId = locale;
                        i.ModifyUserName = user;
                        i.MoneyCodeId = i.MoneyCode == null ? 0 : _codeItems.Where(c => c.NameTW == i.MoneyCode && c.CodeType == "02").Max(c => c.Id);
                        i.OwnerCustomerId = null;

                        group.Knife = i;
                        group.KnifeItem = _items;

                        this.Save(group);
                    });
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    
        public IEnumerable<ERP.Models.Views.Knife> GetKnifeNo (string knifeNo, int localeId) {
            return Knife.Get().Where(i => i.KnifeNo.ToLower() == knifeNo.ToLower() && i.LocaleId == localeId).ToList();
        }
    }
}