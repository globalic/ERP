﻿using MediatR;
using Restmium.ERP.Services.Warehouse.Application.Commands;
using Restmium.ERP.Services.Warehouse.Domain.Entities;
using Restmium.ERP.Services.Warehouse.Domain.Events;
using Restmium.ERP.Services.Warehouse.Domain.Exceptions;
using Restmium.ERP.Services.Warehouse.Infrastructure.Database;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Restmium.ERP.Services.Warehouse.Application.Handlers.Commands
{
    public class UpdateStockTakingItemCommandHandler : IRequestHandler<UpdateStockTakingItemCommand, StockTaking.Item>
    {
        public UpdateStockTakingItemCommandHandler(DatabaseContext context, IMediator mediator)
        {
            this.DatabaseContext = context;
            this.Mediator = mediator;
        }

        protected DatabaseContext DatabaseContext { get; }
        protected IMediator Mediator { get; }

        public async Task<StockTaking.Item> Handle(UpdateStockTakingItemCommand request, CancellationToken cancellationToken)
        {
            // TODO: Split command between two

            StockTaking.Item item = this.DatabaseContext.StockTakingItems.FirstOrDefault(x =>
                x.StockTakingId == request.StockTakingId &&
                x.PositionId == request.PositionId);
            if (item == null)
            {
                throw new EntityNotFoundException(string.Format(Resources.Exceptions.Values["StockTakingItem_EntityNotFoundException"], request.StockTakingId, request.PositionId));
            }

            item.WareId = request.WareId;
            item.CurrentStock = request.CurrentStock;
            item.CountedStock = request.CountedStock;
            item.UtcCounted = request.UtcCounted;

            await this.DatabaseContext.SaveChangesAsync(cancellationToken);

            // Publish Domain Event that the StockTaking.Item has been updated
            await this.Mediator.Publish(new StockTakingItemUpdatedDomainEvent(item), cancellationToken);

            return item;
        }
    }
}
