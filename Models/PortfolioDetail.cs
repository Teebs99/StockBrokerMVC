using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class PortfolioDetail
    {
        public int Id { get; set; }
        public double Cash { get; set; }
        public double Value { get; set; }
        public List<OrderDetail> Orders { get; set; }

        


    }
}
