using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UAutoServiceLearn.Data;
using UAutoServiceLearn.Models;
using UAutoServiceLearn.Models.AccountViewModels;
using UAutoServiceLearn.Utilities;

namespace UAutoServiceLearn.Controllers
{
    [Authorize(Roles = StaticDetails.Admin)]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _db;

        public UsersController(ApplicationDbContext db)
        {
            _db = db;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _db.Dispose();

            base.Dispose(disposing);
        }

        public IActionResult Index(string option=null,string search=null)
        {
            var users = _db.Users.ToList();

            if (search == null)
                return View(users);

            search = search.ToLower();
            if(option=="name")
            {
                users = users.Where(u => u.FirstName.ToLower().Contains(search) || u.LastName.ToLower().Contains(search)).ToList();
            }
            else if(option=="email")
            {
                users = users.Where(u => u.Email.ToLower().Contains(search)).ToList();
            }
            else if(option=="phone")
            {
                users = users.Where(u => u.PhoneNumber.ToLower().Contains(search)).ToList();
            }

            return View(users);
        }

        //get: Users/Details
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Index");

            var userDetail = await _db.Users.FindAsync(id);
            if (userDetail == null)
                return NotFound();

            return View(userDetail);
        }

        //get: Users/Edit
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Index");

            var userDetail = await _db.Users.FindAsync(id);
            if (userDetail == null)
                return NotFound();

            return View(userDetail);
        }

        //post: Users/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicationUser user)
        {
            if(!ModelState.IsValid)
            {
                return View(user);
            }

            var userDetail = await _db.Users.FindAsync(user.Id);
            if (userDetail == null)
                return NotFound();

            userDetail.FirstName = user.FirstName;
            userDetail.LastName = user.LastName;
            userDetail.PhoneNumber = user.PhoneNumber;
            userDetail.PostalCode = user.PostalCode;
            userDetail.Address = user.Address;
            userDetail.City = user.City;

            _db.Users.Update(userDetail);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        //get: Users/Delete
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Index");

            var userDetail = await _db.Users.FindAsync(id);
            if (userDetail == null)
                return NotFound();

            return View(userDetail);
        }

        //post: Users/Delete
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var userDetail = await _db.Users.FindAsync(id);
            if (userDetail == null)
                return NotFound();

            var cars = _db.Cars.Where(x => x.UserId == userDetail.Id);

            if (cars.Count() > 0)
            {
                foreach (var car in cars.ToList())
                {
                    var services = _db.Services.Where(x => x.CarId == car.Id);
                    _db.Services.RemoveRange(services);
                }

                _db.Cars.RemoveRange(cars);
            }

            _db.Users.Remove(userDetail);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");

        }


    }
}