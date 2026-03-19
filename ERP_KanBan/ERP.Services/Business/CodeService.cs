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
    public class CodeService : BusinessService
    {
        private ERP.Services.Business.Entities.CodeService Code { get; set; }
        private ERP.Services.Business.Entities.CodeItemService CodeItem { get; set; }
        public CodeService(
            ERP.Services.Business.Entities.CodeService codeService,
            ERP.Services.Business.Entities.CodeItemService codeItemService,
            UnitOfWork unitOfWork
        ) : base(unitOfWork)
        {
            Code = codeService;
            CodeItem = codeItemService;
        }

        public IQueryable<ERP.Models.Views.Code> GetCode()
        {   
            return Code.Get();
        }
        public IQueryable<ERP.Models.Views.CodeItem> GetCodeItem()
        {   
            return CodeItem.Get();
        }
        public ERP.Models.Views.CodeItem CreateCodeItem(CodeItem item)
        {   
            UnitOfWork.BeginTransaction();
            try
            {
                item = CodeItem.Create(item);
                UnitOfWork.Commit();
                return item;
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        public ERP.Models.Views.CodeItem UpdateCodeItem(CodeItem item)
        {   
            try
            {
                UnitOfWork.BeginTransaction();
                item = CodeItem.Update(item);
                UnitOfWork.Commit();
                return item;
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
        public void RemoveCodeItem(CodeItem item)
        {   
            try
            {
                UnitOfWork.BeginTransaction();
                CodeItem.Remove(item);
                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                UnitOfWork.Rollback();
                throw e;
            }
        }
    }
}
