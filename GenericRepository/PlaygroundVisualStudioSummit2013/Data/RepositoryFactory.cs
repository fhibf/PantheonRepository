using Pantheon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaygroundVisualStudioSummit2013.Data {

    public class RepositoryFactory<T> : IRepositoryFactory<T>
        where T : class {

        public IRepository<T> CriarRepositorio() {

            return new PantheonRepository<T, Data.DataContext>();
        }

        public static IRepository<T> Criar() {

            var rep = new RepositoryFactory<T>();

            return rep.CriarRepositorio();
        }
    }
}
