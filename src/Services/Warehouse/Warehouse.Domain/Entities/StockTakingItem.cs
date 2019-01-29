﻿using Restmium.ERP.BuildingBlocks.Common.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restmium.ERP.Services.Warehouse.Domain.Entities
{
    public partial class StockTaking
    {
        public class Item : DatabaseEntity
        {
            public Item()
            {

            }
            public Item(int stockTakingId, int wareId, long positionId, int currentStock, int countedStock, int employeeId) : this()
            {
                this.StockTakingId = stockTakingId;
                this.WareId = wareId;
                this.PositionId = positionId;
                this.CurrentStock = currentStock;
                this.CountedStock = countedStock;
                this.EmployeeId = employeeId;
            }
            public Item(int stockTakingId, int wareId, long positionId, int currentStock, int countedStock, int employeeId, DateTime? utcCounted) : this(stockTakingId, wareId, positionId, currentStock, countedStock, employeeId)
            {
                this.UtcCounted = utcCounted;
            }

            [Required]
            public int WareId { get; set; }
            public virtual Ware Ware { get; set; }

            [Required]
            public long PositionId { get; set; }
            public virtual Position Position { get; set; }

            /// <summary>
            /// FK to Employees.API
            /// </summary>
            [Required]
            public int EmployeeId { get; set; }

            [Required]
            public int StockTakingId { get; set; }
            public virtual StockTaking StockTaking { get; set; }

            [Required]
            public int CurrentStock { get; set; }

            [Required]
            public int CountedStock { get; set; }

            public DateTime? UtcCounted { get; set; }

            #region NotMapped properties
            [NotMapped]
            public int Variance => this.CountedStock - this.CurrentStock;
            #endregion
        }
    }        
}
