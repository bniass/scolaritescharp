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
using System.Windows.Shapes;
using scolarite.service;
using Microsoft.Win32;
using System.IO;

namespace scolarite
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IParametre parametre;
        private List<classe> classes;
        public MainWindow()
        {
            InitializeComponent();
            parametre = new ParametreRepository();
            classes = parametre.findAllClasse();
            classeDtg.ItemsSource = classes;
            List<filiere> filieres = parametre.findAllFiliere();
            filiereCbx.ItemsSource = filieres;
            filiereCbx.DisplayMemberPath = "libelle";
            saveBtn.IsEnabled = false;
            updateBtn.IsEnabled = false;
            deleteBtn.IsEnabled = false;
        }

        private bool verif()
        {
            if (codeTbx.Text.Trim().Equals("") || libelleTbx.Text.Trim().Equals("") ||
                fraisinscriptionTbx.Text.Trim().Equals("") || mensualiteTbx.Text.Trim().Equals("") ||
                filiereCbx.SelectedIndex == -1)
            {
                MessageBox.Show("Tous les champs sont obligatoires !");
                return false;
            }

            if (data == null)
            {
                MessageBox.Show("La photo est obligatoire !");
                return false; ;
            }

            return true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            classe classe = new classe();
            
            try
            {
                saveOrUpdate(classe);
                data = null;
                
                classeDtg.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Clear();
            classeDtg.ItemsSource = parametre.findAllClasse();
        }

        private void saveOrUpdate(classe classe)
        {
            try
            {
                if (!verif())
                {
                    return;
                }
                classe.code = codeTbx.Text.Trim();
                classe.libelle = libelleTbx.Text.Trim();

                try
                {
                    classe.fraisinscription = int.Parse(fraisinscriptionTbx.Text.Trim());
                }
                catch (Exception)
                {
                    MessageBox.Show("Frais d'inscription non numérique !");
                    return;
                }
                try
                {
                    classe.mensualite = int.Parse(mensualiteTbx.Text.Trim());
                }
                catch (Exception)
                {
                    MessageBox.Show("Mensualite non numérique !");
                    return;
                }

                classe.filiere = (filiere)filiereCbx.SelectedItem;
                classe.photo = data;
                if (classe.Id == 0)
                {
                    classe = parametre.saveClasse(classe);
                    MessageBox.Show("Classe ajoutée !");
                }
                else
                {
                    classe = parametre.updateClasse(classe);
                    MessageBox.Show("Classe modifiée !");
                }
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        byte[] data = null;
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                img.Source = new BitmapImage(new Uri(op.FileName));
                FileStream fs = new FileStream(op.FileName, FileMode.Open, FileAccess.Read);
                //MessageBox.Show(fs.Length+"");
                data = new byte[fs.Length];
                fs.Read(data, 0, System.Convert.ToInt32(fs.Length));
            }
        }
        classe selectedClasse = null;
        private void ClasseDtg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(classeDtg.SelectedIndex < classeDtg.Items.Count -1)
            {
                activate(false);
                selectedClasse = (classe)classeDtg.SelectedItem;
                if(selectedClasse != null)
                {
                    codeTbx.Text = selectedClasse.code;
                    libelleTbx.Text = selectedClasse.libelle;
                    fraisinscriptionTbx.Text = selectedClasse.fraisinscription+"";
                    mensualiteTbx.Text = selectedClasse.mensualite+"";
                    filiereCbx.SelectedItem = selectedClasse.filiere;
                    if(selectedClasse.photo != null)
                    {
                        img.Source = Utilitaire.BitmapImageFromBytes(selectedClasse.photo);
                    }
                    else
                    {
                        img.Source = null;
                    }
                }
            }
            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            
            classeDtg.SelectedIndex = -1;
            Clear();
            activate(true);
        }

        private void activate(bool active)
        {
            saveBtn.IsEnabled = active;
            updateBtn.IsEnabled = !active;
            deleteBtn.IsEnabled = !active;
        }

        private void Clear()
        {
            codeTbx.Text = "";
            libelleTbx.Text = "";
            fraisinscriptionTbx.Text = "";
            mensualiteTbx.Text = "";
            filiereCbx.SelectedIndex = -1;
            img.Source = null;
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                saveOrUpdate(selectedClasse);
                data = null;

                classeDtg.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Clear();
            classeDtg.ItemsSource = parametre.findAllClasse();

        }
    }
}
