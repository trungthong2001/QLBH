using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLBH_NguyenHoangTrungThong.Models;
using System.Transactions;

namespace QLBH_NguyenHoangTrungThong.Controllers
{
    public class CartController : Controller
    {

        private NƯDataClassesDataContext da = new NƯDataClassesDataContext();

        // GET: Cart
        public List<Cart> GetListCart()
        {
            List<Cart> carts = Session["Cart"] as List<Cart>;
            if (carts == null)
            {
                carts = new List<Models.Cart>();
                Session["Cart"] = carts;
            }

            return carts;
        }


        public ActionResult AddCart(int id)
        {
            List<Cart> carts = GetListCart();
            Cart c = carts.Find(s => s.ProductID == id);
            if (c == null)
            {
                c = new Models.Cart(id);
                carts.Add(c);
            }
            else
            {
                c.Quatity++;
            }
            return RedirectToAction("ListCart");

        }

        public ActionResult ListCart()
        {
            List<Cart> carts = GetListCart();
            if (carts.Count == 0)
            {
                return RedirectToAction("ListProduct", "Product");
            }
            ViewBag.CountProduct = Count();
            ViewBag.Total = Total();
            return View(carts);
        }


        public ActionResult Delete(int id)
        {
            List<Cart> carts = GetListCart();
            Cart c = carts.Find(s => s.ProductID == id);
            if (c != null)
            {
                carts.RemoveAll(s => s.ProductID == id);
                return RedirectToAction("ListCart");
            }
            if (carts.Count == 0)
            {
                return RedirectToAction("ListProduct", "Product");
            }
            return RedirectToAction("ListCart");
        }

        public int Count()
        {
            int n = 0;
            List<Cart> carts = Session["Cart"] as List<Cart>;
            if (carts != null)
            {
                n = carts.Sum(s => s.Quatity);
            }
            return n;
        }

        public decimal? Total()
        {
            decimal? total = 0;
            List<Cart> carts = Session["Cart"] as List<Cart>;
            if (carts != null)
            {
                total = carts.Sum(s => s.Total);
            }
            return total;
        }

        public ActionResult OrderProduct(FormCollection fCollection)
        {
            using (TransactionScope tranScope = new TransactionScope())
            {
                try
                {
                    Order order = new Order();
                    order.OrderDate = DateTime.Now;
                    da.Orders.InsertOnSubmit(order);
                    da.SubmitChanges();
                    order = da.Orders.OrderByDescending(s => s.OrderID).Take(1).SingleOrDefault();

                    List<Cart> carts = GetListCart();
                    foreach (var item in carts)
                    {
                        Order_Detail d = new Models.Order_Detail();
                        d.OrderID = order.OrderID;
                        d.ProductID = item.ProductID;
                        d.Quantity = short.Parse(item.Quatity.ToString());
                        d.UnitPrice = decimal.Parse(item.UnitPrice.ToString());
                        d.Discount = 0;

                        da.Order_Details.InsertOnSubmit(d);
                    }

                    da.SubmitChanges();
                    Session["Cart"] = null;
                }
                catch (Exception)
                {
                    tranScope.Dispose();
                    return RedirectToAction("ListCarts");
                }
                return RedirectToAction("OrderDetailList", "Cart");
            }
        }

        public ActionResult OrderDetailList()
        {
            var p = da.Order_Details.OrderByDescending(s => s.OrderID).Select(s => s).ToList();
            return View(p);
        }
    }
}