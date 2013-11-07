using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace WebUI.Models
{
    public class OrderModelView
    {
        public OrderModelView()
        {
            reperairTypePrice = new List<TypePricePair>();
            for (int i = 0; i < 10; i++)
            {
                reperairTypePrice.Add(new TypePricePair());
            }
        }
        public Order Order { set; get; }
        [Required(ErrorMessage="Podaj nazwę klienta")]
        public string Name { get; set; }
        public List<TypePricePair> reperairTypePrice { get; set; }
        [Display(Name="Status zlecenia")]
        public SelectList OrderStatusList { set; get; }
        public string selectedStatus { set; get; }
    }
    public class TypePricePair
    {
        public TypePricePair() { type = ""; price = 0; }
        public string type { set; get; }
        public double price { set; get; }
    }
}