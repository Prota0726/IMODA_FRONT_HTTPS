using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IMODA_FRONT_HTTPS.Models;


namespace IMODA_FRONT_HTTPS.ViewModels
{
    public class IndexViewModel
    {
        private static test1Entities db = new test1Entities();

        public static List<product_class> product_class_data()
        {
            var product_class = db.product_class.OrderBy(m => m.id).ToList();
            return product_class;
        }
        public static List<product_class1> product_class_data1()
        {
            var product_class1 = db.product_class1.OrderBy(m => m.id).ToList();
            return product_class1;
        }
        public static List<product_class2> product_class_data2()
        {
            var product_class2 = db.product_class2.OrderBy(m => m.id).ToList();
            return product_class2;
        }
        public static List<banner> banner_top()
        {
            var banner_top = db.banner.Where(m => m.banner_top == 1).ToList();
            return banner_top;
        }
        public List<banner> banner
        {
            get;
            set;
        }
    }
}