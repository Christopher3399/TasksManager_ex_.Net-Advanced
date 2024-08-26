using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksWPF
{
    // Classe ApplicationContext héritant de DbContext, utilisée pour interagir avec la base de données
    public class ApplicationContext : DbContext
    {
        // Représente une table dans la base de données pour stocker les tâches (assignments)
        public DbSet<Assignment> Assignments { get; set; } = null!;

        // Méthode de configuration du contexte de base de données
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configure le contexte pour utiliser SQLite comme base de données avec un fichier nommé "tasks.db"
            optionsBuilder.UseSqlite("Data Source=tasks.db");
        }
    }
}
