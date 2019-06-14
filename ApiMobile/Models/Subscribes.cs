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
        public DateTime Start_date_subscribe { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime End_date_subscribe { get; set; }       
        public int MagazinesID { get; set; }    
        public int UsersID { get; set; }
        
        public Magazines Magazines { get; set; }
        public Users Users { get; set; }

        public ICollection<Magazines> Magazine { get; set; }
        public ICollection<Payments> Payment { get; set; }
    }
}
