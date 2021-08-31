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
    public class PortfolioService
    {
        private readonly Guid _userId;
        public PortfolioService(Guid userId)
        {
            _userId = userId;
        }
        public bool AddPortfolio(CreatePortfolio model)
        {
            var p = new Portfolio()
            {
                UserId = _userId,
                Cash = model.Cash,
                Value = model.Cash,
                Orders = new List<Order>(),
            };

            using (var ctx = new ApplicationDbContext())
            {
                ctx.Portfolios.Add(p);
                return ctx.SaveChanges() == 1;
            }
        }
        public IEnumerable<PortfolioListItem> GetPortfolios()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query = ctx.Portfolios.Where(q => q.UserId == _userId).Select(q => new PortfolioListItem()
                {
                    Id = q.Id,
                    Cash = q.Cash,

                }) ;
                var items = query.ToArray();
                foreach(var p in items)
                {
                    p.Value = GetValue(ctx.Portfolios.Find(p.Id));
                }

                return items;
                
            }

        }
        public PortfolioDetail GetPortfolio(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var orderService = new OrderService(_userId);
                var entity = ctx.Portfolios.Single(e => e.UserId == _userId && e.Id == id);
                return new PortfolioDetail()
                {
                    Id = entity.Id,
                    Cash = entity.Cash,
                    Value = GetValue(entity),
                    Orders = ctx.Orders.Where(o => o.UserId == _userId).Select(o => new OrderDetail()
                    {
                        OrderId = o.OrderId,
                        PortfolioId = o.PortfolioId,
                        OrderType = o.OrderType,
                        NumberOfUnits = o.NumberOfUnits,
                        TickerSymbol = o.TickerSymbol,
                        Price = o.Stock.Price,
                        IsFilled = o.IsFilled,
                        OrderValue = o.NumberOfUnits * o.Stock.Price

                    }).ToList()
                    

                };
            }
        }
        public bool UpdatePortfolio(PortfolioEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Portfolios.Single(e => e.Id == model.Id && e.UserId == _userId);
                entity.Cash = model.Cash;
                return ctx.SaveChanges() == 1;
            }

        }
        public bool DeletePortfolio(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var p = ctx.Portfolios.Find(id);
                if (p.Cash > 0)
                {
                    throw new InvalidOperationException("Please liquidate all assets and move money out of the account before closing it");
                   
                }
                else
                {
                    ctx.Portfolios.Remove(p);
                    return ctx.SaveChanges() == 1;

                }
            }
        }

        public double GetValue(Portfolio p)
        {
            
            using (var ctx = new ApplicationDbContext()) {
                double val = 0;
                foreach (var item in p.Orders.Where(o => o.IsFilled))
                {
                    
                    var stock = ctx.Stocks.Find(item.TickerSymbol);
                    if (item.OrderType == OrderType.Buy)
                    {
                        val += stock.Price * item.NumberOfUnits;
                    }
                    if (item.OrderType == OrderType.Sell)
                    {
                        val -= stock.Price * item.NumberOfUnits;
                    }

                }
                return p.Cash + val;
            }
        }

    }
}
