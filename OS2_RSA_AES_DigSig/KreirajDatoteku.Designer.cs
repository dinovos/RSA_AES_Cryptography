namespace OS2_RSA_AES_DigSig
{
    partial class KreirajDatoteku
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KreirajDatoteku));
            this.label1 = new System.Windows.Forms.Label();
            this.txtTekstZaKriptiranje = new System.Windows.Forms.TextBox();
            this.btnKreirajDatoteku = new System.Windows.Forms.Button();
            this.btnOdustani = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tekst za kriptiranje:";
            // 
            // txtTekstZaKriptiranje
            // 
            this.txtTekstZaKriptiranje.Location = new System.Drawing.Point(13, 34);
            this.txtTekstZaKriptiranje.Multiline = true;
            this.txtTekstZaKriptiranje.Name = "txtTekstZaKriptiranje";
            this.txtTekstZaKriptiranje.Size = new System.Drawing.Size(329, 101);
            this.txtTekstZaKriptiranje.TabIndex = 1;
            // 
            // btnKreirajDatoteku
            // 
            this.btnKreirajDatoteku.Location = new System.Drawing.Point(267, 160);
            this.btnKreirajDatoteku.Name = "btnKreirajDatoteku";
            this.btnKreirajDatoteku.Size = new System.Drawing.Size(75, 23);
            this.btnKreirajDatoteku.TabIndex = 2;
            this.btnKreirajDatoteku.Text = "Kreiraj";
            this.btnKreirajDatoteku.UseVisualStyleBackColor = true;
            this.btnKreirajDatoteku.Click += new System.EventHandler(this.btnKreirajDatoteku_Click);
            // 
            // btnOdustani
            // 
            this.btnOdustani.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOdustani.Location = new System.Drawing.Point(13, 157);
            this.btnOdustani.Name = "btnOdustani";
            this.btnOdustani.Size = new System.Drawing.Size(75, 23);
            this.btnOdustani.TabIndex = 3;
            this.btnOdustani.Text = "Odustani";
            this.btnOdustani.UseVisualStyleBackColor = true;
            this.btnOdustani.Click += new System.EventHandler(this.btnOdustani_Click);
            // 
            // KreirajDatoteku
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnOdustani;
            this.ClientSize = new System.Drawing.Size(354, 195);
            this.Controls.Add(this.btnOdustani);
            this.Controls.Add(this.btnKreirajDatoteku);
            this.Controls.Add(this.txtTekstZaKriptiranje);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KreirajDatoteku";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kreiraj test datoteku";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTekstZaKriptiranje;
        private System.Windows.Forms.Button btnKreirajDatoteku;
        private System.Windows.Forms.Button btnOdustani;
    }
}