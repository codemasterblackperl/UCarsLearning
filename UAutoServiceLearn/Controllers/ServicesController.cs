using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UAutoServiceLearn.Data;
using UAutoServiceLearn.Models;
using UAutoServiceLearn.Utilities;
using UAutoServiceLearn.ViewModels;

namespace UAutoServiceLearn.Controllers
{
    public class ServicesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ServicesController(ApplicationDbContext db)
        {
            _db = db;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _db.Dispose();
            base.Dispose(disposing);
        }

        [Authorize]
        public async Task<IActionResult> Index(int? carId)
        {
            if (carId == null)
                return NotFound();

            var car = await _db.Cars.FindAsync(carId);
            if (car == null)
                return NotFound();

            var model = new CarAndServicesViewModel()
            {
                CarId = car.Id,
                Make = car.Make,
                Model = car.Model,
                Style = car.Style,
                VIN = car.VIN,
                Year = car.Year,
                UserId = car.UserId,
                LstServiceType = _db.ServiceTypes.ToList(),
                PastServices = _db.Services.Where(x => x.CarId == carId).OrderByDescending(d => d.DateAdded)
            };

            return View(model);
        }


        //get: Service/Create
        [Authorize(Roles = StaticDetails.Admin)]
        public async Task<IActionResult> Create(int? carId)
        {
            if (carId == null)
                return NotFound();

            var car = await _db.Cars.FindAsync(carId);
            if (car == null)
                return NotFound();

            var model = new CarAndServicesViewModel()
            {
                CarId=car.Id,
                Make=car.Make,
                Model=car.Model,
                Style=car.Style,
                VIN=car.VIN,
                Year=car.Year,
                UserId=car.UserId,
                LstServiceType = _db.ServiceTypes.ToList(),
                PastServices = _db.Services.Where(x => x.CarId == carId).OrderByDescending(d => d.DateAdded).Take(5)
            };

            return View(model);
        }

        //post Service/Create
        [Authorize(Roles = StaticDetails.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CarAndServicesViewModel csModel)
        {
            if(!ModelState.IsValid)
            {
                var car = await _db.Cars.FindAsync(csModel.CarId);

                var model = new CarAndServicesViewModel()
                {
                    CarId = car.Id,
                    Make = car.Make,
                    Model = car.Model,
                    Style = car.Style,
                    VIN = car.VIN,
                    Year = car.Year,
                    UserId = car.UserId,
                    LstServiceType = _db.ServiceTypes.ToList(),
                    PastServices = _db.Services.Where(x => x.CarId == csModel.CarId).OrderByDescending(d => d.DateAdded).Take(5)
                };

                return View(model);
            }

            csModel.NewService.CarId = csModel.CarId;
            csModel.NewService.DateAdded = DateTime.Now;
            _db.Services.Add(csModel.NewService);
            await _db.SaveChangesAsync();

            return RedirectToAction("Create",new { carId=csModel.CarId});

            
        }

        //get: delete/serviceId
        [Authorize(Roles = StaticDetails.Admin)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var service = await _db.Services.Include(s => s.Car).Include(x => x.ServiceType).SingleOrDefaultAsync(x => x.Id == id);
            if (service == null)
                return NotFound();

            return View(service);
        }

        //post: delete/serviceId
        [Authorize(Roles = StaticDetails.Admin)]
        [HttpPost,ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Service model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var serviceId = model.Id;
            var carid = model.CarId;

            var service = await _db.Services.SingleOrDefaultAsync(x => x.Id == serviceId);
            if (service == null)
                return NotFound();

            _db.Services.Remove(service);
            await _db.SaveChangesAsync();

            return RedirectToAction("Create", new { carId = carid });
        }
    }
}