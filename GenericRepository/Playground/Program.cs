using GenericRepository;
using SaleData.Repository;
using SaleEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playground
{   
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 50; i++)
            {            
                // Criar novos países
                CriarNovosPaises();

                // Criar novos produtos
                CriarNovosProdutos();

                // Criar novos clientes
                CriarNovosClientes();

                // Criar novos pedidos
                CriarNovosPedidos();
            }
        }

        private static void CriarNovosPedidos()
        {
            var repProdutos = new ProductRepository();
            var repClientes = new CustomerRepository();

            var produtos = repProdutos.GetAll().ToArray();
            var clientes = repClientes.GetAll("CurrentAddress").ToArray();

            var listOfPedidos = new List<SalesOrder>(1);
            for (int i = 0; i < 50; i++)
            {
                Random random = new Random(DateTime.Now.Millisecond);

                SalesOrder pedido = new SalesOrder();
                pedido.Customer = clientes[random.Next(0, clientes.Length)];                

                int quantidadeItens = random.Next(0, 20);

                for (int j = 0; j < quantidadeItens; j++)
                {
                    SalesOrderItem itemVenda = new SalesOrderItem();

                    itemVenda.Product = produtos[random.Next(0, produtos.Length)];
                    itemVenda.Quantity = random.Next(0, 5);
                    itemVenda.Value = itemVenda.Product.SaleValue;
                    itemVenda.SalesOrder = pedido;
                    
                    pedido.Items.Add(itemVenda);
                }
                
                System.Threading.Thread.Sleep(1);

                listOfPedidos.Add(pedido);
            }

            var repPedidos = new SalesOrderRepository();
            repPedidos.Save(listOfPedidos, true);

            repPedidos.Save(listOfPedidos);
        }

        private static void CriarNovosClientes()
        {
            var repPaises= new CountryRepository();
            
            var paises = repPaises.GetAll().ToArray();

            var listOfClientes = new List<Customer>();
            for (int i = 0; i < 50; i++)
            {
                var randomGenerator = new Random(DateTime.Now.Millisecond);

                Customer novoCliente = new Customer();

                novoCliente.Name = "Cliente " + (i + 1).ToString();
                novoCliente.CurrentAddress = new Address();
                novoCliente.CurrentAddress.ZipCode = "CEP";
                novoCliente.CurrentAddress.Street = "Logradouro";
                novoCliente.CurrentAddress.Number = "Numero";
                novoCliente.CurrentAddress.Country = paises[randomGenerator.Next(0, paises.Count())];
                
                listOfClientes.Add(novoCliente);

                System.Threading.Thread.Sleep(1);
            }

            var repClientes = new CustomerRepository();

            repClientes.Save(listOfClientes, true);

            repClientes.Save(listOfClientes);
        }

        private static void CriarNovosProdutos()
        {
            var listOfProdutos = new List<Product>(100);
            for (int i = 0; i < 100; i++)
            {
                var randomGenerator = new Random(DateTime.Now.Millisecond);

                Product produto = new Product();
                produto.Name = "Produto " + (i + 1).ToString();
                produto.BuyValue = (decimal)(randomGenerator.Next(1000) + randomGenerator.NextDouble());
                produto.SaleValue = produto.BuyValue * 1.15M;

                listOfProdutos.Add(produto);

                System.Threading.Thread.Sleep(1);
            }

            var rep = new ProductRepository();

            rep.Save(listOfProdutos);

            rep.Save(listOfProdutos);
        }

        private static void CriarNovosPaises()
        {
            var rep = new CountryRepository();

            var query = rep.GetAll();

            if (query.Count() == 0)
            {
                Country paisBrasil = new Country() { Continent = Continent.America, Name = "Brasil" };
                Country paisChina = new Country() { Continent = Continent.Asia, Name = "China" };
                Country paisEUA = new Country() { Continent = Continent.America, Name = "EUA" };
                Country paisPortugal = new Country() { Continent = Continent.Europe, Name = "Portugal" };
                Country paisJapao = new Country() { Continent = Continent.Asia, Name = "Japão" };
                Country paisEgito = new Country() { Continent = Continent.Africa, Name = "Egito" };
                Country paisInglaterra = new Country() { Continent = Continent.Europe, Name = "Inglaterra" };
                
                rep.Save(new Country[] {
                                    paisBrasil,
                                    paisChina,
                                    paisEUA,
                                    paisPortugal,
                                    paisJapao,
                                    paisEgito,
                                    paisInglaterra
                                });
            }
        }
    }
}
