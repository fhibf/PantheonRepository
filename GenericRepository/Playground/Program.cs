using Pantheon;
using SaleData.DataContext;
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
            for (int i = 0; i < 1; i++)
            {            
                // Create new countries
                CreateNewCountries();

                // Create new products
                CreateNewProducts();

                // Create new customers
                CreateNewCustomers();

                // Create new orders
                CreateNewOrders();
            }
        }
                
        private static void CreateNewOrders()
        {
            var repProducts = (new RepositoryFactory<Product>()).CriarRepositorio();
            var repCustomers = (new RepositoryFactory<Customer>()).CriarRepositorio();
            
            var products = repProducts.GetAll().ToArray();
            var customers = repCustomers.GetAll("CurrentAddress").ToArray();

            var listOfSales = new List<SalesOrder>(1);
            for (int i = 0; i < 50; i++)
            {
                Random random = new Random(DateTime.Now.Millisecond);

                SalesOrder sale = new SalesOrder();
                sale.Customer = customers[random.Next(0, customers.Length)];                

                int countOfItems = random.Next(0, 20);

                for (int j = 0; j < countOfItems; j++)
                {
                    SalesOrderItem salesOrderItem = new SalesOrderItem();

                    salesOrderItem.Product = products[random.Next(0, products.Length)];
                    salesOrderItem.Quantity = random.Next(0, 5);
                    salesOrderItem.Value = salesOrderItem.Product.SaleValue;
                    salesOrderItem.SalesOrder = sale;
                    
                    sale.Items.Add(salesOrderItem);
                }
                
                System.Threading.Thread.Sleep(1);

                listOfSales.Add(sale);
            }

            var repSales = (new RepositoryFactory<SalesOrder>()).CriarRepositorio();
            repSales.Save(listOfSales, true);

            repSales.Save(listOfSales);
        }

        private static void CreateNewCustomers()
        {
            var repCountries = (new RepositoryFactory<Country>()).CriarRepositorio(); 
            
            var countries = repCountries.GetAll().ToArray();

            var listOfCustomers = new List<Customer>();
            for (int i = 0; i < 50; i++)
            {
                var randomGenerator = new Random(DateTime.Now.Millisecond);

                Customer newCustomer = new Customer();

                newCustomer.Name = "Customer " + (i + 1).ToString();
                newCustomer.CurrentAddress = new Address();
                newCustomer.CurrentAddress.ZipCode = "Zip Code";
                newCustomer.CurrentAddress.Street = "Street";
                newCustomer.CurrentAddress.Number = "Number";
                newCustomer.CurrentAddress.Country = countries[randomGenerator.Next(0, countries.Count())];
                
                listOfCustomers.Add(newCustomer);

                System.Threading.Thread.Sleep(1);
            }

            var repCustomers = (new RepositoryFactory<Customer>()).CriarRepositorio();

            repCustomers.Save(listOfCustomers, true);

            repCustomers.Save(listOfCustomers);
        }

        private static void CreateNewProducts()
        {
            var listOfProducts = new List<Product>(100);
            for (int i = 0; i < 100; i++)
            {
                var randomGenerator = new Random(DateTime.Now.Millisecond);

                Product newProduct = new Product();
                newProduct.Name = "Product " + (i + 1).ToString();
                newProduct.BuyValue = (decimal)(randomGenerator.Next(1000) + randomGenerator.NextDouble());
                newProduct.SaleValue = newProduct.BuyValue * 1.15M;

                listOfProducts.Add(newProduct);

                System.Threading.Thread.Sleep(1);
            }

            var rep = (new RepositoryFactory<Product>()).CriarRepositorio();

            rep.Save(listOfProducts);

            rep.Save(listOfProducts);
        }

        private static void CreateNewCountries()
        {
            var rep = (new RepositoryFactory<Country>()).CriarRepositorio();

            var query = rep.GetAll();

            if (query.Count() == 0)
            {
                Country paisBrasil = new Country() { Continent = Continent.America, Name = "Brazil" };
                Country paisChina = new Country() { Continent = Continent.Asia, Name = "China" };
                Country paisEUA = new Country() { Continent = Continent.America, Name = "USA" };
                Country paisPortugal = new Country() { Continent = Continent.Europe, Name = "Portugal" };
                Country paisJapao = new Country() { Continent = Continent.Asia, Name = "Japan" };
                Country paisEgito = new Country() { Continent = Continent.Africa, Name = "Egypt" };
                Country paisInglaterra = new Country() { Continent = Continent.Europe, Name = "UK" };
                
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
