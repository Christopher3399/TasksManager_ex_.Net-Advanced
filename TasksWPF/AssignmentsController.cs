using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Microsoft.EntityFrameworkCore;
using System.Windows.Shapes;
using System.Transactions;


namespace TasksWPF
{
    public class AssignmentsController
    {
        // Création du contexte de la base de données
        public ApplicationContext db = new ApplicationContext();

        // Constructeur de la classe AssignmentsController
        public AssignmentsController()
        {
            // Initialisation de la liste des assignments
            assignments = new List<Assignment>();
            // Initialisation de la base de données et chargement des données
            DBInit();
        }

        // Liste des assignments
        public List<Assignment> assignments;

        // Compteur des assignments incomplets
        public int Incompleted = 0;

        // Méthode d'initialisation de la base de données
        private void DBInit()
        {
            // S'assurer que la base de données est créée
            db.Database.EnsureCreated();
            // Charger les assignments depuis la base de données
            db.Assignments.Load();
            // Récupérer tous les assignments et les assigner à la liste locale
            assignments = GetAll();
            // Trier les assignments
            Sort();
        }

        // Ajouter un nouvel assignment
        public void AddAssignment(Assignment assignment)
        {
            // Insérer l'assignment au début de la liste
            assignments.Insert(0, assignment);
            // Ajouter l'assignment à la base de données
            db.Assignments.Add(assignment);
            // Sauvegarder les modifications dans la base de données
            db.SaveChanges();
            // Incrémenter le compteur des assignments incomplets
            Incompleted++;
        }

        // Trouver un assignment par son identifiant
        public Assignment Find(int id)
        {
            // Chercher l'assignment dans la liste locale en fonction de l'ID
            return assignments.Find(x => x.Id == id);
        }

        // Supprimer un assignment
        public void DeleteAssignment(Assignment assignment)
        {
            // Si l'assignment n'est pas complété, décrémenter le compteur des incomplets
            if (!assignment.isDone)
                Incompleted--;
            // Retirer l'assignment de la liste locale
            assignments.Remove(assignment);
            // Supprimer l'assignment de la base de données
            db.Assignments.Remove(assignment);
            // Sauvegarder les modifications dans la base de données
            db.SaveChanges();
        }

        // Marquer un assignment comme complété
        public void CompleteAssignment(Assignment assignment)
        {
            // Retirer l'assignment de sa position actuelle dans la liste locale
            assignments.Remove(assignment);
            // Marquer l'assignment comme complété
            assignment.isDone = true;
            // Insérer l'assignment à la fin de la liste
            assignments.Insert(assignments.Count, assignment);
            // Sauvegarder les modifications dans la base de données
            db.SaveChanges();
            // Décrémenter le compteur des assignments incomplets
            Incompleted--;
        }

        // Trier les assignments
        public void Sort()
        {
            // Trier par date décroissante
            assignments = assignments.OrderByDescending(x => x.Date).ToList();
            // Trier par statut de complétion (les incomplets en premier)
            assignments = assignments.OrderBy(u => !u.isDone ? 0 : 1).ToList();

            // Calculer le nombre d'assignments incomplets
            foreach (var assignment in assignments)
            {
                if (!assignment.isDone)
                {
                    Incompleted++;
                }
            }
        }

        // Supprimer tous les assignments
        public void Clear()
        {
            // Vider la liste locale
            assignments.Clear();
            // Réinitialiser le compteur des incomplets
            Incompleted = 0;
            // Supprimer tous les assignments de la base de données
            db.Assignments.RemoveRange(db.Assignments);
            // Sauvegarder les modifications dans la base de données
            db.SaveChanges();
        }

        // Récupérer tous les assignments depuis la base de données
        private List<Assignment> GetAll()
        {
            return db.Assignments.ToList();
        }
    }
}
