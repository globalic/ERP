﻿using MediatR;
using Restmium.ERP.Services.Warehouse.Application.Commands;
using Restmium.ERP.Services.Warehouse.Application.Models;
using Restmium.ERP.Services.Warehouse.Domain.Entities;
using Restmium.ERP.Services.Warehouse.Infrastructure.Database;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Restmium.ERP.Services.Warehouse.Application.Handlers.Commands
{
    public class FindReceiptsOnPageCommandHandler : IRequestHandler<FindReceiptsOnPageCommand, PageDTO<Receipt>>
    {
        protected DatabaseContext DatabaseContext { get; set; }

        public FindReceiptsOnPageCommandHandler(DatabaseContext context)
        {
            this.DatabaseContext = context;
        }

        public async Task<PageDTO<Receipt>> Handle(FindReceiptsOnPageCommand request, CancellationToken cancellationToken)
        {
            return new PageDTO<Receipt>(
                request.Page,
                request.ItemsPerPage,
                this.DatabaseContext.Receipts.Skip(request.ItemsPerPage * --request.Page).Take(request.ItemsPerPage).AsEnumerable());
        }
    }
}
