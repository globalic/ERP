﻿using Restmium.Models.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Warehouse.API.Models
{
    public class Ware : DatabaseEntity
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public double Width { get; set; }
        [Required]
        public double Height { get; set; }
        [Required]
        public double Depth { get; set; }
        [Required]
        public double Weight { get; set; }

        public virtual ICollection<Movement> Movements { get; set; }
        public virtual ICollection<IssueSlip.Item> IssueSlipItems { get; set; }
        public virtual ICollection<StockTaking.Item> StockTakingItems { get; set; }
    }
}