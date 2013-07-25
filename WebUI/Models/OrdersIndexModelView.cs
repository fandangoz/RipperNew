using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace WebUI.Models
{
    public class OrdersIndexModelView
    {
        public SelectList ordersStatusesList { set; get; }
        public string selectedStatus { set; get; }
    }
}