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
    public class StockController : Controller
    {
        public StockService CreateService()
        {
            return new StockService();
        }
        // GET: Stock
        public ActionResult Index()
        {
            var service = CreateService();
            var model = service.GetStocks();
            return View(model);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(CreateStock model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var service = CreateService();
            if (service.AddStock(model))
                return RedirectToAction("Index");
            return View(model);
        }
        public ActionResult Details(string id)
        {
            var service = CreateService();
            var entity = service.GetStock(id);
            return View(entity);
        }
        public ActionResult Edit(string symbol)
        {
            return View();
        }
        [HttpPost,ActionName("Edit")]
        public ActionResult Edit(StockEdit model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var service = CreateService();
            if (service.UpdateStock(model))
                return RedirectToAction("Index");
            return View(model);
        }
        public ActionResult Delete(string id)
        {
            var service = CreateService();
            var model = service.GetStock(id);
            return View(model);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteStock(string id)
        {
            var service = CreateService();
            if (service.DeleteStock(id))
                return RedirectToAction("Index");
            return View(service.GetStock(id));
        }

    }
}