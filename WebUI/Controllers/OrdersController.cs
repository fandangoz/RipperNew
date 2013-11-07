using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Entities;
using Domain.Concrete;
using Domain.Abstract;
using WebUI.Models;
using WebUI.Helpers;
namespace WebUI.Controllers
{
    public class OrdersController : Controller
    {
        public OrdersController(IOrdersRepository Orepo, IUserRepository Urepo,
            ICompaniesRepository Crepo, IEquipmentTypesRepository eqTyperepo,
            IReperairType reperairRepo, IReperairTypePrice reperairPriceRepo, IOrderStatusesRepository OSrepo)
        {
            OrdersRepository = Orepo;
            UsersRepository = Urepo;
            CompaniesRepository = Crepo;
            EquipmentTypeRepo = eqTyperepo;
            ReperairTypeRepo = reperairRepo;
            ReperairPriceRepo = reperairPriceRepo;
            OrderStatusesRepo = OSrepo;
        }
        private int PageSize = 10;
        private IOrdersRepository OrdersRepository;
        private IUserRepository UsersRepository;
        private ICompaniesRepository CompaniesRepository;
        private IEquipmentTypesRepository EquipmentTypeRepo;
        private IReperairType ReperairTypeRepo;
        private IReperairTypePrice ReperairPriceRepo;
        private IOrderStatusesRepository OrderStatusesRepo;
        //
        // GET: /Orders/
         [Authorize(Roles = "Admin, Biuro")]
        public ActionResult Index()
        {
            var orderStatuses = OrderStatusesRepo.OrderStatuses.Select( o => new SelectListItem { Value = o.OrderStatusName, Text = o.OrderStatusName}).ToList();
            orderStatuses.Add(new SelectListItem{ Value = "wszystkie" , Text ="wszystkie"});
            SelectList orderStatusList = new SelectList(orderStatuses, "Value", "Text");

            return View(new OrdersIndexModelView { ordersStatusesList = orderStatusList });
        }
         [Authorize(Roles = "Admin, Biuro")]
        public PartialViewResult OrdersList(int page = 1, string name = "", string status ="", int userId = 0, bool? isCompany =false)
        {

            var pagedData = new PagedData<Order>();
            var orders = OrdersRepository.Orders;

            if (status.Length > 0 && !status.Equals("wszystkie"))
            {
                orders = from o in orders where o.OrderStatus.OrderStatusName.ToLower().StartsWith(status) select o;
            }
            if (isCompany == false)
            {
                orders = from o in orders where o.Customer != null select o;
                if (userId != 0)
                {
                    orders = from o in orders where o.Customer.UserID == userId select o;
                }
                if (name.Length > 0)
                {
                    orders = from o in orders where o.Customer.Login.ToLower().StartsWith(name.ToLower()) select o;
                }
            }
            else
            {
                orders = from o in orders where o.Company != null select o;
                if (userId != 0)
                {
                    orders = from o in orders where o.Company.CompanyID == userId select o;
                }
                if (name.Length > 0)
                {
                    orders = from o in orders where o.Company.CompanyName.ToLower().StartsWith(name.ToLower()) select o;
                }
            }
            pagedData.Data = orders.OrderBy(o => o.receivingDate).Skip(PageSize * (page - 1)).Take(PageSize).ToList();
            pagedData.NumberOfPages = Convert.ToInt32(Math.Ceiling((double)orders.Count() / PageSize));
            pagedData.CurrentPage = page;
            return PartialView(pagedData);
        }
         [Authorize(Roles = "Admin, Biuro")]
        public ActionResult Details(int id = 0)
        {
            Order order = OrdersRepository.Orders.FirstOrDefault(o => o.OrderID == id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }
         [Authorize(Roles = "Admin, Biuro")]
        public ActionResult CreateBoot()
        {
            return RedirectToAction("Create");
        }

        //
        // GET: /Orders/Create
         [Authorize(Roles = "Admin, Biuro")]
        public ActionResult Create(bool? isCompany, int id =0)
        {
            OrderModelView oMV = new OrderModelView { Order = new Order { receivingDate = DateTime.Now } };
            if (id != 0)
            {
                if (isCompany == true)
                {
                    oMV.Order.isCompany = true;
                    oMV.Name = CompaniesRepository.Companies.FirstOrDefault(c => c.CompanyID == id).CompanyName;
                }
                if (isCompany == false)
                {
                    oMV.Name = UsersRepository.Users.FirstOrDefault(u => u.UserID == id).Login;
                    oMV.Order.isCompany = false;
                }
            }
            return View(oMV);
        }

        //
        // POST: /Orders/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Biuro")]
        public ActionResult Create(OrderModelView orderMV)
        {
            if (orderMV.Order.isCompany == false)
            {
                orderMV.Order.Customer = UsersRepository.Users.FirstOrDefault(u => u.Login == orderMV.Name);
            }
            else
            {
                orderMV.Order.Company = CompaniesRepository.Companies.FirstOrDefault(c => c.CompanyName == orderMV.Name);
            }
            if (orderMV.Order.Customer == null && orderMV.Order.Company == null)
            {
                ModelState.AddModelError("", "Nie znaleziono użytkownika lub firmy");
            }
            if (ModelState["Order.OrderStatus"] != null)
            {
                ModelState["Order.OrderStatus"].Errors.Clear();
            }
            foreach (var item in orderMV.reperairTypePrice)
            {
                if (item.price == 0 && item.type != null || item.price != 0 && item.type == null)
                {
                    ModelState.AddModelError("", "Nie można zapisać rodzaju naprawy bez ceny lub ceny bez rodzaju naprawy");
                    break;
                }
                if (item.price != 0 && item.type != null)
                {
                    orderMV.Order.ReperairCostList.Add(new ReperairTypePrice { Price = item.price, ReperairType = new ReperairType { Description = item.type } });
                }
            }
            if (EquipmentTypeRepo.EquipmentTypes.FirstOrDefault(eq => eq.EquipmentTypeName.ToLower() == orderMV.Order.EquipmentType.EquipmentTypeName.ToLower()) == null)
            {
                EquipmentTypeRepo.saveEquipment(orderMV.Order.EquipmentType);
            }
            foreach (var item in orderMV.Order.ReperairCostList)
            {
                if (ReperairTypeRepo.ReperairTypes.FirstOrDefault(rtr => rtr.Description.ToLower() == item.ReperairType.Description.ToLower()) == null)
                {
                    ReperairTypeRepo.SaveReperairType(item.ReperairType);
                }
            }

            if (ModelState.IsValid)
            {

                OrdersRepository.SaveOrder(orderMV.Order);
               return RedirectToAction("Details", new { id = orderMV.Order.OrderID });
            }

            return View(orderMV);
        }

        //
        // GET: /Orders/Edit/5
         [Authorize(Roles = "Admin, Biuro")]
        public ActionResult Edit(int id = 0)
        {
            Order order = OrdersRepository.Orders.FirstOrDefault(o => o.OrderID == id);
            if (order == null)
            {
                return HttpNotFound();
            }
            var orderStatuses = OrderStatusesRepo.OrderStatuses.Select(o => new SelectListItem { Value = o.OrderStatusName, Text = o.OrderStatusName }).ToList();
            SelectList orderStatusList = new SelectList(orderStatuses, "Value", "Text");
            OrderModelView OMV = new OrderModelView { OrderStatusList = orderStatusList, selectedStatus = order.OrderStatus.OrderStatusName };
            int i = 0;
            foreach (var item in order.ReperairCostList)
            {
                OMV.reperairTypePrice[i].price = item.Price;
                OMV.reperairTypePrice[i].type = item.ReperairType.Description;
                ++i;
            }
            OMV.Order = order;
            return View(OMV);
        }

        //
        // POST: /Orders/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Biuro")]
        public ActionResult Edit(OrderModelView orderMV)
        {
            foreach (var item in orderMV.reperairTypePrice)
            {
                if (item.price == 0 && item.type != null || item.price != 0 && item.type == null)
                {
                    ModelState.AddModelError("", "Nie można zapisać rodzaju naprawy bez ceny lub ceny bez rodzaju naprawy");
                    break;
                }
                if (item.price != 0 && item.type != null)
                {
                    ReperairType rT = ReperairTypeRepo.ReperairTypes.FirstOrDefault(rType => rType.Description == item.type);
                    orderMV.Order.ReperairCostList.Add(new ReperairTypePrice { Price = item.price, ReperairType = rT != null ? rT : new ReperairType { Description = item.type } });
                }
            }
            if (EquipmentTypeRepo.EquipmentTypes.FirstOrDefault(eq => eq.EquipmentTypeName == orderMV.Order.EquipmentType.EquipmentTypeName) == null)
            {
                EquipmentTypeRepo.saveEquipment(orderMV.Order.EquipmentType);
            }
            foreach (var item in orderMV.Order.ReperairCostList)
            {
                if (ReperairTypeRepo.ReperairTypes.FirstOrDefault(rtr => rtr.Description == item.ReperairType.Description) == null)
                {
                    ReperairTypeRepo.SaveReperairType(item.ReperairType);
                }
            }
            Order dbOrder = OrdersRepository.Orders.FirstOrDefault(o => o.OrderID == orderMV.Order.OrderID);
            orderMV.Order.OrderStatus = OrderStatusesRepo.OrderStatuses.FirstOrDefault(os => os.OrderStatusName == orderMV.selectedStatus);
            ModelState["Name"].Errors.Clear() ;
            if (orderMV.Order.OrderStatus != null)
            {
                ModelState["Order.OrderStatus"].Errors.Clear();
                if (ModelState["Order.endDate"] != null && orderMV.Order.OrderStatus.OrderStatusName != "Anulowane" && orderMV.Order.OrderStatus.OrderStatusName != "Zakończone")
                {
                    ModelState["Order.endDate"].Errors.Clear();
                }
            }
            if (ModelState.IsValid)
            {
                OrdersRepository.EditOrder(orderMV.Order);
                return RedirectToAction("Details", new { id = orderMV.Order.OrderID });
            }
            var orderStatuses = OrderStatusesRepo.OrderStatuses.Select(o => new SelectListItem { Value = o.OrderStatusName, Text = o.OrderStatusName }).ToList();
            SelectList orderStatusList = new SelectList(orderStatuses, "Value", "Text");
            orderMV.OrderStatusList = orderStatusList;
            return View(orderMV);
        }

        //
        // GET: /Orders/Delete/5
         [Authorize(Roles = "Admin, Biuro")]
        public ActionResult Delete(int id = 0)
        {
            Order order = OrdersRepository.Orders.FirstOrDefault(o => o.OrderID == id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }
         [Authorize(Roles = "Admin, Biuro")]
        public JsonResult AutoCompleteClientName(string term, string type)
        {
            if (type == "Firma")
            {
                var result = (from c in CompaniesRepository.Companies
                              where c.CompanyName.ToLower().StartsWith(term.ToLower())
                              select new { Name = c.CompanyName }).Distinct().ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = (from u in UsersRepository.Users
                              where u.Login.ToLower().StartsWith(term.ToLower())
                              select new {Name = u.Login }).Distinct().ToList();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
         [Authorize(Roles = "Admin, Biuro")]
        public JsonResult GerReperairTypes(string term)
        {
            var result = (from Rt in ReperairTypeRepo.ReperairTypes
                          where Rt.Description.ToLower().StartsWith(term.ToLower())
                          select new { Type = Rt.Description }).Distinct().ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
         [Authorize(Roles = "Admin, Biuro")]
        public JsonResult GetEqupmentTypes(string term)
        {
            var result = (from EqType in EquipmentTypeRepo.EquipmentTypes
                          where EqType.EquipmentTypeName.ToLower().StartsWith(term.ToLower())
                          select new { Type = EqType.EquipmentTypeName }).Distinct().ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ViewResult BootIndex()
        {
            return View();
        }

    }
}