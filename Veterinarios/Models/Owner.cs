namespace Veterinarios.Models
{
    public class Owner {
        //construtores
        public Owner() {
            Animals = new HashSet<Animal>();
        }


        /* Declaração dos atributos */
        public int Id { get; set; }
        public string Name { get; set; }
        public string NIF { get; set; }
        public string Sex { get; set; }

        public ICollection<Animal> Animals { get; set; }

    }
}
