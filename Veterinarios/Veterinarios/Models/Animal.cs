using System.ComponentModel.DataAnnotations.Schema;

namespace Veterinarios.Models {
   public class Animal {

      public Animal() {
         Appointements = new HashSet<Appointement>();
      }

      public int Id { get; set; }

      public string Name { get; set; }

      public string Breed { get; set; } // Raça

      public string Species { get; set; } // espécie

      public DateTime BirthDate { get; set; }

      public double Weight { get; set; }

      public string Photo { get; set; }


      [ForeignKey(nameof(Owner))]  //  [ForeignKey("Owner")]
      public int OwnerFK { get; set; }
      public Owner Owner { get; set; }


      public ICollection<Appointement> Appointements { get; set; }
   }
}
