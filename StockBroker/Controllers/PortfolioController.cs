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
    public class PortfolioController : Controller
    {
        public PortfolioService CreateService()
        {
            var id = Guid.Parse(User.Identity.GetUserId());
            return new PortfolioService(id);
        }
        // GET: Portfolio
        public ActionResult Index()
        {
            var service = CreateService();
            var portfolios = service.GetPortfolios();
            return View(portfolios);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(CreatePortfolio model)
        {
            if (!ModelState.IsValid)
                View(model);
            var service = CreateService();
            if (service.AddPortfolio(model))
            {
                return RedirectToAction("Index");
            }
            return View(model);

        }
        public ActionResult Details(int id)
        {
            
            var service = CreateService();
            var entity = service.GetPortfolio(id);
            return View(entity);
        }
        public ActionResult Edit(int id)
        {
            return View();
        }
        [HttpPost, ActionName("Edit")]
        public ActionResult Edit(PortfolioEdit model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var service = CreateService();
            if (service.UpdatePortfolio(model))
                return RedirectToAction("Index");
            return View(model);
            
        }
        public ActionResult Delete(int id)
        {
            var service = CreateService();
            var entity = service.GetPortfolio(id);
            return View(entity);
        }
        [HttpPost,ActionName("Delete")]
        public ActionResult DeletePortfolio(int id)
        {
            var service = CreateService();
            try
            {
                service.DeletePortfolio(id);
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["SaveResult"] = "Please remove all cash before deleting portfolio";
                return View(service.GetPortfolio(id));
            }

        }
    }
}