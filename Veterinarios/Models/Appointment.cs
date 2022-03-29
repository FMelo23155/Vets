using System.ComponentModel.DataAnnotations.Schema;

namespace Veterinarios.Models
{
    public class Appointment {

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Obs { get; set; }
        public decimal Price { get; set; }

        [ForeignKey(nameof(Animal))]
        public int AnimalFK { get; set; }
        public Animal Animal { get; set; }

        [ForeignKey(nameof(Vet))]
        public int VetFK { get; set; }
        public Vet Vet { get; set; }
    }
}
