using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlaygroundVisualStudioSummit2013.Model;
using System.Data.Entity;

namespace PlaygroundVisualStudioSummit2013.Data {
    public class DataContext : DbContext {

        public DbSet<Funcionario> Funcionarios { get; set; }

        public DbSet<Departamento> Departamentos { get; set; }

        public DataContext() {

            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DataContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {

            modelBuilder.Entity<Funcionario>()
                        .HasRequired(f => f.Departamento)
                        .WithMany()
                        .HasForeignKey(f => f.IdDepartamento);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
