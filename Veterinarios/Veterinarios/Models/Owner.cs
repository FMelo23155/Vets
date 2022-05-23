using System.ComponentModel.DataAnnotations;

namespace Veterinarios.Models {

   /// <summary>
   /// describes the owner's data
   /// </summary>
   public class Owner {

      public Owner() {
         Animals = new HashSet<Animal>();
      }


      /// <summary>
      /// PK for owners table
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// name of owner's animal
      /// </summary>
      [Required(ErrorMessage = "O {0} é de preenchimento obrigatório.")]
      [StringLength(30, ErrorMessage = "O {0} não pode ter mais do que {1} carateres.")]
      [Display(Name = "Nome")]
      [RegularExpression("[A-ZÂÓÍa-záéíóúàèìòùâêîôûãõäëïöüñç '-]+", ErrorMessage = "Só pode escrever letras no {0}")]
      public string Name { get; set; }

      /// <summary>
      /// vat number
      /// </summary>
      [Required(ErrorMessage = "O {0} é de preenchimento obrigatório.")]
      [StringLength(9, MinimumLength = 9, ErrorMessage = "O {0} tem de ter 9 carateres.")]
      [RegularExpression("[123578][0-9]{8}", ErrorMessage = "O {0} deve começar por 1,2,3,5,7,8 seguido de 8 digitos numéricos.")]
      public string NIF { get; set; }

      /// <summary>
      /// owner's sex 
      /// Ff-female; Mm-male
      /// </summary>
      [StringLength(1, ErrorMessage = "O {0} não pode ter mais do que 1 caráter.")]
      [Display(Name = "Sexo")]
      [RegularExpression("[MmFf]", ErrorMessage = "Só pode usar F, ou M, no campo {0}")]
      public string Sex { get; set; }

      /// <summary>
      /// Email
      /// </summary>
      [EmailAddress(ErrorMessage = "Escreva um {0} válido, pf.")]
      public string Email { get; set; }


      /// <summary>
      /// set of owner's animals
      /// </summary>
      public ICollection<Animal> Animals { get; set; }

      //########################################################################
      /// <summary>
      /// this FK is used to connect our 'business data' to 'authetication DB'
      /// </summary>
      public string UserID { get; set; }
      //########################################################################

   }
}
