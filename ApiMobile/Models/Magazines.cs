using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ApiMobile.Models
{
    public class Magazines
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Nombre de numéros")]        
        public int Nb_of_realease { get; set; }
        [Display(Name = "Description")]        
        public string Description { get; set; }
        [DataType(DataType.Currency)]
        [Display(Name = "Prix")]
        [Required]
        public int Price { get; set; }
        [Display(Name = "Image")]        
        public string WallpageURL { get; set; }
        public int SubscribesID { get; set; }
        public Subscribes Subscribes { get; set; }
    }
}
