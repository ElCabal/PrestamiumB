﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Prestamium.Entities;
using System.Reflection;

namespace Prestamium.Persistence
{
    public class ApplicationDbContext : IdentityUserContext<User>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        //Fluent Api

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            //modelBuilder.Entity<Client>().Property(x => x.Name).HasMaxLength(50);
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable(name: "User");
            });

        }
    }
}
