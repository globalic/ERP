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
    public class UpdateWareCommandHandler : IRequestHandler<UpdateWareCommand, Ware>
    {
        //TODO: Check grammar
        protected const string UpdateWareCommandHandlerEntityNotFoundException = "Unable to update Ware with id={0}. Ware not found!";

        public UpdateWareCommandHandler(DatabaseContext context, IMediator mediator)
        {
            this.DatabaseContext = context;
            this.Mediator = mediator;
        }

        protected DatabaseContext DatabaseContext { get; }
        protected IMediator Mediator { get; }

        public async Task<Ware> Handle(UpdateWareCommand request, CancellationToken cancellationToken)
        {
            if (!this.DatabaseContext.Wares.Any(x => x.Id == request.Model.Id))
            {
                throw new EntityNotFoundException(string.Format(UpdateWareCommandHandlerEntityNotFoundException, request.Model.Id));
            }

            Ware ware = this.DatabaseContext.Wares.Find(request.Model.Id);
            ware.ProductName = request.Model.ProductName;
            ware.Width = request.Model.Width;
            ware.Height = request.Model.Height;
            ware.Depth = request.Model.Depth;
            ware.Weight = request.Model.Weight;

            await this.DatabaseContext.SaveChangesAsync(cancellationToken);

            await this.Mediator.Publish(new WareUpdatedDomainEvent(ware));

            return ware;
        }
    }
}
