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
    public class GroupService : BusinessService {
        private ERP.Services.Business.Entities.GroupService Group { get; set; }
        private ERP.Services.Business.Entities.GroupPermissionService GroupPermission { get; set; }

        public GroupService(
            ERP.Services.Business.Entities.GroupService groupService,
            ERP.Services.Business.Entities.GroupPermissionService groupPermissionService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork) {
            Group = groupService;
            GroupPermission = groupPermissionService;
        }

        public IQueryable<ERP.Models.Views.Group> Get() {
            return Group.Get();
        }

        public ERP.Models.Views.Group Create(Group item) {
            UnitOfWork.BeginTransaction();
            try {
                item = Group.Create(item);
                UnitOfWork.Commit();
                return item;
            } catch (Exception e) {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        public ERP.Models.Views.Group Update(Group item) {
            try {
                UnitOfWork.BeginTransaction();
                item = Group.Update(item);
                UnitOfWork.Commit();
                return item;
            } catch (Exception e) {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        public void Remove(Group item) {
            try {
                UnitOfWork.BeginTransaction();
                Group.Remove(item);
                UnitOfWork.Commit();
            } catch (Exception e) {
                UnitOfWork.Rollback();
                throw e;
            }
        }

        public List<TreeItem> GroupTree() {
            List<TreeItem> group = new List<TreeItem>();
            var items = Group.Get().ToList();
            var rootItems = items.Where(i => i.ParentGroupId == 0).OrderBy(i => i.Name).ToList();
            foreach (var rootItem in rootItems) {
                var treeItem = new TreeItem {
                    Id = rootItem.Id.ToString(),
                    Name = rootItem.Name,
                    Validate = (bool) rootItem.Validate,
                    ParentMenuId = "0"
                };
                treeItem.Items = RecursiveItem(treeItem, items);
                group.Add(treeItem);
            }

            return group;
        }
        private List<TreeItem> RecursiveItem(TreeItem parentNode, List<Group> items) {
            var childNodes = items.Where(i => i.ParentGroupId.ToString() == parentNode.Id).OrderBy(i => i.Name).ToList();

            if (childNodes.Count() > 0) {
                List<TreeItem> treeItems = new List<TreeItem>();
                foreach (var childNode in childNodes) {
                    var treeItem = new TreeItem {
                        Id = childNode.Id.ToString(),
                        Name = childNode.Name,
                        Validate = (bool) childNode.Validate,
                        ParentMenuId = parentNode.Id
                    };
                    treeItem.Items = RecursiveItem(treeItem, items);
                    treeItems.Add(treeItem);
                }
                return treeItems;
            }

            return null;
        }
    
        public ERP.Models.Views.GroupPermissions GetByGroup(int id) {
            return new GroupPermissions() {
                GroupId = id,
                Permission = GroupPermission.Get().Where(i => i.GroupId == id).Select(i => i.FunctionId)
            };
        }
        public ERP.Models.Views.GroupPermissions SavePermission(GroupPermissions item) {
            var groupId = item.GroupId;
            var permission = item.Permission;
            try {
                UnitOfWork.BeginTransaction();                           
                GroupPermission.RemoveRange(i => i.GroupId == groupId);
                GroupPermission.CreateRange(item.Permission.Select(i => new ERP.Models.Views.GroupPermission {
                    GroupId = groupId,
                    FunctionId = i,
                    LastUpdateTime = DateTime.Now,
                    ModifyUserName = "Admin",
                }));
                UnitOfWork.Commit();
                return GetByGroup(groupId);
            } catch (Exception e) {
                UnitOfWork.Rollback();
                throw e;
            }
        }
    }
}