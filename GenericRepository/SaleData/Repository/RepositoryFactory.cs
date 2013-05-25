using Pantheon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaleData.Repository
{
    public class RepositoryFactory<T> : IRepositoryFactory<T>
        where T : class
    {
        public IRepository<T> CriarRepositorio()
        {
            return new PantheonRepository<T, DataContext.DataContext>();
        }
    }
}
