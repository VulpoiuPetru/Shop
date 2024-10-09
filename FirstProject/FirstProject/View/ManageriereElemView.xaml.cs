using FirstProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
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
using System.Windows.Shapes;
using FirstProject.ViewModel;

namespace FirstProject.View
{
    /// <summary>
    /// Interaction logic for ManageriereElemView.xaml
    /// </summary>
    public partial class ManageriereElemView : Window
    {
        public ManageriereElemView()
        {
            InitializeComponent();
            this.DataContext = new FirstProject.ViewModel.ManageriereElemVM();
        }

        private void Selectare_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PersoanaPanel.Visibility = Visibility.Collapsed;
            AmbalajPanel.Visibility = Visibility.Collapsed;
            TipProdusPanel.Visibility = Visibility.Collapsed;
            ProdusPanel.Visibility = Visibility.Collapsed;

            ComboBoxItem selectedItem = (ComboBoxItem)Selectare.SelectedItem;
            string selectedContent = selectedItem.Content.ToString();

            switch (selectedContent)
            {
                case "Persoana":
                    //  PersoanaPanel.Visibility = Visibility.Visible;
                    LoadPersoanaData();
                    break;
                case "Ambalaj":
                    //AmbalajPanel.Visibility = Visibility.Visible;
                    LoadAmbalajData();
                    break;
                case "TipProdus":
                    LoadTipProdusData();
                    // TipProdusPanel.Visibility = Visibility.Visible;
                    break;
                case "Produs":
                    // ProdusPanel.Visibility = Visibility.Visible;
                    LoadProdusData();
                    ModificareTabel.Visibility = Visibility.Collapsed;
                    break;
            }
        }
        private void LoadPersoanaData()
        {
            try
            {
                using (var context = new Store1Entities())
                {
                    //creeare parametru pentru valoarea returnata
                    ObjectParameter idPersoanaParameter = new ObjectParameter("IDPersoana", typeof(int));

                    //executie procedura stocata
                    var result = context.selectPersoana().ToList();

                    //Popularea datagrid-urilor cu date

                    DataGridMagazin.ItemsSource = result;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la încărcarea datelor: " + ex.Message);
            }
        }
        private void LoadAmbalajData()
        {
            try
            {
                using (var context = new Store1Entities())
                {
                    ObjectParameter idAmbalajParameter = new ObjectParameter("IDAmbalaj", typeof(int));

                    var result = context.selectAmbalaj().ToList();

                    DataGridMagazin.ItemsSource = result;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la încărcarea datelor: " + ex.Message);
            }

        }
        private void LoadTipProdusData()
        {
            try
            {
                using (var context = new Store1Entities())
                {
                    ObjectParameter idTipProdusParameter = new ObjectParameter("IDTipProdus", typeof(int));
                    var result = context.selectTipProduse().ToList();
                    DataGridMagazin.ItemsSource = result;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la încărcarea datelor: " + ex.Message);
            }
        }

        private void LoadProdusData()
        {
            try
            {
                using (var context = new Store1Entities())
                {
                    ObjectParameter idProdusParameter = new ObjectParameter("IDProdus", typeof(int));
                    var result = context.selectProdus().ToList();
                    DataGridMagazin.ItemsSource = result;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la încărcarea datelor: " + ex.Message);
            }
        }
        private void DataGridMagazin_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                //determinarea tipului selectata din combobox
                ComboBoxItem selectedItem = (ComboBoxItem)Selectare.SelectedItem;
                string selectedContent = selectedItem.Content.ToString();
                if (selectedContent == "Persoana")
                {
                    // Obține datele modificate
                    var editedItem = e.Row.Item as selectPersoana_Result;

                    if (editedItem != null)
                    {
                        // Actualizează în baza de date folosind procedura stocată
                        UpdatePersoanaInDatabase(editedItem);
                    }
                }
                else
                    if (selectedContent == "Ambalaj")
                {
                    // Obține datele modificate
                    var editedItem = e.Row.Item as selectAmbalaj_Result;

                    if (editedItem != null)
                    {
                        // Actualizează în baza de date folosind procedura stocată
                        UpdateAmbalajInDatabase(editedItem);
                    }
                }
                else
                    if (selectedContent == "TipProdus")
                {
                    var editedItem = e.Row.Item as selectTipProduse_Result;
                    if (editedItem != null)
                    {
                        UpdateTipProdusInDatabase(editedItem);
                    }
                }
            }
        }
        private void UpdatePersoanaInDatabase(selectPersoana_Result persoana)
        {
            try
            {
                using (var context = new Store1Entities())
                {
                    // Creează parametrii pentru procedura stocată
                    //  ObjectParameter idPersoanaParameter = new ObjectParameter("IDPersoana", persoana.IDPersoana);
                    // ObjectParameter numeParameter = new ObjectParameter("Nume", persoana.Nume);
                    // Adaugă ceilalți parametri în funcție de coloanele din tabel


                    // Accesează direct proprietățile persoanei
                    int? idPersoana = persoana.IDPersoana;
                    string nume = persoana.Nume;


                    // Execută procedura stocată
                    //context.updatePersoana(idPersoanaParameter, numeParameter);
                    context.updatePersoana(nume, idPersoana);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la actualizarea datelor: " + ex.Message);
            }
        }
        private void UpdateAmbalajInDatabase(selectAmbalaj_Result ambalaj)
        {
            try
            {
                using (var context = new Store1Entities())
                {
                    int? idAmbalaj = ambalaj.IDAmbalaj;
                    string denumire = ambalaj.Denumire;


                    context.updateAmbalaj(denumire, idAmbalaj);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la actualizarea datelor: " + ex.Message);
            }
        }
        private void UpdateTipProdusInDatabase(selectTipProduse_Result tipProdus)
        {
            try
            {
                using (var context = new Store1Entities())
                {

                    int? idTipProdus = tipProdus.IDTipProdus;
                    string nume = tipProdus.Nume;
                    string unitateMasura = tipProdus.UnitateMasura;
                    int pretKg = tipProdus.PretKg;
                    int? cantitate = tipProdus.Cantitate;
                    int? termenGarantie = tipProdus.TermenGarantie;


                    context.updateTipProduse(nume, unitateMasura, pretKg, cantitate, termenGarantie, idTipProdus);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la actualizarea datelor: " + ex.Message);
            }
        }

        private void UpdateProdusInDataBase(selectProdus_Result produs)
        {
            try
            {
                using (var context = new Store1Entities())
                {
                    //int? idProdus = produs.IDProdus;
                    //string nume = produs.Nume;
                    //int? pretKg = produs.PretKg;
                    //DateTime data = produs.DataAmbalare;
                    //string denumire = produs.Denumire;
                    //int? idPersoana = produs.IDPersoana;

                    //// context.updateProduse()
                    ///int? idProdus = produs.IDProdus;
                    string tipProdus = produs.TipProdus;  // Înlocuit Nume cu TipProdus
                    int? pretKg = produs.PretKg;

                    // Gestionarea DateTime? (Nullable) și conversia la DateTime cu o valoare implicită
                    DateTime data = produs.DataAmbalare ?? DateTime.Now;  // Folosește DataAmbalare sau valoarea implicită DateTime.Now

                    string ambalaj = produs.Ambalaj;  // Înlocuit Denumire cu Ambalaj
                    string persoana = produs.Persoana;  // Înlocuit IDPersoana cu Persoana

                    // Acum poți continua cu context.updateProduse() folosind valorile corectate
                    // Exemplu:
                    // context.updateProduse(idProdus, tipProdus, pretKg, data, ambalaj, persoana);

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la actualizarea datelor: " + ex.Message);
            }
        }

        private void DeletePersoanaFromDataBase(selectPersoana_Result persoana)
        {
            try
            {
                using (var context = new Store1Entities())
                {
                    // Creează parametrii pentru procedura stocată
                    // Accesează direct proprietățile persoanei
                    int? idPersoana = persoana.IDPersoana;


                    // Execută procedura stocată
                    context.deletePersoana(idPersoana);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la actualizarea datelor: " + ex.Message);
            }
        }
        private void DeleteAmbalajFromDatabase(selectAmbalaj_Result ambalaj)
        {
            try
            {
                using (var context = new Store1Entities())
                {
                    int? idAmbalaj = ambalaj.IDAmbalaj;


                    context.deleteAmbalaj(idAmbalaj);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la actualizarea datelor: " + ex.Message + " - Inner Exception: " + ex.InnerException?.Message);
                // MessageBox.Show("Eroare la actualizarea datelor: " + ex.Message);
            }
        }

        private void DeleteTipProdusFromDatabase(selectTipProduse_Result tipProdus)
        {
            try
            {
                using (var context = new Store1Entities())
                {

                    int? idTipProdus = tipProdus.IDTipProdus;



                    context.deleteTipProduse(idTipProdus);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la actualizarea datelor: " + ex.Message);
            }
        }

        private void ModificareTabel_Click(object sender, RoutedEventArgs e)
        {
            //verificarea daca obtinuea selectata din cobobox este persoana
            ComboBoxItem selectedItem = (ComboBoxItem)Selectare.SelectedItem;
            string selectedContent = selectedItem.Content.ToString();

            switch (selectedContent)
            {
                case "Persoana":
                    //obtinerea randului selectat din datagrid
                    var selectedPerson = DataGridMagazin.SelectedItem as selectPersoana_Result;

                    if (selectedPerson != null)
                    {
                        //actualizeaza persona in baza de date folosind procedura stocata
                        UpdatePersoanaInDatabase(selectedPerson);
                        MessageBox.Show("Persoana a fost modificata cu succes!");
                    }
                    else
                    {
                        MessageBox.Show("Selecteaza o persoana din tabel.");
                    }
                    break;
                case "Ambalaj":
                    //obtinerea randului selectat din datagrid
                    var selectedAmbalaj = DataGridMagazin.SelectedItem as selectAmbalaj_Result;

                    if (selectedAmbalaj != null)
                    {
                        //actualizeaza persona in baza de date folosind procedura stocata
                        UpdateAmbalajInDatabase(selectedAmbalaj);
                        MessageBox.Show("Ambalajul a fost modificata cu succes!");
                    }
                    else
                    {
                        MessageBox.Show("Selecteaza un ambalaj din tabel.");
                    }
                    break;
                case "TipProdus":
                    //obtinerea randului selectat din datagrid
                    var selectedTipProdus = DataGridMagazin.SelectedItem as selectTipProduse_Result;

                    if (selectedTipProdus != null)
                    {
                        //actualizeaza persona in baza de date folosind procedura stocata
                        DeleteTipProdusFromDatabase(selectedTipProdus);
                        MessageBox.Show("Tipul produsului a fost sters cu succes!");
                    }
                    else
                    {
                        MessageBox.Show("Selecteaza un tip de produs din tabel.");
                    }
                    break;
            }


            //if (selectedItem != null && selectedItem.Content.ToString() == "Persoana")
            //{
            //    //obtinerea randului selectat din datagrid
            //    var selectedPerson = DataGridMagazin.SelectedItem as selectPersoana_Result;

            //    if (selectedPerson != null)
            //    {
            //        //actualizeaza persona in baza de date folosind procedura stocata
            //        UpdatePersoanaInDatabase(selectedPerson);
            //        MessageBox.Show("Persoana a fost modificata cu succes!");
            //    }
            //    else
            //    {
            //        MessageBox.Show("Selecteaza o persoana din tabel.");
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Selectează opțiunea 'Persoana' din ComboBox pentru a modifica datele.");
            //}
        }

        private void StergereTabel_Click(object sender, RoutedEventArgs e)
        {
            //verificarea daca obtinuea selectata din cobobox este persoana
            ComboBoxItem selectedItem = (ComboBoxItem)Selectare.SelectedItem;
            string selectedContent = selectedItem.Content.ToString();

            switch (selectedContent)
            {
                case "Persoana":
                    //obtinerea randului selectat din datagrid
                    var selectedPerson = DataGridMagazin.SelectedItem as selectPersoana_Result;

                    if (selectedPerson != null)
                    {
                        //actualizeaza persona in baza de date folosind procedura stocata
                        DeletePersoanaFromDataBase(selectedPerson);
                        MessageBox.Show("Persoana a fost stearsa cu succes!");
                    }
                    else
                    {
                        MessageBox.Show("Selecteaza o persoana din tabel.");
                    }
                    break;
                case "Ambalaj":
                    //obtinerea randului selectat din datagrid
                    var selectedAmbalaj = DataGridMagazin.SelectedItem as selectAmbalaj_Result;

                    if (selectedAmbalaj != null)
                    {
                        //actualizeaza persona in baza de date folosind procedura stocata
                        DeleteAmbalajFromDatabase(selectedAmbalaj);
                        MessageBox.Show("Ambalajul a fost sters cu succes!");
                    }
                    else
                    {
                        MessageBox.Show("Selecteaza un ambalaj din tabel.");
                    }
                    break;
                case "TipProdus":
                    //obtinerea randului selectat din datagrid
                    var selectedTipProdus = DataGridMagazin.SelectedItem as selectTipProduse_Result;

                    if (selectedTipProdus != null)
                    {
                        //actualizeaza persona in baza de date folosind procedura stocata
                        UpdateTipProdusInDatabase(selectedTipProdus);
                        MessageBox.Show("Tipul produsului a fost modificata cu succes!");
                    }
                    else
                    {
                        MessageBox.Show("Selecteaza un tip de produs din tabel.");
                    }
                    break;
            }
        }
    }
}
