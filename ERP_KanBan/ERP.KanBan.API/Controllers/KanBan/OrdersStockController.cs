using System;
using Diamond.DataSource.Extensions;
using Diamond.DataSource.Models;
using Diamond.DataSource.Webs;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Linq.Dynamic.Core;
using ERP.KanBan.API.Controllers.Bases;

namespace ERP.KanBan.API.Controllers.Search
{
    public class OrdersStockController : KanBanController
    {
        private ERP.Services.KanBan.Search.OrdersStockService Service { get; }

        public OrdersStockController(
            ERP.Services.KanBan.Search.OrdersStockService service
        )
        {
            Service = service;
        }

        // [HttpGet("OrdersStock")]
        // public IActionResult GetOrdersStock([QueryRequest] QueryRequest queryRequest)
        // {
        //     if (queryRequest == null)
        //         return BadRequest("");

        //     return Ok(Service.GetOrderSotck(queryRequest.ToWhere()).ToQueryResult(queryRequest, false));
        // }
        [HttpGet("OrdersStockStyle")]
        public IActionResult GetOrdersStockStyle([QueryRequest] QueryRequest queryRequest)
        {
            try
            {
                return Ok(Service.GetOrderSotckStyle(queryRequest.ToWhere()));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}