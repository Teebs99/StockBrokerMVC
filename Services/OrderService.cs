using Data;
using Models;
using StockBroker.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class OrderService
    {
        private readonly Guid _userId;

        public OrderService(Guid UserId)
        {
            _userId = UserId;
        }

        public bool AddOrder(CreateOrder model)
        {
            var service = new StockService();
            var pService = new PortfolioService(_userId);
            var stock = service.GetStock(model.TickerSymbol);
            var entity = new Order() { UserId = _userId, PortfolioId = model.PortfolioId, OrderType = model.OrderType, TickerSymbol = stock.TickerSymbol, NumberOfUnits = model.NumberOfUnits };
            using (var ctx = new ApplicationDbContext())
            {
                var portfolio = ctx.Portfolios.Find(model.PortfolioId);
                var value = stock.Price * entity.NumberOfUnits;
                if (portfolio.Cash >= value && model.OrderType == OrderType.Buy)
                {
                    portfolio.Orders.Add(entity);
                    ctx.Orders.Add(entity);
                }
                else if (model.OrderType == OrderType.Sell)
                {
                    portfolio.Orders.Add(entity);
                    ctx.Orders.Add(entity);
                }
                return ctx.SaveChanges() >= 1;
            }
        }

        public IEnumerable<OrderListItem> GetOrders()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query = ctx.Orders.Where(q => q.UserId == _userId).Select(q => new OrderListItem() { OrderId = q.OrderId, PortfolioId = q.PortfolioId, OrderType = q.OrderType, NumberOfUnits = q.NumberOfUnits, TickerSymbol = q.TickerSymbol });
                return query.ToArray();
            }
        }

        public OrderDetail GetOrder(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Orders.Single(e => e.UserId == _userId && e.OrderId == id);
                return new OrderDetail()
                {
                    OrderId = entity.OrderId,
                    PortfolioId = entity.PortfolioId,
                    OrderType = entity.OrderType,
                    TickerSymbol = entity.TickerSymbol,
                    NumberOfUnits = entity.NumberOfUnits,
                    IsFilled = entity.IsFilled,
                };
            }
        }

        public bool UpdateOrder(OrderEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Orders.Single(e => e.UserId == _userId && e.OrderId == model.OrderId);
                if (!entity.IsFilled)
                {
                    entity.TickerSymbol = model.TickerSymbol;
                    entity.NumberOfUnits = model.NumberOfUnits;
                    return ctx.SaveChanges() == 1;
                }
                return false;
            }
        }

        public bool DeleteOrder(int orderId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Orders.Single(e => e.UserId == _userId && e.OrderId == orderId);
                var portfolio = ctx.Portfolios.Find(entity.PortfolioId);
                portfolio.Orders.Remove(entity);
                ctx.Orders.Remove(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public bool FillOrder(int orderId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var order = ctx.Orders.Find(orderId);
                var portfolio = ctx.Portfolios.Find(order.PortfolioId);
                var stock = ctx.Stocks.Find(order.TickerSymbol);
                var value = stock.Price * order.NumberOfUnits;

                if (order.OrderType == OrderType.Buy && portfolio.Cash >= value)
                    portfolio.Cash -= value;
                else if (order.OrderType == OrderType.Sell)
                    portfolio.Cash += value;

                order.IsFilled = true;

                return ctx.SaveChanges() >= 1;
            }
        }
    }
}