using Diamond.DataSource.Extensions;
using Diamond.DataSource.Models;
using Diamond.DataSource.Webs;
using ERP.KanBan.API.Controllers.Bases;
using ERP.Models.Entities;
using ERP.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.KanBan.API.Controllers
{
    public class HomeController : BaseController
    {
        [HttpGet("")]
        public virtual IActionResult Get([QueryRequest] QueryRequest queryRequest)
        {
            return Json("ERP KanBan Webservice!!");
        }
    }
}