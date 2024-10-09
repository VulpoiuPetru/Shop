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

namespace FirstProject.View
{
    /// <summary>
    /// Interaction logic for AdaugareElemView.xaml
    /// </summary>
    public partial class AdaugareElemView : Window
    {
        public AdaugareElemView()
        {
            InitializeComponent();

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
                    PersoanaPanel.Visibility = Visibility.Visible;
                    break;
                case "Ambalaj":
                    AmbalajPanel.Visibility = Visibility.Visible;
                    break;
                case "TipProdus":
                    TipProdusPanel.Visibility = Visibility.Visible;
                    break;
                case "Produs":
                    ProdusPanel.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void AdaugareP_Click(object sender, RoutedEventArgs e)
        {
            string nume = NumeP.Text;

            try
            {
                using (var context = new Store1Entities())
                {
                    //Object parameter :este o clasă folosită în Entity Framework pentru a reprezenta un parametru de ieșire (sau de intrare/ieșire) pentru procedurile stocate din SQL Server.
                    //Când execuți o procedură stocată care returnează valori prin parametri de ieșire, ObjectParameter permite captarea acestor valori după ce procedura a fost executată.
                    // Crearea parametrului pentru valoarea returnată
                    ObjectParameter idPersoanaParameter = new ObjectParameter("IDPersoana", typeof(int));

                    // Apelarea procedurii stocate
                    context.insertPersoana(nume, idPersoanaParameter);

                    // Salvare modificări în baza de date
                    context.SaveChanges();

                    MessageBox.Show("Persoana a fost adăugată cu succes! ID: " + idPersoanaParameter.Value);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare" + ex.Message);
            }

        }

        private void AdugareA_Click(object sender, RoutedEventArgs e)
        {
            string denumire = DenumireA.Text;

            try
            {
                using (var context = new Store1Entities())
                {
                    //Object parameter :este o clasă folosită în Entity Framework pentru a reprezenta un parametru de ieșire (sau de intrare/ieșire) pentru procedurile stocate din SQL Server.
                    //Când execuți o procedură stocată care returnează valori prin parametri de ieșire, ObjectParameter permite captarea acestor valori după ce procedura a fost executată.
                    // Crearea parametrului pentru valoarea returnată
                    ObjectParameter idAmbalajParameter = new ObjectParameter("IDAmbalaj", typeof(int));

                    // Apelarea procedurii stocate
                    context.insertAmbalaj(denumire, idAmbalajParameter);

                    // Salvare modificări în baza de date
                    context.SaveChanges();

                    MessageBox.Show("Ambalaj a fost adăugată cu succes! ID: " + idAmbalajParameter.Value);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare" + ex.Message);
            }
        }

        private void AdugareTP_Click(object sender, RoutedEventArgs e)
        {
            string nume = NumeTP.Text;
            string unitateDeMasura = UnitatemasuareTP.Text;
            //transformare din string in int
            int pretKg = int.Parse(PretKgTP.Text);
            int cantitate = int.Parse(CantitateTP.Text);
            int termenGarantie = int.Parse(TermengarantieTP.Text);

            try
            {
                using (var context = new Store1Entities())
                {
                    //Object parameter :este o clasă folosită în Entity Framework pentru a reprezenta un parametru de ieșire (sau de intrare/ieșire) pentru procedurile stocate din SQL Server.
                    //Când execuți o procedură stocată care returnează valori prin parametri de ieșire, ObjectParameter permite captarea acestor valori după ce procedura a fost executată.
                    // Crearea parametrului pentru valoarea returnată
                    ObjectParameter idTipProdus = new ObjectParameter("IDTipProdus", typeof(int));

                    // Apelarea procedurii stocate
                    context.insertTipProduse(nume, unitateDeMasura, pretKg, cantitate, termenGarantie, idTipProdus);

                    // Salvare modificări în baza de date
                    context.SaveChanges();

                    MessageBox.Show("Tipul de produs a fost adăugat cu succes! ID: " + idTipProdus.Value);
                }
            }
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Eroare" + ex.Message);
            //}
            catch (Exception ex)
            {
                MessageBox.Show("Eroare: " + ex.Message + "\nDetalii: " + ex.InnerException?.Message);
            }

        }

        private void AdaugareProd_Click(object sender, RoutedEventArgs e)
        {
            int idAmbalaj = int.Parse(AmbalajPro.Text);
            int idPersoana = int.Parse(PersoanaPro.Text);
            int idTipProdus = int.Parse(TipProdusPro.Text);
            //  string status = "nevandut";
            string status = null;

            try
            {
                using (var context = new Store1Entities())
                {
                    // Crearea parametrului pentru valoarea returnată a ID-ului produsului
                    var idProdusParameter = new ObjectParameter("IDProdus", typeof(int));

                    // Apelarea procedurii stocate
                    context.insertProduse(idProdusParameter, idAmbalaj, idPersoana, status, idTipProdus);

                    // Salvarea modificărilor în baza de date
                    context.SaveChanges();

                    // Afișarea ID-ului produsului generat
                    MessageBox.Show("Produsul a fost adăugat cu succes! ID: " + idProdusParameter.Value);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare: " + ex.Message);
            }
        }
    }
}
