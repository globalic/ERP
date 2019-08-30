﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restmium.ERP.Services.Warehouse.API.Models.Application;
using Restmium.ERP.Services.Warehouse.API.Models.Domain.Entities;
using Restmium.ERP.Services.Warehouse.Application.Commands;
using Restmium.ERP.Services.Warehouse.Application.Models;
using Restmium.ERP.Services.Warehouse.Domain.Entities;
using Restmium.ERP.Services.Warehouse.Domain.Exceptions;
using System;
using System.Threading.Tasks;

namespace Warehouse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptsController : ControllerBase
    {
        protected IMapper Mapper { get; }
        protected IMediator Mediator { get; }

        public ReceiptsController(IMapper mapper, IMediator mediator)
        {
            this.Mapper = mapper;
            this.Mediator = mediator;
        }

        // GET: api/Receipts/5
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ReceiptDTO>> GetReceipt(long id)
        {
            try
            {
                Receipt receipt = await this.Mediator.Send(new FindReceiptByIdCommand(id));
                return this.Ok(this.Mapper.Map<ReceiptDTO>(receipt));
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

        // GET: api/Receipts/1/20
        [HttpGet("All/{page}/{itemsPerPage}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<PageDTO<ReceiptDTO>>> GetAll(int page, int itemsPerPage)
        {
            try
            {
                Page<Receipt> entity = await this.Mediator.Send(new FindReceiptsOnPageCommand(page, itemsPerPage));
                return this.Ok(this.Mapper.Map<PageDTO<ReceiptDTO>>(entity));
            }
            catch (Exception)
            {
                throw;
            }
        }

        // PUT: api/Receipts/5
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PutReceipt(long id, ReceiptDTO receipt)
        {
            if (id != receipt.Id)
            {
                return this.BadRequest();
            }

            try
            {
                await this.Mediator.Send(new UpdateReceiptCommand(receipt.Id, receipt.UtcExpected, receipt.UtcReceived));
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

        // GET: api/Receipts/MoveToBin/1
        [HttpGet("MoveToBin/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ReceiptDTO>> GetMoveToBin(long id)
        {
            try
            {
                Receipt entity = await this.Mediator.Send(new MoveReceiptToBinCommand(id, false));
                return this.Ok(this.Mapper.Map<ReceiptDTO>(entity));
            }
            catch (EntityNotFoundException ex)
            {
                return this.NotFound(ex.Message);
            }
            catch (EntityMoveToBinException ex)
            {
                return this.Conflict(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: api/Receipts/Restore/1
        [HttpGet("Restore/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ReceiptDTO>> GetRestore(long id)
        {
            try
            {
                Receipt entity = await this.Mediator.Send(new RestoreReceiptFromBinCommand(id));
                return this.Ok(this.Mapper.Map<ReceiptDTO>(entity));
            }
            catch (EntityNotFoundException ex)
            {
                return this.NotFound(ex.Message);
            }
            catch (EntityRestoreFromBinException ex)
            {
                return this.Conflict(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: api/Receipts/Deleted/1/20
        [HttpGet("Deleted/{page}/{itemsPerPage}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<PageDTO<ReceiptDTO>>> GetDeleted(int page, int itemsPerPage)
        {
            try
            {
                Page<Receipt> entity = await this.Mediator.Send(new FindDeletedReceiptsOnPageCommand(page, itemsPerPage));
                return this.Ok(this.Mapper.Map<PageDTO<ReceiptDTO>>(entity));
            }
            catch (Exception)
            {
                throw;
            }
        }

        // DELETE: api/Receipts/5
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ReceiptDTO>> DeleteReceipt(long id)
        {
            try
            {
                Receipt receipt = await this.Mediator.Send(new DeleteReceiptCommand(id));
                return this.Ok(this.Mapper.Map<ReceiptDTO>(receipt));
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
