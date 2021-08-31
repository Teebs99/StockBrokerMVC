using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Portfolio
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public double Cash { get; set; }
        public double Value { get; set; }

        //public virtual List<Position> Positions { get; set; } = new List<Position>();
        public virtual List<Order> Orders { get; set; } = new List<Order>();
        
    }
}
