using Data;
using Microsoft.AspNet.Identity;
using Models;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockBroker.Controllers
{
    public class OrderController : Controller
    {
        public OrderService CreateService()
        {
            var id = Guid.Parse(User.Identity.GetUserId());
            return new OrderService(id);
        }
        // GET: Order
        public ActionResult Index()
        {
            var service = CreateService();
            var model = service.GetOrders();
            return View(model);
        }
        public ActionResult Create()
        {
            var id = Guid.Parse(User.Identity.GetUserId());
            PortfolioService pService = new PortfolioService(id);
            StockService stockService = new StockService();
            ViewBag.Portfolios = new SelectList(pService.GetPortfolios(), "Id", "Id");
            ViewBag.Stocks = new SelectList(stockService.GetStocks(), "TickerSymbol", "TickerSymbol");
 
            return View();
        }
        [HttpPost]
        public ActionResult Create(CreateOrder model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var service = CreateService();
            if (service.AddOrder(model))
                return RedirectToAction("Index");
            return View(model);
        }
        public ActionResult Details(int id)
        {
            var service = CreateService();
            var model = service.GetOrder(id);
            return View(model);
        }
        [Route("Fill/{id}")]
        public ActionResult Fill(int id)
        {
            var service = CreateService();
            if(service.FillOrder(id))
                return RedirectToAction("Index");
            return RedirectToAction("Details", new { id = id });
            
        }
        public ActionResult Edit(int id)
        {
            return View();
        }
        [HttpPost,ActionName("Edit")]
        public ActionResult Edit(OrderEdit model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var service = CreateService();
            if (service.UpdateOrder(model))
                return RedirectToAction("Index");
            return View(model);
        }
        public ActionResult Delete(int id)
        {
            var service = CreateService();
            var model = service.GetOrder(id);
            return View(model);
        }
        [HttpPost,ActionName("Delete")]
        public ActionResult DeleteOrder(int id)
        {
            var service = CreateService();
            if(service.DeleteOrder(id))
                return RedirectToAction("Index");
            return View(service.GetOrder(id));

        }
    }
}