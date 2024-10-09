using FirstProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
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
    /// Interaction logic for ReceptiaView.xaml
    /// </summary>
    public partial class ReceptiaView : Window
    {
        //private MagazinEntities context;
        private FirstProject.Models.Store1Entities context;
        public ReceptiaView()
        {
            InitializeComponent();
            Loaded += MyWindow_Loaded;
        }
        private void MyWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // do work here
        }

        private void Confirmare_Click(object sender, RoutedEventArgs e)
        {
            // LoadProdusData();
            //ProdusSelectare.ItemsSource =

            int idProdus = int.Parse(ProdusSelect.Text);
            int idPersoana = int.Parse(PersoanaSelect.Text);
            //Pentru a extrage valoarea bifată (checked) a unui CheckBox, ar trebui să accesezi proprietatea IsChecked, care returnează un bool? (nullable bool).
            //ButonRebut.IsChecked returnează un bool?, adică poate fi true, false sau null.
            //Operatorul ?? asigură că dacă valoarea este null, rebutText va fi setat la false.
            bool rebutText = ButonRebut.IsChecked ?? false;
            string observatiiprodText = ObservatiiProdus.Text;
            try
            {
                using (var context = new Store1Entities())
                {
                    //Object parameter :este o clasă folosită în Entity Framework pentru a reprezenta un parametru de ieșire (sau de intrare/ieșire) pentru procedurile stocate din SQL Server.
                    //Când execuți o procedură stocată care returnează valori prin parametri de ieșire, ObjectParameter permite captarea acestor valori după ce procedura a fost executată.
                    // Crearea parametrului pentru valoarea returnată
                    ObjectParameter idReceptie = new ObjectParameter("IDReceptie", typeof(int));

                    // Apelarea procedurii stocate
                    context.insertReceptie(idReceptie, idProdus, idPersoana, observatiiprodText, rebutText);

                    // Salvare modificări în baza de date
                    context.SaveChanges();

                    MessageBox.Show("Tipul de produs a fost adăugat cu succes! ID: " + idReceptie.Value);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare" + ex.Message);
            }

        }
        //private void Confirmare_Click(object sender, RoutedEventArgs e)
        //{
        //    // Declarăm variabile pentru ID-urile care vor fi extrase din controale
        //    int idProdus;
        //    int idPersoana;

        //    // Verificăm dacă textul din `ProdusSelect` poate fi convertit într-un număr întreg
        //    if (!int.TryParse(ProdusSelect.Text, out idProdus))
        //    {
        //        MessageBox.Show("Introduceți un ID valid pentru produs.");
        //        return;
        //    }

        //    // Verificăm dacă textul din `PersoanaSelect` poate fi convertit într-un număr întreg
        //    if (!int.TryParse(PersoanaSelect.Text, out idPersoana))
        //    {
        //        MessageBox.Show("Introduceți un ID valid pentru persoană.");
        //        return;
        //    }

        //    // Extragem starea checkbox-ului și textul din câmpul ObservatiiProdus
        //    bool rebutText = ButonRebut.IsChecked ?? false;
        //    string observatiiprodText = ObservatiiProdus.Text;

        //    try
        //    {
        //        using (var context = new Store1Entities())
        //        {
        //            // Creăm parametrul pentru valoarea returnată de procedura stocată
        //            ObjectParameter idReceptie = new ObjectParameter("IDReceptie", typeof(int));

        //            // Apelăm procedura stocată pentru a insera o recepție
        //            context.insertReceptie(idReceptie, idProdus, idPersoana, observatiiprodText, rebutText);

        //            // Salvăm modificările în baza de date
        //            context.SaveChanges();

        //            // Afișăm un mesaj de confirmare
        //            MessageBox.Show("Tipul de produs a fost adăugat cu succes! ID: " + idReceptie.Value);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Eroare: " + ex.Message);
        //    }
        //}


        private void LoadProdusData()
        {
            try
            {
                using (var context = new Store1Entities())
                {
                    //creeare parametru pentru valoarea returnata
                    ObjectParameter idProdusParameter = new ObjectParameter("IDProdus", typeof(int));

                    //executie procedura stocata
                    var result = context.selectProdus();

                    //Popularea datagrid-urilor cu date

                    // ProdusSelectare.ItemsSource = result;
                    //ProdusSelectare.DisplayMemberPath = "";

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la încărcarea datelor: " + ex.Message);
            }
        }

        private void ProdusSelect_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string produsIdText = ProdusSelect.Text;
                if (int.TryParse(produsIdText, out int produsId))
                {
                    // Apelează stored procedure-ul și verifică dacă ID-ul există
                    if (VerificareProdus(produsId))
                    {
                        MessageBox.Show("Produsul este valid");
                        ButonRebut.IsChecked = false;
                        ObservatiiProdus.Text = "Produsul este valid!";
                    }
                    else
                    {
                        MessageBox.Show("Produsul nu este valid!");
                        ButonRebut.IsChecked = true;
                        ObservatiiProdus.Text = "Produsul este expirat!";
                    }
                }
                else
                {
                    MessageBox.Show("Introdu un ID");
                }
            }
        }
        private void PersoanaSelect_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string produsIdText = ProdusSelect.Text;
                if (int.TryParse(produsIdText, out int produsId))
                {
                    //if (VerificareProdus(produsId))
                    //{
                    if (PersoanaSelect.Text != null)
                    {
                        string persoanaIdText = PersoanaSelect.Text;
                        if (int.TryParse(persoanaIdText, out int persoanaId))
                        {
                            // Apelează stored procedure-ul și verifică dacă ID-ul există
                            if (VerificarePersoana(persoanaId, produsId))
                            {
                                MessageBox.Show("Persoana este valida!");
                                //ButonRebut.IsChecked = true;
                                //ObservatiiProdus.Text = "Produsul este valid!";
                            }
                            else
                            {
                                MessageBox.Show("Persoana nu este valida!");
                                PersoanaSelect.Text = null;
                                //ButonRebut.IsChecked = false;
                                //ObservatiiProdus.Text = "Produsul este expirat!";
                            }

                        }
                    }
                    else
                    {
                        MessageBox.Show("Introdu un ID la Persoana");
                    }
                    //}
                    //else
                    //{
                    //    MessageBox.Show("Produsul nu este valid. Verificați produsul înainte de a verifica persoana.");
                    //}
                }
                else
                {
                    MessageBox.Show("Introdu un ID valid la Produs.");
                }
            }
        }

        private bool VerificareProdus(int produsId)
        {
            using (var context = new Store1Entities())
            {
                //        //creeare parametru pentru valoarea returnata
                //        //ObjectParameter idProdusParameter = new ObjectParameter("IDProdus", typeof(int));
                //        ObjectParameter idReceptieParameter = new ObjectParameter("IDReceptie", typeof(int));
                //        ObjectParameter idProdusParameter = new ObjectParameter("IDProdus", produsId);

                //        //executie procedura stocata
                //        //var produse = context.selectProdus().ToList();
                //        var rezultat = context.Database.ExecuteSqlCommand(
                //    "EXEC verificareLaReceptie @IDReceptie OUTPUT, @IDProdus",
                //    idReceptieParameter,
                //    idProdusParameter
                //);
                // creeaza parametrii pentru procedura stocata
                //initializeaza variabila cu id din baza de date
                var idReceptieParameter = new SqlParameter("@IDReceptie", System.Data.SqlDbType.Int)
                {
                    Direction = System.Data.ParameterDirection.Output
                };
                //ii atribuie variabilei id-ul din produs
                var idProdusParameter = new SqlParameter("@IDProdus", produsId);

                // executa procedura stocata, pt verififcarea daca idul introdus este bun
                context.Database.ExecuteSqlCommand(
                    "EXEC verificareLaReceptie @IDReceptie OUTPUT, @IDProdus",
                    idReceptieParameter,
                    idProdusParameter
                );


                ////var produse=context.selectProdus().ToList();
                //var produs = produse.FirstOrDefault(p => p.IDProdus == produsId);
                //if (produs != null)
                //{
                //    return true;
                //}
                //else
                //{
                //    return false;
                //}
                if (idReceptieParameter.Value != DBNull.Value)
                {
                    int idReceptie = (int)idReceptieParameter.Value;
                    return true; // Produs valid
                }
                else
                {
                    return false; // Produs invalid
                }
            }
        }
        //private bool VerificarePersoana(int produsId)
        //{
        //    return true;
        //}
        //private bool VerificarePersoana(int persoanaId, int produsId)
        //{
        //    using (var context = new Store1Entities())
        //    {

        //        //executare store procedures selectPersoana și verifică dacă ID-ul persoanei este valid
        //        var persoane = context.selectPersoana().ToList();
        //        var persoana = persoane.FirstOrDefault(p => p.IDPersoana == persoanaId);
        //        var produse = context.selectProdus().ToList();
        //        var produs = produse.FirstOrDefault(prod => prod.IDProdus == produsId);
        //        if (persoana != null && produs.IDPersoana != persoanaId)
        //        {
        //            return true;
        //        }
        //        else
        //        { return false; }
        //    }

        //}
        private bool VerificarePersoana(int persoanaId, int produsId)
        {
            using (var context = new Store1Entities())
            {
                // Execută procedura pentru a obține lista de persoane
                var persoane = context.selectPersoana().ToList();
                var persoana = persoane.FirstOrDefault(p => p.IDPersoana == persoanaId);

                // Execută procedura pentru a obține lista de produse
                var produse = context.selectProdus().ToList();
                var produs = produse.FirstOrDefault(prod => prod.IDProdus == produsId);

                // Verifică dacă persoana și produsul există și dacă numele persoanei corespunde
                if (persoana != null && produs != null && produs.Persoana != persoana.Nume)
                {
                    return true;  // Returnează true dacă persoana nu corespunde
                }
                else
                {
                    return false;  // Returnează false dacă persoana corespunde
                }
            }
        }


    }
}
