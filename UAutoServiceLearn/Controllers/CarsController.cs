using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UAutoServiceLearn.Data;
using UAutoServiceLearn.Models;
using UAutoServiceLearn.ViewModels;

namespace UAutoServiceLearn.Controllers
{
    [Authorize]
    public class CarsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CarsController(ApplicationDbContext db)
        {
            _db = db;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _db.Dispose();
            base.Dispose(disposing);
        }

        public async Task<IActionResult> Index(string id=null)
        {
            if(string.IsNullOrEmpty(id))
            {
                //only called when guest user sign in
                id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            var model = new CarAndCustomerViewModel
            {
                Cars = _db.Cars.Where(c => c.UserId == id),
                User = _db.Users.FirstOrDefault(u => u.Id == id)
            };

            return View(model);
        }

        //get: Cars/Create
        public IActionResult Create(string userId)
        {
            var car = new Car
            {
                Year = DateTime.Now.Year,
                UserId=userId
            };

            return View(car);
        }

        //post: Cars/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Car car)
        {
            if (!ModelState.IsValid)
                return View(car);

            await _db.Cars.AddAsync(car);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index",new {id=car.UserId});
        }

        //get: Cars/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();
            //if (string.IsNullOrEmpty(id))
            //    return NotFound();

            //int carId = int.Parse(id);

            var car = await _db.Cars.FindAsync(id);
            if (car == null)
                return NotFound();

            return View(car);
        }

        //get: Cars/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var car = await _db.Cars.FindAsync(id);
            if (car == null)
                return NotFound();

            return View(car);
        }

        //post: Cars/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Car car)
        {
            if (!ModelState.IsValid)
                return View(car);

            //var updatedCar = await _db.Cars.FindAsync(car.Id);
            //if (updatedCar == null)
            //    return NotFound();

            _db.Cars.Update(car);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index",new {id=car.UserId });
        }


        //get: Cars/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var car = await _db.Cars.FindAsync(id);
            if (car == null)
                return NotFound();

            return View(car);
        }

        //post: Cars/Delete
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
                return NotFound();

            var car = await _db.Cars.FindAsync(id);
            if (car == null)
                return NotFound();

            _db.Cars.Remove(car);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index", new { id = car.UserId });
        }

    }
}