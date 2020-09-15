using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using IMODA_FRONT_HTTPS.Models;

namespace IMODA_FRONT_HTTPS.ViewModels
{
    public class customerViewModels
    {
        private static test1Entities db = new test1Entities();
        [Key]
        public int ID { get; set; }
        public static List<service_category> service_category_data()
        {
            var service_category = db.service_category.OrderBy(m => m.id).ToList();
            return service_category;
        }
        public static List<service_order> service_order_data()
        {
            var service_order = db.service_order.OrderBy(m => m.id).ToList();
            return service_order;
        }
    }
}