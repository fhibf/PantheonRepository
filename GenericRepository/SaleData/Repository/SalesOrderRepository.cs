using GenericRepository;
using SaleEntities;

namespace SaleData.Repository
{
    public class SalesOrderRepository : PantheonRepository<SalesOrder, DataContext.DataContext>
    {
    }
}
