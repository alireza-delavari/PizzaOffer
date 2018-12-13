using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaOffer.DataLayer.Context
{
    public class ApplicationDbContext : DbContext , IUnitOfWork
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // it should be placed here, otherwise it will rewrite the following settings!
            base.OnModelCreating(modelBuilder);
        }
    }
}
