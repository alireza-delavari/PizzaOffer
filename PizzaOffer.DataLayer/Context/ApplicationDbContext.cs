using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PizzaOffer.Common.Utilities;
using PizzaOffer.DomainClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PizzaOffer.DataLayer.Context
{
    public class ApplicationDbContext : DbContext, IUnitOfWork
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        { }

        public virtual DbSet<User> Users { set; get; }
        public virtual DbSet<Role> Roles { set; get; }
        public virtual DbSet<UserRole> UserRoles { set; get; }
        public virtual DbSet<UserToken> UserTokens { set; get; }
        public virtual DbSet<Order> Orders { set; get; }
        public virtual DbSet<OrderDetail> OrderDetails { set; get; }
        public virtual DbSet<Food> Foods { set; get; }
        public virtual DbSet<FoodCategory> FoodCategories { set; get; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // it should be placed here, otherwise it will rewrite the following settings!
            base.OnModelCreating(modelBuilder);

            var entitiesAssembly = typeof(IBaseEntity).Assembly;
            
            //modelBuilder.RegisterAllEntities<IBaseEntity>(entitiesAssembly);
            modelBuilder.RegisterEntityTypeConfiguration(entitiesAssembly);
            modelBuilder.AddRestrictDeleteBehaviorConvention();
            //modelBuilder.AddSequentialGuidForIdConvention();
            //modelBuilder.AddPluralizingTableNameConvention();
            ApplyBaseClassConfiguration(modelBuilder);
            //Todo: override savechanges methodes and clean texts
        }

        private void ApplyBaseClassConfiguration(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                .Where(c => c.ClrType.IsClass && !c.ClrType.IsAbstract && c.ClrType.IsPublic && typeof(IBaseEntity).IsAssignableFrom(c.ClrType)))
            {
                if (Database.IsSqlServer())
                {
                    modelBuilder
                        .Entity(entityType.ClrType)
                        .Property(nameof(BaseEntity.CreatedDate)).ValueGeneratedOnAdd().HasDefaultValueSql("SYSDATETIMEOFFSET()");
                    modelBuilder
                        .Entity(entityType.ClrType)
                        .Property(nameof(BaseEntity.UpdatedDate)).ValueGeneratedOnAddOrUpdate().HasDefaultValueSql("SYSDATETIMEOFFSET()");
                }
                else if (Database.IsNpgsql())
                {
                    modelBuilder
                        .Entity(entityType.ClrType)
                        .Property(nameof(BaseEntity.CreatedDate)).ValueGeneratedOnAdd().HasDefaultValueSql("now()");
                    modelBuilder
                        .Entity(entityType.ClrType)
                        .Property(nameof(BaseEntity.UpdatedDate)).ValueGeneratedOnAddOrUpdate().HasDefaultValueSql("now()");
                }
            }
        }
    }
}
