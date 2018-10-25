using ErrorTest.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ErrorTest
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<User> Users { get; set; }

        public DbSet<UserClaims> Claims { get; set; }

        public ApplicationDbContext( DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().OwnsOne(x => x.Name, fullName =>
            {
                fullName.OwnsOne(x => x.FirstName, firstName =>
                {
                    firstName.Property(p => p.FirstNamePart).HasColumnName("FirstName_FirstPart").HasMaxLength(255)
                        .IsRequired();
                    firstName.Property(p => p.NameSeperator).HasColumnName("FirstName_NameSeperator").HasMaxLength(5);
                    firstName.Property(p => p.LastNamePart).HasColumnName("FirstName_LastPart").HasMaxLength(255);
                });
                fullName.OwnsOne(x => x.LastName, lastName =>
                {
                    lastName.Property(p => p.FirstNamePart).HasColumnName("LastName_FirstPart").HasMaxLength(255)
                        .IsRequired();
                    lastName.Property(p => p.NameSeperator).HasColumnName("LastName_NameSeperator").HasMaxLength(5);
                    lastName.Property(p => p.LastNamePart).HasColumnName("LastName_LastPart").HasMaxLength(255);
                });
            });
        }

    }
}