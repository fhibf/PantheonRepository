using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pantheon
{
    public interface IRepositoryFactory<T>
        where T : class 
    {
        IRepository<T> CriarRepositorio();
    }
}
