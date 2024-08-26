using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksWPF
{
    // Classe représentant une tâche (Assignment)
    public class Assignment
    {
        // Constructeur qui initialise une nouvelle tâche avec le texte donné
        public Assignment(string text)
        {
            // Assigne le texte de la tâche
            Text = text;
            // Initialise la date de création de la tâche à l'heure actuelle
            Date = DateTime.Now;
            // Par défaut, la tâche n'est pas encore accomplie
            isDone = false;
        }

        // Propriété pour stocker l'ID unique de la tâche (généré automatiquement par le système)
        public int Id { get; set; }

        // Propriété pour stocker le texte ou la description de la tâche
        public string Text { get; set; }

        // Propriété indiquant si la tâche est terminée ou non
        public bool isDone { get; set; }

        // Propriété pour stocker la date de création de la tâche
        public DateTime Date { get; set; }
    }
}
