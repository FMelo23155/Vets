namespace Veterinarios.Models
{
    public class Vet {

        public Vet()
        {
            Appointment = new HashSet<Appointment>();
        }


        public int Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public string ProfessionalLicense { get; set; }

        public ICollection<Appointment> Appointment { get; set; }
    }
}
