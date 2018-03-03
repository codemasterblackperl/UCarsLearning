using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UAutoServiceLearn.Data;
using UAutoServiceLearn.Models;

namespace UAutoServiceLearn.Controllers
{
    public class ServiceTypesController : Controller
    {
        protected readonly ApplicationDbContext _db;

        public ServiceTypesController(ApplicationDbContext db)
        {
            _db = db;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }

            base.Dispose(disposing);
        }

        //Get: ServiceTypes/Index
        public IActionResult Index()
        {
            return View(_db.ServiceTypes.ToList());
        }

        //Get: ServiceTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        //post: ServiceTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceType serviceType)
        {
            if (!ModelState.IsValid)
                return View(serviceType);

            await _db.ServiceTypes.AddAsync(serviceType);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        //Get: ServiceTypes/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");

            var service = await _db.ServiceTypes.FindAsync(id);
            if(service==null)
                return RedirectToAction("Index");

            return View(service);
        }

        //post: ServiceTypes/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(int id,ServiceType serviceType)
        {
            if (!ModelState.IsValid)
                return View(serviceType);

            if (id != serviceType.Id)
                return View(serviceType);

            //var service = await _db.ServiceTypes.FindAsync(serviceType.Id);
            //service.Name = serviceType.Name;

            _db.ServiceTypes.Update(serviceType);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        //get: ServiceTypes/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");

            var service = await _db.ServiceTypes.FindAsync(id);
            if(service==null)
                return RedirectToAction("Index");

            return View(service);
        }

        //post: ServiceTypes/Delete
        [HttpPost]
        public async Task<IActionResult> Delete(ServiceType serviceType)
        {
            //var service = await _db.ServiceTypes.FindAsync(serviceType.Id);
            _db.ServiceTypes.Remove(serviceType);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        //get: ServiceTypes/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var service = await _db.ServiceTypes.FindAsync(id);
            if (service == null)
                return NotFound();

            return View(service);
        }

    }
}