using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ApiMobile.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admins>().ToTable("Admins");
            modelBuilder.Entity<Contacts>().ToTable("Contacts");
            modelBuilder.Entity<Magazines>().ToTable("Magazines");
            modelBuilder.Entity<Payments>().ToTable("Payments");
            modelBuilder.Entity<Subscribes>().ToTable("Subscribes");
            modelBuilder.Entity<Users>().ToTable("Users");
            modelBuilder.Entity<Contacts>()
                .HasOne<Users>(u => u.Users)
                .WithMany(c => c.Contact)
                .HasForeignKey(s => s.UserContactID);
            modelBuilder.Entity<Payments>()
                .HasOne<Subscribes>(s => s.Subscribe)
                .WithMany(g => g.Payment)
                .HasForeignKey(s => s.SubscribeID);
            modelBuilder.Entity<Magazines>()
                .HasOne<Subscribes>(m=>m.Subscribes)
                .WithMany(p=>p.Magazine)
                .HasForeignKey(s => s.SubscribesID);
        }

        public DbSet<Users> User { get; set; }
        public DbSet<Magazines> Magazine { get; set; }
        public DbSet<Contacts> Contact { get; set; }
        public DbSet<Subscribes> Subscribe { get; set; }
        public DbSet<Payments> Payment { get; set; }
        public DbSet<Admins> Admin { get; set; }
    }
}
