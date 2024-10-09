using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
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
using FirstProject.Models;
using Zen.Barcode;
using System.Drawing.Imaging;
using System.Data.SqlClient;
using System.Data;
using System.Security.Claims;
using System.Drawing.Printing;
using System.Drawing;
using System.Net.Sockets;

namespace FirstProject.View
{
    /// <summary>
    /// Interaction logic for GenerareQRCodeBarcodeView.xaml
    /// </summary>
    public partial class GenerareQRCodeBarcodeView : Window
    {
        private readonly Store1Entities _context;
        public GenerareQRCodeBarcodeView()
        {
            InitializeComponent();
            _context = new Store1Entities();
        }
        private void ProdusSelect_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string produsIdText = ProdusSelect.Text;
                if (int.TryParse(produsIdText, out int produsId))
                {
                    PreiaCodQRDinBD(produsId);
                }
                else
                {
                    MessageBox.Show("Introdu un ID");
                }
            }
        }

        // Functia care apeleaza procedura stocată InfoCod și returnează datele sub formă de obiect
        private InfoCodModel PreiaInfoCodDinBD(string codQR)
        {
            InfoCodModel infoCod = null;

            try
            {
                MessageBox.Show($"Codul QR trimis: {codQR}");

                using (var sqlConnection = new SqlConnection(_context.Database.Connection.ConnectionString))
                {
                    using (var sqlCommand = new SqlCommand("dbo.InfoCod", sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.Add(new SqlParameter("@CodQR", codQR));

                        sqlConnection.Open();

                        using (var sqlDataReader = sqlCommand.ExecuteReader())
                        {
                            if (sqlDataReader.Read())
                            {
                                infoCod = new InfoCodModel
                                {
                                    IDProdus = Convert.ToInt32(sqlDataReader["IDProdus"]),
                                    NumeTipProdus = sqlDataReader["NumeTipProdus"].ToString(),
                                    DataExpirare = sqlDataReader["DataExpirare"].ToString()
                                };
                            }
                            else
                            {
                                MessageBox.Show("Nu s-au găsit date pentru codul QR specificat.");
                            }
                        }

                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la conectarea cu baza de date: {ex.Message}");
            }

            return infoCod;
        }



        // Definirea modelului InfoCodModel pentru a map structura rezultatelor procedurii stocate
        public class InfoCodModel
        {
            public int IDProdus { get; set; }
            public string NumeTipProdus { get; set; }
            public string DataExpirare { get; set; }
        }



        //Explicatii:SqlConnection și SqlCommand: Acestea sunt folosite pentru a stabili conexiunea la baza de date și a executa procedura stocată.
        // Adăugarea Parametrilor: Parametrul @IDProdus este adăugat la comanda SQL.Procedura stocată așteaptă acest parametru pentru a funcționa corect.
        //ExecuteReader(): Folosit pentru a executa comanda și a obține un SqlDataReader care poate fi utilizat pentru a citi datele returnate de procedură.
        //Verificarea și Citirea Rezultatelor: Verificăm dacă există date(prin Read()) și apoi citim valoarea din prima coloană a rândului rezultat.
        //Set de Rezultate: Procedura stocată returnează un set de rezultate (un tabel), iar codul C# trebuie să fie capabil să citească datele din acest set.
        // Coloana InfoQR: Asigură-te că numele coloanei InfoQR corespunde cu numele coloanei din setul de rezultate returnat de procedura stocată.

        private void PreiaCodQRDinBD(int produsId)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(_context.Database.Connection.ConnectionString))
                {
                    using (var sqlCommand = new SqlCommand("dbo.CodQR", sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;

                        // Adaugă parametrii necesari pentru procedura stocată
                        sqlCommand.Parameters.Add(new SqlParameter("@IDProdus", produsId));

                        sqlConnection.Open();

                        // Execute the command and get the result
                        using (var sqlDataReader = sqlCommand.ExecuteReader())
                        {
                            if (sqlDataReader.Read()) // Read the first row from the result
                            {
                                // Assuming the result has a single column named "InfoQR"
                                CodGenerat.Text = sqlDataReader["InfoQR"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("Nu s-au găsit date pentru produsul specificat.");
                            }
                        }

                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la conectarea cu baza de date: {ex.Message}");
            }
        }



        private void ButonQRCode_Click(object sender, RoutedEventArgs e)
        {
            /// Presupunem că codul QR a fost deja generat și stocat în `CodGenerat.Text` de `PreiaCodQRDinBD`
            string codQR = CodGenerat.Text;
            // Adăugăm un mesaj de debug pentru a verifica codul QR generat
            MessageBox.Show($"Cod QR Generat: {codQR}");

            // Verificăm dacă există un cod QR generat
            if (!string.IsNullOrEmpty(codQR))
            {
                // Apelăm procedura stocată InfoCod cu codul QR generat
                var infoCod = PreiaInfoCodDinBD(codQR);

                if (infoCod != null)
                {
                    // Concatenează datele pentru a le afișa în codul QR
                    string codQRData = $"ID Produs: {infoCod.IDProdus}\n" +
                                       $"Nume Tip Produs: {infoCod.NumeTipProdus}\n" +
                                       $"Data Expirare: {infoCod.DataExpirare}";

                    Zen.Barcode.CodeQrBarcodeDraw qrcode = Zen.Barcode.BarcodeDrawFactory.CodeQr;

                    // Generarea imaginii QR cu datele concatenate
                    var qrImage = qrcode.Draw(codQRData, 50);

                    // Convertim imaginea de tip System.Drawing.Image la BitmapImage care este suportată de WPF
                    using (var ms = new System.IO.MemoryStream())
                    {
                        qrImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        ms.Seek(0, System.IO.SeekOrigin.Begin);

                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.StreamSource = ms;
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.EndInit();

                        ImagineQRCode.Source = bitmapImage; // setăm sursa imaginii în controlul Image
                    }
                }
                else
                {
                    MessageBox.Show("Nu s-au găsit date pentru codul QR specificat.");
                }
            }
            else
            {
                MessageBox.Show("Nu există niciun cod QR generat.");
            }

        }

        private void ButonBarCode_Click(object sender, RoutedEventArgs e)
        {
            /// Presupunem că codul QR a fost deja generat și stocat în `CodGenerat.Text` de `PreiaCodQRDinBD`
            string codQR = CodGenerat.Text;
            // Adăugăm un mesaj de debug pentru a verifica codul QR generat
            MessageBox.Show($"Cod QR Generat: {codQR}");

            // Verificăm dacă există un cod QR generat
            if (!string.IsNullOrEmpty(codQR))
            {
                // Apelăm procedura stocată InfoCod cu codul QR generat
                var infoCod = PreiaInfoCodDinBD(codQR);

                if (infoCod != null)
                {
                    // Concatenează datele pentru a le afișa în codul QR
                    string codQRData = $"ID Produs: {infoCod.IDProdus}\n" +
                                       $"Nume Tip Produs: {infoCod.NumeTipProdus}\n" +
                                       $"Data Expirare: {infoCod.DataExpirare}";

                    Zen.Barcode.Code128BarcodeDraw barcode = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
                    // ImagineQRCode.Image = qrcode.Draw(CodGenerat.Text, 50);

                    var qrImage = barcode.Draw(CodGenerat.Text, 50);
                    // Convertim imaginea de tip System.Drawing.Image la BitmapImage care este suportată de WPF
                    using (var ms = new System.IO.MemoryStream())
                    {
                        qrImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        ms.Seek(0, System.IO.SeekOrigin.Begin);

                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.StreamSource = ms;
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.EndInit();

                        ImagineQRCode.Source = bitmapImage; // setăm sursa imaginii în controlul Image
                    }
                }
                else
                {
                    MessageBox.Show("Nu s-au găsit date pentru codul bara specificat.");
                }
            }
            else
            {
                MessageBox.Show("Nu există niciun cod QR generat.");
            }
        }

        private void ButonPrintCabQR_Click(object sender, RoutedEventArgs e)
        {
            /// Presupunem că codul QR a fost deja generat și stocat în `CodGenerat.Text` de `PreiaCodQRDinBD`
            string codQR = CodGenerat.Text;
            // Adăugăm un mesaj de debug pentru a verifica codul QR generat
            MessageBox.Show($"Cod QR Generat: {codQR}");

            // Verificăm dacă există un cod QR generat
            if (!string.IsNullOrEmpty(codQR))
            {
                // Apelăm procedura stocată InfoCod cu codul QR generat
                var infoCod = PreiaInfoCodDinBD(codQR);

                if (infoCod != null)
                {
                    // Concatenează datele pentru a le afișa în codul QR
                    string codQRData = $"ID Produs: {infoCod.IDProdus}\n" +
                                       $"Nume Tip Produs: {infoCod.NumeTipProdus}\n" +
                                       $"Data Expirare: {infoCod.DataExpirare}";
                    PrintlabelQR(codQRData);
                }
            }
        }
        private void PrintlabelQR(string codProdus)
        {
            string stLabel = "m m\n" +
"J\n" +
"H 100\n" +
"S l1;0,0,34,35,50\n" +
"O R\n" +
"B 10,20,0,QR,SC2;{codProdus}\n" +
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
        private void ButonPrintCabBarcode_Click(object sender, RoutedEventArgs e)
        {
            /// Presupunem că codul QR a fost deja generat și stocat în `CodGenerat.Text` de `PreiaCodQRDinBD`
            string codQR = CodGenerat.Text;
            // Adăugăm un mesaj de debug pentru a verifica codul QR generat
            MessageBox.Show($"Cod QR Generat: {codQR}");

            // Verificăm dacă există un cod QR generat
            if (!string.IsNullOrEmpty(codQR))
            {
                // Apelăm procedura stocată InfoCod cu codul QR generat
                var infoCod = PreiaInfoCodDinBD(codQR);

                if (infoCod != null)
                {
                    // Concatenează datele pentru a le afișa în codul QR
                    string codQRData = $"ID Produs: {infoCod.IDProdus}\n" +
                                       $"Nume Tip Produs: {infoCod.NumeTipProdus}\n" +
                                       $"Data Expirare: {infoCod.DataExpirare}";
                    PrintlabelBarcode(codQRData);
                }
            }
        }
        private void PrintlabelBarcode(string codProdus)
        {
            string stLabel = "m m\n" +
"J\n" +
"H 100\n" +
"S l1;0,0,34,35,50\n" +
"O R\n" +
"B 10,20,0,EAN-13,SC2;{codProdus}\n" +
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
        private void ButonPrint_Click(object sender, RoutedEventArgs e)
        {
            //verifica daca sursa imaginii e de tip bitmap
            if (ImagineQRCode.Source is BitmapImage bitmapImage)
            {
                //converteste bitmapimage in System.Drawing.Bitmap
                var bitmap = BitmanDinBitmapImage(bitmapImage);

                //imprima imaginea convertita
                PrintImage(bitmap);
            }
        }

        private System.Drawing.Bitmap BitmanDinBitmapImage(BitmapImage bitmapImage)
        {
            //creeaza un flux de memorie pentru a stoca datele imaginii
            using (MemoryStream outStreram = new MemoryStream())
            {
                //creeaza un encoder png pt a salva imaginea in fluxul de memorie
                PngBitmapEncoder encoder = new PngBitmapEncoder();

                //adauga frame-ul imaginii la encoder
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));

                encoder.Save(outStreram);

                //seteaza pozitia fluxului la inceput pentru a permite citirea
                outStreram.Seek(0, SeekOrigin.Begin);


                return new System.Drawing.Bitmap(outStreram);
            }
        }

        private void PrintImage(System.Drawing.Bitmap bitmap)
        {

            //crearea unui obiect printdocument pentru a gestiona procesul de imprimare
            PrintDocument printDocument = new PrintDocument();

            //adaugarea unui handler pt evenientu PrintPage care e declansat la imprimare
            printDocument.PrintPage += (sender, e) =>
            {
                //deseneaza imaginea pe pagina de imprimare in limitele specificate
                e.Graphics.DrawImage(bitmap, e.MarginBounds);
            };

            //crearea unui dialog de imprimare pt a selecta imprimanta si optiunile de imprimanta

            PrintDialog printDialog = new PrintDialog();

            //afiseaza dialogul si verifica daca utilizatorul a selectat o imprimanata si a dat ok
            if (printDialog.ShowDialog() == true)
            {
                //seteaza imprimanata selectata in setarile de imprimare
                printDocument.PrinterSettings.PrinterName = printDialog.PrintQueue.Name;

                //imprima documentul
                printDocument.Print();
            }

        }


    }
}
