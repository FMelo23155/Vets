using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using Veterinarios.Data;
using Veterinarios.Models;

namespace Veterinarios.Controllers {

   [Authorize]
   public class AppointementsController : Controller {
      private readonly ApplicationDbContext _context;

      public AppointementsController(ApplicationDbContext context) {
         _context = context;
      }

      // GET: Appointements
      public async Task<IActionResult> Index() {


         var applicationDbContext = _context.Appointements
                                            .Include(a => a.Animal)
                                            .Include(a => a.Vet);
     
         
         return View(await applicationDbContext.ToListAsync());
      }

      // GET: Appointements/Details/5
      public async Task<IActionResult> Details(int? id) {
         if (id == null) {
            return NotFound();
         }

         var appointement = await _context.Appointements
                                          .Include(a => a.Animal)
                                          .Include(a => a.Vet)
                                          .FirstOrDefaultAsync(m => m.Id == id);
         if (appointement == null) {
            return NotFound();
         }

         return View(appointement);
      }





      // GET: Appointements/Create
      public IActionResult Create() {
         ViewData["AnimalFK"] = new SelectList(_context.Animals.OrderBy(a=>a.Name), "Id", "Name");
         ViewData["VetFK"] = new SelectList(_context.Vets, "Id", "Name");
         return View();
      }


      // POST: Appointements/Create
      // To protect from overposting attacks, enable the specific properties you want to bind to.
      // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Create([Bind("Date,Obs,AuxPrice,Price,AnimalFK,VetFK")] Appointement appointement) {

         // transfer data from AuxPrice to Price
         appointement.Price = Convert.ToDecimal( appointement.AuxPrice.Replace('.',','));
         
         
         if (ModelState.IsValid) {
            _context.Add(appointement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
         }
         ViewData["AnimalFK"] = new SelectList(_context.Animals, "Id", "Id", appointement.AnimalFK);
         ViewData["VetFK"] = new SelectList(_context.Vets, "Id", "Name", appointement.VetFK);
         return View(appointement);
      }




      // GET: Appointements/Edit/5
      public async Task<IActionResult> Edit(int? id) {
         if (id == null) {
            return NotFound();
         }

         var appointement = await _context.Appointements.FindAsync(id);
         if (appointement == null) {
            return NotFound();
         }
         ViewData["AnimalFK"] = new SelectList(_context.Animals, "Id", "Id", appointement.AnimalFK);
         ViewData["VetFK"] = new SelectList(_context.Vets, "Id", "Name", appointement.VetFK);
         return View(appointement);
      }

      // POST: Appointements/Edit/5
      // To protect from overposting attacks, enable the specific properties you want to bind to.
      // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Obs,Price,AnimalFK,VetFK")] Appointement appointement) {
         if (id != appointement.Id) {
            return NotFound();
         }

         if (ModelState.IsValid) {
            try {
               _context.Update(appointement);
               await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
               if (!AppointementExists(appointement.Id)) {
                  return NotFound();
               }
               else {
                  throw;
               }
            }
            return RedirectToAction(nameof(Index));
         }
         ViewData["AnimalFK"] = new SelectList(_context.Animals, "Id", "Id", appointement.AnimalFK);
         ViewData["VetFK"] = new SelectList(_context.Vets, "Id", "Name", appointement.VetFK);
         return View(appointement);
      }

      // GET: Appointements/Delete/5
      public async Task<IActionResult> Delete(int? id) {
         if (id == null) {
            return NotFound();
         }

         var appointement = await _context.Appointements
             .Include(a => a.Animal)
             .Include(a => a.Vet)
             .FirstOrDefaultAsync(m => m.Id == id);
         if (appointement == null) {
            return NotFound();
         }

         return View(appointement);
      }

      // POST: Appointements/Delete/5
      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> DeleteConfirmed(int id) {
         var appointement = await _context.Appointements.FindAsync(id);
         _context.Appointements.Remove(appointement);
         await _context.SaveChangesAsync();
         return RedirectToAction(nameof(Index));
      }

      private bool AppointementExists(int id) {
         return _context.Appointements.Any(e => e.Id == id);
      }
   }
}
