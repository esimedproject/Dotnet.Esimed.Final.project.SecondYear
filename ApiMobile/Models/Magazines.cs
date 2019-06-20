﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ApiMobile.Models
{
    public class Magazines
    {
        [Key]
        public int Id { get; set; }
        [DefaultValue(0)]
        [Display(Name = "Nombre de numéros")]        
        public int Nb_of_realease { get; set; }
        [Display(Name = "Nom")]
        public string Nom { get; set; }
        [Display(Name = "Description")]        
        public string Description { get; set; }
        [DefaultValue(0)]
        [DataType(DataType.Currency)]
        [Display(Name = "Prix")]
        [Required]
        public double Price { get; set; }
        [Display(Name = "Image")]
        [DefaultValue("/")]
        public string WallpagePATH { get; set; }

        public ICollection<Subscribes> SubscribeMagazine { get; set; }
    }
}
