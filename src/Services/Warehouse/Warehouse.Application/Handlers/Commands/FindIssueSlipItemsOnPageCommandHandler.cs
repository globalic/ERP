﻿using MediatR;
using Restmium.ERP.Services.Warehouse.Application.Commands;
using Restmium.ERP.Services.Warehouse.Application.Models;
using Restmium.ERP.Services.Warehouse.Domain.Entities;
using Restmium.ERP.Services.Warehouse.Infrastructure.Database;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Entities = Restmium.ERP.Services.Warehouse.Domain.Entities;

namespace Restmium.ERP.Services.Warehouse.Application.Handlers.Commands
{
    public class FindIssueSlipItemsOnPageCommandHandler : IRequestHandler<FindIssueSlipItemsOnPageCommand, PageDto<Entities.IssueSlip.Item>>
    {
        protected DatabaseContext DatabaseContext { get; set; }

        public FindIssueSlipItemsOnPageCommandHandler(DatabaseContext context)
        {
            this.DatabaseContext = context;
        }

        public async Task<PageDto<IssueSlip.Item>> Handle(FindIssueSlipItemsOnPageCommand request, CancellationToken cancellationToken)
        {
            IQueryable<IssueSlip.Item> items = this.DatabaseContext.IssueSlipItems.Where(x => x.UtcMovedToBin == null);

            return new PageDto<IssueSlip.Item>(
                request.Page,
                request.ItemsPerPage,
                items.Count(),
                items.Skip(request.ItemsPerPage * --request.Page).Take(request.ItemsPerPage).AsEnumerable());
        }
    }
}
