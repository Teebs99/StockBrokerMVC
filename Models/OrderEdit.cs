using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class OrderEdit
    {
        public int OrderId { get; set; }
        public string TickerSymbol { get; set; }
        public int NumberOfUnits { get; set; }

    }
}
