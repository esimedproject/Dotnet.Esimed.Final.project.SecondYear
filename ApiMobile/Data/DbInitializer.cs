using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ApiMobile.Models; 

namespace ApiMobile.Data
{
    public static class DbInitializer
    {

        public static void Seed(this Context _context)
        {
            _context.Database.EnsureCreated();

            if (_context.User.Any())
            {
                return;
            }

            var Admin = new Admins[]
            {
                new Admins{
                    Email ="loic.wernert@outlook.fr",
                    Password = Salt.GetPasswordHash("secret2")},
                new Admins{
                    Email ="loic.wernert@gmail.fr",
                    Password =Salt.GetPasswordHash("secret1")}
            };
            foreach (Admins s in Admin)
            {
                _context.Admin.Add(s);
            }
            _context.SaveChanges();

            var Payment = new Payments[]
            {
                new Payments{
                    PaymentAmount = 100,
                    MeansOfPayment ="cardpay"},
                new Payments{
                    PaymentAmount = 50,
                    MeansOfPayment ="cardpay"}
            };
            foreach (Payments s in Payment)
            {
                _context.Payment.Add(s);
            }
            _context.SaveChanges();           

            var Contact = new Contacts[]
            {
                new Contacts {
                    Subject ="bug d'interface mauvaise cover de journal",
                    Comment ="remplassement de la photo par la bonne fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff",
                    Date =DateTime.Parse("05-01-2015") },
                new Contacts {
                    Subject ="erreur mauvais abonement",
                    Comment ="suppression du mauvais ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff",
                    Date =DateTime.Parse("16-06-2015") }
            };
            foreach(Contacts s in Contact)
            {
                _context.Contact.Add(s);
            }
            _context.SaveChanges();

            var Magazine = new Magazines[]
            {
                new Magazines{
                    Nb_of_realease =250,
                    Nom ="l'équipe",
                    Description ="journal de sport",
                    Price =18,
                    WallpagePATH ="https://www.lequipe.fr/explore/lf26-70-ans-d-histoire/img/JOURNAL_LARGE/chap2000-journal-2003-couleur.jpg"},
                new Magazines{
                    Nb_of_realease =150,
                    Nom ="le monde",
                    Description ="journal politique",
                    Price =25,
                    WallpagePATH ="https://s1.lemde.fr/journalelectronique/vignettes/la_une/20180105/QUO_232_coupee.jpg"}
            };
            foreach (Magazines s in Magazine)
            {
                _context.Magazine.Add(s);
            }
            _context.SaveChanges();

            var Subscribe = new Subscribes[]
{
                new Subscribes{
                    Start_date_subscribe =DateTime.Parse("12-05-2017"),
                    End_date_subscribe =DateTime.Parse("12-05-2018")},
                new Subscribes{
                    Start_date_subscribe =DateTime.Parse("06-11-2015"),
                    End_date_subscribe =DateTime.Parse("06-11-2016")}};
            foreach (Subscribes d in Subscribe)
            {
                _context.Subscribe.Add(d);
            }
            _context.SaveChanges();

            var User = new Users[]
            {
                new Users {
                    Email = "alexandre.astier@outlook.fr",
                    Lastname = "Alexandre",
                    Firstname ="Astier",
                    Place_of_birth ="Paris 12 ème",
                    Date_of_birth = DateTime.Parse("09-01-2010"),
                    Address ="Paris 8ème",
                    Phone = 0788161749,
                    Password =Salt.GetPasswordHash("password")},
                new Users {
                    Email = "alexandra.lastierier@outlook.fr",
                    Lastname = "Alexandra",
                    Firstname ="Astierier",
                    Place_of_birth ="Paris 13 ème",
                    Date_of_birth = DateTime.Parse("10-02-2011"),
                    Address ="Paris 7ème",
                    Phone = 0788161749,
                    Password =Salt.GetPasswordHash("password2")}
            };
            foreach (Users s in User)
            {
                _context.User.Add(s);
            }
            _context.SaveChanges();
        }
    }
}
