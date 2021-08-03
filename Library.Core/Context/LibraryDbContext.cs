using System;
using Library.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Library.Core.Context
{
    public class LibraryDbContext : IdentityDbContext<LibraryUser>
    {

        public LibraryDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Book> Book { get; set; }
        public DbSet<BookRezervation> BookRezervation { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.HasAnnotation("Relational:Collation", "Turkish_CI_AS");



            builder.Entity<LibraryUser>().ToTable("LIBRARY_API_USERS"); // Your custom IdentityUser class
            builder.Entity<IdentityUserLogin<string>>().ToTable("LIBRARY_API_USER_LOGINS");
            builder.Entity<IdentityUserToken<string>>().ToTable("LIBRARY_API_USER_TOKENS");
            builder.Entity<IdentityUserClaim<string>>().ToTable("LIBRARY_API_USER_CLAIMS");
            builder.Entity<IdentityUserRole<string>>().ToTable("LIBRARY_API_USER_ROLES");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("LIBRARY_API_ROLE_CLAIMS");
            builder.Entity<IdentityRole>().ToTable("LIBRARY_API_ROLES");


            builder.Entity<Book>().ToTable("LIBRARY_API_BOOK");
            builder.Entity<BookRezervation>().ToTable("LIBRARY_API_BOOK_REZERVATION");



        }




    }
}
