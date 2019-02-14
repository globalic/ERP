﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restmium.ERP.Services.Warehouse.Application.Commands;
using Restmium.ERP.Services.Warehouse.Application.Models;
using Restmium.ERP.Services.Warehouse.Domain.Entities;
using Restmium.ERP.Services.Warehouse.Domain.Exceptions;

namespace Warehouse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockTakingItemsController : ControllerBase
    {
        protected IMediator Mediator { get; }

        public StockTakingItemsController(IMediator mediator)
        {
            this.Mediator = mediator;
        }

        // GET: api/StockTakingItems/1/20
        [HttpGet("All/{page}/{itemsPerPage}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<PageDTO<StockTaking.Item>>> GetAll(int page, int itemsPerPage)
        {
            try
            {
                return this.Ok(await this.Mediator.Send(new FindStockTakingItemsOnPageCommand(page, itemsPerPage)));
            }
            catch (Exception)
            {
                throw;
            }
        }

        // PUT: api/StockTakingItems/5
        [HttpPut("{stockTakingId}/{positionId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<StockTaking.Item>> PutStockTakingItem(int stockTakingId, long positionId, StockTaking.Item item)
        {
            if (stockTakingId != item.StockTakingId || positionId != item.PositionId)
            {
                return this.BadRequest();
            }

            try
            {
                return this.Ok(await this.Mediator.Send(new UpdateStockTakingItemCommand(item.StockTakingId, item.WareId, item.PositionId, item.CurrentStock, item.CountedStock, item.EmployeeId, item.UtcCounted)));
            }
            catch (EntityNotFoundException ex)
            {
                return this.NotFound(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
