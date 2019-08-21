﻿using MediatR;
using Restmium.ERP.Services.Warehouse.Application.Commands;
using Restmium.ERP.Services.Warehouse.Domain.Exceptions;
using Restmium.ERP.Services.Warehouse.Infrastructure.Database;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Restmium.ERP.Services.Warehouse.Application.Handlers.Commands
{
    public class RestoreWarehouseFromBinCommandHandler : IRequestHandler<RestoreWarehouseFromBinCommand, Warehouse.Domain.Entities.Warehouse>
    {
        public RestoreWarehouseFromBinCommandHandler(DatabaseContext databaseContext)
        {
            this.DatabaseContext = databaseContext;
        }

        protected DatabaseContext DatabaseContext { get; }

        public async Task<Warehouse.Domain.Entities.Warehouse> Handle(RestoreWarehouseFromBinCommand request, CancellationToken cancellationToken)
        {
            Warehouse.Domain.Entities.Warehouse warehouse = this.DatabaseContext.Warehouses.FirstOrDefault(x => x.Id == request.WarehouseId);

            if (warehouse == null)
            {
                throw new EntityNotFoundException(string.Format(Resources.Exceptions.Values["Warehouse_EntityNotFoundException"], request.WarehouseId));
            }

            warehouse.UtcMovedToBin = null;

            await this.DatabaseContext.SaveChangesAsync(cancellationToken);

            return warehouse;
        }
    }
}
