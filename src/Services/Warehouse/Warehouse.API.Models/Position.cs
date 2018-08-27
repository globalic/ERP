﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Warehouse.API.Models.StockTaking;

namespace Warehouse.API.Models
{
    public class Position
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int SectionId { get; set; }
        public virtual Section Section { get; set; }

        public int? WareId { get; set; }
        public virtual Ware Ware { get; set; }

        public virtual ICollection<Movement> Movements { get; set; }
        public virtual ICollection<StockTakingItem> StockTakingItems { get; set; }
    }
}
