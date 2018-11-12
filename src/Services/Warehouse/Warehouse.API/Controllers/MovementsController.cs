﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Warehouse.API.Models;
using Warehouse.API.Models.Extensions;

namespace Warehouse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovementsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public MovementsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Movements
        [HttpGet]
        public IEnumerable<Movement> GetMovements()
        {
            return _context.Movements;
        }

        // GET: api/Movements/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovement([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var movement = await _context.Movements.FindAsync(id);

            if (movement == null)
            {
                return NotFound();
            }

            return Ok(movement);
        }

        [HttpGet("TransferPosition")]
        public async Task<IActionResult> TransferPosition([FromQuery] long fromId, [FromQuery] long toId)
        {
            // If ModelState is not valid or positions don't exist
            if (!ModelState.IsValid || !_context.Positions.Any(x => x.Id == fromId) || !_context.Positions.Any(x => x.Id == toId))
            {
                return BadRequest(ModelState);
            }

            Position positionFrom = await this._context.Positions.FindAsync(fromId);
            Position positionTo = await this._context.Positions.FindAsync(toId);

            if (positionFrom.Count() == 0 || (positionTo.WareId.HasValue && positionFrom.WareId.Value != positionTo.WareId.Value))
            {
                return BadRequest(ModelState);
            }

            ////TODO: Fix? What the hell is this? Useless! Movement represents action in past.
            //if (positionFrom.Movements.Any(x => x.DateMoved == null && x.EntryContent != EntryContent.PositionTransfer))
            //{
            //    return BadRequest(ModelState);
            //}

            int countChange = positionFrom.Count();

            Movement movementFrom = new Movement()
            {
                Id = 0,
                WareId = positionFrom.WareId.Value,
                PositionId = fromId,
                MovementDirection = Movement.Direction.Out,
                Content = Movement.EntryContent.PositionTransfer,
                CountChange = -countChange,
                CountTotal = 0,
                DateMoved = DateTime.UtcNow
            };
            Movement movementTo = new Movement()
            {
                Id = 0,
                WareId = positionFrom.WareId.Value,
                PositionId = toId,
                MovementDirection = Movement.Direction.In,
                Content = Movement.EntryContent.PositionTransfer,
                CountChange = countChange,
                CountTotal = positionTo.Count() + countChange,
                DateMoved = DateTime.UtcNow
            };
            positionTo.WareId = positionFrom.WareId;
            positionFrom.WareId = null;

            _context.Entry(positionFrom).State = EntityState.Modified;
            _context.Entry(positionTo).State = EntityState.Modified;

            _context.Movements.Add(movementFrom);
            _context.Movements.Add(movementTo);

            await _context.SaveChangesAsync();

            return Ok();
        }

        // PUT: api/Movements/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovement([FromRoute] long id, [FromBody] Movement movement)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != movement.Id)
            {
                return BadRequest();
            }

            _context.Entry(movement).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovementExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Movements
        [HttpPost]
        public async Task<IActionResult> PostMovement([FromBody] Movement movement)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Movements.Add(movement);
            _context.Positions.Where(x => x.Id == movement.PositionId).FirstOrDefault().WareId = movement.WareId;
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovement", new { id = movement.Id }, movement);
        }

        // DELETE: api/Movements/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovement([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var movement = await _context.Movements.FindAsync(id);
            if (movement == null)
            {
                return NotFound();
            }

            _context.Movements.Remove(movement);
            await _context.SaveChangesAsync();

            return Ok(movement);
        }

        private bool MovementExists(long id)
        {
            return _context.Movements.Any(e => e.Id == id);
        }
    }
}