using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite.EF6;
using System.Data.Entity.Core.Common;
using System.IO;

namespace Cloud77.Middleware
{
    public class Tester
    {
        public long Id { get; set; }
        public string Guid { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class SQLiteConfig : DbConfiguration
    {
        public SQLiteConfig()
        {
            SetProviderFactory("System.Data.SQLite", SQLiteFactory.Instance);
            SetProviderFactory("System.Data.SQLite.EF6", SQLiteProviderFactory.Instance);
            SetProviderServices("System.Data.SQLite", SQLiteProviderFactory.Instance.GetService(typeof(DbProviderServices)) as DbProviderServices);
        }
    }

    public class TesterDbContext: DbContext
    {
        public TesterDbContext(string dataSource) : base(new SQLiteConnection()
        {
            ConnectionString = new SQLiteConnectionStringBuilder()
            {
                DataSource = dataSource,
                ForeignKeys = true
            }.ConnectionString
            },
            true)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Tester> Testers { get; set; }
    }

    public class TesterRepository
    {
        private TesterDbContext context;

        public TesterRepository()
        {
            context = new TesterDbContext(Path.Combine(DesktopMiddleware.StartUpPath, "database", "tester.db"));
        }

        public Tester GetTester()
        {
            var tester = context.Testers.FirstOrDefault();
            return tester;
        }
    }
}
