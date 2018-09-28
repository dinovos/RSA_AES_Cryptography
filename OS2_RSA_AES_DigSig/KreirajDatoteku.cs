using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OS2_RSA_AES_DigSig
{
    public partial class KreirajDatoteku : Form
    {

        static private string desktopLocation = Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory);
        public KreirajDatoteku()
        {
            InitializeComponent();
        }

        private void btnOdustani_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnKreirajDatoteku_Click(object sender, EventArgs e)
        {
            string putanjaTestDatoteka = Path.Combine(desktopLocation, "test.txt");

            if (File.Exists(putanjaTestDatoteka))
            {
                File.Delete(putanjaTestDatoteka);
            }

            System.IO.File.WriteAllText(putanjaTestDatoteka, txtTekstZaKriptiranje.Text);

            this.Close();
        }
    }
}
