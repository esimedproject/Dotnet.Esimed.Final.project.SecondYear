using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ApiMobile.Models
{
    public class Payments
    {
        [Key]
        public int Id { get; set; }
        [DataType(DataType.Currency)]
        [Display(Name = "Prix")]
        [Required]
        public int Payment { get; set; }
        [Display(Name = "Status")]
        [Required]
        public bool Status { get; set; }
        public int SubscribeID { get; set; }
        public Subscribes Subscribe { get; set; }
    }
}
