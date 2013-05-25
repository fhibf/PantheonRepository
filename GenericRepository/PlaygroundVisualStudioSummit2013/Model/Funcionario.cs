using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaygroundVisualStudioSummit2013.Model {
    public class Funcionario {
        
        public int Id { get; set; }

        public string Nome { get; set; }

        public int IdDepartamento { get; set; }

        public Departamento Departamento { get; set; }
    }
}
