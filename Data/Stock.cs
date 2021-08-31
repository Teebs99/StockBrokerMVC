using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Stock
    {
        [Key]
        public string TickerSymbol { get; set; }
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public double Price { get; set; }
        
    }
}
