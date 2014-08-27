using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Finances.Data.Banking;
using System.Configuration;

namespace Finances.Data
{
    public class Entities : DbContext
    {
        public Entities() : base() { }
        public Entities(string connectionString) : base(connectionString) { }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<Account> Account { get; set; }
        public DbSet<AccountTransaction> AccountTransaction { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<BankAccount> BankAccount { get; set; }
        public DbSet<BankStatement> BankStatement { get; set; }
        public DbSet<BankStatementLine> BankStatementLine { get; set; }
        public DbSet<BankReconciliation> BankReconciliation { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {   
            modelBuilder.Entity<Transaction>().Property(o => o.ExchangeRate).HasPrecision(18, 5);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    
        /// <summary>
        /// Récupère une référence à un objet Entity Framework, avec la chaîne de connexion déjà précisée (à partir du web.config).
        /// </summary>
        public static Entities GetContext()
        {
            Database.SetInitializer(new CustomInitializer());
            var entities = new Entities(ConfigurationManager.ConnectionStrings["Entities"].ConnectionString);
            //entities.Configuration.ProxyCreationEnabled = false;
            //entities.Configuration.LazyLoadingEnabled = false;
            return entities;
        }

        class CustomInitializer : IDatabaseInitializer<Entities>
        {
            public void InitializeDatabase(Entities context)
            {
                // Do nothing
            }
        }

    }

}
