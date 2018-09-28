using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OS2_RSA_AES_DigSig
{
    public partial class Form1 : Form
    {
        //Asimetrično kriptiranje-------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------

        static private string desktopLocation = Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory);
        string RSAfolder = System.IO.Path.Combine(desktopLocation, "RSA");
        string AESfolder = System.IO.Path.Combine(desktopLocation, "AES");

        private string privatniKljucRSA;
        private string javniKljucRSA;
        private byte[] digitalniPotpis;
        private byte[] digitalniPotpisAES;

        private string datotekaProcitano;
        private byte[] kriptiranaDatotekaProcitano;
        private string kriptiranaDatototekaProcitanoAES;

        public Form1()
        {
            InitializeComponent();
            txtLog.Text = txtLog.Text + "Dobro došli!" + Environment.NewLine;

            if (Directory.Exists(RSAfolder) && Directory.Exists(AESfolder))
            {
                txtLog.Text = txtLog.Text + "AES i RSA direktoriji već postoje!" + Environment.NewLine + "AES i RSA direktorij obrisani!" + Environment.NewLine;
                Directory.Delete(RSAfolder, true);
                Directory.Delete(AESfolder, true);
            }

            System.IO.Directory.CreateDirectory(RSAfolder);
            System.IO.Directory.CreateDirectory(AESfolder);
            txtLog.Text = txtLog.Text + "Kreirani novi direktoriji AES i RSA!" + Environment.NewLine;

            if(File.Exists(Path.Combine(desktopLocation, "log.txt")))
            {
                File.Delete(Path.Combine(desktopLocation, "log.txt"));
                txtLog.Text = txtLog.Text + "Izbrisana postojeća log datoteka!" + Environment.NewLine + Environment.NewLine;
            }

        }


        private void btnObrisiLog_Click(object sender, EventArgs e)
        {
            txtLog.Clear();
        }

        private void btnIzlaz_Click(object sender, EventArgs e)
        {
            
            string putanjaLog = Path.Combine(desktopLocation, "log.txt");
            if (File.Exists(putanjaLog))
            {
                File.Delete(putanjaLog);
            }

            string log = txtLog.Text;
            System.IO.File.WriteAllText(putanjaLog, log);

            DialogResult rezultat = MessageBox.Show("Želite li izbrisati sve stvorene datoteke?", "Pitanje", MessageBoxButtons.YesNo);

            if(rezultat == DialogResult.Yes)
            {
                if(Directory.Exists(RSAfolder) && Directory.Exists(AESfolder) || File.Exists(Path.Combine(desktopLocation, "test.txt")))
                {
                    Directory.Delete(RSAfolder, true);
                    Directory.Delete(AESfolder, true);
                    File.Delete(Path.Combine(desktopLocation, "test.txt"));
                    this.Close();
                }
            }
            if(rezultat == DialogResult.No)
            {
                this.Close();
            }

            
        }

        private void btnKreirajKljuceve_Click(object sender, EventArgs e)
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(2048);

            privatniKljucRSA = RSA.ToXmlString(true);
            javniKljucRSA = RSA.ToXmlString(false);

            string putanjaPrivatniKljuc = Path.Combine(RSAfolder, "privatni_kljuc.txt");
            string putanjaJavniKljuc = Path.Combine(RSAfolder, "javni_kljuc.txt");

            if (File.Exists(putanjaJavniKljuc) && File.Exists(putanjaPrivatniKljuc))
            {
                File.Delete(putanjaJavniKljuc);
                File.Delete(putanjaPrivatniKljuc);
                MessageBox.Show("Postojeći par ključeva izbrisan! \nKreiran je novi par ključeva!", "Obavijest");
                txtLog.Text = txtLog.Text + "RSA - Postojeći par ključeva izbrisan" + Environment.NewLine + "----------------" + Environment.NewLine;
            }

            System.IO.File.WriteAllText(putanjaPrivatniKljuc, privatniKljucRSA);
            System.IO.File.WriteAllText(putanjaJavniKljuc, javniKljucRSA);

            txtLog.Text = txtLog.Text + "RSA - Kreiran par ključeva" + Environment.NewLine + "----------------" + Environment.NewLine;

            btnOdaberiDatotekuRSA.Enabled = true;
            pbKreiranjeKljuceva.Visible = true;
        }

        private void btnOdaberiDatotekuRSA_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "txt files (*.txt)|*.txt";
            fileDialog.InitialDirectory = Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory);
            fileDialog.Title = "Odaberi datoteku";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = fileDialog.FileName;
                datotekaProcitano = System.IO.File.ReadAllText(fileName);
                txtLog.Text = txtLog.Text + "RSA - Iz datoteke pročitano: " + datotekaProcitano + Environment.NewLine + "----------------" + Environment.NewLine;
                btnKriptirajDatotekuRSA.Enabled = true;
                btnKreirajKljuceve.Enabled = false;
                btnOdaberiDatotekuRSA.Enabled = false;
                pbOdabirDatotekeRSA.Visible = true;
            }
            else
            {
                MessageBox.Show("Potrebno učitati datoteku za kriptiranje!");
            }

        }

        private void btnKriptirajDatotekuRSA_Click(object sender, EventArgs e)
        {
            btnKreirajKljuceve.Enabled = false;

            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(2048);
            RSA.FromXmlString(javniKljucRSA);

            byte[] porukaZaKriptiranje = Encoding.UTF8.GetBytes(datotekaProcitano);
            byte[] kriptiranaPoruka = RSA.Encrypt(porukaZaKriptiranje, false);

            string putanjaKriptiraniTekst = Path.Combine(RSAfolder, "kriptirana_poruka.txt");

            if (File.Exists(putanjaKriptiraniTekst))
            {
                File.Delete(putanjaKriptiraniTekst);
                txtLog.Text = txtLog.Text + "RSA - Postojeća kriptirana poruka obrisana" + Environment.NewLine + "----------------" + Environment.NewLine;
            }

            System.IO.File.WriteAllBytes(putanjaKriptiraniTekst, kriptiranaPoruka);
            txtLog.Text = txtLog.Text + "RSA - Poruka kriptirana" + Environment.NewLine + "----------------" + Environment.NewLine;

            SHA256CryptoServiceProvider SHA256 = new SHA256CryptoServiceProvider();
            RSA.FromXmlString(privatniKljucRSA);
            digitalniPotpis = RSA.SignData(kriptiranaPoruka, SHA256);
            string putanjaDigitalniPotpis = Path.Combine(RSAfolder, "SHA256.txt");

            if (File.Exists(putanjaDigitalniPotpis))
            {
                File.Delete(putanjaDigitalniPotpis);
                txtLog.Text = txtLog.Text + "RSA - Postojeći potpis obrisan!" + Environment.NewLine + "----------------" + Environment.NewLine;
            }

            System.IO.File.WriteAllBytes(putanjaDigitalniPotpis, digitalniPotpis);
            txtLog.Text = txtLog.Text + "RSA - Poruka digitalno potpisana!" + Environment.NewLine + "----------------" + Environment.NewLine;

            pbKriptiranjeDatotekeRSA.Visible = true;
            btnKriptirajDatotekuRSA.Enabled = false;
            btnOdaberiDatZaDekripRSA.Enabled = true;
        }

        private void btnOdaberiDatZaDekripRSA_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "txt files (*.txt)|*.txt";
            fileDialog.InitialDirectory = RSAfolder;
            fileDialog.Title = "Odaberi datoteku";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = fileDialog.FileName;
                kriptiranaDatotekaProcitano = System.IO.File.ReadAllBytes(fileName);
                pbOdaberiZaDekripRSA.Visible = true;
                btnProvjeriDigitalniRSA.Enabled = true;
                txtLog.Text = txtLog.Text + "RSA - Učitana datoteka za dekriptiranje" + Environment.NewLine + "----------------" + Environment.NewLine;
            }
            else
            {
                MessageBox.Show("Potrebno učitati datoteku za dekriptiranje!");
            }
        }

        private void btnProvjeriDigitalniRSA_Click(object sender, EventArgs e)
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(2048);
            RSA.FromXmlString(javniKljucRSA);
            SHA256CryptoServiceProvider SHA256 = new SHA256CryptoServiceProvider();

            if (RSA.VerifyData(kriptiranaDatotekaProcitano, SHA256, digitalniPotpis))
            {
                txtLog.Text = txtLog.Text + "RSA - Digitalni potpis: OK" + Environment.NewLine + "-----------------" + Environment.NewLine;
                pbProvjeriDigitalniRSA.Visible = true; 
                pbDigitalniRSAFalse.Visible = false; 
                pbDekriptirajRSA.Visible = false; 
                btnDekriptirajPorukuRSA.Enabled = true;  
            }
            else
            {
                MessageBox.Show("Digitalni potpis NIJE ispravan", "Obavijest");
                txtLog.Text = txtLog.Text + "RSA - Digitalni potpis: NIJE ISPRAVAN" + Environment.NewLine + "-----------------" + Environment.NewLine;
                pbProvjeriDigitalniRSA.Visible = false;  
                btnDekriptirajPorukuRSA.Enabled = false;  
                pbDigitalniRSAFalse.Visible = true;  
                pbDekriptirajRSA.Visible = false;   
            }
        }

        private void btnDekriptirajPorukuRSA_Click(object sender, EventArgs e)
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(2048);
            RSA.FromXmlString(privatniKljucRSA);

            byte[] dekriptiranaPoruka = RSA.Decrypt(kriptiranaDatotekaProcitano, false);
            string dekriptiraniTekstPoruke = Encoding.UTF8.GetString(dekriptiranaPoruka, 0, dekriptiranaPoruka.Length);

            string putanjaDekriptiranaPoruka = Path.Combine(RSAfolder, "dekriptirana_poruka.txt");

            if (File.Exists(putanjaDekriptiranaPoruka))
            {
                File.Delete(putanjaDekriptiranaPoruka);
                txtLog.Text = txtLog.Text + "RSA - Postojeća dekriptirana poruka je izbrisana!" + Environment.NewLine + "----------------" + Environment.NewLine;
            }

            System.IO.File.WriteAllText(putanjaDekriptiranaPoruka, dekriptiraniTekstPoruke);
            txtLog.Text = txtLog.Text + "RSA - Poruka uspješno dekriptirana" + Environment.NewLine + "-----------------" + Environment.NewLine;

            btnDekriptirajPorukuRSA.Enabled = false;
            btnProvjeriDigitalniRSA.Enabled = false;
            pbDekriptirajRSA.Visible = true;
        }

        //Simetrnično kriptiranje AES ------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------

        private string tajniKljucAES;
        private string datotekaProcitanoAES;

        private void btnKreirajTajniKljuc_Click(object sender, EventArgs e)
        {

            AESKriptiranjeDekriptiranje tajniKljuc = new AESKriptiranjeDekriptiranje();
            RijndaelManaged rijndael = tajniKljuc.VratiTajniKljuc();
            tajniKljucAES = Convert.ToBase64String(rijndael.Key);
        
            string putanjaTajniKljuc = Path.Combine(AESfolder, "tajni_kljuc.txt");

            if (File.Exists(putanjaTajniKljuc))
            {
                File.Delete(putanjaTajniKljuc);
                MessageBox.Show("Postojeći tajni ključ izbrisan!", "Obavijest");
                txtLog.Text = txtLog.Text + "AES - Postojeći tajni ključ izbrisan" + Environment.NewLine + "----------------" + Environment.NewLine;
            }

            System.IO.File.WriteAllText(putanjaTajniKljuc, tajniKljucAES);

            txtLog.Text = txtLog.Text + "AES - Kreiran tajni ključ" + Environment.NewLine + "----------------" + Environment.NewLine;

            pbTajniKljuc.Visible = true;
            btnOdaberiDatotekuAES.Enabled = true;
        }

        private void btnOdaberiDatotekuAES_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "txt files (*.txt)|*.txt";
            fileDialog.InitialDirectory = Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory);
            fileDialog.Title = "Odaberi datoteku";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = fileDialog.FileName;
                datotekaProcitanoAES = System.IO.File.ReadAllText(fileName);
                txtLog.Text = txtLog.Text + "AES - Iz datoteke pročitano: " + datotekaProcitanoAES + Environment.NewLine + "----------------" + Environment.NewLine;
                pbOdabirDatotekeAES.Visible = true;
                btnKriptirajDatotekuAES.Enabled = true;
                btnOdaberiDatotekuAES.Enabled = false;
                btnKreirajTajniKljuc.Enabled = false;
            }
            else
            {
                MessageBox.Show("Potrebno učitati datoteku za kriptiranje!");
            }
        }


        private RSACryptoServiceProvider RSAzaAES = new RSACryptoServiceProvider(1024);

        private void btnKriptirajDatotekuAES_Click(object sender, EventArgs e)
        {
            AESKriptiranjeDekriptiranje kriptiranje = new AESKriptiranjeDekriptiranje();

            string kriptiranaPoruka = kriptiranje.KriptirajAES(datotekaProcitanoAES);

            string putanjaKriptiranaPoruka = Path.Combine(AESfolder, "kriptirana_poruka.txt");

            if (File.Exists(putanjaKriptiranaPoruka))
            {
                File.Delete(putanjaKriptiranaPoruka);
                MessageBox.Show("Postojeća kriptirana poruka obrisana!", "Obavijest");
                txtLog.Text = txtLog.Text + "AES - Postojeća kriptirana poruka obrisana!" + Environment.NewLine + "----------------" + Environment.NewLine;
            }

            System.IO.File.WriteAllText(putanjaKriptiranaPoruka, kriptiranaPoruka);

            txtLog.Text = txtLog.Text + "AES - Kreirana kriptirana poruka!" + Environment.NewLine + "----------------" + Environment.NewLine;
            
            SHA256CryptoServiceProvider SHA256 = new SHA256CryptoServiceProvider();
            byte[] kriptiranaZaPotpisivanje = Encoding.UTF8.GetBytes(kriptiranaPoruka);
            digitalniPotpisAES = RSAzaAES.SignData(kriptiranaZaPotpisivanje, SHA256);
            string putanjaDigitalniPotpisAES = Path.Combine(AESfolder, "SHA256.txt");

            if (File.Exists(putanjaDigitalniPotpisAES))
            {
                File.Delete(putanjaDigitalniPotpisAES);
                txtLog.Text = txtLog.Text + "AES - Postojeći potpis obrisan!" + Environment.NewLine + "----------------" + Environment.NewLine;
            }

            System.IO.File.WriteAllBytes(putanjaDigitalniPotpisAES, digitalniPotpisAES);
            txtLog.Text = txtLog.Text + "AES - Poruka digitalno potpisana!" + Environment.NewLine + "----------------" + Environment.NewLine;

            pbKriptiranjeDatotekeAES.Visible = true;
            btnKriptirajDatotekuAES.Enabled = false;
            btnOdaveriDatZaDekAES.Enabled = true;
            

        }

        private void btnOdaveriDatZaDekAES_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "txt files (*.txt)|*.txt";
            fileDialog.InitialDirectory = AESfolder;
            fileDialog.Title = "Odaberi datoteku";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = fileDialog.FileName;
                kriptiranaDatototekaProcitanoAES = System.IO.File.ReadAllText(fileName);
                pbOdaberiZaDekAES.Visible = true;
                btnProvjeriDigitalniAES.Enabled = true;
                txtLog.Text = txtLog.Text + "AES - Učitana datoteka za dekriptiranje" + Environment.NewLine + "----------------" + Environment.NewLine;
            }
            else
            {
                MessageBox.Show("Potrebno učitati datoteku za dekriptiranje!");
            }
        }


        private void btnProvjeriDigitalniAES_Click(object sender, EventArgs e)
        {
            
            SHA256CryptoServiceProvider SHA256 = new SHA256CryptoServiceProvider();
            byte[] tekstZaProvjeru = Encoding.UTF8.GetBytes(kriptiranaDatototekaProcitanoAES);
  
                if (RSAzaAES.VerifyData(tekstZaProvjeru, SHA256, digitalniPotpisAES))
                {
                    txtLog.Text = txtLog.Text + "AES - Digitalni potpis: OK" + Environment.NewLine + "-----------------" + Environment.NewLine;
                    pbDigitalniAEStrue.Visible = true;
                    btnDekriptirajPorukuAES.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Digitalni potpis NIJE ispravan", "Obavijest");
                    txtLog.Text = txtLog.Text + "AES - Digitalni potpis: NIJE ISPRAVAN" + Environment.NewLine + "-----------------" + Environment.NewLine;
                    pbDigitalniAEStrue.Visible = false;
                    btnDekriptirajPorukuAES.Enabled = false;
                    pbDigitalniAESFalse.Visible = true;
                    pbDekriptirajAES.Visible = false;
                }
            
        }


        private void btnDekriptirajPorukuAES_Click(object sender, EventArgs e)
        {
            AESKriptiranjeDekriptiranje kriptiranje = new AESKriptiranjeDekriptiranje();

            string dekriptiranaPoruka = kriptiranje.DekriptirajAES(kriptiranaDatototekaProcitanoAES);

            string putanjaDekriptiranaPoruka = Path.Combine(AESfolder, "dekriptirana_poruka.txt");

            if (File.Exists(putanjaDekriptiranaPoruka))
            {
                File.Delete(putanjaDekriptiranaPoruka);
                txtLog.Text = txtLog.Text + "AES - Postojeća dekriptirana poruka je izbrisana!" + Environment.NewLine + "----------------" + Environment.NewLine;
            }

            System.IO.File.WriteAllText(putanjaDekriptiranaPoruka, dekriptiranaPoruka);
            txtLog.Text = txtLog.Text + "AES - Poruka uspješno dekriptirana" + Environment.NewLine + "-----------------" + Environment.NewLine;

            btnDekriptirajPorukuAES.Enabled = false;
            btnProvjeriDigitalniAES.Enabled = false;
            pbDekriptirajAES.Visible = true;
        }

        private void btnKreirajFile_Click(object sender, EventArgs e)
        {
            txtLog.Text = txtLog.Text + "Kreiranje test datoteke" + Environment.NewLine + "-----------------" + Environment.NewLine;
            KreirajDatoteku kreiranjeDatotekeForma = new KreirajDatoteku();
            kreiranjeDatotekeForma.ShowDialog();           
        }

        private void txtLog_TextChanged(object sender, EventArgs e)
        {
            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.ScrollToCaret();
            txtLog.Refresh();
        }
    }
    
}

