using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ApiMobile.Models
{
    public class Users
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Email")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [StringLength(50, ErrorMessage = "Ne peux dépasser 50 caractères.")]
        [Display(Name = "Prénom")]
        public string Lastname { get; set; }
        [StringLength(50, ErrorMessage = "Ne peux dépasser 50 caractères.")]
        [Display(Name = "Nom")]
        public string Firstname { get; set; }
        [StringLength(100, ErrorMessage = "Ne peux dépasser 100 caractères.")]
        [Display(Name = "Lieu de naissance")]
        public string Place_of_birth { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date de naissance")]
        public DateTime? Date_of_birth { get; set; }
        [StringLength(150, ErrorMessage = "Ne peux dépasser 150 caractères.")]
        [Display(Name = "Adresse")]
        public string Address { get; set; }
        [Display(Name = "Numéro de téléphone")]
        [Phone]
        public int? Phone { get; set; }
        [Display(Name = "Mots de passe")]
        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }
        [Display(Name = "clé d'authentification")]
        public string AuthentificationKey { get; set; }
        public ICollection<Contacts> UsersContact { get; set; }
        public ICollection<Subscribes> UsersSubscribe { get; set; }
    }
}
