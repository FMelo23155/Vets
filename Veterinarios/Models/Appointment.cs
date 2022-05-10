﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Veterinarios.Models
{
    public class Appointment {

        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public string Obs { get; set; }

        /// <summary>
        /// auxiliary attribute to help the app to collect the appointement's price 
        /// </summary>
        [NotMapped] //this anottion tells the EF that this attribute does not
        [Required]
        [RegularExpression("[0-9]{1,8}[,.]?[0-9]{0,2}", ErrorMessage = "YOU must write the price of appointments.")]
        [Display(Name = "Price")]
        public string AuxPrice { get; set; }

        public decimal Price { get; set; }

        [ForeignKey(nameof(Animal))]
        public int AnimalFK { get; set; }
        public Animal Animal { get; set; }

        [ForeignKey(nameof(Vet))]
        public int VetFK { get; set; }
        public Vet Vet { get; set; }
    }
}
