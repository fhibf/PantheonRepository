using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Reflection;

namespace GenericRepository
{
    public enum Continent
    {
        Oceania,
        Asia,
        Europa,
        America,
        Africa
    }

    public class Customer
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public int EnderecoAtualId { get; set; }

        public virtual Address EnderecoAtual { get; set; }

        public IEnumerable<SalesOrder> Pedidos { get; set; }

        public Customer()
        {
            //this.EnderecoAtual = new Address();
            
            this.Pedidos = new List<SalesOrder>();
        }
    }
    
    public class Country
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public Continent Continente { get; set; }
    }

    public class Address
    {
        public int Id { get; set; }

        public string Logradouro { get; set; }

        public string Numero { get; set; }
        
        public string Bairro { get; set; }

        public int PaisId { get; set; }

        public Country Pais { get; set; }

        public string Cep { get; set; }

        public IEnumerable<SalesOrder> Pedidos { get; set; }

        public Address()
        {
            this.Pedidos = new List<SalesOrder>();            
        }
    }

    public class SalesOrderItem
    {
        public int Id { get; set; }

        public int ProdutoId { get; set; }

        public Produto Produto { get; set; }

        public int PedidoVendaId { get; set; }

        public SalesOrder PedidoVenda { get; set; }

        public int Quantidade { get; set; }

        public decimal ValorVenda { get; set; }
    }

    public class SalesOrder
    {
        public int Id { get; set; }
        
        public DateTime DataVenda { get; set; }

        public int ClienteId { get; set; }
        
        public Customer Cliente { get; set; }

        public ICollection<SalesOrderItem> Items { get; set; }

        public SalesOrder()
        {
            this.DataVenda = DateTime.Now;
            this.Items = new List<SalesOrderItem>();
        }
    }

    public class Produto
    {
        public int Id { get; set; }

        public decimal ValorVenda { get; set; }

        public string Nome { get; set; }

        public decimal ValorCompra { get; set; }
    }

    public class ContextoDados : DbContext
    {
        public DbSet<Produto> Produtos { get; set; }

        public DbSet<Customer> Clientes { get; set; }

        public DbSet<SalesOrder> Pedidos { get; set; }

        public DbSet<Address> Enderecos { get; set; }

        public DbSet<Country> Paises { get; set; }

        public ContextoDados()
        {
            Database.SetInitializer<ContextoDados>(new DropCreateDatabaseIfModelChanges<ContextoDados>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Produto>().ToTable("Produto");
            modelBuilder.Entity<Customer>().ToTable("Cliente");
            modelBuilder.Entity<SalesOrder>().ToTable("PedidoVenda");
            modelBuilder.Entity<SalesOrderItem>().ToTable("ItemPedidoVenda");
            modelBuilder.Entity<Address>().ToTable("Endereco");
            modelBuilder.Entity<Country>().ToTable("Pais");

            modelBuilder.Entity<Customer>()
                        .HasRequired(e => e.EnderecoAtual)
                        .WithMany()
                        .HasForeignKey(e => e.EnderecoAtualId);
            
            modelBuilder.Entity<Address>()
                       .HasRequired(e => e.Pais)
                       .WithMany()
                       .HasForeignKey(e => e.PaisId);

            modelBuilder.Entity<SalesOrder>()
                        .HasRequired(e => e.Cliente)
                        .WithMany()
                        .HasForeignKey(e => e.ClienteId);

            modelBuilder.Entity<SalesOrderItem>()
                      .HasRequired(e => e.PedidoVenda)
                      .WithMany()
                      .HasForeignKey(e => e.PedidoVendaId);

            modelBuilder.Entity<SalesOrderItem>()
                      .HasRequired(e => e.Produto)
                      .WithMany()
                      .HasForeignKey(e => e.ProdutoId);
                        
            base.OnModelCreating(modelBuilder);
        }
    }

    
}
