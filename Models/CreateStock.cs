using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CreateStock
    {
        public string TickerSymbol { get; set; }
        public string CompanyName { get; set; }
        public double Price { get; set; }
    }
}
