﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace ApiMobile.Models
{
    public class Admins
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50, ErrorMessage = "Ne peux dépasser 50 caractères.")]
        [Display(Name = "Email")]
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Mots de passe")]
        [PasswordPropertyText]
        public string Password { get; set; }
        [Display(Name = "clé d'authentification")]
        public string AuthentificationKey { get; set; }
    }
}
