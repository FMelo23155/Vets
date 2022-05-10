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

            modelBuilder.Entity<Owner>().HasData(
               new Owner { Id = 1, Name = "Luís Freitas", Sex = "M", NIF = "813635582" },
               new Owner { Id = 2, Name = "Andreia Gomes", Sex = "F", NIF = "854613462" },
               new Owner { Id = 3, Name = "Cristina Sousa", Sex = "F", NIF = "265368715" },
               new Owner { Id = 4, Name = "Sónia Rosa", Sex = "F", NIF = "835623190" }
            );

            modelBuilder.Entity<Animal>().HasData(
               new Animal { Id = 1, Name = "Bubi", Species = "Cão", Breed = "Pastor Alemão", Weight = 24, Photo = "animal1.jpg", OwnerFK = 1 },
               new Animal { Id = 2, Name = "Pastor", Species = "Cão", Breed = "Serra Estrela", Weight = 50, Photo = "animal2.jpg", OwnerFK = 3 },
               new Animal { Id = 3, Name = "Tripé", Species = "Cão", Breed = "Serra Estrela", Weight = 4, Photo = "animal3.jpg", OwnerFK = 2 },
               new Animal { Id = 4, Name = "Saltador", Species = "Cavalo", Breed = "Lusitana", Weight = 580, Photo = "animal4.jpg", OwnerFK = 3 },
               new Animal { Id = 5, Name = "Tareco", Species = "Gato", Breed = "siamês", Weight = 1, Photo = "animal5.jpg", OwnerFK = 3 },
               new Animal { Id = 6, Name = "Cusca", Species = "Cão", Breed = "Labrador", Weight = 45, Photo = "animal6.jpg", OwnerFK = 2 },
               new Animal { Id = 7, Name = "Morde Tudo", Species = "Cão", Breed = "Dobermann", Weight = 39, Photo = "animal7.jpg", OwnerFK = 4 },
               new Animal { Id = 8, Name = "Forte", Species = "Cão", Breed = "Rottweiler", Weight = 20, Photo = "animal8.jpg", OwnerFK = 2 },
               new Animal { Id = 9, Name = "Castanho", Species = "Vaca", Breed = "Mirandesa", Weight = 652, Photo = "animal9.jpg", OwnerFK = 3 },
               new Animal { Id = 10, Name = "Saltitão", Species = "Gato", Breed = "Persa", Weight = 2, Photo = "animal10.jpg", OwnerFK = 1 }
            );


        }

        // define table on the database
        public DbSet<Animal> Animals { get; set; }
        public DbSet<Vet> Vets { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Owner> Owners { get; set; }
    }
}