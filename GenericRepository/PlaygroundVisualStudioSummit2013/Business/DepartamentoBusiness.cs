using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlaygroundVisualStudioSummit2013.Model;

namespace PlaygroundVisualStudioSummit2013.Business {
    public class DepartamentoBusiness {

        public void Salvar(Departamento departamento) {

            #region [ Validações ]
                       
            if (departamento == null)
                throw new ArgumentNullException("departamento");

            if (string.IsNullOrEmpty(departamento.Nome))
                throw new InvalidOperationException("Forneça um nome para o departamento.");

            #endregion

            var rep = Data.RepositoryFactory<Departamento>.Criar();

            rep.Save(departamento);
        }

        public Departamento GetDepartamento(string nome) {

            #region [ Validações ]

            if (nome == null)
                throw new ArgumentNullException("nome");

            if (string.IsNullOrWhiteSpace(nome))
                return null;

            #endregion

            Departamento returnValue = null;

            var rep = Data.RepositoryFactory<Departamento>.Criar();

            returnValue = rep.Query(d => d.Nome == nome).FirstOrDefault();

            return returnValue;
        }

        public Departamento GetDepartamento(int id) {

            #region [ Validações ]

            if (id <= 0)
                return null;

            #endregion

            Departamento returnValue = null;

            var rep = Data.RepositoryFactory<Departamento>.Criar();

            returnValue = rep.Query(d => d.Id == id).FirstOrDefault();

            return returnValue;
        }
    }
}
