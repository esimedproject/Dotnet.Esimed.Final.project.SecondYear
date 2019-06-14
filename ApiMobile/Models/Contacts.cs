using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ApiMobile.Models
{
    public class Contacts
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50, ErrorMessage = "Ne peux dépasser 50 caractères.")]
        [Display(Name = "Sujet")]
        [Required]
        public int Subject { get; set; }
        [Display(Name = "Commentaires")]
        public string Comment { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "date")]
        [Required]
        public DateTime Date { get; set; }
        [Display(Name = "Moyen de paiement")]
        public string Means_of_payment { get; set; }
        public int UserContactID { get; set; }
        public Users Users { get; set; }
    }
}
