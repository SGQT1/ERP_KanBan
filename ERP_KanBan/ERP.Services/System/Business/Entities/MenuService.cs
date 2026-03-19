using System;
using System.Collections.Generic;
using System.Linq;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Entities;
using ERP.Models.Views;
using ERP.Services.Bases;
using Newtonsoft.Json;

namespace ERP.Services.Business.Entities
{
    public class MenuService : BusinessService
    {
        // private Services.Entities.SysFunctionService SysFunction { get; }
        private Services.Entities.SysUserFunctionService SysUserFunction { get; }

        public MenuService(
            // Services.Entities.SysFunctionService sysFunctionService,
            Services.Entities.SysUserFunctionService sysUserFunctionService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            // SysFunction = sysFunctionService;
            SysUserFunction = sysUserFunctionService;
        }
        public IQueryable<Models.Views.Menu> Get()
        {
            // return GetMenuDataSource().Where(i => i.Validate == true).AsQueryable();
            var menuItems = new List<Models.Views.Menu>();
            var trees = GetJsonData();
            trees.ForEach(i =>
            {
                menuItems.Add(new Models.Views.Menu
                {
                    Id = i.Id,
                    ParentMenuId = i.ParentMenuId,
                    Name = i.Name,
                    URL = i.URL,
                    ItemSort = 0,
                    Validate = true,
                    MenuCode = i.Code,
                });
                if (i.Items != null && i.Items.Count() > 0)
                {
                    menuItems = RecursiveMenu(menuItems, i);
                }
            });
            // var seqItem = menuItems.OrderBy(i => Convert.ToInt32(i.Id)).ToList();
            // seqItem.ForEach(i =>
            // {
            //     Console.WriteLine(i.Id + ">>" + i.Name);
            // });
            return menuItems.AsQueryable();
        }
        private List<Models.Views.Menu> RecursiveMenu(List<Models.Views.Menu> data, Models.Views.Common.TreeItem node)
        {
            node.Items.ForEach(i =>
            {
                data.Add(new Models.Views.Menu
                {
                    Id = i.Id,
                    ParentMenuId = i.ParentMenuId,
                    Name = i.Name,
                    URL = i.URL,
                    ItemSort = 0,
                    Validate = true,
                    MenuCode = i.Code,
                });
                if (i.Items != null && i.Items.Count() > 0)
                {
                    data = RecursiveMenu(data, i);
                }
            });
            return data;
        }
        public List<Models.Views.Menu> GetByUser(string userId)
        {
            var functions = Get().ToList();
            var userFunction = SysUserFunction.Get().Where(i => i.UserId == userId).ToList();

            var permission = (
                from f in functions
                join uf in userFunction on f.Id equals uf.FunctionId
                where f.Validate == true
                select new Models.Views.Menu
                {
                    Id = f.Id,
                    ParentMenuId = f.ParentMenuId,
                    Name = f.Name,
                    URL = f.URL,
                    ItemSort = f.ItemSort,
                    Validate = f.Validate,
                    MenuCode = f.MenuCode
                }
            ).ToList();
            return permission;
        }
        private static List<Models.Views.Common.TreeItem> GetJsonData()
        {
            // Id=278
            var menuJson = @"[
                {
                    'Id': 101,
                    'ParentMenuId': null,
                    'Name': 'Menu.AdminMgt',
                    'Items': [
                    {
                        'Id': 102,
                        'ParentMenuId': 101,
                        'Name': 'Menu.Group',
                        'Items': [
                        {
                            'Id': 105,
                            'ParentMenuId': 102,
                            'Name': 'Menu.GroupEdit',
                            'Items': null,
                            'URL': 'GroupEditComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 104,
                        'ParentMenuId': 101,
                        'Name': 'Menu.User',
                        'Items': [
                        {
                            'Id': 107,
                            'ParentMenuId': 104,
                            'Name': 'Menu.UserEdit',
                            'Items': null,
                            'URL': 'UserEditComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 223,
                        'ParentMenuId': 101,
                        'Name': 'Menu.Permission',
                        'Items': [
                        {
                            'Id': 224,
                            'ParentMenuId': 223,
                            'Name': 'Menu.PermissionEdit',
                            'Items': null,
                            'URL': 'PermissionEditComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    }
                    ],
                    'URL': null,
                    'Validate': true
                },
                {
                    'Id': 1,
                    'ParentMenuId': null,
                    'Name': 'Menu.BasicSettingMgt',
                    'Items': [
                    {
                        'Id': 8,
                        'ParentMenuId': 1,
                        'Name': 'Menu.Common',
                        'Items': [
                        {
                            'Id': 32,
                            'ParentMenuId': 8,
                            'Name': 'Menu.CodeEdit',
                            'Items': null,
                            'URL': 'CodeEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 204,
                            'ParentMenuId': 8,
                            'Name': 'Menu.MaterialEdit',
                            'Items': null,
                            'URL': 'MaterialEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 225,
                            'ParentMenuId': 8,
                            'Name': 'Menu.MaterialCopyEdit',
                            'Items': null,
                            'URL': 'MaterialCopyEditComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 203,
                        'ParentMenuId': 1,
                        'Name': 'Menu.Product',
                        'Items': [
                        {
                            'Id': 344,
                            'ParentMenuId': 203,
                            'Name': 'Menu.KnifeEdit',
                            'Items': null,
                            'URL': 'KnifeEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 345,
                            'ParentMenuId': 203,
                            'Name': 'Menu.OutsoleEdit',
                            'Items': null,
                            'URL': 'OutsoleEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 346,
                            'ParentMenuId': 203,
                            'Name': 'Menu.LastEdit',
                            'Items': null,
                            'URL': 'LastEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 347,
                            'ParentMenuId': 203,
                            'Name': 'Menu.PartEdit',
                            'Items': null,
                            'URL': 'PartEditComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 9,
                        'ParentMenuId': 1,
                        'Name': 'Menu.Orders',
                        'Items': [
                        {
                            'Id': 35,
                            'ParentMenuId': 9,
                            'Name': 'Menu.CustomerEdit',
                            'Items': null,
                            'URL': 'CustomerEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 36,
                            'ParentMenuId': 9,
                            'Name': 'Menu.PortEdit',
                            'Items': null,
                            'URL': 'PortEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 115,
                            'ParentMenuId': 9,
                            'Name': 'Menu.SizeMappingEdit',
                            'Items': null,
                            'URL': 'SizeMappingEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 238,
                            'ParentMenuId': 9,
                            'Name': 'Menu.LabelArticleEdit',
                            'Items': null,
                            'URL': 'LabelArticleEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 239,
                            'ParentMenuId': 9,
                            'Name': 'Menu.LabelCustomerEdit',
                            'Items': null,
                            'URL': 'LabelCustomerEditComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 402,
                        'ParentMenuId': 1,
                        'Name': 'Menu.Stock',
                        'Items': [
                        {
                            'Id': 403,
                            'ParentMenuId': 402,
                            'Name': 'Menu.OrdersStockLocaleEdit',
                            'Items': null,
                            'URL': 'OrdersStockLocaleEditComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 11,
                        'ParentMenuId': 1,
                        'Name': 'Menu.Account',
                        'Items': [
                        {
                            'Id': 117,
                            'ParentMenuId': 11,
                            'Name': 'Menu.ExchangeRateEdit',
                            'Items': null,
                            'URL': 'ExchangeRateEditComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 183,
                        'ParentMenuId': 1,
                        'Name': 'Menu.Warehouse',
                        'Items': [
                        {
                            'Id': 184,
                            'ParentMenuId': 183,
                            'Name': 'Menu.OrgUnitEdit',
                            'Items': null,
                            'URL': 'OrgUnitEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 185,
                            'ParentMenuId': 183,
                            'Name': 'Menu.WarehouseEdit',
                            'Items': null,
                            'URL': 'WarehouseEditComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 201,
                        'ParentMenuId': 1,
                        'Name': 'Menu.Purchase',
                        'Items': [
                        {
                            'Id': 202,
                            'ParentMenuId': 201,
                            'Name': 'Menu.VendorEdit',
                            'Items': null,
                            'URL': 'VendorEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 310,
                            'ParentMenuId': 201,
                            'Name': 'Menu.ReceivedStandardEdit',
                            'Items': null,
                            'URL': 'ReceivedStandardEditComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 313,
                        'ParentMenuId': 1,
                        'Name': 'Menu.Bond',
                        'Items': [
                        {
                            'Id': 314,
                            'ParentMenuId': 313,
                            'Name': 'Menu.BondProductChinaEdit',
                            'Items': null,
                            'URL': 'BondProductChinaEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 315,
                            'ParentMenuId': 313,
                            'Name': 'Menu.BondMaterialChinaEdit',
                            'Items': null,
                            'URL': 'BondMaterialChinaEditComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },                    
                    {
                        'Id': 326,
                        'ParentMenuId': 1,
                        'Name': 'Menu.ProductPlan',
                        'Items': [
                        {
                            'Id': 327,
                            'ParentMenuId': 326,
                            'Name': 'Menu.Dispatch',
                            'Items': [
                                {
                                    'Id': 337,
                                    'ParentMenuId': 327,
                                    'Name': 'Menu.MPSArticleEdit',
                                    'Items': null,
                                    'URL': 'MPSArticleEditComponent',
                                    'Validate': true
                                },
                                {
                                    'Id': 338,
                                    'ParentMenuId': 327,
                                    'Name': 'Menu.MPSProcessEdit',
                                    'Items': null,
                                    'URL': 'MPSProcessEditComponent',
                                    'Validate': true
                                },
                                {
                                    'Id': 339,
                                    'ParentMenuId': 327,
                                    'Name': 'Menu.MPSProcessOrgEdit',
                                    'Items': null,
                                    'URL': 'MPSProcessOrgEditComponent',
                                    'Validate': true
                                },
                                {
                                    'Id': 340,
                                    'ParentMenuId': 327,
                                    'Name': 'Menu.MPSProceduresEdit',
                                    'Items': null,
                                    'URL': 'MPSProceduresEditComponent',
                                    'Validate': true
                                },
                                {
                                    'Id': 341,
                                    'ParentMenuId': 327,
                                    'Name': 'Menu.MPSProcessUnitEdit',
                                    'Items': null,
                                    'URL': 'MPSProcessUnitEditComponent',
                                    'Validate': true
                                }
                            ],
                            'URL': null,
                            'Validate': true
                        },
                        {
                            'Id': 328,
                            'ParentMenuId': 326,
                            'Name': 'Menu.MPSOutsource',
                            'Items': [
                                {
                                    'Id': 329,
                                    'ParentMenuId': 328,
                                    'Name': 'Menu.MPSWarehouseEdit',
                                    'Items': null,
                                    'URL': 'MPSWarehouseEditComponent',
                                    'Validate': true
                                },
                                {
                                    'Id': 330,
                                    'ParentMenuId': 328,
                                    'Name': 'Menu.MPSVendorEdit',
                                    'Items': null,
                                    'URL': 'MPSVendorEditComponent',
                                    'Validate': true
                                }
                            ],
                            'URL': null,
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    }
                    ],
                    'URL': null,
                    'Validate': true
                },
                {
                    'Id': 365,
                    'ParentMenuId': null,
                    'Name': 'Menu.RDMgt',
                    'Items': [
                    {
                        'Id': 366,
                        'ParentMenuId': 365,
                        'Name': 'Menu.RDPurchase',
                        'Items': [
                        {
                            'Id': 367,
                            'ParentMenuId': 366,
                            'Name': 'Menu.RDPOEdit',
                            'Items': null,
                            'URL': 'RDPOEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 393,
                            'ParentMenuId': 366,
                            'Name': 'Menu.RDPOConfirmEdit',
                            'Items': null,
                            'URL': 'RDPOConfirmEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 368,
                            'ParentMenuId': 366,
                            'Name': 'Menu.RDAPMonthEdit',
                            'Items': null,
                            'URL': 'RDAPMonthEditComponent',
                            'Validate': true
                        },
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 369,
                        'ParentMenuId': 365,
                        'Name': 'Menu.RDWarehose',
                        'Items': [
                        {
                            'Id': 370,
                            'ParentMenuId': 369,
                            'Name': 'Menu.RDReceivedEdit',
                            'Items': null,
                            'URL': 'RDReceivedEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 371,
                            'ParentMenuId': 369,
                            'Name': 'Menu.RDStockInEdit',
                            'Items': null,
                            'URL': 'RDStockInEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 372,
                            'ParentMenuId': 369,
                            'Name': 'Menu.RDStockOutEdit',
                            'Items': null,
                            'URL': 'RDStockOutEditComponent',
                            'Validate': true
                        },
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    ],
                    'URL': null,
                    'Validate': true
                },
                {
                    'Id': 355,
                    'ParentMenuId': null,
                    'Name': 'Menu.ProductMgt',
                    'Items': [
                    {
                        'Id': 356,
                        'ParentMenuId': 355,
                        'Name': 'Menu.Product',
                        'Items': [
                        {
                            'Id': 358,
                            'ParentMenuId': 356,
                            'Name': 'Menu.ArticleEdit',
                            'Items': null,
                            'URL': 'ArticleEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 359,
                            'ParentMenuId': 356,
                            'Name': 'Menu.ArticleSizeRunEdit',
                            'Items': null,
                            'URL': 'ArticleSizeRunEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 360,
                            'ParentMenuId': 356,
                            'Name': 'Menu.StyleEdit',
                            'Items': null,
                            'URL': 'StyleEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 364,
                            'ParentMenuId': 356,
                            'Name': 'Menu.StylePartUsageEdit',
                            'Items': null,
                            'URL': 'StylePartUsageEditComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 357,
                        'ParentMenuId': 355,
                        'Name': 'Menu.Usage',
                        'Items': [
                        {
                            'Id': 363,
                            'ParentMenuId': 357,
                            'Name': 'Menu.ArticlePartUsageEdit',
                            'Items': null,
                            'URL': 'ArticlePartUsageEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 361,
                            'ParentMenuId': 357,
                            'Name': 'Menu.ArticlePartUsageBatchEdit',
                            'Items': null,
                            'URL': 'ArticlePartUsageBatchEditComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    }
                    ],
                    'URL': null,
                    'Validate': true
                },
                {
                    'Id': 82,
                    'ParentMenuId': null,
                    'Name': 'Menu.QuotationMgt',
                    'Items': [
                    {
                        'Id': 83,
                        'ParentMenuId': 82,
                        'Name': 'Menu.ProductPrice',
                        'Items': [
                        {
                            'Id': 84,
                            'ParentMenuId': 83,
                            'Name': 'Menu.ProductPriceBrowse',
                            'Items': null,
                            'URL': 'ProductPriceBrowseComponent',
                            'Validate': true
                        },
                        {
                            'Id': 85,
                            'ParentMenuId': 83,
                            'Name': 'Menu.ProductPriceEdit',
                            'Items': null,
                            'URL': 'ProductPriceEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 205,
                            'ParentMenuId': 83,
                            'Name': 'Menu.ProductPriceBatchEdit',
                            'Items': null,
                            'URL': 'ProductPriceBatchEditComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 87,
                        'ParentMenuId': 82,
                        'Name': 'Menu.ToolingCost',
                        'Items': [
                        {
                            'Id': 88,
                            'ParentMenuId': 87,
                            'Name': 'Menu.OutsoleToolingCostEdit',
                            'Items': null,
                            'URL': 'OutsoleToolingCostEditComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    }
                    ],
                    'URL': null,
                    'Validate': true
                },
                {
                    'Id': 10,
                    'ParentMenuId': null,
                    'Name': 'Menu.AccountMgt',
                    'Items': [
                    {
                        'Id': 118,
                        'ParentMenuId': 10,
                        'Name': 'Menu.Account',
                        'Items': [
                        {
                            'Id': 125,
                            'ParentMenuId': 118,
                            'Name': 'Menu.ShipmentCloseEdit',
                            'Items': null,
                            'URL': 'ShipmentCloseEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 120,
                            'ParentMenuId': 118,
                            'Name': 'Menu.InvoiceCloseEdit',
                            'Items': null,
                            'URL': 'InvoiceCloseEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 121,
                            'ParentMenuId': 118,
                            'Name': 'Menu.OutsoleTCCloseEdit',
                            'Items': null,
                            'URL': 'OutsoleTCCloseEditComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 119,
                        'ParentMenuId': 10,
                        'Name': 'Menu.CostAnalysis',
                        'Items': [
                        {
                            'Id': 123,
                            'ParentMenuId': 119,
                            'Name': 'Menu.BatchOrdersCostEdit',
                            'Items': null,
                            'URL': 'BatchOrdersCostEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 124,
                            'ParentMenuId': 119,
                            'Name': 'Menu.BatchOrdersCostCloseEdit',
                            'Items': null,
                            'URL': 'BatchOrdersCostCloseEditComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    }
                    ],
                    'URL': null,
                    'Validate': true
                },
                {
                    'Id': 90,
                    'ParentMenuId': null,
                    'Name': 'Menu.ShipmentMgt',
                    'Items': [
                    {
                        'Id': 91,
                        'ParentMenuId': 90,
                        'Name': 'Menu.Shipment',
                        'Items': [
                        {
                            'Id': 95,
                            'ParentMenuId': 91,
                            'Name': 'Menu.ShipmentEdit',
                            'Items': null,
                            'URL': 'ShipmentEditComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 92,
                        'ParentMenuId': 90,
                        'Name': 'Menu.Invoice',
                        'Items': [
                        {
                            'Id': 97,
                            'ParentMenuId': 92,
                            'Name': 'Menu.InvoiceEdit',
                            'Items': null,
                            'URL': 'InvoiceEditComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 93,
                        'ParentMenuId': 90,
                        'Name': 'Menu.Payment',
                        'Items': [
                        {
                            'Id': 99,
                            'ParentMenuId': 93,
                            'Name': 'Menu.PaymentEdit',
                            'Items': null,
                            'URL': 'PaymentEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 100,
                            'ParentMenuId': 93,
                            'Name': 'Menu.PaymentBatchEdit',
                            'Items': null,
                            'URL': 'PaymentBatchEditComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    }
                    ],
                    'URL': null,
                    'Validate': true
                },
                {
                    'Id': 2,
                    'ParentMenuId': null,
                    'Name': 'Menu.OrdersMgt',
                    'Items': [
                    {
                        'Id': 12,
                        'ParentMenuId': 2, 
                        'Name': 'Menu.ImportOrders',
                        'Items': [
                        {
                            'Id': 37,
                            'ParentMenuId': 12,
                            'Name': 'Menu.PUMAOrders',
                            'Items': [
                            {
                                'Id': 78,
                                'ParentMenuId': 37,
                                'Name': 'Menu.PUMAOrdersBrowse',
                                'Items': null,
                                'URL': 'PUMAOrdersBrowseComponent',
                                'Validate': true
                            },
                            {
                                'Id': 79,
                                'ParentMenuId': 37,
                                'Name': 'Menu.PUMAOrdersEdit',
                                'Items': null,
                                'URL': 'PUMAOrdersEditComponent',
                                'Validate': true
                            }
                            ],
                            'URL': null,
                            'Validate': true
                        },
                        {
                            'Id': 38,
                            'ParentMenuId': 12,
                            'Name': 'Menu.NBOrders',
                            'Items': [
                            {
                                'Id': 80,
                                'ParentMenuId': 38,
                                'Name': 'Menu.NBOrdersBrowse',
                                'Items': null,
                                'URL': 'NBOrdersBrowseComponent',
                                'Validate': true
                            },
                            {
                                'Id': 81,
                                'ParentMenuId': 38,
                                'Name': 'Menu.NBOrdersEdit',
                                'Items': null,
                                'URL': 'NBOrdersEditComponent',
                                'Validate': true
                            }
                            ],
                            'URL': null,
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 13,
                        'ParentMenuId': 2,
                        'Name': 'Menu.ProductionOrders',
                        'Items': [
                        {
                            'Id': 39,
                            'ParentMenuId': 13,
                            'Name': 'Menu.ProductionOrdersBrowse',
                            'Items': null,
                            'URL': 'ProductionOrdersBrowseComponent',
                            'Validate': true
                        },
                        {
                            'Id': 40,
                            'ParentMenuId': 13,
                            'Name': 'Menu.ProductionOrdersEdit',
                            'Items': null,
                            'URL': 'ProductionOrdersEditComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 14,
                        'ParentMenuId': 2,
                        'Name': 'Menu.PackingMaterial',
                        'Items': [
                        {
                            'Id': 41,
                            'ParentMenuId': 14,
                            'Name': 'Menu.PackingMaterialBrowse',
                            'Items': null,
                            'URL': 'PackingMaterialBrowseComponent',
                            'Validate': true
                        },
                        {
                            'Id': 42,
                            'ParentMenuId': 14,
                            'Name': 'Menu.PackingMaterialEdit',
                            'Items': null,
                            'URL': 'PackingMaterialEditComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 15,
                        'ParentMenuId': 2,
                        'Name': 'Menu.CloseOrders',
                        'Items': null,
                        'URL': 'CloseOrdersEditComponent',
                        'Validate': true
                    }
                    ],
                    'URL': null,
                    'Validate': true
                },
                {
                    'Id': 3,
                    'ParentMenuId': null,
                    'Name': 'Menu.PackingListMgt',
                    'Items': [
                    {
                        'Id': 16,
                        'ParentMenuId': 3,
                        'Name': 'Menu.PackingSpec',
                        'Items': [
                        {
                            'Id': 43,
                            'ParentMenuId': 16,
                            'Name': 'Menu.PackingSpecBrowse',
                            'Items': null,
                            'URL': 'PackingSpecBrowseComponent',
                            'Validate': true
                        },
                        {
                            'Id': 44,
                            'ParentMenuId': 16,
                            'Name': 'Menu.PackingSpecEdit',
                            'Items': null,
                            'URL': 'PackingSpecEditComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 17,
                        'ParentMenuId': 3,
                        'Name': 'Menu.PackingArticle',
                        'Items': [
                        {
                            'Id': 45,
                            'ParentMenuId': 17,
                            'Name': 'Menu.PackingArticleBrowse',
                            'Items': null,
                            'URL': 'PackingArticleBrowseComponent',
                            'Validate': true
                        },
                        {
                            'Id': 46,
                            'ParentMenuId': 17,
                            'Name': 'Menu.PackingArticleEdit',
                            'Items': null,
                            'URL': 'PackingArticleEditComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 18,
                        'ParentMenuId': 3,
                        'Name': 'Menu.PackingPlan',
                        'Items': [
                        {
                            'Id': 47,
                            'ParentMenuId': 18,
                            'Name': 'Menu.PackingPlanBrowse',
                            'Items': null,
                            'URL': 'PackingPlanBrowseComponent',
                            'Validate': true
                        },
                        {
                            'Id': 48,
                            'ParentMenuId': 18,
                            'Name': 'Menu.PackingPlanEdit',
                            'Items': null,
                            'URL': 'PackingPlanEditComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 20,
                        'ParentMenuId': 3,
                        'Name': 'Menu.PackingMark',
                        'Items': [
                        {
                            'Id': 51,
                            'ParentMenuId': 20,
                            'Name': 'Menu.PackingMarkBrowse',
                            'Items': null,
                            'URL': 'PackingMarkBrowseComponent',
                            'Validate': true
                        },
                        {
                            'Id': 52,
                            'ParentMenuId': 20,
                            'Name': 'Menu.PackingMarkEdit',
                            'Items': null,
                            'URL': 'PackingMarkEditComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 19,
                        'ParentMenuId': 3,
                        'Name': 'Menu.PackingLabel',
                        'Items': [
                        {
                            'Id': 49,
                            'ParentMenuId': 19,
                            'Name': 'Menu.PackingLabelBrowse',
                            'Items': null,
                            'URL': 'PackingLabelBrowseComponent',
                            'Validate': true
                        },
                        {
                            'Id': 50,
                            'ParentMenuId': 19,
                            'Name': 'Menu.PackingLabelEdit',
                            'Items': null,
                            'URL': 'PackingLabelEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 131,
                            'ParentMenuId': 19,
                            'Name': 'Menu.PackingLabelCustomerEdit',
                            'Items': null,
                            'URL': 'PackingLabelCustomerEditComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 139,
                        'ParentMenuId': 3,
                        'Name': 'Menu.PackingInvoice',
                        'Items': [
                        {
                            'Id': 141,
                            'ParentMenuId': 139,
                            'Name': 'Menu.PackingInvoiceEdit',
                            'Items': null,
                            'URL': 'PackingInvoiceEditComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    ],
                    'URL': null,
                    'Validate': true
                },
                {
                    'Id': 132,
                    'ParentMenuId': null,
                    'Name': 'Menu.StockMgt',
                    'Items': [
                        {
                            'Id': 133,
                            'ParentMenuId': 132,
                            'Name': 'Menu.StockWeightImport',
                            'Items': [
                                {
                                    'Id': 135,
                                    'ParentMenuId': 133,
                                    'Name': 'Menu.StockWeightImportEdit',
                                    'Items': null,
                                    'URL': 'StockWeightImportEditComponent',
                                    'Validate': true
                                }
                            ],
                            'URL': null,
                            'Validate': true
                        },
                        {
                            'Id': 404,
                            'ParentMenuId': 132,
                            'Name': 'Menu.OrdersStockLocaleIn',
                            'Items': [
                                {
                                    'Id': 405,
                                    'ParentMenuId': 404,
                                    'Name': 'Menu.OrdersStockLocaleInEdit',
                                    'Items': null,
                                    'URL': 'OrdersStockLocaleInEditComponent',
                                    'Validate': true
                                }
                            ],
                            'URL': null,
                            'Validate': true
                        }
                    ],
                    'URL': null,
                    'Validate': true
                },
                {
                    'Id': 4,
                    'ParentMenuId': null,
                    'Name': 'Menu.BOMMgt',
                    'Items': [
                    {
                        'Id': 21,
                        'ParentMenuId': 4,
                        'Name': 'Menu.BOMGenerate',
                        'Items': [
                        {
                            'Id': 53,
                            'ParentMenuId': 21,
                            'Name': 'Menu.BOMGenerateBrowse',
                            'Items': null,
                            'URL': 'BOMGenerateBrowseComponent',
                            'Validate': true
                        },
                        {
                            'Id': 54,
                            'ParentMenuId': 21,
                            'Name': 'Menu.BOMGenerateEdit',
                            'Items': null,
                            'URL': 'BOMGenerateEditComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 22,
                        'ParentMenuId': 4,
                        'Name': 'Menu.BOMTask',
                        'Items': [
                        {
                            'Id': 55,
                            'ParentMenuId': 22,
                            'Name': 'Menu.BOMTaskBrowse',
                            'Items': null,
                            'URL': 'BOMTaskBrowseComponent',
                            'Validate': true
                        },
                        {
                            'Id': 56,
                            'ParentMenuId': 22,
                            'Name': 'Menu.BOMTaskEdit',
                            'Items': null,
                            'URL': 'BOMTaskEditComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 23,
                        'ParentMenuId': 4,
                        'Name': 'Menu.BOMLog',
                        'Items': [
                        {
                            'Id': 57,
                            'ParentMenuId': 23,
                            'Name': 'Menu.BOMTaskLog',
                            'Items': null,
                            'URL': 'BOMTaskLogBrowseComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    }
                    ],
                    'URL': null,
                    'Validate': true
                },
                {
                    'Id': 142,
                    'ParentMenuId': null,
                    'Name': 'Menu.PurchaseMgt',
                    'Items': [
                    {
                        'Id': 143,
                        'ParentMenuId': 142,
                        'Name': 'Menu.Quotation',
                        'Items': [
                        {
                            'Id': 145,
                            'ParentMenuId': 143,
                            'Name': 'Menu.MaterialQuotationBrowse',
                            'Items': null,
                            'URL': 'MaterialQuotationBrowseComponent',
                            'Validate': true
                        },
                        {
                            'Id': 146,
                            'ParentMenuId': 143,
                            'Name': 'Menu.MaterialQuotationEdit',
                            'Items': null,
                            'URL': 'MaterialQuotationEditComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 144,
                        'ParentMenuId': 142,
                        'Name': 'Menu.Purchase',
                        'Items': [
                        {
                            'Id': 262,
                            'ParentMenuId': 144,
                            'Name': 'Menu.BOMPurchase',
                            'Items': [
                                {
                                    'Id': 147,
                                    'ParentMenuId': 262,
                                    'Name': 'Menu.MaterialPurBatchEdit',
                                    'Items': null,
                                    'URL': 'MaterialPurBatchEditComponent',
                                    'Validate': true
                                },
                                {
                                    'Id': 134,
                                    'ParentMenuId': 262,
                                    'Name': 'Menu.MaterialPurPlanEdit',
                                    'Items': null,
                                    'URL': 'MaterialPurPlanEditComponent',
                                    'Validate': true
                                },
                                {
                                    'Id': 252,
                                    'ParentMenuId': 262,
                                    'Name': 'Menu.MaterialPurPOEdit',
                                    'Items': null,
                                    'URL': 'MaterialPurPOEditComponent',
                                    'Validate': true
                                },
                                {
                                    'Id': 267,
                                    'ParentMenuId': 262,
                                    'Name': 'Menu.MaterialBatchPOItemEdit',
                                    'Items': null,
                                    'URL': 'MaterialBatchPOItemEditComponent',
                                    'Validate': true
                                }
                            ],
                            'URL': null,
                            'Validate': true
                        },
                        {
                            'Id': 263,
                            'ParentMenuId': 144,
                            'Name': 'Menu.OthersPurchase',
                            'Items': [
                                {
                                    'Id': 265,
                                    'ParentMenuId': 263,
                                    'Name': 'Menu.MaterialSinglePOEdit',
                                    'Items': null,
                                    'URL': 'MaterialSinglePOEditComponent',
                                    'Validate': true
                                },
                                {
                                    'Id': 284,
                                    'ParentMenuId': 263,
                                    'Name': 'Menu.MaterialBatchSinglePOEdit',
                                    'Items': null,
                                    'URL': 'MaterialBatchSinglePOEditComponent',
                                    'Validate': true
                                }, 
                                {
                                    'Id': 266,
                                    'ParentMenuId': 263,
                                    'Name': 'Menu.MaterialSupplementPOEdit',
                                    'Items': null,
                                    'URL': 'MaterialSupplementPOEditComponent',
                                    'Validate': true
                                }, 
                                {
                                    'Id': 283,
                                    'ParentMenuId': 263,
                                    'Name': 'Menu.MaterialBatchSupplementPOEdit',
                                    'Items': null,
                                    'URL': 'MaterialBatchSupplementPOEditComponent',
                                    'Validate': true
                                }, 
                                {
                                    'Id': 268,
                                    'ParentMenuId': 263,
                                    'Name': 'Menu.MaterialOutsourcePOEdit',
                                    'Items': null,
                                    'URL': 'MaterialOutsourcePOEditComponent',
                                    'Validate': true
                                },
                                {
                                    'Id': 281,
                                    'ParentMenuId': 263,
                                    'Name': 'Menu.MaterialBatchOutsourcePOEdit',
                                    'Items': null,
                                    'URL': 'MaterialBatchOutsourcePOEditComponent',
                                    'Validate': true
                                },
                                {
                                    'Id': 269,
                                    'ParentMenuId': 263,
                                    'Name': 'Menu.MaterialOutsourceSupplementPOEdit',
                                    'Items': null,
                                    'URL': 'MaterialOutsourceSupplementPOEditComponent',
                                    'Validate': true
                                },
                                {
                                    'Id': 282,
                                    'ParentMenuId': 263,
                                    'Name': 'Menu.MaterialBatchOutsourceSupplementPOEdit',
                                    'Items': null,
                                    'URL': 'MaterialBatchOutsourceSupplementPOEditComponent',
                                    'Validate': true
                                },

                            ],
                            'URL': null,
                            'Validate': true
                        },                        
                        {
                            'Id': 126,
                            'ParentMenuId': 144,
                            'Name': 'Menu.MaterialPOItemAdjustEdit',
                            'Items': null,
                            'URL': 'MaterialPOItemAdjustEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 264,
                            'ParentMenuId': 144,
                            'Name': 'Menu.MaterialPOItemCloseEdit',
                            'Items': null,
                            'URL': 'MaterialPOItemCloseEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 311,
                            'ParentMenuId': 144,
                            'Name': 'Menu.MaterialPOItemClosureEdit',
                            'Items': null,
                            'URL': 'MaterialPOItemClosureEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 96,
                            'ParentMenuId': 144,
                            'Name': 'Menu.MaterialBatchPOCloseEdit',
                            'Items': null,
                            'URL': 'MaterialBatchPOCloseEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 285,
                            'ParentMenuId': 144,
                            'Name': 'Menu.MaterialPOItemRemoveEdit',
                            'Items': null,
                            'URL': 'MaterialPOItemRemoveEditComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 260,
                        'ParentMenuId': 142,
                        'Name': 'Menu.OutsoleTC',
                        'Items':  [
                            {
                                'Id': 261,
                                'ParentMenuId': 260,
                                'Name': 'Menu.OutsoleTCEdit',
                                'Items': null,
                                'URL': 'OutsoleTCEditComponent',
                                'Validate': true
                            },
                        ],
                        'URL': 'null',
                        'Validate': true
                    },
                    {
                        'Id': 226,
                        'ParentMenuId': 142,
                        'Name': 'Menu.AccountPayable',
                        'Items':  [
                            {
                                'Id': 228,
                                'ParentMenuId': 226,
                                'Name': 'Menu.APMonthEdit',
                                'Items': null,
                                'URL': 'APMonthEditComponent',
                                'Validate': true
                            },
                            {
                                'Id': 410,
                                'ParentMenuId': 226,
                                'Name': 'Menu.APMonthAdvEdit',
                                'Items': null,
                                'URL': 'APMonthAdvEditComponent',
                                'Validate': true
                            },
                        ],
                        'URL': 'null',
                        'Validate': true
                    }
                    ]
                },
                {
                    'Id': 179,
                    'ParentMenuId': null,
                    'Name': 'Menu.WarehouseMgt',
                    'Items': [
                    {
                        'Id': 186,
                        'ParentMenuId': 179,
                        'Name': 'Menu.MaterialReceive',
                        'Items': [
                        {
                            'Id': 187,
                            'ParentMenuId': 186,
                            'Name': 'Menu.MaterialReceivedEdit',
                            'Items': null,
                            'URL': 'MaterialReceivedEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 188,
                            'ParentMenuId': 186,
                            'Name': 'Menu.MaterialTransferReceivedEdit',
                            'Items': null,
                            'URL': 'MaterialTransferReceivedEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 189,
                            'ParentMenuId': 186,
                            'Name': 'Menu.MaterialTransferEdit',
                            'Items': null,
                            'URL': 'MaterialTransferEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 190,
                            'ParentMenuId': 186,
                            'Name': 'Menu.MaterialOutsourceReceivedEdit',
                            'Items': null,
                            'URL': 'MaterialOutsourceReceivedEditComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 191,
                        'ParentMenuId': 179,
                        'Name': 'Menu.MaterialStockIn',
                        'Items': [
                        {
                            'Id': 192,
                            'ParentMenuId': 191,
                            'Name': 'Menu.MaterialStockInPOEdit',
                            'Items': null,
                            'URL': 'MaterialStockInPOEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 193,
                            'ParentMenuId': 191,
                            'Name': 'Menu.MaterialStockInOtherEdit',
                            'Items': null,
                            'URL': 'MaterialStockInOtherEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 300,
                            'ParentMenuId': 191,
                            'Name': 'Menu.MaterialStockInReturnEdit',
                            'Items': null,
                            'URL': 'MaterialStockInReturnEditComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 194,
                        'ParentMenuId': 179,
                        'Name': 'Menu.MaterialStockOut',
                        'Items': [
                        {
                            'Id': 195,
                            'ParentMenuId': 194,
                            'Name': 'Menu.MaterialStockOutPOEdit',
                            'Items': null,
                            'URL': 'MaterialStockOutPOEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 196,
                            'ParentMenuId': 194,
                            'Name': 'Menu.MaterialStockOutPOBatchEdit',
                            'Items': null,
                            'URL': 'MaterialStockOutPOBatchEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 406,
                            'ParentMenuId': 194,
                            'Name': 'Menu.MaterialStockOutMPSDailyBatchEdit',
                            'Items': null,
                            'URL': 'MaterialStockOutMPSDailyBatchEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 292,
                            'ParentMenuId': 194,
                            'Name': 'Menu.MaterialStockOutSubEdit',
                            'Items': null,
                            'URL': 'MaterialStockOutSubEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 293,
                            'ParentMenuId': 194,
                            'Name': 'Menu.MaterialStockOutSupplementEdit',
                            'Items': null,
                            'URL': 'MaterialStockOutSupplementEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 298,
                            'ParentMenuId': 194,
                            'Name': 'Menu.MaterialStockOutOtherEdit',
                            'Items': null,
                            'URL': 'MaterialStockOutOtherEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 299,
                            'ParentMenuId': 194,
                            'Name': 'Menu.MaterialStockOutOutsourceEdit',
                            'Items': null,
                            'URL': 'MaterialStockOutOutsourceEditComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 294,
                        'ParentMenuId': 179,
                        'Name': 'Menu.MaterialStockTransfer',
                        'Items': [
                        {   
                            'Id': 297,
                            'ParentMenuId': 294,
                            'Name': 'Menu.MaterialStockTransferInterOrdersEdit',
                            'Items': null,
                            'URL': 'MaterialStockTransferInterOrdersEditComponent',
                            'Validate': true
                        },
                        {   
                            'Id': 295,
                            'ParentMenuId': 294,
                            'Name': 'Menu.MaterialStockTransferOrdersEdit',
                            'Items': null,
                            'URL': 'MaterialStockTransferOrdersEditComponent',
                            'Validate': true
                        },
                        {   
                            'Id': 296,
                            'ParentMenuId': 294,
                            'Name': 'Menu.MaterialStockTransferAvailableEdit',
                            'Items': null,
                            'URL': 'MaterialStockTransferAvailableEditComponent',
                            'Validate': true
                        },
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 180,
                        'ParentMenuId': 179,
                        'Name': 'Menu.MaterialStockAdj',
                        'Items': [
                        {
                            'Id': 312,
                            'ParentMenuId': 180,
                            'Name': 'Menu.MaterialReceivedPriceEdit',
                            'Items': null,
                            'URL': 'MaterialReceivedPriceEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 273,
                            'ParentMenuId': 180,
                            'Name': 'Menu.MaterialStockEdit',
                            'Items': null,
                            'URL': 'MaterialStockEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 182,
                            'ParentMenuId': 180,
                            'Name': 'Menu.MaterialStockBalanceEdit',
                            'Items': null,
                            'URL': 'MaterialStockBalanceEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 181,
                            'ParentMenuId': 180,
                            'Name': 'Menu.MaterialStockAdjEdit',
                            'Items': null,
                            'URL': 'MaterialStockAdjEditComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    }
                    ]
                },
                {
                    'Id': 227,
                    'ParentMenuId': null,
                    'Name': 'Menu.QualityMgt',
                    'Items': [
                    {
                        'Id': 232,
                        'ParentMenuId': 227,
                        'Name': 'Menu.MaterialInspection',
                        'Items': [
                        {
                            'Id': 233,
                            'ParentMenuId': 232,
                            'Name': 'Menu.MaterialInspectionEdit',
                            'Items': null,
                            'URL': 'MaterialInspectionEditComponent',
                            'Validate': true
                        },
                        ],
                        'URL': null,
                        'Validate': true
                    }
                    ],
                    'URL': 'null',
                    'Validate': true
                },
                {
                    'Id': 316,
                    'ParentMenuId': null,
                    'Name': 'Menu.BondMgt',
                    'Items': [
                    {
                        'Id': 317,
                        'ParentMenuId': 316,
                        'Name': 'Menu.BondProductChinaContrastEdit',
                        'Items': null,
                        'URL': 'BondProductChinaContrastEditComponent',
                        'Validate': true
                    },
                    {
                        'Id': 318,
                        'ParentMenuId': 316,
                        'Name': 'Menu.BondMaterialChinaContrastEdit',
                        'Items': null,
                        'URL': 'BondMaterialChinaContrastEditComponent',
                        'Validate': true
                    },
                    {
                        'Id': 319,
                        'ParentMenuId': 316,
                        'Name': 'Menu.BondMRPEdit',
                        'Items': null,
                        'URL': 'BondMRPEditComponent',
                        'Validate': true
                    },
                    {
                        'Id': 320,
                        'ParentMenuId': 316,
                        'Name': 'Menu.BondMRPItemEdit',
                        'Items': null,
                        'URL': 'BondMRPItemEditComponent',
                        'Validate': true
                    }
                    ],
                    'URL': null,
                    'Validate': true
                },
                {
                    'Id': 160,
                    'ParentMenuId': null,
                    'Name': 'Menu.ProductionPlanMgt',
                    'Items': [
                    {
                        'Id': 163,
                        'ParentMenuId': 160,
                        'Name': 'Menu.Usage',
                        'Items': [
                        {
                            'Id': 174,
                            'ParentMenuId': 163,
                            'Name': 'Menu.MPSStyleEdit',
                            'Items': null,
                            'URL': 'MPSStyleEditComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 161,
                        'ParentMenuId': 160,
                        'Name': 'Menu.Dispatch',
                        'Items': [
                         {
                            'Id': 170,
                            'ParentMenuId': 161,
                            'Name': 'Menu.MPSOrdersEdit',
                            'Items': null,
                            'URL': 'MPSOrdersEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 172,
                            'ParentMenuId': 161,
                            'Name': 'Menu.MPSPlanEdit',
                            'Items': null,
                            'URL': 'MPSPlanEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 173,
                            'ParentMenuId': 161,
                            'Name': 'Menu.MPSDailyEdit',
                            'Items': null,
                            'URL': 'MPSDailyEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 168,
                            'ParentMenuId': 161,
                            'Name': 'Menu.MPSDailyAddEdit',
                            'Items': null,
                            'URL': 'MPSDailyAddEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 343,
                            'ParentMenuId': 161,
                            'Name': 'Menu.MPSDailyDiffEdit',
                            'Items': null,
                            'URL': 'MPSDailyDiffEditComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 162,
                        'ParentMenuId': 160,
                        'Name': 'Menu.MPSOutsource',
                        'Items': [
                        {
                            'Id': 331,
                            'ParentMenuId': 162,
                            'Name': 'Menu.MPSOutsourceQuotationEdit',
                            'Items': null,
                            'URL': 'MPSOutsourceQuotationEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 332,
                            'ParentMenuId': 162,
                            'Name': 'Menu.MPSOutsourceComponentEdit',
                            'Items': null,
                            'URL': 'MPSOutsourceComponentEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 333,
                            'ParentMenuId': 162,
                            'Name': 'Menu.MPSOutsourceProcessEdit',
                            'Items': null,
                            'URL': 'MPSOutsourceProcessEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 342,
                            'ParentMenuId': 162,
                            'Name': 'Menu.MPSOutsourcePOEdit',
                            'Items': null,
                            'URL': 'MPSOutsourcePOEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 401,
                            'ParentMenuId': 162,
                            'Name': 'Menu.MPSOutsourcePOBatchEdit',
                            'Items': null,
                            'URL': 'MPSOutsourcePOBatchEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 334,
                            'ParentMenuId': 162,
                            'Name': 'Menu.MPSOutsourceReceivedEdit',
                            'Items': null,
                            'URL': 'MPSOutsourceReceivedEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 335,
                            'ParentMenuId': 162,
                            'Name': 'Menu.MPSOutsourceInspectEdit',
                            'Items': null,
                            'URL': 'MPSOutsourceInspectEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 336,
                            'ParentMenuId': 162,
                            'Name': 'Menu.MPSOutsourceAPEdit',
                            'Items': null,
                            'URL': 'MPSOutsourceAPEditComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 411,
                        'ParentMenuId': 160,
                        'Name': 'Menu.Inspect',
                        'Items': [
                        {
                            'Id': 412,
                            'ParentMenuId': 411,
                            'Name': 'Menu.OrdersInspPlanEdit',
                            'Items': null,
                            'URL': 'OrdersInspPlanEditComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 407,
                        'ParentMenuId': 160,
                        'Name': 'Menu.WIP',
                        'Items': [
                            {
                                'Id': 408,
                                'ParentMenuId': 407,
                                'Name': 'Menu.WIPInvTxnEdit',
                                'Items': null,
                                'URL': 'WIPInvTxnEditComponent',
                                'Validate': true
                            }
                        ],
                        'URL': null,
                        'Validate': true
                    }
                    ],
                    'URL': null,
                    'Validate': true
                },
                {
                    'Id': 5,
                    'ParentMenuId': null,
                    'Name': 'Menu.BrowseMgt',
                    'Items': [
                    {
                        'Id': 110,
                        'ParentMenuId': 5,
                        'Name': 'Menu.Quotation',
                        'Items': [
                        {
                            'Id': 111,
                            'ParentMenuId': 110,
                            'Name': 'Menu.ProductPriceBrowse',
                            'Items': null,
                            'URL': 'ProductPriceBrowseComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 24,
                        'ParentMenuId': 5,
                        'Name': 'Menu.Orders',
                        'Items': [
                        {
                            'Id': 58,
                            'ParentMenuId': 24,
                            'Name': 'Menu.PUMAOrdersBrowse',
                            'Items': null,
                            'URL': 'PUMAOrdersBrowseComponent',
                            'Validate': true
                        },
                        {
                            'Id': 59,
                            'ParentMenuId': 24,
                            'Name': 'Menu.NBOrdersBrowse',
                            'Items': null,
                            'URL': 'NBOrdersBrowseComponent',
                            'Validate': true
                        },
                        {
                            'Id': 60,
                            'ParentMenuId': 24,
                            'Name': 'Menu.ProductionOrdersBrowse',
                            'Items': null,
                            'URL': 'ProductionOrdersBrowseComponent',
                            'Validate': true
                        },
                        {
                            'Id': 61,
                            'ParentMenuId': 24,
                            'Name': 'Menu.PackingMaterialBrowse',
                            'Items': null,
                            'URL': 'PackingMaterialBrowseComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 25,
                        'ParentMenuId': 5,
                        'Name': 'Menu.Packing',
                        'Items': [
                        {
                            'Id': 62,
                            'ParentMenuId': 25,
                            'Name': 'Menu.PackingSpecBrowse',
                            'Items': null,
                            'URL': 'PackingSpecBrowseComponent',
                            'Validate': true
                        },
                        {
                            'Id': 63,
                            'ParentMenuId': 25,
                            'Name': 'Menu.PackingArticleBrowse',
                            'Items': null,
                            'URL': 'PackingArticleBrowseComponent',
                            'Validate': true
                        },
                        {
                            'Id': 64,
                            'ParentMenuId': 25,
                            'Name': 'Menu.PackingPlanBrowse',
                            'Items': null,
                            'URL': 'PackingPlanBrowseComponent',
                            'Validate': true
                        },
                        {
                            'Id': 65,
                            'ParentMenuId': 25,
                            'Name': 'Menu.PackingLabelBrowse',
                            'Items': null,
                            'URL': 'PackingLabelBrowseComponent',
                            'Validate': true
                        },
                        {
                            'Id': 66,
                            'ParentMenuId': 25,
                            'Name': 'Menu.PackingMarkBrowse',
                            'Items': null,
                            'URL': 'PackingMarkBrowseComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 26,
                        'ParentMenuId': 5,
                        'Name': 'Menu.BOM',
                        'Items': [
                        {
                            'Id': 67,
                            'ParentMenuId': 26,
                            'Name': 'Menu.BOMGenerateBrowse',
                            'Items': null,
                            'URL': 'BOMGenerateBrowseComponent',
                            'Validate': true
                        },
                        {
                            'Id': 68,
                            'ParentMenuId': 26,
                            'Name': 'Menu.BOMTaskBrowse',
                            'Items': null,
                            'URL': 'BOMTaskBrowseComponent',
                            'Validate': true
                        },
                        {
                            'Id': 69,
                            'ParentMenuId': 26,
                            'Name': 'Menu.BOMTaskLog',
                            'Items': null,
                            'URL': 'BOMTaskLogBrowseComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    }
                    ],
                    'URL': null,
                    'Validate': true
                },
                {
                    'Id': 6,
                    'ParentMenuId': null,
                    'Name': 'Menu.SearchMgt',
                    'Items': [
                    {
                        'Id': 375,
                        'ParentMenuId': 6,
                        'Name': 'Menu.RD',
                        'Items': [
                        {
                            'Id': 376,
                            'ParentMenuId': 375,
                            'Name': 'Menu.RDPurchase',
                            'Items': [
                            {
                                'Id': 378,
                                'ParentMenuId': 376,
                                'Name': 'Menu.RDPOItemSearch',
                                'Items': null,
                                'URL': 'RDPOItemSearchComponent',
                                'Validate': true
                            },
                            {
                                'Id': 379,
                                'ParentMenuId': 376,
                                'Name': 'Menu.AccountPayable',
                                'Items': [
                                {
                                    'Id': 380,
                                    'ParentMenuId': 379,
                                    'Name': 'Menu.RDMonthlyAPRecdSearch',
                                    'Items': null,
                                    'URL': 'RDMonthlyAPRecdSearchComponent',
                                    'Validate': true
                                },
                                {
                                    'Id': 381,
                                    'ParentMenuId': 379,
                                    'Name': 'Menu.RDMonthlyAPPaySearch',
                                    'Items': null,
                                    'URL': 'RDMonthlyAPPaySearchComponent',
                                    'Validate': true
                                }
                                ],
                                'URL': null,
                                'Validate': true
                            }
                            ],               
                            'URL': null,
                            'Validate': true
                        },
                        {
                            'Id': 377,
                            'ParentMenuId': 375,
                            'Name': 'Menu.RDWarehouse',
                            'Items': [
                            {
                                'Id': 382,
                                'ParentMenuId': 377,
                                'Name': 'Menu.RDMaterialStockItemSearch',
                                'Items': null,
                                'URL': 'RDMaterialStockItemSearchComponent',
                                'Validate': true
                            },
                            {
                                'Id': 383,
                                'ParentMenuId': 377,
                                'Name': 'Menu.RDMaterialStockBalanceSearch',
                                'Items': null,
                                'URL': 'RDMaterialStockBalanceSearchComponent',
                                'Validate': true
                            }
                            ],
                            'URL': null,
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 274,
                        'ParentMenuId': 6,
                        'Name': 'Menu.Product',
                        'Items': [
                        {
                            'Id': 29,
                            'ParentMenuId': 274,
                            'Name': 'Menu.StyleSearch',
                            'Items': null,
                            'URL': 'StyleSearchComponent',
                            'Validate': true
                        },
                        {
                            'Id': 275,
                            'ParentMenuId': 274,
                            'Name': 'Menu.StyleMaterialSearch',
                            'Items': null,
                            'URL': 'StyleMaterialSearchComponent',
                            'Validate': true
                        },
                        {
                            'Id': 352,
                            'ParentMenuId': 274,
                            'Name': 'Menu.StylePartUsageSearch',
                            'Items': null,
                            'URL': 'StylePartUsageSearchComponent',
                            'Validate': true
                        },
                        {
                            'Id': 351,
                            'ParentMenuId': 274,
                            'Name': 'Menu.StyleLogSearch',
                            'Items': null,
                            'URL': 'StyleLogSearchComponent',
                            'Validate': true
                        },
                        {
                            'Id': 278,
                            'ParentMenuId': 274,
                            'Name': 'Menu.BOMMaterialSearch',
                            'Items': null,
                            'URL': 'BOMMaterialSearchComponent',
                            'Validate': true
                        },
                        {
                            'Id': 286,
                            'ParentMenuId': 274,
                            'Name': 'Menu.BOMMaterialUsageSearch',
                            'Items': null,
                            'URL': 'BOMMaterialUsageSearchComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 27,
                        'ParentMenuId': 6,
                        'Name': 'Menu.Orders',
                        'Items': [
                        {
                            'Id': 70,
                            'ParentMenuId': 27,
                            'Name': 'Menu.OrdersListSearch',
                            'Items': null,
                            'URL': 'OrdersListSearchComponent',
                            'Validate': true
                        },
                        {
                            'Id': 71,
                            'ParentMenuId': 27,
                            'Name': 'Menu.OrdersSizeRunSearch',
                            'Items': null,
                            'URL': 'OrdersSizeRunSearchComponent',
                            'Validate': true
                        },
                        {
                            'Id': 112,
                            'ParentMenuId': 27,
                            'Name': 'Menu.OrdersSummarySearch',
                            'Items': null,
                            'URL': 'OrdersSummarySearchComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 28,
                        'ParentMenuId': 6,
                        'Name': 'Menu.Packing',
                        'Items': [
                        {
                            'Id': 136,
                            'ParentMenuId': 28,
                            'Name': 'Menu.PackingMaterialSearch',
                            'Items': null,
                            'URL': 'PackingMaterialSearchComponent',
                            'Validate': true
                        },
                        {
                            'Id': 74,
                            'ParentMenuId': 28,
                            'Name': 'Menu.PackingListSearch',
                            'Items': null,
                            'URL': 'PackingListSearchComponent',
                            'Validate': true
                        },
                        {
                            'Id': 241,
                            'ParentMenuId': 28,
                            'Name': 'Menu.OrdersLabelSearch',
                            'Items': null,
                            'URL': 'OrdersLabelSearchComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 137,
                        'ParentMenuId': 6,
                        'Name': 'Menu.Stock',
                        'Items': [
                        {
                            'Id': 72,
                            'ParentMenuId': 137,
                            'Name': 'Menu.OrdersStockSearch',
                            'Items': null,
                            'URL': 'OrdersStockSearchComponent',
                            'Validate': true
                        },
                        {
                            'Id': 73,
                            'ParentMenuId': 137,
                            'Name': 'Menu.OrdersStockSummarySearch',
                            'Items': null,
                            'URL': 'OrdersStockSummarySearchComponent',
                            'Validate': true
                        },
                        {
                            'Id': 138,
                            'ParentMenuId': 137,
                            'Name': 'Menu.StocktakingSearch',
                            'Items': null,
                            'URL': 'StocktakingSearchComponent',
                            'Validate': true
                        },
                        {
                            'Id': 171,
                            'ParentMenuId': 137,
                            'Name': 'Menu.OrdersStockLocaleInSearch',
                            'Items': null,
                            'URL': 'OrdersStockLocaleInSearchComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 108,
                        'ParentMenuId': 6,
                        'Name': 'Menu.Quotation',
                        'Items': [
                        {
                            'Id': 109,
                            'ParentMenuId': 108,
                            'Name': 'Menu.ProductPriceSearch',
                            'Items': null,
                            'URL': 'ProductPriceSearchComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 113,
                        'ParentMenuId': 6,
                        'Name': 'Menu.Shipment',
                        'Items': [
                        {
                            'Id': 114,
                            'ParentMenuId': 113,
                            'Name': 'Menu.OrderShipmentSearch',
                            'Items': null,
                            'URL': 'OrderShipmentSearchComponent',
                            'Validate': true
                        },
                        {
                            'Id': 116,
                            'ParentMenuId': 113,
                            'Name': 'Menu.OrderIncomeEstimateSearch',
                            'Items': null,
                            'URL': 'OrderIncomeSearchComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 127,
                        'ParentMenuId': 6,
                        'Name': 'Menu.Purchase',
                        'Items': [
                        {
                            'Id': 148,
                            'ParentMenuId': 127,
                            'Name': 'Menu.MaterialQuotationSearch',
                            'Items': null,
                            'URL': 'MaterialQuotationSearchComponent',
                            'Validate': true
                        },
                        {
                            'Id': 98,
                            'ParentMenuId': 127,
                            'Name': 'Menu.BOMSizeRunSearch',
                            'Items': null,
                            'URL': 'BOMSizeRunSearchComponent',
                            'Validate': true
                        },
                        {
                            'Id': 140,
                            'ParentMenuId': 127,
                            'Name': 'Menu.PurBatchOrdersSearch',
                            'Items': null,
                            'URL': 'PurBatchOrdersSearchComponent',
                            'Validate': true
                        },
                        {
                            'Id': 106,
                            'ParentMenuId': 127,
                            'Name': 'Menu.POProcessSearch',
                            'Items': null,
                            'URL': 'POProcessSearchComponent',
                            'Validate': true
                        },
                        {
                            'Id': 288,
                            'ParentMenuId': 127,
                            'Name': 'Menu.POItemSearch',
                            'Items': null,
                            'URL': 'POItemSearchComponent',
                            'Validate': true
                        },
                        {
                            'Id': 409,
                            'ParentMenuId': 127,
                            'Name': 'Menu.POItemSizeRunSearch',
                            'Items': null,
                            'URL': 'POItemSizeRunSearchComponent',
                            'Validate': true
                        },
                        {
                            'Id': 156,
                            'ParentMenuId': 127,
                            'Name': 'Menu.POBOMSearch',
                            'Items': null,
                            'URL': 'POBOMSearchComponent',
                            'Validate': true
                        },
                        {
                            'Id': 164,
                            'ParentMenuId': 127,
                            'Name': 'Menu.POVendorSearch',
                            'Items': null,
                            'URL': 'POVendorSearchComponent',
                            'Validate': true
                        },
                        {
                            'Id': 230,
                            'ParentMenuId': 127,
                            'Name': 'Menu.POPeddingReceiptSearch',
                            'Items': null,
                            'URL': 'POPeddingReceiptSearchComponent',
                            'Validate': true
                        },
                        {
                            'Id': 128,
                            'ParentMenuId': 127,
                            'Name': 'Menu.POSizeRunSearch',
                            'Items': null,
                            'URL': 'POSizeRunSearchComponent',
                            'Validate': true
                        },
                        {
                            'Id': 211,
                            'ParentMenuId': 127,
                            'Name': 'Menu.AccountPayable',
                            'Items':  [
                                {
                                    'Id': 212,
                                    'ParentMenuId': 211,
                                    'Name': 'Menu.MonthlyAPRecdSearch',
                                    'Items': null,
                                    'URL': 'MonthlyAPRecdSearchComponent',
                                    'Validate': true
                                },
                                {
                                    'Id': 213,
                                    'ParentMenuId': 211,
                                    'Name': 'Menu.MonthlyAPPaySearch',
                                    'Items': null,
                                    'URL': 'MonthlyAPPaySearchComponent',
                                    'Validate': true
                                },
                            ],
                            'URL': 'null',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 152,
                        'ParentMenuId': 6,
                        'Name': 'Menu.Warehouse',
                        'Items': [
                        {
                            'Id': 234,
                            'ParentMenuId': 152,
                            'Name': 'Menu.MaterialProcess',
                            'Items': [
                            {
                                'Id': 235,
                                'ParentMenuId': 234,
                                'Name': 'Menu.MaterialProcessSearch',
                                'Items': null,
                                'URL': 'MaterialProcessSearchComponent',
                                'Validate': true
                            }
                            ],
                            'URL': null,
                            'Validate': true
                        },
                        {
                            'Id': 198,
                            'ParentMenuId': 152,
                            'Name': 'Menu.MaterialReceive',
                            'Items': [
                                {
                                    'Id': 301,
                                    'ParentMenuId': 198,
                                    'Name': 'Menu.MaterialReceiveSearch',
                                    'Items': null,
                                    'URL': 'MaterialReceiveSearchComponent',
                                    'Validate': true
                                },
                                {
                                    'Id': 200,
                                    'ParentMenuId': 198,
                                    'Name': 'Menu.MaterialTransferSearch',
                                    'Items': null,
                                    'URL': 'MaterialTransferSearchComponent',
                                    'Validate': true
                                }
                            ],
                            'URL': null,
                            'Validate': true
                        },
                        {
                            'Id': 197,
                            'ParentMenuId': 152,
                            'Name': 'Menu.MaterialStock',
                            'Items': [
                                {
                                    'Id': 153,
                                    'ParentMenuId': 197,
                                    'Name': 'Menu.MaterialStockSearch',
                                    'Items': null,
                                    'URL': 'MaterialStockSearchComponent',
                                    'Validate': true
                                },
                                {
                                    'Id': 34,
                                    'ParentMenuId': 197,
                                    'Name': 'Menu.MaterialStockItemSearch',
                                    'Items': null,
                                    'URL': 'MaterialStockItemSearchComponent',
                                    'Validate': true
                                },
                                {
                                    'Id': 219,
                                    'ParentMenuId': 197,
                                    'Name': 'Menu.MaterialStockBalanceSearch',
                                    'Items': null,
                                    'URL': 'MaterialStockBalanceSearchComponent',
                                    'Validate': true
                                },
                                {
                                    'Id': 287,
                                    'ParentMenuId': 197,
                                    'Name': 'Menu.MaterialCoordinateSearch',
                                    'Items': null,
                                    'URL': 'MaterialCoordinateSearchComponent',
                                    'Validate': true
                                }
                            ],
                            'URL': null,
                            'Validate': true
                        },
                        {
                            'Id': 206,
                            'ParentMenuId': 152,
                            'Name': 'Menu.MaterialStockClose',
                            'Items': [
                                {
                                    'Id': 207,
                                    'ParentMenuId': 206,
                                    'Name': 'Menu.StockCloseTimeSearch',
                                    'Items': null,
                                    'URL': 'StockCloseTimeSearchComponent',
                                    'Validate': true
                                }
                            ],
                            'URL': null,
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 154,
                        'ParentMenuId': 6,
                        'Name': 'Menu.ProductPlan',
                        'Items': [
                        {
                            'Id': 155,
                            'ParentMenuId': 154,
                            'Name': 'Menu.Dispatch',
                            'Items': [
                            ],
                            'URL': null,
                            'Validate': true
                        },
                        {
                            'Id': 246,
                            'ParentMenuId': 154,
                            'Name': 'Menu.MPSOutsource',
                            'Items': [
                            {
                                'Id': 397,
                                'ParentMenuId': 246,
                                'Name': 'Menu.OutsourceSearch',
                                'Items': null,
                                'URL': 'OutsourceSearchComponent',
                                'Validate': true
                            },
                            {
                                'Id': 398,
                                'ParentMenuId': 246,
                                'Name': 'Menu.MonthlyAPOutsourceSearch',
                                'Items': null,
                                'URL': 'MonthlyAPOutsourceSearchComponent',
                                'Validate': true
                            }
                            ],
                            'URL': null,
                            'Validate': true
                        } 
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 321,
                        'ParentMenuId': 6,
                        'Name': 'Menu.Bond',
                        'Items': [
                        {
                            'Id': 322,
                            'ParentMenuId': 321,
                            'Name': 'Menu.BondMRPItemSearch',
                            'Items': null,
                            'URL': 'BondMRPItemSearchComponent',
                            'Validate': true
                        },
                        {
                            'Id': 323,
                            'ParentMenuId': 321,
                            'Name': 'Menu.BondOrderSizeRunSearch',
                            'Items': null,
                            'URL': 'BondOrderSizeRunSearchComponent',
                            'Validate': true
                        },
                        {
                            'Id': 324,
                            'ParentMenuId': 321,
                            'Name': 'Menu.BondMaterialStockItemSearch',
                            'Items': null,
                            'URL': 'BondMaterialStockItemSearchComponent',
                            'Validate': true
                        },
                        {
                            'Id': 325,
                            'ParentMenuId': 321,
                            'Name': 'Menu.BondMaterialStockBalanceSearch',
                            'Items': null,
                            'URL': 'BondMaterialStockBalanceSearchComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 122,
                        'ParentMenuId': 6,
                        'Name': 'Menu.Account',
                        'Items': [
                        {
                            'Id': 149,
                            'ParentMenuId': 122,
                            'Name': 'Menu.ARBalanceSearch',
                            'Items': null,
                            'URL': 'ARBalanceSearchComponent',
                            'Validate': true
                        },
                        {
                            'Id': 129,
                            'ParentMenuId': 122,
                            'Name': 'Menu.MaterialUseCostSearch',
                            'Items': null,
                            'URL': 'MaterialUseCostSearchComponent',
                            'Validate': true
                        },
                        {
                            'Id': 150,
                            'ParentMenuId': 122,
                            'Name': 'Menu.BatchOrdersCost',
                            'Items': [
                                {
                                    'Id': 270,
                                    'ParentMenuId': 150,
                                    'Name': 'Menu.BatchOrdersCostSearch',
                                    'Items': null,
                                    'URL': 'BatchOrdersCostSearchComponent',
                                    'Validate': true
                                },
                                {
                                    'Id': 271,
                                    'ParentMenuId': 150,
                                    'Name': 'Menu.BatchOrdersCostNBSearch',
                                    'Items': null,
                                    'URL': 'BatchOrdersCostNBSearchComponent',
                                    'Validate': true
                                },
                                {
                                    'Id': 272,
                                    'ParentMenuId': 150,
                                    'Name': 'Menu.BatchOrdersCostPUMASearch',
                                    'Items': null,
                                    'URL': 'BatchOrdersCostPUMASearchComponent',
                                    'Validate': true
                                }
                            ],
                            'URL': null,
                            'Validate': true
                        },
                        {
                            'Id': 151,
                            'ParentMenuId': 122,
                            'Name': 'Menu.OrderShipmentCostSearch',
                            'Items': null,
                            'URL': 'OrderShipmentCostSearchComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 306,
                        'ParentMenuId': 6,
                        'Name': 'Menu.Management',
                        'Items': [
                        {
                            'Id': 291,
                            'ParentMenuId': 306,
                            'Name': 'Menu.OrdersRecordSummarySearch',
                            'Items': null,
                            'URL': 'OrdersRecordSummarySearchComponent',
                            'Validate': true
                        } ,
                        {
                            'Id': 303,
                            'ParentMenuId': 306,
                            'Name': 'Menu.MaterialFirstStockOutSearch',
                            'Items': null,
                            'URL': 'MaterialFirstStockOutSearchComponent',
                            'Validate': true
                        },
                        ],
                        'URL': null,
                        'Validate': true
                    }
                    ],
                    'URL': null,
                    'Validate': true
                },
                {
                    'Id': 157,
                    'ParentMenuId': null,
                    'Name': 'Menu.CrossSearchMgt',
                    'Items': [
                    {
                        'Id': 384,
                        'ParentMenuId': 157,
                        'Name': 'Menu.RD',
                        'Items': [
                        {
                            'Id': 385,
                            'ParentMenuId': 384,
                            'Name': 'Menu.RDPurchase',
                            'Items': [
                            {
                                'Id': 386,
                                'ParentMenuId': 385,
                                'Name': 'Menu.RDPOItemCrossSearch',
                                'Items': null,
                                'URL': 'RDPOItemCrossSearchComponent',
                                'Validate': true
                            },
                            {
                                'Id': 387,
                                'ParentMenuId': 385,
                                'Name': 'Menu.AccountPayable',
                                'Items': [
                                {
                                    'Id': 388,
                                    'ParentMenuId': 387,
                                    'Name': 'Menu.RDMonthlyAPRecdCrossSearch',
                                    'Items': null,
                                    'URL': 'RDMonthlyAPRecdCrossSearchComponent',
                                    'Validate': true
                                },
                                {
                                    'Id': 389,
                                    'ParentMenuId': 387,
                                    'Name': 'Menu.RDMonthlyAPPayCrossSearch',
                                    'Items': null,
                                    'URL': 'RDMonthlyAPPayCrossSearchComponent',
                                    'Validate': true
                                }
                                ],
                                'URL': null,
                                'Validate': true
                            }
                            ],               
                            'URL': null,
                            'Validate': true
                        },
                        {
                            'Id': 390,
                            'ParentMenuId': 384,
                            'Name': 'Menu.RDWarehouse',
                            'Items': [
                            {
                                'Id': 391,
                                'ParentMenuId': 390,
                                'Name': 'Menu.RDMaterialStockItemCrossSearch',
                                'Items': null,
                                'URL': 'RDMaterialStockItemCrossSearchComponent',
                                'Validate': true
                            },
                            {
                                'Id': 392,
                                'ParentMenuId': 390,
                                'Name': 'Menu.RDMaterialStockBalanceCrossSearch',
                                'Items': null,
                                'URL': 'RDMaterialStockBalanceCrossSearchComponent',
                                'Validate': true
                            }
                            ],
                            'URL': null,
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 276,
                        'ParentMenuId': 157,
                        'Name': 'Menu.Product',
                        'Items': [
                        {
                            'Id': 33,
                            'ParentMenuId': 276,
                            'Name': 'Menu.StyleCrossSearch',
                            'Items': null,
                            'URL': 'StyleCrossSearchComponent',
                            'Validate': true
                        },
                        {
                            'Id': 280,
                            'ParentMenuId': 276,
                            'Name': 'Menu.StyleMaterialCrossSearch',
                            'Items': null,
                            'URL': 'StyleMaterialCrossSearchComponent',
                            'Validate': true
                        },
                        {
                            'Id': 354,
                            'ParentMenuId': 276,
                            'Name': 'Menu.StylePartUsageCrossSearch',
                            'Items': null,
                            'URL': 'StylePartUsageCrossSearchComponent',
                            'Validate': true
                        },
                        {
                            'Id': 279,
                            'ParentMenuId': 276,
                            'Name': 'Menu.BOMMaterialCrossSearch',
                            'Items': null,
                            'URL': 'BOMMaterialCrossSearchComponent',
                            'Validate': true
                        },
                        {
                            'Id': 353,
                            'ParentMenuId': 276,
                            'Name': 'Menu.BOMMaterialUsageCrossSearch',
                            'Items': null,
                            'URL': 'BOMMaterialUsageCrossSearchComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 248,
                        'ParentMenuId': 157,
                        'Name': 'Menu.Orders',
                        'Items': [
                        {
                            'Id': 249,
                            'ParentMenuId': 248,
                            'Name': 'Menu.OrdersListCrossSearch',
                            'Items': null,
                            'URL': 'OrdersListCrossSearchComponent',
                            'Validate': true
                        },
                        {
                            'Id': 250,
                            'ParentMenuId': 248,
                            'Name': 'Menu.OrdersSizeRunCrossSearch',
                            'Items': null,
                            'URL': 'OrdersSizeRunCrossSearchComponent',
                            'Validate': true
                        },
                        {
                            'Id': 251,
                            'ParentMenuId': 248,
                            'Name': 'Menu.OrdersSummaryCrossSearch',
                            'Items': null,
                            'URL': 'OrdersSummaryCrossSearchComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 242,
                        'ParentMenuId': 157,
                        'Name': 'Menu.Packing',
                        'Items': [
                        {
                            'Id': 243,
                            'ParentMenuId': 242,
                            'Name': 'Menu.OrdersLabelCrossSearch',
                            'Items': null,
                            'URL': 'OrdersLabelCrossSearchComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 175,
                        'ParentMenuId': 157,
                        'Name': 'Menu.Stock',
                        'Items': [
                        {
                            'Id': 216,
                            'ParentMenuId': 175,
                            'Name': 'Menu.OrdersStockCrossSearch',
                            'Items': null,
                            'URL': 'OrdersStockCrossSearchComponent',
                            'Validate': true
                        },
                        {
                            'Id': 176,
                            'ParentMenuId': 175,
                            'Name': 'Menu.OrdersStockSummaryCrossSearch',
                            'Items': null,
                            'URL': 'OrdersStockSummaryCrossSearchComponent',
                            'Validate': true
                        },
                        {
                            'Id': 229,
                            'ParentMenuId': 175,
                            'Name': 'Menu.StocktakingCrossSearch',
                            'Items': null,
                            'URL': 'StocktakingCrossSearchComponent',
                            'Validate': true
                        }

                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 165,
                        'ParentMenuId': 157,
                        'Name': 'Menu.Purchase',
                        'Items': [
                        {
                            'Id': 217,
                            'ParentMenuId': 165,
                            'Name': 'Menu.MaterialQuotationCrossSearch',
                            'Items': null,
                            'URL': 'MaterialQuotationCrossSearchComponent',
                            'Validate': true
                        },
                        {
                            'Id': 289,
                            'ParentMenuId': 165,
                            'Name': 'Menu.POItemCrossSearch',
                            'Items': null,
                            'URL': 'POItemCrossSearchComponent',
                            'Validate': true
                        },
                        {
                            'Id': 218,
                            'ParentMenuId': 165,
                            'Name': 'Menu.POBOMCrossSearch',
                            'Items': null,
                            'URL': 'POBOMCrossSearchComponent',
                            'Validate': true
                        },
                        {
                            'Id': 166,
                            'ParentMenuId': 165,
                            'Name': 'Menu.POVendorCrossSearch',
                            'Items': null,
                            'URL': 'POVendorCrossSearchComponent',
                            'Validate': true
                        },
                        {
                            'Id': 231,
                            'ParentMenuId': 165,
                            'Name': 'Menu.POPeddingReceiptCrossSearch',
                            'Items': null,
                            'URL': 'POPeddingReceiptCrossSearchComponent',
                            'Validate': true
                        },
                        {
                            'Id': 210,
                            'ParentMenuId': 165,
                            'Name': 'Menu.AccountPayable',
                            'Items': [
                                {
                                    'Id': 214,
                                    'ParentMenuId': 210,
                                    'Name': 'Menu.MonthlyAPRecdCrossSearch',
                                    'Items': null,
                                    'URL': 'MonthlyAPRecdCrossSearchComponent',
                                    'Validate': true
                                },
                                {
                                    'Id': 215,
                                    'ParentMenuId': 210,
                                    'Name': 'Menu.MonthlyAPPayCrossSearch',
                                    'Items': null,
                                    'URL': 'MonthlyAPPayCrossSearchComponent',
                                    'Validate': true
                                }
                            ],
                            'URL': null,
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 158,
                        'ParentMenuId': 157,
                        'Name': 'Menu.Warehouse',
                        'Items': [
                        {
                            'Id': 236,
                            'ParentMenuId': 158,
                            'Name': 'Menu.MaterialProcess',
                            'Items': [
                            {
                                'Id': 237,
                                'ParentMenuId': 236,
                                'Name': 'Menu.MaterialProcessCrossSearch',
                                'Items': null,
                                'URL': 'MaterialProcessCrossSearchComponent',
                                'Validate': true
                            }
                            ],
                            'URL': null,
                            'Validate': true
                        },
                        {
                            'Id': 89,
                            'ParentMenuId': 158,
                            'Name': 'Menu.MaterialReceive',
                            'Items': [
                                {
                                    'Id': 302,
                                    'ParentMenuId': 89,
                                    'Name': 'Menu.MaterialReceiveCrossSearch',
                                    'Items': null,
                                    'URL': 'MaterialReceiveCrossSearchComponent',
                                    'Validate': true
                                },
                                {
                                    'Id': 94,
                                    'ParentMenuId': 89,
                                    'Name': 'Menu.MaterialTransferCrossSearch',
                                    'Items': null,
                                    'URL': 'MaterialTransferCrossSearchComponent',
                                    'Validate': true
                                }
                            ],
                            'URL': null,
                            'Validate': true
                        },
                        {
                            'Id': 199,
                            'ParentMenuId': 158,
                            'Name': 'Menu.MaterialStock',
                            'Items': [
                                {
                                    'Id': 159,
                                    'ParentMenuId': 199,
                                    'Name': 'Menu.MaterialStockCrossSearch',
                                    'Items': null,
                                    'URL': 'MaterialStockCrossSearchComponent',
                                    'Validate': true
                                },
                                {
                                    'Id': 86,
                                    'ParentMenuId': 199,
                                    'Name': 'Menu.MaterialStockItemCrossSearch',
                                    'Items': null,
                                    'URL': 'MaterialStockItemCrossSearchComponent',
                                    'Validate': true
                                },
                                {
                                    'Id': 220,
                                    'ParentMenuId': 199,
                                    'Name': 'Menu.MaterialStockBalanceCrossSearch',
                                    'Items': null,
                                    'URL': 'MaterialStockBalanceCrossSearchComponent',
                                    'Validate': true
                                },
                                {
                                    'Id': 290,
                                    'ParentMenuId': 199,
                                    'Name': 'Menu.MaterialCoordinateCrossSearch',
                                    'Items': null,
                                    'URL': 'MaterialCoordinateCrossSearchComponent',
                                    'Validate': true
                                }
                            ],
                            'URL': null,
                            'Validate': true
                        },
                        {
                            'Id': 208,
                            'ParentMenuId': 158,
                            'Name': 'Menu.MaterialStockClose',
                            'Items': [
                                {
                                    'Id': 209,
                                    'ParentMenuId': 208,
                                    'Name': 'Menu.StockCloseTimeCrossSearch',
                                    'Items': null,
                                    'URL': 'StockCloseTimeCrossSearchComponent',
                                    'Validate': true
                                }
                            ],
                            'URL': null,
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 244,
                        'ParentMenuId': 157,
                        'Name': 'Menu.ProductPlan',
                        'Items': [
                        {
                            'Id': 245,
                            'ParentMenuId': 244,
                            'Name': 'Menu.Dispatch',
                            'Items': [
                            ],
                            'URL': null,
                            'Validate': true
                        },
                        {
                            'Id': 247,
                            'ParentMenuId': 244,
                            'Name': 'Menu.MPSOutsource',
                            'Items': [
                            {
                                'Id': 399,
                                'ParentMenuId': 247,
                                'Name': 'Menu.OutsourceCrossSearch',
                                'Items': null,
                                'URL': 'OutsourceCrossSearchComponent',
                                'Validate': true
                            },
                            {
                                'Id': 400,
                                'ParentMenuId': 247,
                                'Name': 'Menu.MonthlyAPOutsourceCrossSearch',
                                'Items': null,
                                'URL': 'MonthlyAPOutsourceCrossSearchComponent',
                                'Validate': true
                            }
                            ],
                            'URL': null,
                            'Validate': true
                        } 
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 177,
                        'ParentMenuId': 157,
                        'Name': 'Menu.Account',
                        'Items': [
                        {
                            'Id': 178,
                            'ParentMenuId': 177,
                            'Name': 'Menu.MaterialUseCostCrossSearch',
                            'Items': null,
                            'URL': 'MaterialUseCostCrossSearchComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 305,
                        'ParentMenuId': 157,
                        'Name': 'Menu.Management',
                        'Items': [
                        {
                            'Id': 103,
                            'ParentMenuId': 305,
                            'Name': 'Menu.OrdersRecordSummaryCrossSearch',
                            'Items': null,
                            'URL': 'OrdersRecordSummaryCrossSearchComponent',
                            'Validate': true
                        } ,
                        {
                            'Id': 304,
                            'ParentMenuId': 305,
                            'Name': 'Menu.MaterialFirstStockOutCrossSearch',
                            'Items': null,
                            'URL': 'MaterialFirstStockOutCrossSearchComponent',
                            'Validate': true
                        },
                        ],
                        'URL': null,
                        'Validate': true
                    }
                    ],
                    'URL': null,
                    'Validate': true
                },
                {
                    'Id': 7,
                    'ParentMenuId': null,
                    'Name': 'Menu.ReportMgt',
                    'Items': [
                    {
                        'Id': 373,
                        'ParentMenuId': 7,
                        'Name': 'Menu.RD',
                        'Items': [
                        {
                            'Id': 374,
                            'ParentMenuId': 373,
                            'Name': 'Menu.RDPOPrint',
                            'Items': null,
                            'URL': 'RDPOPrintComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 348,
                        'ParentMenuId': 7,
                        'Name': 'Menu.Product',
                        'Items': [
                        {
                            'Id': 349,
                            'ParentMenuId': 348,
                            'Name': 'Menu.StyleMaterialPrint',
                            'Items': null,
                            'URL': 'StyleMaterialPrintComponent',
                            'Validate': true
                        },
                        {
                            'Id': 350,
                            'ParentMenuId': 348,
                            'Name': 'Menu.ArticleUsagePrint',
                            'Items': null,
                            'URL': 'ArticleUsagePrintComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 30,
                        'ParentMenuId': 7,
                        'Name': 'Menu.BOM',
                        'Items': [
                        {
                            'Id': 75,
                            'ParentMenuId': 30,
                            'Name': 'Menu.BOMPrint',
                            'Items': null,
                            'URL': 'BOMListPrintComponent',
                            'Validate': true
                        },
                        {
                            'Id': 130,
                            'ParentMenuId': 30,
                            'Name': 'Menu.BOMUsagePrint',
                            'Items': null,
                            'URL': 'BOMUsagePrintComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 31,
                        'ParentMenuId': 7,
                        'Name': 'Menu.Packing',
                        'Items': [
                        {
                            'Id': 76,
                            'ParentMenuId': 31,
                            'Name': 'Menu.PackingListPrint',
                            'Items': null,
                            'URL': 'PackingListPrintComponent',
                            'Validate': true
                        },
                        {
                            'Id': 77,
                            'ParentMenuId': 31,
                            'Name': 'Menu.PackingLabelListPrint',
                            'Items': null,
                            'URL': 'PackingLabelListPrintComponent',
                            'Validate': true
                        },
                        {
                            'Id': 240,
                            'ParentMenuId': 31,
                            'Name': 'Menu.OrdersLabelPrint',
                            'Items': null,
                            'URL': 'OrdersLabelPrintComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 167,
                        'ParentMenuId': 7,
                        'Name': 'Menu.ProductPlan',
                        'Items': [
                        {
                            'Id': 169,
                            'ParentMenuId': 167,
                            'Name': 'Menu.Dispatch',
                            'Items': [
                            {
                                'Id': 394,
                                'ParentMenuId': 169,
                                'Name': 'Menu.MPSDailyPrint',
                                'Items': null,
                                'URL': 'MPSDailyPrintComponent',
                                'Validate': true
                            },
                            {
                                'Id': 395,
                                'ParentMenuId': 169,
                                'Name': 'Menu.MPSDailyAddPrint',
                                'Items': null,
                                'URL': 'MPSDailyAddPrintComponent',
                                'Validate': true
                            }
                            ],
                            'URL': null,
                            'Validate': true
                        },
                        {
                            'Id': 362,
                            'ParentMenuId': 167,
                            'Name': 'Menu.MPSOutsource',
                            'Items': [
                            {
                                'Id': 396,
                                'ParentMenuId': 362,
                                'Name': 'Menu.MPSOutsourceReceivedPrint',
                                'Items': null,
                                'URL': 'MPSOutsourceReceivedPrintComponent',
                                'Validate': true
                            }
                            ],
                            'URL': null,
                            'Validate': true
                        } 
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 221,
                        'ParentMenuId': 7,
                        'Name': 'Menu.Purchase',
                        'Items': [
                        {
                            'Id': 222,
                            'ParentMenuId': 221,
                            'Name': 'Menu.POPrint',
                            'Items': null,
                            'URL': 'POPrintComponent',
                            'Validate': true
                        },
                        {
                            'Id': 307,
                            'ParentMenuId': 221,
                            'Name': 'Menu.POGroupPrint',
                            'Items': null,
                            'URL': 'POGroupPrintComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 308,
                        'ParentMenuId': 7,
                        'Name': 'Menu.Warehouse',
                        'Items': [
                        {
                            'Id': 309,
                            'ParentMenuId': 308,
                            'Name': 'Menu.AcceptancePrint',
                            'Items': null,
                            'URL': 'AcceptancePrintComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    }
                    ],
                    'URL': null,
                    'Validate': true
                },
                {
                    'Id': 253,
                    'ParentMenuId': null,
                    'Name': 'Menu.NBMgt',
                    'Items': [
                    {
                        'Id': 254,
                        'ParentMenuId': 253,
                        'Name': 'Menu.NBMaterial',
                        'Items': [
                        {
                            'Id': 256,
                            'ParentMenuId': 254,
                            'Name': 'Menu.NBMaterialEdit',
                            'Items': null,
                            'URL': 'NBMaterialEditComponent',
                            'Validate': true
                        },
                        {
                            'Id': 257,
                            'ParentMenuId': 254,
                            'Name': 'Menu.NBPPMEdit',
                            'Items': null,
                            'URL': 'NBPPMEditComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    },
                    {
                        'Id': 255,
                        'ParentMenuId': 253,
                        'Name': 'Menu.NBSearch',
                        'Items': [
                        {
                            'Id': 258,
                            'ParentMenuId': 255,
                            'Name': 'Menu.NBMaterialSearch',
                            'Items': null,
                            'URL': 'NBMaterialSearchComponent',
                            'Validate': true
                        },
                        {
                            'Id': 259,
                            'ParentMenuId': 255,
                            'Name': 'Menu.NBPOSearch',
                            'Items': null,
                            'URL': 'NBPOSearchComponent',
                            'Validate': true
                        },
                        {
                            'Id': 277,
                            'ParentMenuId': 255,
                            'Name': 'Menu.NBOSRSearch',
                            'Items': null,
                            'URL': 'NBOSRSearchComponent',
                            'Validate': true
                        }
                        ],
                        'URL': null,
                        'Validate': true
                    }
                    ],
                    'URL': null,
                    'Validate': true
                }
            ]         
            ";

            return JsonConvert.DeserializeObject<List<Models.Views.Common.TreeItem>>(menuJson);
        }
    }
}