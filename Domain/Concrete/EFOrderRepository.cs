using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Abstract;
using Domain.Entities;
namespace Domain.Concrete
{
    public class EFOrderRepository : IOrdersRepository
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<Order> Orders 
        {
            get { return context.Orders; }
        }
        public void SaveOrder(Order order)
        {
            order.OrderStatus= context.OrderStatuses.FirstOrDefault(oS => oS.OrderStatusName.Equals("Przyjęte"));
            order.EquipmentType = context.EquipmentTypes.FirstOrDefault(eQ => eQ.EquipmentTypeName == order.EquipmentType.EquipmentTypeName);
            if (order.Customer != null)
            {
                order.Customer = context.Users.FirstOrDefault(u => u.Login == order.Customer.Login);
            }
            if (order.Company != null)
            {
                order.Company = context.Companies.FirstOrDefault(c => c.CompanyName == order.Company.CompanyName);
            }
            List<ReperairTypePrice> rprList = new List<ReperairTypePrice>();
            foreach (var item in order.ReperairCostList)
            {
                ReperairTypePrice reperairTypePrice = new ReperairTypePrice { Price = item.Price, ReperairType = context.ReperairTypes.FirstOrDefault(rt => rt.Description == item.ReperairType.Description) };
                rprList.Add(reperairTypePrice);
                context.ReperairTypePrice.Add(reperairTypePrice);
                context.SaveChanges();
                
            }
            order.ReperairCostList = rprList;
            context.Orders.Add(order);
            context.SaveChanges();
            
        }
        public void EditOrder(Order order)
        {
            Order dbOrder = context.Orders.FirstOrDefault(o => o.OrderID == order.OrderID);
            dbOrder.additionalInformation = order.additionalInformation;
            dbOrder.Description = order.Description;
            dbOrder.endDate = order.endDate;
            dbOrder.receivingDate = order.receivingDate;
            dbOrder.EquipmentType = context.EquipmentTypes.FirstOrDefault(eqT => eqT.EquipmentTypeName == order.EquipmentType.EquipmentTypeName);
            dbOrder.OrderStatus = context.OrderStatuses.FirstOrDefault(oS => oS.OrderStatusID == order.OrderStatus.OrderStatusID);
            List<ReperairTypePrice> rpList = dbOrder.ReperairCostList.ToList();
            foreach (var item in rpList)
            {
                context.ReperairTypePrice.Remove(item);
            }
            dbOrder.ReperairCostList = new List<ReperairTypePrice>();
            foreach (var item in order.ReperairCostList)
            {
                ReperairType rT = context.ReperairTypes.FirstOrDefault(rt => rt.Description == item.ReperairType.Description);
                dbOrder.ReperairCostList.Add(new ReperairTypePrice { Price = item.Price, ReperairType = rT });
                
            }

            context.Entry(dbOrder).State = System.Data.EntityState.Modified;
            context.Entry(dbOrder.EquipmentType).State = System.Data.EntityState.Modified;
            context.Entry(dbOrder.OrderStatus).State = System.Data.EntityState.Modified;
            context.SaveChanges();
        }
    }
}
