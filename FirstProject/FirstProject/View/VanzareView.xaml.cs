using FirstProject.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Sockets;
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
    /// Interaction logic for VanzareView.xaml
    /// </summary>
    public partial class VanzareView : Window
    {
        private FirstProject.Models.Store1Entities context;
        public VanzareView()
        {
            InitializeComponent();
            context = new Store1Entities();
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
                    }
                    else
                    {
                        MessageBox.Show("Produsul nu este valid!");
                    }
                }
                else
                {
                    MessageBox.Show("Introdu un ID");
                }
            }
        }
        private bool VerificareProdus(int produsId)
        {
            using (var context = new Store1Entities())
            {

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
        private void Buton_Click(object sender, RoutedEventArgs e)
        {
            Printlabel(1);
        }

        private void ButonConfirmare_Click(object sender, RoutedEventArgs e)
        {
            string produsIdText = ProdusSelect.Text;
            if (int.TryParse(produsIdText, out int produsId))
            {
                //preluarea detei din calendar
                DateTime dataVanzare = DataVanzare.SelectedDate ?? DateTime.Now;

                // Inițializează contextul doar dacă nu a fost inițializat anterior
                if (context == null)
                {
                    context = new Store1Entities();
                }

                // Caută produsul după ID
                var produs = context.Produses.FirstOrDefault(p => p.IDProdus == produsId);

                //!! determinare element din tabela care are id-ul respectv
                // Caută produsul după ID
                // var produs = context.Produses.FirstOrDefault(p => p.IDProdus == produsId);

                var resultatConfirmare = VerificareVanzari(produsId, dataVanzare);

                if (resultatConfirmare)
                {
                    MessageBox.Show("Produsul este vandut");
                    produs.Status = "vandut";
                    context.SaveChanges();
                }
                else
                {

                    MessageBox.Show("Produsul nu se poate vinde");
                    produs.Status = "nevandut";
                    context.SaveChanges();
                }

            }
        }
        private bool VerificareVanzari(int prodId, DateTime dataVanzare)
        {
            using (var context = new Store1Entities())
            {
                //initializare parametru de iesire pt idvanzare(crearea sa,care este automata)
                var idVanzareParameter = new SqlParameter("@IDVanzari", System.Data.SqlDbType.Int)
                {
                    Direction = System.Data.ParameterDirection.Output
                };
                var produsIdParameter = new SqlParameter("@IDProdus", prodId);

                //executare stored procedure completVerificare
                context.Database.ExecuteSqlCommand(
                   "EXEC completVerificareVanzari @IDVanzari OUTPUT, @IDProdus",
            idVanzareParameter,
            produsIdParameter
                    );

                if (idVanzareParameter.Value != DBNull.Value)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }

        private void Printlabel(int idprodus)
        {
            string stLabel = "m m\n" +
"J\n" +
"H 100\n" +
"S l1;0,0,68,70,100\n" +
"O R\n" +
"T 10,10,0,5,pt20;sample\n" +
"B 10,20,0,EAN-13,SC2;401234512345\n" +
"G 8,4,0;R:30,9,0.3,0.3\n" +
"A 1\n";

            TcpClient tcpClient = new TcpClient("10.58.100.67", 9100);
            tcpClient.Client.Send(Encoding.ASCII.GetBytes(stLabel));
            tcpClient.Client.Send(new byte[] { 0x1B, 0x67 });
            tcpClient.Client.Shutdown(SocketShutdown.Both);
            tcpClient.Client.Disconnect(false);
            tcpClient.Client.Close();
            tcpClient.Close();
            tcpClient = null;
        }
    }
}
