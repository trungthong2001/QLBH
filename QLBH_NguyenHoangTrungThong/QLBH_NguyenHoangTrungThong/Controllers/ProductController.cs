using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLBH_NguyenHoangTrungThong.Models;

namespace QLBH_NguyenHoangTrungThong.Controllers
{
    public class ProductController : Controller
    {
        private NƯDataClassesDataContext da = new NƯDataClassesDataContext();

        // GET: Product

        public ActionResult Index()
        {
            var p = da.Products.Select(s => s).ToList();
            return View(p);
        }

        public ActionResult ListProduct()
        {
            var p = da.Products.Select(s => s).ToList();
            return View(p);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Delete(int id)
        {
            var p = da.Products.FirstOrDefault(s => s.ProductID == id);
            return View(p);
        }

        public ActionResult Edit(int id)
        {
            var p = da.Products.FirstOrDefault(s => s.ProductID == id);
            return View(p);
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection, Product product)
        {
            da.Products.InsertOnSubmit(product);
            da.SubmitChanges();
            return RedirectToAction("ListProduct");
        }

        [HttpPost]
        public ActionResult Delete(FormCollection collection, int id)
        {
            da.Products.DeleteOnSubmit(da.Products.First(s => s.ProductID == id));
            da.SubmitChanges();
            return RedirectToAction("ListProduct");
        }


        [HttpPost]
        public ActionResult Edit(FormCollection collection, int id)
        {
            var sp = da.Products.First(s => s.ProductID == id);
            sp.ProductName = collection["ProductName"];
            sp.UnitPrice = decimal.Parse(collection["UnitPrice"]);
            sp.SupplierID = int.Parse(collection["SupplierID"]);
            sp.UnitsInStock = short.Parse(collection["UnitsInStock"]);

            UpdateModel(sp);

            da.SubmitChanges();
            return RedirectToAction("ListProduct");
        }

        public ActionResult Details(int id)
        {
            var p = da.Products.FirstOrDefault(s => s.ProductID == id);
            return View(p);
        }
    }
}