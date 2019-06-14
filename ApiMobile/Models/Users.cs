using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ApiMobile.Models
{
    public class Users
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Display(Name = "Email")]
        [Required]
        public string Email { get; set; }
        [StringLength(50, ErrorMessage = "Ne peux dépasser 50 caractères.")]
        [Display(Name = "Prénom")]
        [Required]
        public string Lastname { get; set; }
        [StringLength(50, ErrorMessage = "Ne peux dépasser 50 caractères.")]
        [Display(Name = "Nom")]
        [Required]
        public string Firstname { get; set; }
        [StringLength(100, ErrorMessage = "Ne peux dépasser 100 caractères.")]
        [Display(Name = "Lieu de naissance")]
        public string Place_of_birth { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date de naissance")]
        public DateTime Date_of_birth { get; set; }
        [StringLength(150, ErrorMessage = "Ne peux dépasser 150 caractères.")]
        [Display(Name = "Adresse")]
        public string Address { get; set; }
        [Display(Name = "Numéro de téléphone")]
        public int Phone { get; set; }
        [Display(Name = "Mots de passe")]
        public string Password { get; set; }
        public ICollection<Contacts> Contact { get; set; }
    }
}
