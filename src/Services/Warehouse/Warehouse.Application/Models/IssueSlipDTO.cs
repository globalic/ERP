﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Restmium.ERP.Services.Warehouse.Application.Models
{
    /// <summary>
    /// DTO for <see cref="Domain.Entities.IssueSlip"/>
    /// </summary>
    public partial class IssueSlipDTO
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public long OrderId { get; set; }

        public DateTime UtcDispatchDate { get; set; }

        [Required]
        public DateTime UtcDeliveryDate { get; set; }

        //TODO: Add Collection of Items
    }
}
