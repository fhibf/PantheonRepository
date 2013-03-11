using GenericRepository;
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
            var repProdutos = new PantheonRepository<Produto>();
            var repClientes = new PantheonRepository<Customer>();

            var produtos = repProdutos.GetAll().ToArray();
            var clientes = repClientes.GetAll("EnderecoAtual").ToArray();

            var listOfPedidos = new List<SalesOrder>(1);
            for (int i = 0; i < 50; i++)
            {
                Random random = new Random(DateTime.Now.Millisecond);

                SalesOrder pedido = new SalesOrder();
                pedido.Cliente = clientes[random.Next(0, clientes.Length)];                

                int quantidadeItens = random.Next(0, 20);

                for (int j = 0; j < quantidadeItens; j++)
                {
                    SalesOrderItem itemVenda = new SalesOrderItem();

                    itemVenda.Produto = produtos[random.Next(0, produtos.Length)];
                    itemVenda.Quantidade = random.Next(0, 5);
                    itemVenda.ValorVenda = itemVenda.Produto.ValorVenda;
                    itemVenda.PedidoVenda = pedido;
                    
                    pedido.Items.Add(itemVenda);
                }
                
                System.Threading.Thread.Sleep(1);

                listOfPedidos.Add(pedido);
            }

            var repPedidos = new PantheonRepository<SalesOrder>();
            repPedidos.Save(listOfPedidos, true);

            repPedidos.Save(listOfPedidos);
        }

        private static void CriarNovosClientes()
        {
            var repPaises= new PantheonRepository<Country>();
            
            var paises = repPaises.GetAll().ToArray();

            var listOfClientes = new List<Customer>();
            for (int i = 0; i < 50; i++)
            {
                var randomGenerator = new Random(DateTime.Now.Millisecond);

                Customer novoCliente = new Customer();

                novoCliente.Nome = "Cliente " + (i + 1).ToString();
                novoCliente.EnderecoAtual = new Address();
                novoCliente.EnderecoAtual.Bairro = "Bairro";
                novoCliente.EnderecoAtual.Cep = "CEP";
                novoCliente.EnderecoAtual.Logradouro = "Logradouro";
                novoCliente.EnderecoAtual.Numero = "Numero";
                novoCliente.EnderecoAtual.Pais = paises[randomGenerator.Next(0, paises.Count())];
                
                listOfClientes.Add(novoCliente);

                System.Threading.Thread.Sleep(1);
            }

            var repClientes = new PantheonRepository<Customer>();

            repClientes.Save(listOfClientes, true);

            repClientes.Save(listOfClientes);
        }

        private static void CriarNovosProdutos()
        {
            var listOfProdutos = new List<Produto>(100);
            for (int i = 0; i < 100; i++)
            {
                var randomGenerator = new Random(DateTime.Now.Millisecond);

                Produto produto = new Produto();
                produto.Nome = "Produto " + (i + 1).ToString();
                produto.ValorCompra = (decimal)(randomGenerator.Next(1000) + randomGenerator.NextDouble());
                produto.ValorVenda = produto.ValorCompra * 1.15M;

                listOfProdutos.Add(produto);

                System.Threading.Thread.Sleep(1);
            }

            var rep = new PantheonRepository<Produto>();

            rep.Save(listOfProdutos);

            rep.Save(listOfProdutos);
        }

        private static void CriarNovosPaises()
        {
            var rep = new PantheonRepository<Country>();

            var query = rep.GetAll();

            if (query.Count() == 0)
            {
                Country paisBrasil = new Country() { Continente = Continent.America, Nome = "Brasil" };
                Country paisChina = new Country() { Continente = Continent.Asia, Nome = "China" };
                Country paisEUA = new Country() { Continente = Continent.America, Nome = "EUA" };
                Country paisPortugal = new Country() { Continente = Continent.Europa, Nome = "Portugal" };
                Country paisJapao = new Country() { Continente = Continent.Asia, Nome = "Japão" };
                Country paisEgito = new Country() { Continente = Continent.Africa, Nome = "Egito" };
                Country paisInglaterra = new Country() { Continente = Continent.Europa, Nome = "Inglaterra" };
                
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
