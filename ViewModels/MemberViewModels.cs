using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using IMODA_FRONT_HTTPS.Models;

namespace IMODA_FRONT_HTTPS.ViewModels
{
    public class MemberViewModels
    {
        [Key]
        public int ID { get; set; }
        public List<member> member_data
        {           
            get;
            set;
        }
        public List<order_> order_data
        {
            get;
            set;
        }
        public List<member_level> member_level_data
        {
            get;
            set;
        }
        public List<String> ob_order_status
        {
            get;
            set;
        }
        public int ordersum
        {
            get;
            set;
        }
        public int wait_shopping_count
        {
            get;
            set;
        }
        public int shopping_count
        {
            get;
            set;
        }
    }
}