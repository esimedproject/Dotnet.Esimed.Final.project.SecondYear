using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

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
        public ICollection<Users> UsersSubscribe { get; set; }
        public int? SubscribesPaymentID { get; set; }
        public int? SubscribesMagazineID { get; set; }
        public Payments PaymentsSubscribes { get; set; }
        public Magazines MagazinesSubscribes { get; set; }


    }
}
