using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Veterinarios.Data;
using Veterinarios.Models;

namespace Veterinarios.Controllers
{
    public class VetsController : Controller {

        /// <summary>
        /// this atribute refers the database
        /// </summary>

        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public VetsController(
            ApplicationDbContext context,
            IWebHostEnvironment _webHostEnvironment) {
            _context = context;
            this._webHostEnvironment = _webHostEnvironment;
        }

        // GET: Vets
        public async Task<IActionResult> Index() {
            /* execute the db command
             *      select *
             *      from vets
             * and send data to View
             */

            return View(await _context.Vets.ToListAsync());
        }

        // GET: Vets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vet = await _context.Vets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vet == null)
            {
                return NotFound();
            }

            return View(vet);
        }

        // GET: Vets/Create
        /// <summary>
        /// opens the View 'Create' when it is use for the first time
        /// </summary>
        /// <returns></returns>
        public IActionResult Create() {
            return View();
        }

        // POST: Vets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// uses data provided by Browser, when a new vet is created
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
             *      add a pref-define image to vet
             * else
             *      if file is not an image
             *          send an error message to user, asking for an image
             *      else
             *          - define the name that the image must have
             *          - add the file name to vet data
             *          - save the file on the disk
             */

            if(newPhotoVet == null) {
                vet.Photo = "noVet.png";
            } else {
                if(!(newPhotoVet.ContentType == "image/jpeg" || newPhotoVet.ContentType == "image/png")) {
                    //write the error message
                    ModelState.AddModelError("","Please, if you want to send a file, please choose an image...");
                    //resend control to view, with data provided by user
                    return View(vet);
                } else {
                    //define image name
                    Guid g;
                    g=Guid.NewGuid();
                    string imageName=vet.ProfessionalLicense+"_"+g.ToString(); //gera o nome da imagem
                    string extensionOfImage = Path.GetExtension(newPhotoVet.FileName).ToLower();
                    imageName += extensionOfImage;
                    //add image name to vet data
                    vet.Photo = imageName;
                }
            }

            // validate if data provided by user is good...
            if (ModelState.IsValid) {
                //add vet data to database
                _context.Add(vet);
                // commit
                await _context.SaveChangesAsync();

                //save image file to disk
                //***************************************
                //ask the server what address it wants to use
                string addressToStoreFile = _webHostEnvironment.WebRootPath;
                string newImageLocalization = Path.Combine(addressToStoreFile, "Photos", vet.Photo);
                // save image file to disk
                using var stream = new FileStream(newImageLocalization, FileMode.Create);
                await newPhotoVet.CopyToAsync(stream);


                return RedirectToAction(nameof(Index));
            }
            return View(vet);
        }

        // GET: Vets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vet = await _context.Vets.FindAsync(id);
            if (vet == null)
            {
                return NotFound();
            }
            return View(vet);
        }

        // POST: Vets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Photo,ProfessionalLicense")] Vet vet)
        {
            if (id != vet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VetExists(vet.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vet);
        }

        // GET: Vets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vet = await _context.Vets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vet == null)
            {
                return NotFound();
            }

            return View(vet);
        }

        // POST: Vets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vet = await _context.Vets.FindAsync(id);
            _context.Vets.Remove(vet);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VetExists(int id)
        {
            return _context.Vets.Any(e => e.Id == id);
        }
    }
}
