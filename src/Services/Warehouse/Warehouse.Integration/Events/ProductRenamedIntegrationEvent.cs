﻿using Restmium.ERP.BuildingBlocks.EventBus.Events;

namespace Restmium.ERP.Services.Warehouse.Integration.Events
{
    /// <summary>
    /// Event produced by Catalog.API
    /// </summary>
    public class ProductRenamedIntegrationEvent : IntegrationEvent
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
    }
}