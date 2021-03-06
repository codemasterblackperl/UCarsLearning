﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UAutoServiceLearn.Models;

namespace UAutoServiceLearn.Data
{
    
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ServiceType> ServiceTypes { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Service> Services { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(entity => entity.Property(p => p.Id).HasMaxLength(128));
            builder.Entity<ApplicationUser>(entity => entity.Property(p => p.NormalizedEmail).HasMaxLength(128));
            builder.Entity<ApplicationUser>(entity => entity.Property(p => p.NormalizedUserName).HasMaxLength(128));

            builder.Entity<IdentityRole>(entity => entity.Property(p => p.Id).HasMaxLength(128));
            builder.Entity<IdentityRole>(entity => entity.Property(p => p.NormalizedName).HasMaxLength(128));

            builder.Entity<IdentityUserToken<string>>(entity => entity.Property(p => p.LoginProvider).HasMaxLength(128));
            builder.Entity<IdentityUserToken<string>>(entity => entity.Property(p => p.UserId).HasMaxLength(128));
            builder.Entity<IdentityUserToken<string>>(entity => entity.Property(p => p.Name).HasMaxLength(128));

            builder.Entity<IdentityUserRole<string>>(entity => entity.Property(p => p.UserId).HasMaxLength(128));
            builder.Entity<IdentityUserRole<string>>(entity => entity.Property(p => p.RoleId).HasMaxLength(128));


            builder.Entity<IdentityUserLogin<string>>(entity => entity.Property(p => p.LoginProvider).HasMaxLength(128));
            builder.Entity<IdentityUserLogin<string>>(entity => entity.Property(p => p.ProviderKey).HasMaxLength(128));
            builder.Entity<IdentityUserLogin<string>>(entity => entity.Property(p => p.UserId).HasMaxLength(128));

            builder.Entity<IdentityUserClaim<string>>(entity => entity.Property(p => p.Id).HasMaxLength(128));
            builder.Entity<IdentityUserClaim<string>>(entity => entity.Property(p => p.UserId).HasMaxLength(128));

            builder.Entity<IdentityRoleClaim<string>>(entity => entity.Property(p => p.Id).HasMaxLength(128));
            builder.Entity<IdentityRoleClaim<string>>(entity => entity.Property(p => p.RoleId).HasMaxLength(128));
        }
    }
}
