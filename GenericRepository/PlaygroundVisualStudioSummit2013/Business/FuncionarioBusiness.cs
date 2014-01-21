using PlaygroundVisualStudioSummit2013.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaygroundVisualStudioSummit2013.Business {
    public class FuncionarioBusiness {

        public void Salvar(Funcionario funcionario) {

            #region [ Validações ]

            if (funcionario == null)
                throw new ArgumentNullException("funcionario");

            if (string.IsNullOrEmpty(funcionario.Nome))
                throw new InvalidOperationException("Forneça um nome para o funcionário.");

            #endregion
            
            var rep = Data.RepositoryFactory<Funcionario>.Criar();

            rep.Save(funcionario);
        }

        public Funcionario GetFuncionario(string nome) {

            #region [ Validações ]

            if (nome == null)
                throw new ArgumentNullException("nome");

            if (string.IsNullOrEmpty(nome))
                return null;

            #endregion

            Funcionario returnValue = null;

            var rep = Data.RepositoryFactory<Funcionario>.Criar();

            returnValue = rep.Query(d => d.Nome == nome).FirstOrDefault();

            return returnValue;
        }

        public IEnumerable<Funcionario> GetFuncionarios() {

            IEnumerable<Funcionario> returnValue = null;

            var rep = Data.RepositoryFactory<Funcionario>.Criar();

            returnValue = rep.Query(f => f.Nome == "teste");

            return returnValue;
        }
    }
}
