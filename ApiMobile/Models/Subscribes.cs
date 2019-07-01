using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ApiMobile.Models
{
    public class Subscribes
    {
        [Key]
        public int Id { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime? Start_date_subscribe { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime? End_date_subscribe { get; set; }
        [Display(Name = "Status")]
        [Required]
        [DefaultValue(false)]
        public bool Status { get; set; }
        public int? UserSubscribeID { get; set; }
        public Users user { get; set; }
        public ICollection<Payments> SubscribesPayment { get; set; }
        public ICollection<Magazines> SubscribeMagazine { get; set; }
    }
}
