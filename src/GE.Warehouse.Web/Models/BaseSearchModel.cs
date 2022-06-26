using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GE.Warehouse.Web.Models
{
    public class BaseSearchModel
    {
        public BaseSearchModel()
        {
            PageIndex = 1;
        }
        public string Keyword { get; set; }
        public int PageIndex { get; set; }
    }
}