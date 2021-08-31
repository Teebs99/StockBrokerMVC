using Data;
using Models;
using StockBroker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class StockService
    {
        public bool AddStock(CreateStock model)
        {
            var entity = new Stock() { TickerSymbol = model.TickerSymbol, CompanyName = model.CompanyName, Price = model.Price };
            using (var ctx = new ApplicationDbContext())
            {
                ctx.Stocks.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }
        public IEnumerable<StockListItem> GetStocks()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query = ctx.Stocks.Select(q => new StockListItem() { TickerSymbol = q.TickerSymbol, Price = q.Price });
                return query.ToArray();
            }
        }
        public StockDetail GetStock(string symbol)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Stocks.Find(symbol);
                return new StockDetail()
                {
                    TickerSymbol = entity.TickerSymbol,
                    CompanyName = entity.CompanyName,
                    Price = entity.Price
                };

            }
        }
        public bool UpdateStock(StockEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Stocks.Find(model.TickerSymbol);
                entity.Price = model.Price;
                return ctx.SaveChanges() == 1;
            }
        }
        public bool DeleteStock(string id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Stocks.Find(id);
                ctx.Stocks.Remove(entity);
                return ctx.SaveChanges() == 1;
            }
        }
    }
}
