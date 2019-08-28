﻿using MediatR;
using Restmium.ERP.Services.Warehouse.Domain.Entities;

namespace Restmium.ERP.Services.Warehouse.Application.Commands
{
    public class MoveIssueSlipToBinCommand : IRequest<IssueSlip>
    {
        public MoveIssueSlipToBinCommand(long issueSlipId, bool movedToBinInCascade)
        {
            this.IssueSlipId = issueSlipId;
            this.MovedToBinInCascade = movedToBinInCascade;
        }

        public long IssueSlipId { get; }
        public bool MovedToBinInCascade { get; }
    }
}
