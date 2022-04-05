using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Veterinarios.Models;

namespace Veterinarios.Data {
    /// <summary>
    /// thos class conects outcode with database
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) {
        }

        // define table on the database
        public DbSet<Animal> Animals { get; set; }
        public DbSet<Vet> Vets { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Owner> Owners { get; set; }
    }
}