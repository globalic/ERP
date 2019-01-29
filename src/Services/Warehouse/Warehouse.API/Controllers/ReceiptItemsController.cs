﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restmium.ERP.Services.Warehouse.Application.Commands;
using Restmium.ERP.Services.Warehouse.Domain.Entities;
using Restmium.ERP.Services.Warehouse.Domain.Exceptions;

namespace Warehouse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptItemsController : ControllerBase
    {
        protected IMediator Mediator { get; }

        public ReceiptItemsController(IMediator mediator)
        {
            this.Mediator = mediator;
        }

        // GET: api/ReceiptItems/5/1
        [HttpGet("{receiptId}/{wareId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Receipt.Item>> GetReceiptItem(long receiptId, int wareId)
        {
            try
            {
                return this.Ok(await this.Mediator.Send(new FindReceiptItemByReceiptIdAndWareIdCommand(receiptId, wareId)));
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

        // PUT: api/ReceiptItems/5
        [HttpPut("{receiptId}/{wareId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Receipt.Item>> PutReceiptItem(long receiptId, int wareId, Receipt.Item item)
        {
            if (receiptId != item.ReceiptId || wareId != item.WareId)
            {
                return this.BadRequest();
            }

            try
            {
                item = await this.Mediator.Send(new UpdateReceiptItemCommand(item.WareId, item.PositionId, item.ReceiptId, item.CountOrdered, item.CountReceived, item.UtcProcessed, item.EmployeeId));
                return this.NoContent();
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