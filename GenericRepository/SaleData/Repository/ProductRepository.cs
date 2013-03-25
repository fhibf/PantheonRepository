using GenericRepository;
using SaleEntities;

namespace SaleData.Repository
{
    public class ProductRepository : PantheonRepository<Product, DataContext.DataContext>
    {
    }
}
