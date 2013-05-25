using PlaygroundVisualStudioSummit2013.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaygroundVisualStudioSummit2013 {
    class Program {
        static void Main(string[] args) {

            var depBusiness = new Business.DepartamentoBusiness();
            var funBusiness = new Business.FuncionarioBusiness();

            Departamento novoDepartamento = new Departamento();
            novoDepartamento.Nome = "TI";

            //  Salvar o departamento na fonte de dados.
            depBusiness.Salvar(novoDepartamento);

            Funcionario novoFuncionario = new Funcionario();
            novoFuncionario.Nome = "Wolverine";
            novoFuncionario.Departamento = novoDepartamento;

            // Salvar o funcionário na fonte de dados.
            funBusiness.Salvar(novoFuncionario);

            var todosFuncionarios = funBusiness.GetFuncionarios();
        }
    }
}
