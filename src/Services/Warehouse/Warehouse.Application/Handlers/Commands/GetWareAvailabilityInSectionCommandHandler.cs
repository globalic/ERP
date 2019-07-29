﻿using MediatR;
using Restmium.ERP.Services.Warehouse.Application.Commands;
using Restmium.ERP.Services.Warehouse.Application.Models;
using Restmium.ERP.Services.Warehouse.Domain.Entities;
using Restmium.ERP.Services.Warehouse.Domain.Entities.Extensions;
using Restmium.ERP.Services.Warehouse.Domain.Exceptions;
using Restmium.ERP.Services.Warehouse.Infrastructure.Database;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Restmium.ERP.Services.Warehouse.Application.Handlers.Commands
{
    public class GetWareAvailabilityInSectionCommandHandler : IRequestHandler<GetWareAvailabilityInSectionCommand, WareAvailabilityInSection>
    {
        public GetWareAvailabilityInSectionCommandHandler(DatabaseContext databaseContext)
        {
            this.DatabaseContext = databaseContext;
        }

        protected DatabaseContext DatabaseContext { get; }

        public async Task<WareAvailabilityInSection> Handle(GetWareAvailabilityInSectionCommand request, CancellationToken cancellationToken)
        {
            Ware ware = await this.DatabaseContext.Wares.FindAsync(new object[] { request.WareId }, cancellationToken);
            if (ware == null)
            {
                throw new EntityNotFoundException(string.Format(Resources.Exceptions.Values["Ware_EntityNotFoundException"], request.WareId));
            }

            Section section = await this.DatabaseContext.Sections.FindAsync(new object[] { request.SectionId }, cancellationToken);
            if (section == null)
            {
                throw new EntityNotFoundException(string.Format(Resources.Exceptions.Values["Section_EntityNotFoundException"], request.SectionId));
            }

            IQueryable<Position> positions = this.DatabaseContext.Positions
                .Where(x =>
                    x.GetWare().Id == request.WareId &&
                    x.SectionId == request.SectionId);

            WareAvailabilityInSection wareAvailability = new WareAvailabilityInSection()
            {
                Section = section,
                Ware = ware,
                UnitsAvailable = positions.Aggregate(0, (total, next) => total + next.CountWare())
            };

            return wareAvailability;
        }
    }
}