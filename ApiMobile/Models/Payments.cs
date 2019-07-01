using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiMobile.Models
{
    public class Payments
    {
        [Key]
        public int CId { get; set; }
        [DefaultValue(0)]
        [DataType(DataType.Currency)]
        [Display(Name = "Prix")]
        [Required]
        public double PaymentAmount { get; set; }  
        [NotMapped]
        [Display(Name = "cardid")]
        public long cardid { get; set; }
        [NotMapped]
        [Display(Name = "cardmonth")]
        public string cardmonth { get; set; }
        [NotMapped]
        [Display(Name = "cardyear")]
        public string cardyear { get; set; }
        [Display(Name = "transaction")]
        public long transaction { get; set; }
        [Display(Name = "Moyen de paiement")]
        [DefaultValue("cardpay")]
        public string MeansOfPayment { get; set; }
        public int? SubscribesPaymentID { get; set; }
        public Subscribes subscribe { get; set; }
    }
}
