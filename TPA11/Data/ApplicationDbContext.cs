using System;
namespace TPA11.Data
{
	public class ApplicationDbContext:DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):
			base(options) 
		{

		}

        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientOrder> ClientOrders { get; set; }
        public DbSet<LibraryItem> LibraryItems { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<UserAuthentication> UserAuthentications { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ClientOrder>()
                .HasOne(co => co.Client)
                .WithMany(c => c.ClientOrders)
                .HasForeignKey(co => co.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(co => co.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.LibraryItem)
                .WithMany(li => li.OrderItems)
                .HasForeignKey(oi => oi.ean_isbn13)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Client)
                .WithMany(c => c.Payments)
                .HasForeignKey(p => p.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ShoppingCartItem>()
                .HasOne(sci => sci.Client)
                .WithMany(c => c.ShoppingCartItems)
                .HasForeignKey(sci => sci.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ShoppingCartItem>()
                .HasOne(sci => sci.LibraryItem)
                .WithMany(li => li.ShoppingCartItems)
                .HasForeignKey(sci => sci.ean_isbn13)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserAuthentication>()
                .HasOne(ua => ua.Client)
                .WithOne(c => c.UserAuthentication)
                .HasForeignKey<UserAuthentication>(ua => ua.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            // You can configure additional constraints, indexes, etc., here if needed.
            base.OnModelCreating(modelBuilder);
        }
    }
}

