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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Contrôleur pour gérer les assignments
        AssignmentsController assignmentsController;

        // Constructeur de la fenêtre principale
        public MainWindow()
        {
            // Initialise les composants graphiques de la fenêtre
            InitializeComponent();
            // Initialise le contrôleur des assignments
            assignmentsController = new AssignmentsController();
            // Associe un gestionnaire d'événements pour la pression des touches
            this.KeyDown += new KeyEventHandler(MainWindow_KeyDown);
            // Met à jour la liste des assignments affichés
            UpdateList();
        }

        // Gestionnaire d'événements pour la pression des touches dans la fenêtre
        void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            // Si la touche Enter est pressée
            if (e.Key == Key.Enter)
            {
                // Si la boîte de saisie n'est pas vide
                if (inputBox.Text.Trim() != "")
                {
                    // Crée un nouvel assignment à partir du texte saisi
                    Assignment task = new Assignment(inputBox.Text.Trim());
                    // Ajoute l'assignment via le contrôleur
                    assignmentsController.AddAssignment(task);
                    // Vide la boîte de saisie
                    inputBox.Text = "";
                }

                // Met à jour la liste des assignments affichés
                UpdateList();
            }

            // Si la combinaison Ctrl + Delete est pressée
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.Delete)
            {
                // Supprime tous les assignments
                assignmentsController.Clear();
                // Met à jour la liste des assignments affichés
                UpdateList();
            }
        }

        // Met à jour la liste des assignments affichés dans l'interface utilisateur
        private void UpdateList()
        {
            // Vide le panneau principal des éléments enfants
            mainPanel.Children.Clear();
            // Met à jour le compteur d'assignments incomplets
            counter.Content = assignmentsController.Incompleted;

            // Pour chaque assignment dans le contrôleur, l'ajoute au panneau principal
            foreach (var i in assignmentsController.assignments)
            {
                AddAssignment(i);
            }
        }

        // Ajoute un assignment à l'interface utilisateur
        private void AddAssignment(Assignment assignment)
        {
            // Crée un nouveau StackPanel pour l'assignment
            StackPanel panel = new StackPanel();
            StackPanel smallPanel = new StackPanel();
            panel.Margin = new Thickness(30, 8, 40, 0);

            // Crée un RadioButton pour représenter la tâche
            RadioButton radioButton = new RadioButton();
            radioButton.VerticalContentAlignment = VerticalAlignment.Center;
            radioButton.FontSize = 16;
            radioButton.Content = assignment.Text;
            radioButton.IsChecked = assignment.isDone;
            // Ajoute un gestionnaire d'événements pour marquer la tâche comme terminée
            radioButton.Checked += (sender, e) => RadioButton_Checked(sender, e, assignment.Id);

            // Crée un petit panneau pour afficher la date et le bouton de suppression
            smallPanel.Orientation = Orientation.Horizontal;
            Label label = new Label();
            label.FontSize = 12;
            label.Margin = new Thickness(14, 0, 0, 0);
            label.Foreground = new SolidColorBrush(Color.FromRgb(106, 106, 106));
            label.Content = assignment.Date.TimeOfDay;

            // Crée un bouton de suppression
            Button deleteButton = new Button();
            deleteButton.Background = Brushes.Transparent;
            deleteButton.BorderThickness = new Thickness(0);
            deleteButton.Foreground = new SolidColorBrush(Color.FromRgb(242, 59, 59));
            deleteButton.Content = "Delete";
            // Ajoute un gestionnaire d'événements pour la suppression de la tâche
            deleteButton.Click += (sender, e) => deleteButton_clicked(sender, e, assignment.Id);

            // Ajoute le label et le bouton de suppression au petit panneau
            smallPanel.Children.Add(label);
            smallPanel.Children.Add(deleteButton);

            // Ajoute le RadioButton et le petit panneau au panneau principal de l'assignment
            panel.Children.Add(radioButton);
            panel.Children.Add(smallPanel);

            // Diminue l'opacité si la tâche est terminée
            if (assignment.isDone)
            {
                panel.Opacity = 0.5f;
            }

            // Ajoute le panneau de l'assignment au panneau principal de la fenêtre
            mainPanel.Children.Add(panel);
        }

        // Marque un assignment comme terminé lorsqu'on coche le RadioButton
        private void RadioButton_Checked(object sender, RoutedEventArgs e, int id)
        {
            // Trouve l'assignment par son ID
            Assignment task = assignmentsController.Find(id);
            // Marque l'assignment comme complété via le contrôleur
            assignmentsController.CompleteAssignment(task);
            // Met à jour la liste des assignments affichés
            UpdateList();
        }

        // Supprime un assignment lorsque le bouton "Delete" est cliqué
        private void deleteButton_clicked(object sender, RoutedEventArgs e, int id)
        {
            // Affiche une boîte de dialogue de confirmation
            if (MessageBox.Show("Are you sure to delete it?", "Deleting assignment", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                // Si l'utilisateur confirme, trouve et supprime l'assignment
                Assignment task = assignmentsController.Find(id);
                assignmentsController.DeleteAssignment(task);
            }

            // Met à jour la liste des assignments affichés
            UpdateList();
        }

        // Gère les changements de texte dans la boîte de saisie
        private void inputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Affiche ou cache l'indication en fonction du contenu de la boîte de saisie
            if (inputBox.Text.Length == 0)
            {
                inputHint.Visibility = Visibility.Visible;
            }
            else
            {
                inputHint.Visibility = Visibility.Hidden;
            }
        }
    }
}
