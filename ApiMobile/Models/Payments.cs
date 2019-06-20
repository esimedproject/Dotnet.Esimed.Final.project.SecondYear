using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel; 

namespace ApiMobile.Models
{
    public class Payments
    {
        [Key]
        public int Id { get; set; }
        [DefaultValue(0)]
        [DataType(DataType.Currency)]
        [Display(Name = "Prix")]
        [Required]
        public double PaymentAmount { get; set; }
        [Display(Name = "Status")]
        [Required]
        public bool Status { get; set; }
        [Display(Name = "Moyen de paiement")]
        [DefaultValue("Card")]
        public string Means_of_payment { get; set; }
        public ICollection<Subscribes> SubscribesPayment { get; set; }
    }
}
