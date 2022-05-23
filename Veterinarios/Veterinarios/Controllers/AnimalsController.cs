using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using Veterinarios.Data;
using Veterinarios.Models;

namespace Veterinarios.Controllers {

   [Authorize]
   public class AnimalsController : Controller {
      /// <summary>
      /// reference the application database
      /// </summary>
      private readonly ApplicationDbContext _context;

      /// <summary>
      /// gets all data from authenticated user
      /// </summary>
      private readonly UserManager<ApplicationUser> _userManager;

      public AnimalsController(ApplicationDbContext context,
                               UserManager<ApplicationUser> userManager) {
         _context = context;
         _userManager = userManager;
      }




      // GET: Animals
      /// <summary>
      /// list user's animals
      /// </summary>
      /// <returns></returns>
      public async Task<IActionResult> Index() {

         // SELECT *
         // FROM animal a INNER JOIN owner o on a.ownerFK = o.Id
         //
         // var animals = _context.Animals.Include(a => a.Owner);

         // ##########################################################
         // SELECT *
         // FROM animal a INNER JOIN owner o on a.ownerFK = o.Id
         // WHERE o.OwnerID = (ID of auntenticated user)

         // get the User ID
         string userID = _userManager.GetUserId(User);

         var animals = _context.Animals
                               .Include(a => a.Owner)
                               .Where(a => a.Owner.UserID == userID);


         return View(await animals.ToListAsync());
      }





      // GET: Animals/Details/5
      public async Task<IActionResult> Details(int? id) {
         if (id == null || _context.Animals == null) {
            return RedirectToAction("Index");
         }

         // get the User ID
         string userID = _userManager.GetUserId(User);

         var animal = await _context.Animals
                                    .Include(a => a.Owner)
                                    .Where(m => m.Id == id &&
                                                m.Owner.UserID == userID)
                                    .FirstOrDefaultAsync();
         if (animal == null) {
            return RedirectToAction("Index");
         }

         return View(animal);
      }




      // GET: Animals/Create
      public IActionResult Create() {
         // we do not need anymore the dropdown data
         //      ViewData["OwnerFK"] = new SelectList(_context.Owners, "Id", "NIF");
         return View();
      }



      // POST: Animals/Create
      // To protect from overposting attacks, enable the specific properties you want to bind to.
      // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Create([Bind("Name,Breed,Species,BirthDate,Weight,Photo")] Animal animal) {

         /* we must assign User ID to animal
          * what we need to do?
          * 
          * 1- read authenticaed user ID
          * 2- with that value, read de owner ID
          * 3- assign owner ID to OwnerFK at animal's data
          */

         // (1)
         string userID = _userManager.GetUserId(User);

         // (2)
         int ownerID = _context.Owners
                               .Where(o => o.UserID == userID)
                               .FirstOrDefault()
                               .Id;
         // (3)
         animal.OwnerFK = ownerID;

         if (ModelState.IsValid) {
            try {
               _context.Add(animal);
               await _context.SaveChangesAsync();
               return RedirectToAction(nameof(Index));
            }
            catch (Exception) {

               throw;
            }
         }

         //   ViewData["OwnerFK"] = new SelectList(_context.Owners, "Id", "NIF", animal.OwnerFK);
         return View(animal);
      }

      // GET: Animals/Edit/5
      public async Task<IActionResult> Edit(int? id) {
         if (id == null || _context.Animals == null) {
            return NotFound();
         }

         var animal = await _context.Animals.FindAsync(id);
         if (animal == null) {
            return NotFound();
         }
         ViewData["OwnerFK"] = new SelectList(_context.Owners, "Id", "NIF", animal.OwnerFK);
         return View(animal);
      }

      // POST: Animals/Edit/5
      // To protect from overposting attacks, enable the specific properties you want to bind to.
      // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Breed,Species,BirthDate,Weight,Photo,OwnerFK")] Animal animal) {
         if (id != animal.Id) {
            return NotFound();
         }

         if (ModelState.IsValid) {
            try {
               _context.Update(animal);
               await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
               if (!AnimalExists(animal.Id)) {
                  return NotFound();
               }
               else {
                  throw;
               }
            }
            return RedirectToAction(nameof(Index));
         }
         ViewData["OwnerFK"] = new SelectList(_context.Owners, "Id", "NIF", animal.OwnerFK);
         return View(animal);
      }

      // GET: Animals/Delete/5
      public async Task<IActionResult> Delete(int? id) {
         if (id == null || _context.Animals == null) {
            return NotFound();
         }

         var animal = await _context.Animals
             .Include(a => a.Owner)
             .FirstOrDefaultAsync(m => m.Id == id);
         if (animal == null) {
            return NotFound();
         }

         return View(animal);
      }

      // POST: Animals/Delete/5
      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> DeleteConfirmed(int id) {
         if (_context.Animals == null) {
            return Problem("Entity set 'ApplicationDbContext.Animals'  is null.");
         }
         var animal = await _context.Animals.FindAsync(id);
         if (animal != null) {
            _context.Animals.Remove(animal);
         }

         await _context.SaveChangesAsync();
         return RedirectToAction(nameof(Index));
      }

      private bool AnimalExists(int id) {
         return _context.Animals.Any(e => e.Id == id);
      }
   }
}
