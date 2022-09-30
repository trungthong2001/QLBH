using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLBH_NguyenHoangTrungThong.Models
{
    public class Cart
    {
        private NƯDataClassesDataContext dt = new NƯDataClassesDataContext();

        public int ProductID { get; set; }

        public string ProductName { get; set; }

        public decimal? UnitPrice { get; set; }

        public int Quatity { get; set; }

        public decimal? Total { get { return UnitPrice * Quatity; } }

        public Cart(int ProductID)
        {
            this.ProductID = ProductID;
            Product p = dt.Products.Single(n => n.ProductID == ProductID);
            ProductName = p.ProductName;
            UnitPrice = p.UnitPrice;
            Quatity = 1;
        }
    }
}