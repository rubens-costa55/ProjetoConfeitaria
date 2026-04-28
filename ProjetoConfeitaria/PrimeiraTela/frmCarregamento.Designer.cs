namespace PrimeiraTela
{
    partial class frmCarregamento
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
            components = new System.ComponentModel.Container();
            panel1 = new Panel();
            lblrodape = new Label();
            panelbarrafundo = new Panel();
            panel2 = new Panel();
            lbcarregando = new Label();
            pblogo = new PictureBox();
            tCarregar = new System.Windows.Forms.Timer(components);
            tPontos = new System.Windows.Forms.Timer(components);
            panel1.SuspendLayout();
            panelbarrafundo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pblogo).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(252, 250, 249);
            panel1.Controls.Add(lblrodape);
            panel1.Controls.Add(panelbarrafundo);
            panel1.Controls.Add(lbcarregando);
            panel1.Controls.Add(pblogo);
            panel1.Location = new Point(33, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(540, 367);
            panel1.TabIndex = 0;
            // 
            // lblrodape
            // 
            lblrodape.BackColor = Color.Transparent;
            lblrodape.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblrodape.ForeColor = Color.FromArgb(201, 142, 124);
            lblrodape.Location = new Point(20, 310);
            lblrodape.Name = "lblrodape";
            lblrodape.Size = new Size(500, 25);
            lblrodape.TabIndex = 5;
            lblrodape.Text = "Preparando seu ambiente...";
            lblrodape.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panelbarrafundo
            // 
            panelbarrafundo.BackColor = Color.FromArgb(239, 229, 226);
            panelbarrafundo.Controls.Add(panel2);
            panelbarrafundo.Location = new Point(60, 285);
            panelbarrafundo.Name = "panelbarrafundo";
            panelbarrafundo.Size = new Size(420, 12);
            panelbarrafundo.TabIndex = 4;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(201, 142, 124);
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(0, 12);
            panel2.TabIndex = 0;
            // 
            // lbcarregando
            // 
            lbcarregando.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbcarregando.ForeColor = Color.FromArgb(142, 111, 101);
            lbcarregando.Location = new Point(20, 245);
            lbcarregando.Name = "lbcarregando";
            lbcarregando.Size = new Size(500, 30);
            lbcarregando.TabIndex = 3;
            lbcarregando.Text = "Carregando sistema...";
            lbcarregando.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pblogo
            // 
            pblogo.Image = Properties.Resources.LOGO__1__removebg_preview;
            pblogo.Location = new Point(60, 27);
            pblogo.Name = "pblogo";
            pblogo.Size = new Size(420, 215);
            pblogo.SizeMode = PictureBoxSizeMode.Zoom;
            pblogo.TabIndex = 0;
            pblogo.TabStop = false;
            // 
            // tCarregar
            // 
            tCarregar.Interval = 35;
            // 
            // tPontos
            // 
            tPontos.Interval = 450;
            // 
            // frmCarregamento
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(247, 242, 241);
            ClientSize = new Size(604, 391);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmCarregamento";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Carregando";
            Load += frmCarregamento_Load;
            panel1.ResumeLayout(false);
            panelbarrafundo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pblogo).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private PictureBox pblogo;
        private Panel panelbarrafundo;
        private Label lbcarregando;
        private Panel panel2;
        private Label lblrodape;
        private System.Windows.Forms.Timer tCarregar;
        private System.Windows.Forms.Timer tPontos;
    }
}