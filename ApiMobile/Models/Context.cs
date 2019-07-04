using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ApiMobile.Models;

namespace ApiMobile.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        public DbSet<Users> User { get; set; }
        public DbSet<Magazines> Magazine { get; set; }
        public DbSet<Contacts> Contact { get; set; }
        public DbSet<Subscribes> Subscribe { get; set; }
        public DbSet<Payments> Payment { get; set; }
        public DbSet<Admins> Admin { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admins>().ToTable("Admins");
            modelBuilder.Entity<Contacts>().ToTable("Contacts");
            modelBuilder.Entity<Magazines>().ToTable("Magazines");
            modelBuilder.Entity<Payments>().ToTable("Payments");
            modelBuilder.Entity<Subscribes>().ToTable("Subscribes");
            modelBuilder.Entity<Users>().ToTable("Users");

            modelBuilder.Entity<Users>()
                .HasMany<Subscribes>(j => j.UsersSubscribe)
                .WithOne(h => h.user)
                .HasForeignKey(x => x.UserSubscribeID);

            modelBuilder.Entity<Users>()
                .HasMany<Contacts>(u => u.UsersContact)
                .WithOne(c => c.user)
                .HasForeignKey(s => s.UserContactID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Subscribes>()
                .HasMany<Payments>(s => s.SubscribesPayment)
                .WithOne(g => g.subscribe)
                .HasForeignKey(s => s.SubscribesPaymentID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Subscribes>()
                .HasMany<Magazines>(m=>m.SubscribeMagazine)
                .WithOne(p=>p.Subscribe)
                .HasForeignKey(s => s.SubscribesMagazineID);
        }       
    }
}
