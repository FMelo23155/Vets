using System.ComponentModel.DataAnnotations;

namespace Veterinarios.Models {
   /// <summary>
   /// data from Vets
   /// </summary>
   public class Vet {

      public Vet() {
         Appointements = new HashSet<Appointement>();
      }

      /// <summary>
      /// PK for Vets
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Name of vet
      /// </summary>
      [Required]
      public string Name { get; set; }

      /// <summary>
      /// professional license of vet
      /// </summary>
      [Display(Name = "Professional License")]
      [Required]
      public string ProfessionalLicense { get; set; }

      /// <summary>
      /// name of file that has the Vet photo
      /// </summary>
      public string Photo { get; set; }


      public ICollection<Appointement> Appointements { get; set; }
   }
}
