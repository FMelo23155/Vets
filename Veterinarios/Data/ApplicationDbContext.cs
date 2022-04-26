using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Veterinarios.Models;

namespace Veterinarios.Data {
    /// <summary>
    /// this class connects outcode with database
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) {
        }


        /// <summary>
        /// it executes code before the creation of model
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            // imports the previous execution of this method
            base.OnModelCreating(modelBuilder);

            //******************************************
            // add, at this point, your new code


            // create the seed of your tables
            modelBuilder.Entity<Vet>().HasData (
                new Vet { Id = 1, Name = "´José Silva", ProfessionalLicense = "vet-8252", Photo = "jose.jpg" },
                new Vet { Id = 2, Name = "Maria Gomes", ProfessionalLicense = "vet-4143", Photo = "maria.jpg" },
                new Vet { Id = 3, Name = "Ricardo", ProfessionalLicense = "vet-6240", Photo = "ricardo.jpg" }
            );

        }

        // define table on the database
        public DbSet<Animal> Animals { get; set; }
        public DbSet<Vet> Vets { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Owner> Owners { get; set; }
    }
}