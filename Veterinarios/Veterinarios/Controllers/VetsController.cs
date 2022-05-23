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

   /* Use of Roles
    * 
    * [Authorize(Roles = "Veterinary")]  --> only users that belongs to this
    *                                        role can access it
    * 
    * [Authorize(Roles = "Veterinary,Administrative")]  --> users that are 'veterinary'
    *                                                       OR  'administrative' can access it
    * 
    * [Authorize(Roles = "Veterinary")]
    * [Authorize(Roles = "Administrative")]  -->  users MUST have both roles
    *                                             veterinary AND administrative
    * 
    */


   [Authorize(Roles = "Veterinary,Administrative")]
   public class VetsController : Controller {


      /// <summary>
      /// this attribute refers the database of our project
      /// </summary>
      private readonly ApplicationDbContext _context;

      private readonly IWebHostEnvironment _webHostEnvironment;

      public VetsController(
         ApplicationDbContext context,
         IWebHostEnvironment webHostEnvironment) {
         // add value to attributes
         _context = context;
         _webHostEnvironment = webHostEnvironment;
      }




      // GET: Vets
      [AllowAnonymous]
      public async Task<IActionResult> Index() {
         /* execute the db command
          *    select *
          *    from Vets
          * 
          * and send data to View
          */
         return View(await _context.Vets.ToListAsync());
      }





      // GET: Vets/Details/5
      public async Task<IActionResult> Details(int? id) {
         if (id == null) {
            return NotFound();
         }

         var vet = await _context.Vets
             .FirstOrDefaultAsync(m => m.Id == id);
         if (vet == null) {
            return NotFound();
         }

         return View(vet);
      }





      // GET: Vets/Create
      /// <summary>
      /// opens the View 'Create' when it is used for the first time
      /// </summary>
      /// <returns></returns>
      public IActionResult Create() {
         return View();
      }




      // POST: Vets/Create
      // To protect from overposting attacks, enable the specific properties you want to bind to.
      // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

      /// <summary>
      /// uses data provided by browser, when a new vet is created
      /// </summary>
      /// <param name="vet"></param>
      /// <returns></returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Create([Bind("Id,Name,ProfessionalLicense,Photo")] Vet vet, IFormFile newPhotoVet) {
         /*
          * we must process the image...
          * 
          * if file is null
          *    add a pre-define image to vet
          * else
          *    if file is not an image
          *       send an error message to user, asking for an image
          *    else
          *       - define the name that the image must have
          *       - add the file name to vet data
          *       - save the file on the disk
          */

         if (newPhotoVet == null) {
            vet.Photo = "noVet.png";
         }
         else {
            if (!(newPhotoVet.ContentType == "image/jpeg" || newPhotoVet.ContentType == "image/png")) {
               // write the error message
               ModelState.AddModelError("", "Please, if you want to send a file, please choose an image...");
               // resend control to view, with data provided by user
               return View(vet);
            }
            else {
               // define image name
               Guid g;
               g = Guid.NewGuid();
               string imageName = vet.ProfessionalLicense + "_" + g.ToString();
               string extensionOfImage = Path.GetExtension(newPhotoVet.FileName).ToLower();
               imageName += extensionOfImage;
               // add image name to vet data
               vet.Photo = imageName;
            }
         }

         // validate if data provided by user is good...
         if (ModelState.IsValid) {

            try {
               // add vet data to database
               _context.Add(vet);
               // commit
               await _context.SaveChangesAsync();
            }
            catch (Exception) {
               // if the code arrives here, something wrong has appended
               // we must fix the error, or at least report it

               // add a model error to our code
               ModelState.AddModelError("", "Something went wrong. I can not store data on database");
               // eventually, before sending control to View
               // report error. For instance, write a message to the disc
               // or send an email to admin              

               // send control to View
               return View(vet);
            }
            // save image file to disk
            // ********************************
            if (newPhotoVet != null) {
               // ask the server what address it wants to use
               string addressToStoreFile = _webHostEnvironment.WebRootPath;
               string newImageLocalization = Path.Combine(addressToStoreFile, "Photos//Vets");
               // see if the folder 'Photos' exists
               if (!Directory.Exists(newImageLocalization)) {
                  Directory.CreateDirectory(newImageLocalization);
               }
               // save image file to disk
               newImageLocalization = Path.Combine(newImageLocalization, vet.Photo);
               using var stream = new FileStream(newImageLocalization, FileMode.Create);
               await newPhotoVet.CopyToAsync(stream);
            }

            return RedirectToAction(nameof(Index));
         }
         return View(vet);
      }





      // GET: Vets/Edit/5
      public async Task<IActionResult> Edit(int? id) {
         if (id == null) {
            return RedirectToAction("Index");
         }

         var vet = await _context.Vets.FindAsync(id);
         if (vet == null) {
            return RedirectToAction("Index");
         }

         // we are going to create a 'var session' to store the vet's ID
         // we will use this data to ensure that data does not have been changed by mistake
         HttpContext.Session.SetInt32("VetID", vet.Id);


         return View(vet);
      }




      // POST: Vets/Edit/5
      // To protect from overposting attacks, enable the specific properties you want to bind to.
      // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ProfessionalLicense,Photo")] Vet vet,
                                            IFormFile newPhotoVet) {
         if (id != vet.Id) {
            return NotFound();
         }

         /* before we start editing Vets's data, we need to ensure that data is good
          * so, we:
          *    - read the 'session variable'
          *    - compare it with the data that the browser provided
          *    - if it is different, we have a problem...
          */
         var vetsIDPreviouslyStored = HttpContext.Session.GetInt32("VetID");

         // if the vetsIDPreviouslyStored is null, this means:
         //    - we are accessing to app's method by external tools
         //    - we spend more time than allowed
         if (vetsIDPreviouslyStored == null) {
            // what we need to do?
            // we must decide...

            ModelState.AddModelError("", "You have spent more time than allowed...");
            return View(vet);
            // return RedirectToAction("Index");
         }

         if (vetsIDPreviouslyStored != vet.Id) {
            // if we enter here, something is wrong
            // what we need to do?????

            return RedirectToAction("Index");
         }





         /* if you do not have a new file, nothing is done
          * if a new photo is supplied, you should change it
          *    in this case, the new file takes the older's name?
          *        or, it will have a new one? in this case, 
          *        you need to delete the old photo
          * 
          * and, if the vet that has a photo, wants to stop using a photo?
          * ie. the vet wants to use the 'noVet.jpg' photo...
          *      on the view, ask the user's choice
          *      and, here, act to do what the user wants
          */




         if (ModelState.IsValid) {
            try {
               _context.Update(vet);
               await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
               if (!VetExists(vet.Id)) {
                  return NotFound();
               }
               else {
                  throw;
               }
            }
            return RedirectToAction(nameof(Index));
         }
         return View(vet);
      }

      // GET: Vets/Delete/5
      public async Task<IActionResult> Delete(int? id) {
         if (id == null) {
            return NotFound();
         }

         var vet = await _context.Vets
             .FirstOrDefaultAsync(m => m.Id == id);
         if (vet == null) {
            return NotFound();
         }

         return View(vet);
      }

      // POST: Vets/Delete/5
      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> DeleteConfirmed(int id) {

         var vet = await _context.Vets.FindAsync(id);

         try {
            _context.Vets.Remove(vet);
            await _context.SaveChangesAsync();
            /*
             * you must delete the user's photo
             * IF the user is not using the 'noVet.jpg'
             */
         }
         catch (Exception) {
            // what is going to be done in the 'catch' code?
            //  throw;
         }

         return RedirectToAction(nameof(Index));

      }




      private bool VetExists(int id) {
         return _context.Vets.Any(e => e.Id == id);
      }
   }
}
