using System.ComponentModel.DataAnnotations;

namespace Veterinarios.Models {
    /// <summary>
    /// data from Vets
    /// </summary>
    public class Vet {

        public Vet()
        {
            Appointment = new HashSet<Appointment>();
        }

        /// <summary>
        /// PK for vets
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of vet
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Name of file that has the vet photo
        /// </summary>
        public string Photo { get; set; }

        /// <summary>
        /// Professional license of vet
        /// </summary>
        [Display(Name = "Professional License")]
        public string ProfessionalLicense { get; set; }

        public ICollection<Appointment> Appointment { get; set; }
    }
}
