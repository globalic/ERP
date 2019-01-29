﻿using MediatR;
using Restmium.ERP.Services.Warehouse.Application.Commands;
using Restmium.ERP.Services.Warehouse.Domain.Entities;
using Restmium.ERP.Services.Warehouse.Domain.Events;
using Restmium.ERP.Services.Warehouse.Infrastructure.Database;
using System.Threading;
using System.Threading.Tasks;

namespace Restmium.ERP.Services.Warehouse.Application.Handlers.Commands
{
    public class RemoveIssueSlipReservationCommandHandler : IRequestHandler<RemoveIssueSlipReservationCommand, Position>
    {
        public RemoveIssueSlipReservationCommandHandler(DatabaseContext context, IMediator mediator)
        {
            this.DatabaseContext = context;
            this.Mediator = mediator;
        }

        protected DatabaseContext DatabaseContext { get; }
        protected IMediator Mediator { get; }

        public async Task<Position> Handle(RemoveIssueSlipReservationCommand request, CancellationToken cancellationToken)
        {
            Position position = request.Model.Position;

            // Remove reservation
            position.ReservedUnits -= request.Model.ReservedUnitsToRemove;

            // Update in Database
            position = this.DatabaseContext.Positions.Update(request.Model.Position).Entity;
            await this.DatabaseContext.SaveChangesAsync(cancellationToken);

            // Publish DomainEvent that the reservation has been removed
            await this.Mediator.Publish(new IssueSlipReservationRemovedDomainEvent(position), cancellationToken);

            return position;
        }
    }
}