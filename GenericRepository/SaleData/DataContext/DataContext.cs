using SaleEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleData.DataContext
{
    public class DataContext : DbContext
    {
        public DbSet<Product> Produtos { get; set; }

        public DbSet<Customer> Clientes { get; set; }

        public DbSet<SalesOrder> Pedidos { get; set; }

        public DbSet<Address> Enderecos { get; set; }

        public DbSet<Country> Paises { get; set; }

        public DataContext()
        {
            Database.SetInitializer<DataContext>(new DropCreateDatabaseIfModelChanges<DataContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.ComplexType<CustomerDetails>();

            modelBuilder.Entity<Product>().ToTable("Produto");
            modelBuilder.Entity<Customer>().ToTable("Cliente");
            modelBuilder.Entity<SalesOrder>().ToTable("PedidoVenda");
            modelBuilder.Entity<SalesOrderItem>().ToTable("ItemPedidoVenda");
            modelBuilder.Entity<Address>().ToTable("Endereco");
            modelBuilder.Entity<Country>().ToTable("Pais");

            modelBuilder.Entity<Customer>()
                        .HasRequired(e => e.CurrentAddress)
                        .WithMany()
                        .HasForeignKey(e => e.CurrentAddressId);

            modelBuilder.Entity<Address>()
                       .HasRequired(e => e.Country)
                       .WithMany()
                       .HasForeignKey(e => e.CountryId);

            modelBuilder.Entity<SalesOrder>()
                        .HasRequired(e => e.Customer)
                        .WithMany()
                        .HasForeignKey(e => e.CustomerId);

            modelBuilder.Entity<SalesOrderItem>()
                      .HasRequired(e => e.SalesOrder)
                      .WithMany()
                      .HasForeignKey(e => e.SalesOrderId);

            modelBuilder.Entity<SalesOrderItem>()
                      .HasRequired(e => e.Product)
                      .WithMany()
                      .HasForeignKey(e => e.ProductId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
