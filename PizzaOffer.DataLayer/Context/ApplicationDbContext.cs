using Microsoft.EntityFrameworkCore;
using PizzaOffer.Common.Utilities;
using PizzaOffer.DomainClasses;
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

            var entitiesAssembly = typeof(IBaseEntity).Assembly;
            
            modelBuilder.RegisterAllEntities<IBaseEntity>(entitiesAssembly);
            modelBuilder.RegisterEntityTypeConfiguration(entitiesAssembly);
            modelBuilder.AddRestrictDeleteBehaviorConvention();
            modelBuilder.AddSequentialGuidForIdConvention();
            modelBuilder.AddPluralizingTableNameConvention();
            //Todo: override savechanges methodes and clean texts
        }
    }
}
