using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using System.Reflection.Metadata;
using WebAPI.Entities;
using WebAPI.Helpers;

namespace WebAPI.Services {
    public class DBContext : DbContext {
        public DbSet<User> users { set; get; }
        public DbSet<Category> categories { set; get; }
        public DbSet<Product> products { set; get; }
        public DbSet<Discount> discounts { set; get; }
        public DbSet<Image> images { set; get; }
        public DbSet<Size> sizes { set; get; }
        public DbSet<Order> orders { set; get; }
        public DbSet<OrderDetail> orderDetails { set; get; }
        public DbSet<Shipping> shipping { set; get; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseMySql(AppSettings.Connect, ServerVersion.AutoDetect(AppSettings.Connect));
        }

        public async Task CreateDatabase() {
            String databaseName = Database.GetDbConnection().Database;

            bool result = await Database.EnsureCreatedAsync();
            string resultString = result ? "Success" : "database arealy exits";
            Console.WriteLine($"[{DateTime.Now.ToString()}] CSDL {databaseName} : {resultString}");
        }
        public async Task DeleteDatabase() {
            String databaseName = Database.GetDbConnection().Database;
            bool deleted = await Database.EnsureDeletedAsync();
            string deletionInfo = deleted ? "đã xóa" : "không xóa được";
            Console.WriteLine($"[{DateTime.Now.ToString()}] {databaseName} {deletionInfo}");

        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess) {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(
           bool acceptAllChangesOnSuccess,
           CancellationToken cancellationToken = default
        ) {
            OnBeforeSaving();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess,
                          cancellationToken);
        }

        private void OnBeforeSaving() {
            var entries = ChangeTracker.Entries();
            var utcNow = DateTime.UtcNow;

            foreach (var entry in entries) {
                if (entry.Entity is BaseEntity trackable) {
                    switch (entry.State) {
                        case EntityState.Modified:
                            trackable.UpdatedAt = utcNow;

                            entry.Property("CreatedAt").IsModified = false;
                            break;

                        case EntityState.Added:
                            trackable.CreatedAt = utcNow;
                            trackable.UpdatedAt = utcNow;
                            break;
                    }
                }
            }
        }
    }
}
