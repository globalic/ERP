﻿using MediatR;
using Restmium.ERP.Services.Warehouse.Application.Commands;
using Restmium.ERP.Services.Warehouse.Domain.Entities;
using Restmium.ERP.Services.Warehouse.Domain.Exceptions;
using Restmium.ERP.Services.Warehouse.Infrastructure.Database;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Restmium.ERP.Services.Warehouse.Application.Handlers.Commands
{
    public class MoveStockTakingItemToBinCommandHandler : IRequestHandler<MoveStockTakingItemToBinCommand, StockTaking.Item>
    {
        public MoveStockTakingItemToBinCommandHandler(DatabaseContext databaseContext)
        {
            this.DatabaseContext = databaseContext;
        }

        protected DatabaseContext DatabaseContext { get; }

        public async Task<StockTaking.Item> Handle(MoveStockTakingItemToBinCommand request, CancellationToken cancellationToken)
        {
            StockTaking.Item item = this.DatabaseContext.StockTakingItems.FirstOrDefault(x =>
                x.StockTakingId == request.StockTakingId &&
                x.PositionId == request.PositionId);

            if (item == null)
            {
                throw new EntityNotFoundException(string.Format(Resources.Exceptions.Values["StockTakingItem_EntityNotFoundException"], request.StockTakingId, request.PositionId));
            }

            item.UtcMovedToBin = DateTime.UtcNow;

            await this.DatabaseContext.SaveChangesAsync(cancellationToken);

            return item;
        }
    }
}
