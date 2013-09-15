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
namespace WebUI.Controllers
{
    public class EquipmentTypeController : Controller
    {
        public EquipmentTypeController(IEquipmentTypesRepository eqRepo)
        {
            EqRepo = eqRepo;
        }
        private IEquipmentTypesRepository EqRepo;

        //
        // GET: /EquipmentType/

        public ActionResult Index()
        {
            return View(EqRepo.EquipmentTypes.ToList());
        }

        //
        // GET: /EquipmentType/Details/5

        public ActionResult Details(int id = 0)
        {
            EquipmentType equipmenttype = EqRepo.EquipmentTypes.FirstOrDefault(eq => eq.EquipmentTypeID == id);
            if (equipmenttype == null)
            {
                return HttpNotFound();
            }
            return View(equipmenttype);
        }

        //
        // GET: /EquipmentType/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /EquipmentType/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EquipmentType equipmenttype)
        {
            if (ModelState.IsValid)
            {
                EqRepo.saveEquipment(equipmenttype);
                return RedirectToAction("Index");
            }

            return View(equipmenttype);
        }

        //
        // GET: /EquipmentType/Edit/5

        public ActionResult Edit(int id = 0)
        {
            EquipmentType equipmenttype = EqRepo.EquipmentTypes.FirstOrDefault(eq => eq.EquipmentTypeID == id);
            if (equipmenttype == null)
            {
                return HttpNotFound();
            }
            return View(equipmenttype);
        }

        //
        // POST: /EquipmentType/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EquipmentType equipmenttype)
        {
            if (ModelState.IsValid)
            {
                EqRepo.editEQType(equipmenttype);
                return RedirectToAction("Index");
            }
            return View(equipmenttype);
        }

        //
        // GET: /EquipmentType/Delete/5
        public ViewResult BootIndex()
        {
            return View();
        }

    }
}