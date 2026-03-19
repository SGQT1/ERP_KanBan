using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERP.Data.DbContexts;
using ERP.Data.Repositories;
using ERP.Data.Repositories.Bases;
using ERP.Data.Utilities;
using ERP.Models.Views;
using ERP.Models.Views.Common;
using ERP.Services.Bases;
using ERP.Services.Business.Entities;

namespace ERP.Services.Business {
    public class MenuService : BusinessService {
        private ERP.Services.Business.Entities.MenuService Menu { get; set; }

        public MenuService(
            ERP.Services.Business.Entities.MenuService menuService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork) {
            Menu = menuService;
        }

        public IQueryable<ERP.Models.Views.Menu> Get() {
            return Menu.Get();
        }

        public List<TreeItem> Tree() {
            List<TreeItem> tree = new List<TreeItem>();
            var items = Get().ToList();
            items.ForEach(i => { i.Validate = false; });

            var rootItems = items.Where(i => i.ParentMenuId == null).OrderBy(i => i.ItemSort).ToList();
            foreach (var rootItem in rootItems) {
                var treeItem = new TreeItem {
                    Id = rootItem.Id,
                    Name = rootItem.Name,
                    URL = rootItem.URL,
                    Validate = (bool) rootItem.Validate,
                    ParentMenuId = rootItem.ParentMenuId,
                    Code = rootItem.MenuCode == null ? "" : rootItem.MenuCode
                };
                treeItem.Items = RecursiveMenu(treeItem, items);
                tree.Add(treeItem);
            }
            return tree;
        }
        public List<TreeItem> Permission(string userId) {
            List<TreeItem> permission = new List<TreeItem>();
            var items = Menu.GetByUser(userId);
            var rootItems = items.Where(i => i.ParentMenuId == null).OrderBy(i => i.ItemSort).ToList();
            foreach (var rootItem in rootItems) {
                var treeItem = new TreeItem {
                    Id = rootItem.Id,
                    Name = rootItem.Name,
                    URL = rootItem.URL,
                    Validate = (bool) rootItem.Validate,
                    ParentMenuId = rootItem.ParentMenuId,
                    Code = rootItem.MenuCode == null ? "" : rootItem.MenuCode,
                    IsExpand = true,
                    IsCheck = false,
                };
                treeItem.Items = RecursiveMenu(treeItem, items);
                permission.Add(treeItem);
            }
            return permission;
        }
        private List<TreeItem> RecursiveMenu(TreeItem parentNode, List<Menu> items) {
            var childNodes = items.Where(i => i.ParentMenuId == parentNode.Id).OrderBy(i => i.ItemSort).ToList();

            if (childNodes.Count() > 0) {
                List<TreeItem> treeItems = new List<TreeItem>();
                foreach (var childNode in childNodes) {
                    var treeItem = new TreeItem {
                        Id = childNode.Id,
                        Name = childNode.Name,
                        URL = childNode.URL,
                        Validate = (bool) childNode.Validate,
                        ParentMenuId = parentNode.Id,
                        Code = childNode.MenuCode == null ? "" : childNode.MenuCode,
                        IsExpand = true,
                        IsCheck = false,
                    };
                    treeItem.Items = RecursiveMenu(treeItem, items);
                    treeItems.Add(treeItem);
                }
                return treeItems;
            }

            return null;
        }
    }
}