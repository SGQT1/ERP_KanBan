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
    public class PackLabelService : BusinessService
    {
        private ERP.Services.Business.Entities.PackLabelService PackLabel;
        private ERP.Services.Business.Entities.PackLabelItemService PackLabelItem;
        private ERP.Services.Business.Entities.PackLabelEditionService PackLabelEdition;

        public PackLabelService(
            ERP.Services.Business.Entities.PackLabelService packLabelService,
            ERP.Services.Business.Entities.PackLabelItemService packLabelItemService,
            ERP.Services.Business.Entities.PackLabelEditionService packLabelEdtionService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            PackLabel = packLabelService;
            PackLabelItem = packLabelItemService;
            PackLabelEdition = packLabelEdtionService;
        }
        public ERP.Models.Views.PackLabelGroup GetPackLabelGroup(int packLabelId, int localeId)
        {
            var packLabel = PackLabel.Get().Where(i => i.Id == packLabelId && i.LocaleId == localeId).FirstOrDefault();
            if (packLabel != null)
            {
                return new PackLabelGroup
                {
                    // from CTNLabel
                    PackLabel = packLabel,
                    // from CTNLabel,CTNLabelItem
                    // PackLabelItem = PackLabelItem.GetWithStock(packLabelId, localeId),
                    PackLabelItem = PackLabelItem.Get().Where(i => i.CTNLabelId == packLabelId && i.LocaleId == localeId),
                    // from CTNOrders, OrdersPL
                    PackLabelEdition = PackLabelEdition.Get(packLabelId, packLabel.OrderNo, localeId),
                };
            }
            else
            {
                return new PackLabelGroup { };
            }
        }

        public ERP.Models.Views.PackLabelGroup GetPackLabelOrders(string orderNo, int localeId)
        {
            var packLabel = PackLabel.GetPackLabelOrders(orderNo, localeId);
            if (packLabel != null)
            {
                return new PackLabelGroup
                {
                    // from CTNLabel
                    PackLabel = packLabel,
                    // from CTNLabel,CTNLabelItem
                    // PackLabelItem = new List<PackLabelItem>(),
                    // from CTNOrders, OrdersPL
                    PackLabelEdition = PackLabelEdition.Get(0, packLabel.OrderNo, localeId),
                };
            }
            else
            {
                return new PackLabelGroup { };
            }
        }
        public Models.Views.PackLabelGroup SavePackLabelGroup(PackLabelGroup packLabelGroup)
        {
            var packLabel = packLabelGroup.PackLabel;
            var packLabelItems = packLabelGroup.PackLabelItem;
            var packLabelEditions = packLabelGroup.PackLabelEdition.Where(i => i.IsEdition == true).ToList();

            try
            {
                //recaculater packQty,CTN

                packLabel.CTNS = packLabelItems.Count();
                packLabel.PackingQty = packLabelItems.Sum(i => i.SubPackingQty);

                UnitOfWork.BeginTransaction();
                if (packLabel != null)
                {
                    if (packLabel.Id == 0)
                    {
                        // add new, but LocaleId,OrderNo,ExFactoryDate exist,update
                        //PackLabel
                        var _packLabel = PackLabel.Get().Where(i =>
                                i.LocaleId == packLabel.LocaleId &&
                                i.OrderNo == packLabel.OrderNo &&
                                i.ExFactoryDate == packLabel.ExFactoryDate
                            ).FirstOrDefault();
                        if (_packLabel == null)
                        {
                            packLabel = PackLabel.Create(packLabel);
                        }
                        else
                        {
                            packLabel.Id = _packLabel.Id;
                            packLabel = PackLabel.Update(packLabel);
                        }
                    }
                    else
                    {
                        packLabel = PackLabel.Update(packLabel);
                    }

                    //PackLabelItem
                    if (packLabel.Id != 0)
                    {
                        // barcode cannot remove and create ,when barcodes have be used in StockIn/StockOut
                        var exitLabelItems = PackLabelItem.GetWithStock(PackLabelItem.Get().Where(i => i.CTNLabelId == packLabel.Id).ToList(), (int)packLabel.LocaleId).ToList();
                        var stockItems = exitLabelItems.Where(i => i.HasStockIn == true).ToList();
                        var nonStockItems = exitLabelItems.Where(i => !stockItems.Select(s => s.LabelCode).Contains(i.LabelCode)).ToList();
                        PackLabelItem.RemoveRange(nonStockItems.Select(i => i.LabelCode).ToList(), (int)packLabel.Id, (int)packLabel.LocaleId);

                        // rebuild Barcode
                        packLabelItems.ToList().ForEach(i =>
                        {
                            i.CTNLabelId = packLabel.Id;
                            i.LabelCode = (i.PackingType + 1) + "-" + i.GroupBy + "-" + i.SubPackingQty + "-" + i.SeqNo.ToString("0000") + "-" + packLabel.Id;
                            i.SubLabelCode = packLabel.Id.ToString() + i.SeqNo.ToString("0000");
                        });
                        var newtems = packLabelItems.Where(i => !(stockItems.Select(s => s.LabelCode).Contains(i.LabelCode))).ToList();
                        PackLabelItem.CreateRange(newtems);

                        packLabelEditions.ToList().ForEach(i =>
                        {
                            i.PackLabelId = packLabel.Id;
                        });
                        PackLabelEdition.RemoveRange((int)packLabel.Id, (int)packLabel.LocaleId);
                        PackLabelEdition.CreateRange(packLabelEditions);
                    }
                }
                UnitOfWork.Commit();
                return this.GetPackLabelGroup((int)packLabel.Id, (int)packLabel.LocaleId);
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }

        public Models.Views.PackLabelGroup SaveCustomerPackLabelGroup(PackLabelGroup packLabelGroup)
        {
            var packLabel = packLabelGroup.PackLabel;
            var packLabelItems = packLabelGroup.PackLabelItem;
            var packLabelEditions = packLabelGroup.PackLabelEdition.Where(i => i.IsEdition == true).ToList();

            try
            {
                UnitOfWork.BeginTransaction();
                if (packLabel != null)
                {
                    if (packLabel.Id == 0)
                    {
                        // add new, but LocaleId,OrderNo,ExFactoryDate exist,update
                        //PackLabel
                        var _packLabel = PackLabel.Get().Where(i =>
                                i.LocaleId == packLabel.LocaleId &&
                                i.OrderNo == packLabel.OrderNo &&
                                i.ExFactoryDate == packLabel.ExFactoryDate
                            ).FirstOrDefault();
                        if (_packLabel == null)
                        {
                            packLabel = PackLabel.Create(packLabel);
                        }
                        else
                        {
                            packLabel.Id = _packLabel.Id;
                            packLabel = PackLabel.Update(packLabel);
                        }
                    }
                    else
                    {
                        packLabel = PackLabel.Update(packLabel);
                    }

                    //PackLabelItem
                    if (packLabel.Id != 0)
                    {
                        // barcode cannot remove and create ,when barcodes have be used in StockIn/StockOut
                        var exitLabelItems = PackLabelItem.GetWithStock(PackLabelItem.Get().Where(i => i.CTNLabelId == packLabel.Id).ToList(), (int)packLabel.LocaleId).ToList();
                        var stockItems = exitLabelItems.Where(i => i.HasStockIn == true).ToList();
                        var nonStockItems = exitLabelItems.Where(i => !stockItems.Select(s => s.LabelCode).Contains(i.LabelCode)).ToList();
                        PackLabelItem.CustomerRemoveRange(nonStockItems.Select(i => i.LabelCode).ToList(), (int)packLabel.Id, (int)packLabel.LocaleId);

                        // rebuild Barcode
                        packLabelItems.ToList().ForEach(i =>
                        {
                            i.CTNLabelId = packLabel.Id;
                            i.LabelCode = (i.PackingType + 1) + "-" + i.GroupBy + "-" + i.SubPackingQty + "-" + i.SeqNo.ToString("0000") + "-" + packLabel.Id;
                            i.SubLabelCode = (i.SubLabelCode != null && i.SubLabelCode.Length > 0) ? i.SubLabelCode : packLabel.Id.ToString() + i.SeqNo.ToString("0000");
                        });
                        // var newtems = packLabelItems.Where(i => !(stockItems.Select(s => s.LabelCode).Contains(i.LabelCode))).ToList();
                        PackLabelItem.CreateRange(packLabelItems);

                        packLabelEditions.ToList().ForEach(i =>
                        {
                            i.PackLabelId = packLabel.Id;
                        });
                        PackLabelEdition.RemoveRange((int)packLabel.Id, (int)packLabel.LocaleId);
                        PackLabelEdition.CreateRange(packLabelEditions);
                    }
                }
                UnitOfWork.Commit();
                return this.GetPackLabelGroup((int)packLabel.Id, (int)packLabel.LocaleId);
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        public void ReomvePackLabelGroup(int packLabelId, int localeId)
        {
            var packLabel = PackLabel.Get().Where(i => i.Id == packLabelId && i.LocaleId == localeId).FirstOrDefault();
            try
            {
                UnitOfWork.BeginTransaction();
                if (packLabel != null)
                {
                    var exitLabelItems = PackLabelItem.GetWithStock(PackLabelItem.Get().Where(i => i.CTNLabelId == packLabel.Id).ToList(), (int)packLabel.LocaleId).ToList();
                    PackLabelItem.RemoveRange(exitLabelItems.Select(i => i.LabelCode).ToList(), (int)packLabel.Id, (int)packLabel.LocaleId);
                    PackLabelEdition.RemoveRange((int)packLabel.Id, (int)packLabel.LocaleId);

                    PackLabel.Remove(packLabel);
                }
                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        public IEnumerable<ERP.Models.Views.PackLabelItem> GetPackLabelItems(int from, IEnumerable<PackLabelEdition> editions)
        {
            return PackLabelItem.GetPreBarcode(from, editions);
        }
    }
}
