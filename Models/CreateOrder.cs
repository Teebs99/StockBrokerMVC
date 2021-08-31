using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CreateOrder
    {
        public int PortfolioId { get; set; }
        public string TickerSymbol { get; set; }
        public OrderType OrderType { get; set; }
        public int NumberOfUnits { get; set; }
        
    }
}
