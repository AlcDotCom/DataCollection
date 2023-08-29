namespace Downtime_registration
{
    partial class Form5
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form5));
            this.button2 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.txtID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtPrestoj = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtKategoriaPrestoja = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtLinka = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtDatumZapisu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtZaciatokPrestoja = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtKoniecPrestoja = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtTrvaniePrestoja = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtZmena = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtHotovyVyrobok = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtVstupnyMaterial = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtStanica = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtDetail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtZastavenieVyroby = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtPoruchuOdstranil = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtPricinaAleboPopisPoruchy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtVykonaneOpatrenieOprava = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtStrataOperator = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtCenaOperator1hod = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtRezerva1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtRezerva2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtRezerva3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtRezerva4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtRezerva5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button2.BackColor = System.Drawing.SystemColors.Info;
            this.button2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button2.Location = new System.Drawing.Point(19, 539);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(180, 67);
            this.button2.TabIndex = 2;
            this.button2.Text = "Späť";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.txtID,
            this.txtPrestoj,
            this.txtKategoriaPrestoja,
            this.txtLinka,
            this.txtDatumZapisu,
            this.txtZaciatokPrestoja,
            this.txtKoniecPrestoja,
            this.txtTrvaniePrestoja,
            this.txtZmena,
            this.txtHotovyVyrobok,
            this.txtVstupnyMaterial,
            this.txtStanica,
            this.txtDetail,
            this.txtZastavenieVyroby,
            this.txtPoruchuOdstranil,
            this.txtPricinaAleboPopisPoruchy,
            this.txtVykonaneOpatrenieOprava,
            this.txtStrataOperator,
            this.txtCenaOperator1hod,
            this.txtStatus,
            this.txtRezerva1,
            this.txtRezerva2,
            this.txtRezerva3,
            this.txtRezerva4,
            this.txtRezerva5});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Location = new System.Drawing.Point(12, 43);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.Size = new System.Drawing.Size(921, 449);
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged_1);
            this.dataGridView1.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dataGridView1_UserDeletingRow_1);
            // 
            // txtID
            // 
            this.txtID.DataPropertyName = "ID";
            this.txtID.HeaderText = "ID (P.č.)";
            this.txtID.MinimumWidth = 8;
            this.txtID.Name = "txtID";
            this.txtID.ReadOnly = true;
            this.txtID.Visible = false;
            this.txtID.Width = 150;
            // 
            // txtPrestoj
            // 
            this.txtPrestoj.DataPropertyName = "Prestoj";
            this.txtPrestoj.HeaderText = "Prestoj";
            this.txtPrestoj.MinimumWidth = 8;
            this.txtPrestoj.Name = "txtPrestoj";
            this.txtPrestoj.ReadOnly = true;
            this.txtPrestoj.Width = 125;
            // 
            // txtKategoriaPrestoja
            // 
            this.txtKategoriaPrestoja.DataPropertyName = "KategoriaPrestoja";
            this.txtKategoriaPrestoja.HeaderText = "Kategória Prestoja";
            this.txtKategoriaPrestoja.MinimumWidth = 8;
            this.txtKategoriaPrestoja.Name = "txtKategoriaPrestoja";
            this.txtKategoriaPrestoja.ReadOnly = true;
            this.txtKategoriaPrestoja.Width = 125;
            // 
            // txtLinka
            // 
            this.txtLinka.DataPropertyName = "Linka";
            this.txtLinka.HeaderText = "Linka";
            this.txtLinka.MinimumWidth = 8;
            this.txtLinka.Name = "txtLinka";
            this.txtLinka.ReadOnly = true;
            this.txtLinka.Width = 150;
            // 
            // txtDatumZapisu
            // 
            this.txtDatumZapisu.DataPropertyName = "DatumZapisu";
            this.txtDatumZapisu.HeaderText = "Čas a Dátum Zápisu";
            this.txtDatumZapisu.MinimumWidth = 8;
            this.txtDatumZapisu.Name = "txtDatumZapisu";
            this.txtDatumZapisu.ReadOnly = true;
            this.txtDatumZapisu.Width = 120;
            // 
            // txtZaciatokPrestoja
            // 
            this.txtZaciatokPrestoja.DataPropertyName = "ZaciatokPrestoja";
            this.txtZaciatokPrestoja.HeaderText = "Začiatok Prestoja";
            this.txtZaciatokPrestoja.MinimumWidth = 8;
            this.txtZaciatokPrestoja.Name = "txtZaciatokPrestoja";
            this.txtZaciatokPrestoja.ReadOnly = true;
            this.txtZaciatokPrestoja.Width = 65;
            // 
            // txtKoniecPrestoja
            // 
            this.txtKoniecPrestoja.DataPropertyName = "KoniecPrestoja";
            this.txtKoniecPrestoja.HeaderText = "Koniec Prestoja";
            this.txtKoniecPrestoja.MinimumWidth = 8;
            this.txtKoniecPrestoja.Name = "txtKoniecPrestoja";
            this.txtKoniecPrestoja.ReadOnly = true;
            this.txtKoniecPrestoja.Width = 65;
            // 
            // txtTrvaniePrestoja
            // 
            this.txtTrvaniePrestoja.DataPropertyName = "TrvaniePrestoja";
            this.txtTrvaniePrestoja.HeaderText = "Trvanie Prestoja";
            this.txtTrvaniePrestoja.MinimumWidth = 8;
            this.txtTrvaniePrestoja.Name = "txtTrvaniePrestoja";
            this.txtTrvaniePrestoja.ReadOnly = true;
            this.txtTrvaniePrestoja.Width = 110;
            // 
            // txtZmena
            // 
            this.txtZmena.DataPropertyName = "Zmena";
            this.txtZmena.HeaderText = "Zmena";
            this.txtZmena.MinimumWidth = 8;
            this.txtZmena.Name = "txtZmena";
            this.txtZmena.ReadOnly = true;
            this.txtZmena.Width = 125;
            // 
            // txtHotovyVyrobok
            // 
            this.txtHotovyVyrobok.DataPropertyName = "HotovyVyrobok";
            this.txtHotovyVyrobok.HeaderText = "Hotový Výrobok";
            this.txtHotovyVyrobok.MinimumWidth = 8;
            this.txtHotovyVyrobok.Name = "txtHotovyVyrobok";
            this.txtHotovyVyrobok.ReadOnly = true;
            this.txtHotovyVyrobok.Width = 125;
            // 
            // txtVstupnyMaterial
            // 
            this.txtVstupnyMaterial.DataPropertyName = "VstupnyMaterial";
            this.txtVstupnyMaterial.HeaderText = "Vstupný Materiál";
            this.txtVstupnyMaterial.MinimumWidth = 8;
            this.txtVstupnyMaterial.Name = "txtVstupnyMaterial";
            this.txtVstupnyMaterial.ReadOnly = true;
            this.txtVstupnyMaterial.Width = 125;
            // 
            // txtStanica
            // 
            this.txtStanica.DataPropertyName = "Stanica";
            this.txtStanica.HeaderText = "Stanica";
            this.txtStanica.MinimumWidth = 8;
            this.txtStanica.Name = "txtStanica";
            this.txtStanica.ReadOnly = true;
            this.txtStanica.Width = 70;
            // 
            // txtDetail
            // 
            this.txtDetail.DataPropertyName = "Detail";
            this.txtDetail.HeaderText = "Detail";
            this.txtDetail.MinimumWidth = 8;
            this.txtDetail.Name = "txtDetail";
            this.txtDetail.ReadOnly = true;
            this.txtDetail.Width = 150;
            // 
            // txtZastavenieVyroby
            // 
            this.txtZastavenieVyroby.DataPropertyName = "ZastavenieVyroby";
            this.txtZastavenieVyroby.HeaderText = "Zastavenie Výroby";
            this.txtZastavenieVyroby.MinimumWidth = 8;
            this.txtZastavenieVyroby.Name = "txtZastavenieVyroby";
            this.txtZastavenieVyroby.ReadOnly = true;
            this.txtZastavenieVyroby.Width = 85;
            // 
            // txtPoruchuOdstranil
            // 
            this.txtPoruchuOdstranil.DataPropertyName = "PoruchuOdstranil";
            this.txtPoruchuOdstranil.HeaderText = "Poruchu Odstránil";
            this.txtPoruchuOdstranil.MinimumWidth = 8;
            this.txtPoruchuOdstranil.Name = "txtPoruchuOdstranil";
            this.txtPoruchuOdstranil.ReadOnly = true;
            this.txtPoruchuOdstranil.Width = 175;
            // 
            // txtPricinaAleboPopisPoruchy
            // 
            this.txtPricinaAleboPopisPoruchy.DataPropertyName = "PricinaAleboPopisPoruchy";
            this.txtPricinaAleboPopisPoruchy.HeaderText = "Príčina Alebo Popis Poruchy";
            this.txtPricinaAleboPopisPoruchy.MinimumWidth = 8;
            this.txtPricinaAleboPopisPoruchy.Name = "txtPricinaAleboPopisPoruchy";
            this.txtPricinaAleboPopisPoruchy.ReadOnly = true;
            this.txtPricinaAleboPopisPoruchy.Width = 300;
            // 
            // txtVykonaneOpatrenieOprava
            // 
            this.txtVykonaneOpatrenieOprava.DataPropertyName = "VykonaneOpatrenieOprava";
            this.txtVykonaneOpatrenieOprava.HeaderText = "Vykonané Opatrenie Oprava";
            this.txtVykonaneOpatrenieOprava.MinimumWidth = 8;
            this.txtVykonaneOpatrenieOprava.Name = "txtVykonaneOpatrenieOprava";
            this.txtVykonaneOpatrenieOprava.ReadOnly = true;
            this.txtVykonaneOpatrenieOprava.Width = 300;
            // 
            // txtStrataOperator
            // 
            this.txtStrataOperator.DataPropertyName = "StrataOperator";
            this.txtStrataOperator.HeaderText = "Strata Operátor (Eur)";
            this.txtStrataOperator.MinimumWidth = 8;
            this.txtStrataOperator.Name = "txtStrataOperator";
            this.txtStrataOperator.ReadOnly = true;
            this.txtStrataOperator.Width = 90;
            // 
            // txtCenaOperator1hod
            // 
            this.txtCenaOperator1hod.DataPropertyName = "CenaOperator1hod";
            this.txtCenaOperator1hod.HeaderText = "Cena 1hod na tomto zariadení (Eur)";
            this.txtCenaOperator1hod.MinimumWidth = 8;
            this.txtCenaOperator1hod.Name = "txtCenaOperator1hod";
            this.txtCenaOperator1hod.ReadOnly = true;
            this.txtCenaOperator1hod.Visible = false;
            this.txtCenaOperator1hod.Width = 150;
            // 
            // txtStatus
            // 
            this.txtStatus.DataPropertyName = "Status";
            this.txtStatus.HeaderText = "Status";
            this.txtStatus.MinimumWidth = 8;
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.Visible = false;
            this.txtStatus.Width = 150;
            // 
            // txtRezerva1
            // 
            this.txtRezerva1.DataPropertyName = "Rezerva1";
            this.txtRezerva1.HeaderText = "Rezerva1";
            this.txtRezerva1.MinimumWidth = 8;
            this.txtRezerva1.Name = "txtRezerva1";
            this.txtRezerva1.ReadOnly = true;
            this.txtRezerva1.Visible = false;
            this.txtRezerva1.Width = 150;
            // 
            // txtRezerva2
            // 
            this.txtRezerva2.DataPropertyName = "Rezerva2";
            this.txtRezerva2.HeaderText = "Rezerva2";
            this.txtRezerva2.MinimumWidth = 8;
            this.txtRezerva2.Name = "txtRezerva2";
            this.txtRezerva2.ReadOnly = true;
            this.txtRezerva2.Visible = false;
            this.txtRezerva2.Width = 150;
            // 
            // txtRezerva3
            // 
            this.txtRezerva3.DataPropertyName = "Rezerva3";
            this.txtRezerva3.HeaderText = "Reakčný čas";
            this.txtRezerva3.MinimumWidth = 8;
            this.txtRezerva3.Name = "txtRezerva3";
            this.txtRezerva3.ReadOnly = true;
            this.txtRezerva3.Width = 150;
            // 
            // txtRezerva4
            // 
            this.txtRezerva4.DataPropertyName = "Rezerva4";
            this.txtRezerva4.HeaderText = "Reakčný čas zaznamenal";
            this.txtRezerva4.MinimumWidth = 8;
            this.txtRezerva4.Name = "txtRezerva4";
            this.txtRezerva4.ReadOnly = true;
            this.txtRezerva4.Width = 150;
            // 
            // txtRezerva5
            // 
            this.txtRezerva5.DataPropertyName = "Rezerva5";
            this.txtRezerva5.HeaderText = "Rezerva5";
            this.txtRezerva5.MinimumWidth = 8;
            this.txtRezerva5.Name = "txtRezerva5";
            this.txtRezerva5.ReadOnly = true;
            this.txtRezerva5.Visible = false;
            this.txtRezerva5.Width = 150;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(15, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(387, 18);
            this.label2.TabIndex = 6;
            this.label2.Text = "*Pre výmaz prestoja označte riadok vľavo a stlačte Delete";
            // 
            // Form5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(945, 657);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form5";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Výmaz zapísaných prestojov";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form5_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtID;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtPrestoj;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtKategoriaPrestoja;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtLinka;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtDatumZapisu;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtZaciatokPrestoja;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtKoniecPrestoja;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtTrvaniePrestoja;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtZmena;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtHotovyVyrobok;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtVstupnyMaterial;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtStanica;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtZastavenieVyroby;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtPoruchuOdstranil;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtPricinaAleboPopisPoruchy;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtVykonaneOpatrenieOprava;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtStrataOperator;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtCenaOperator1hod;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtRezerva1;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtRezerva2;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtRezerva3;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtRezerva4;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtRezerva5;
    }
}