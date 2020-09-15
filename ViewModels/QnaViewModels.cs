using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IMODA_FRONT_HTTPS.Models;

namespace IMODA_FRONT_HTTPS.ViewModels
{
    public class QnaViewModels
    {
        //private static test1Entities db = new test1Entities();
        [Key]
        public int ID { get; set; }
        public IEnumerable<question_set> question_set_data
        {
            //var question_set = db.question_set.OrderBy(m => m.id).ToList();
            //return question_set;
            get;
            set;

        }
        public IEnumerable<question_category> question_category_data
        {
            //var question_category = db.question_category.OrderBy(m => m.id).ToList();
            //return question_category;
            get;
            set;
        }

        public class result
        {
            public int id { get; set; }

            public int active { get; set; }
            public string title { get; set; }
            public int data { get; set; }

        }
        public List<result> result_data
        {

            //var result = db.question_category.OrderBy(m => m.id).ToList();
            //return results;
            get;
            set;
        }
    }
}