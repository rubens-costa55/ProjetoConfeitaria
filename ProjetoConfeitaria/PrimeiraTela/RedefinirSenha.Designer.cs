namespace PrimeiraTela
{
    partial class FrmRedefinirSenha
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
            PanelJanela = new Panel();
            lblSair = new Label();
            label2 = new Label();
            PanelDireito = new Panel();
            label5 = new Label();
            PicBola2 = new Label();
            PicBola1 = new Label();
            PanelRegras = new Panel();
            lbRegrasTitulo = new Label();
            lbRegra3 = new Label();
            lbRegra2 = new Label();
            lbRegra1 = new Label();
            PicLogo = new PictureBox();
            PanelEsquerdo = new Panel();
            txtCpf = new TextBox();
            label1 = new Label();
            btnAtualizarSenha = new Button();
            lbConfirmarSenha = new Label();
            txtConfirmarSenha = new TextBox();
            txtNovaSenha = new TextBox();
            lbNovaSenha = new Label();
            lbTitulo = new Label();
            lbDescricao = new Label();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            PanelJanela.SuspendLayout();
            PanelDireito.SuspendLayout();
            PanelRegras.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PicLogo).BeginInit();
            PanelEsquerdo.SuspendLayout();
            SuspendLayout();
            // 
            // PanelJanela
            // 
            PanelJanela.BackColor = Color.FromArgb(249, 245, 243);
            PanelJanela.BorderStyle = BorderStyle.FixedSingle;
            PanelJanela.Controls.Add(lblSair);
            PanelJanela.Controls.Add(label2);
            PanelJanela.Controls.Add(PanelDireito);
            PanelJanela.Controls.Add(PanelEsquerdo);
            PanelJanela.Location = new Point(12, 12);
            PanelJanela.Name = "PanelJanela";
            PanelJanela.Size = new Size(1570, 845);
            PanelJanela.TabIndex = 0;
            // 
            // lblSair
            // 
            lblSair.AutoSize = true;
            lblSair.Font = new Font("Segoe UI", 26.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSair.ForeColor = Color.FromArgb(201, 137, 120);
            lblSair.Location = new Point(1511, 14);
            lblSair.Name = "lblSair";
            lblSair.Size = new Size(43, 47);
            lblSair.TabIndex = 4;
            lblSair.Text = "X";
            lblSair.Click += lblSair_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(716, 31);
            label2.Name = "label2";
            label2.Size = new Size(0, 15);
            label2.TabIndex = 3;
            // 
            // PanelDireito
            // 
            PanelDireito.BackColor = Color.Transparent;
            PanelDireito.Controls.Add(label5);
            PanelDireito.Controls.Add(PicBola2);
            PanelDireito.Controls.Add(PicBola1);
            PanelDireito.Controls.Add(PanelRegras);
            PanelDireito.Controls.Add(lbRegra3);
            PanelDireito.Controls.Add(lbRegra2);
            PanelDireito.Controls.Add(lbRegra1);
            PanelDireito.Controls.Add(PicLogo);
            PanelDireito.Location = new Point(785, 100);
            PanelDireito.Name = "PanelDireito";
            PanelDireito.Size = new Size(700, 615);
            PanelDireito.TabIndex = 2;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.ForeColor = Color.FromArgb(242, 143, 149);
            label5.Location = new Point(117, 453);
            label5.Name = "label5";
            label5.Size = new Size(25, 40);
            label5.TabIndex = 14;
            label5.Text = ".";
            label5.Click += label5_Click;
            // 
            // PicBola2
            // 
            PicBola2.AutoSize = true;
            PicBola2.Font = new Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            PicBola2.ForeColor = Color.FromArgb(242, 143, 149);
            PicBola2.Location = new Point(117, 415);
            PicBola2.Name = "PicBola2";
            PicBola2.Size = new Size(25, 40);
            PicBola2.TabIndex = 13;
            PicBola2.Text = ".";
            // 
            // PicBola1
            // 
            PicBola1.AutoSize = true;
            PicBola1.Font = new Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            PicBola1.ForeColor = Color.FromArgb(242, 143, 149);
            PicBola1.Location = new Point(117, 381);
            PicBola1.Name = "PicBola1";
            PicBola1.Size = new Size(25, 40);
            PicBola1.TabIndex = 6;
            PicBola1.Text = ".";
            // 
            // PanelRegras
            // 
            PanelRegras.BackColor = Color.FromArgb(232, 174, 184);
            PanelRegras.Controls.Add(lbRegrasTitulo);
            PanelRegras.Location = new Point(115, 290);
            PanelRegras.Name = "PanelRegras";
            PanelRegras.Size = new Size(530, 70);
            PanelRegras.TabIndex = 5;
            // 
            // lbRegrasTitulo
            // 
            lbRegrasTitulo.Dock = DockStyle.Fill;
            lbRegrasTitulo.Font = new Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbRegrasTitulo.ForeColor = Color.White;
            lbRegrasTitulo.Location = new Point(0, 0);
            lbRegrasTitulo.Name = "lbRegrasTitulo";
            lbRegrasTitulo.Size = new Size(530, 70);
            lbRegrasTitulo.TabIndex = 1;
            lbRegrasTitulo.Text = "REGRAS SUGERIDAS";
            lbRegrasTitulo.TextAlign = ContentAlignment.MiddleCenter;
            lbRegrasTitulo.Click += lbRegrasTitulo_Click;
            // 
            // lbRegra3
            // 
            lbRegra3.AutoSize = true;
            lbRegra3.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbRegra3.ForeColor = Color.FromArgb(126, 99, 92);
            lbRegra3.Location = new Point(148, 469);
            lbRegra3.Name = "lbRegra3";
            lbRegra3.Size = new Size(185, 20);
            lbRegra3.TabIndex = 4;
            lbRegra3.Text = "Evite usar a senha anterior.";
            lbRegra3.Click += label9_Click;
            // 
            // lbRegra2
            // 
            lbRegra2.AutoSize = true;
            lbRegra2.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbRegra2.ForeColor = Color.FromArgb(126, 99, 92);
            lbRegra2.Location = new Point(148, 431);
            lbRegra2.Name = "lbRegra2";
            lbRegra2.Size = new Size(241, 20);
            lbRegra2.TabIndex = 3;
            lbRegra2.Text = "Misture letras, números e simbolos.";
            // 
            // lbRegra1
            // 
            lbRegra1.AutoSize = true;
            lbRegra1.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbRegra1.ForeColor = Color.FromArgb(126, 99, 92);
            lbRegra1.Location = new Point(148, 397);
            lbRegra1.Name = "lbRegra1";
            lbRegra1.Size = new Size(205, 20);
            lbRegra1.TabIndex = 2;
            lbRegra1.Text = "Use pelo menos 8  caracteres.";
            // 
            // PicLogo
            // 
            PicLogo.Image = Properties.Resources.LOGO__1__removebg_preview;
            PicLogo.Location = new Point(245, 76);
            PicLogo.Name = "PicLogo";
            PicLogo.Size = new Size(220, 150);
            PicLogo.SizeMode = PictureBoxSizeMode.Zoom;
            PicLogo.TabIndex = 0;
            PicLogo.TabStop = false;
            // 
            // PanelEsquerdo
            // 
            PanelEsquerdo.BackColor = Color.FromArgb(235, 221, 218);
            PanelEsquerdo.Controls.Add(txtCpf);
            PanelEsquerdo.Controls.Add(label1);
            PanelEsquerdo.Controls.Add(btnAtualizarSenha);
            PanelEsquerdo.Controls.Add(lbConfirmarSenha);
            PanelEsquerdo.Controls.Add(txtConfirmarSenha);
            PanelEsquerdo.Controls.Add(txtNovaSenha);
            PanelEsquerdo.Controls.Add(lbNovaSenha);
            PanelEsquerdo.Controls.Add(lbTitulo);
            PanelEsquerdo.Controls.Add(lbDescricao);
            PanelEsquerdo.Location = new Point(84, 100);
            PanelEsquerdo.Name = "PanelEsquerdo";
            PanelEsquerdo.Size = new Size(700, 615);
            PanelEsquerdo.TabIndex = 1;
            // 
            // txtCpf
            // 
            txtCpf.BackColor = Color.White;
            txtCpf.BorderStyle = BorderStyle.FixedSingle;
            txtCpf.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtCpf.ForeColor = Color.FromArgb(191, 167, 157);
            txtCpf.Location = new Point(45, 197);
            txtCpf.MaxLength = 12;
            txtCpf.Name = "txtCpf";
            txtCpf.Size = new Size(430, 29);
            txtCpf.TabIndex = 9;
            txtCpf.Text = "Digite seu CPF";
            txtCpf.Click += txtCpf_Click;
            txtCpf.Enter += txtCpf_Enter;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.FromArgb(185, 120, 103);
            label1.Location = new Point(45, 162);
            label1.Name = "label1";
            label1.Size = new Size(244, 21);
            label1.TabIndex = 8;
            label1.Text = "Digite o CPF do administrador:";
            // 
            // btnAtualizarSenha
            // 
            btnAtualizarSenha.BackColor = Color.FromArgb(201, 142, 124);
            btnAtualizarSenha.FlatStyle = FlatStyle.Flat;
            btnAtualizarSenha.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAtualizarSenha.ForeColor = Color.White;
            btnAtualizarSenha.Location = new Point(45, 470);
            btnAtualizarSenha.Name = "btnAtualizarSenha";
            btnAtualizarSenha.RightToLeft = RightToLeft.No;
            btnAtualizarSenha.Size = new Size(485, 52);
            btnAtualizarSenha.TabIndex = 3;
            btnAtualizarSenha.Text = "Atualizar Senha";
            btnAtualizarSenha.UseVisualStyleBackColor = false;
            btnAtualizarSenha.Click += btnAtualizarSenha_Click;
            // 
            // lbConfirmarSenha
            // 
            lbConfirmarSenha.AutoSize = true;
            lbConfirmarSenha.BackColor = Color.FromArgb(235, 221, 218);
            lbConfirmarSenha.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbConfirmarSenha.ForeColor = Color.FromArgb(185, 120, 103);
            lbConfirmarSenha.Location = new Point(55, 381);
            lbConfirmarSenha.Name = "lbConfirmarSenha";
            lbConfirmarSenha.Size = new Size(138, 21);
            lbConfirmarSenha.TabIndex = 7;
            lbConfirmarSenha.Text = "Confirmar Senha";
            // 
            // txtConfirmarSenha
            // 
            txtConfirmarSenha.BackColor = Color.White;
            txtConfirmarSenha.BorderStyle = BorderStyle.FixedSingle;
            txtConfirmarSenha.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtConfirmarSenha.ForeColor = Color.FromArgb(191, 167, 157);
            txtConfirmarSenha.Location = new Point(45, 416);
            txtConfirmarSenha.Name = "txtConfirmarSenha";
            txtConfirmarSenha.Size = new Size(430, 29);
            txtConfirmarSenha.TabIndex = 6;
            txtConfirmarSenha.Text = "Nova Senha";
            txtConfirmarSenha.UseSystemPasswordChar = true;
            txtConfirmarSenha.Click += txtConfirmarSenha_Click;
            // 
            // txtNovaSenha
            // 
            txtNovaSenha.BackColor = Color.White;
            txtNovaSenha.BorderStyle = BorderStyle.FixedSingle;
            txtNovaSenha.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtNovaSenha.ForeColor = Color.FromArgb(191, 167, 157);
            txtNovaSenha.Location = new Point(45, 301);
            txtNovaSenha.MaxLength = 12;
            txtNovaSenha.Name = "txtNovaSenha";
            txtNovaSenha.Size = new Size(430, 29);
            txtNovaSenha.TabIndex = 5;
            txtNovaSenha.Text = "Nova Senha";
            txtNovaSenha.UseSystemPasswordChar = true;
            txtNovaSenha.Click += txtNovaSenha_Click;
            // 
            // lbNovaSenha
            // 
            lbNovaSenha.AutoSize = true;
            lbNovaSenha.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbNovaSenha.ForeColor = Color.FromArgb(185, 120, 103);
            lbNovaSenha.Location = new Point(55, 266);
            lbNovaSenha.Name = "lbNovaSenha";
            lbNovaSenha.Size = new Size(102, 21);
            lbNovaSenha.TabIndex = 4;
            lbNovaSenha.Text = "Nova Senha";
            // 
            // lbTitulo
            // 
            lbTitulo.AutoSize = true;
            lbTitulo.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbTitulo.ForeColor = Color.FromArgb(201, 137, 120);
            lbTitulo.Location = new Point(55, 58);
            lbTitulo.Name = "lbTitulo";
            lbTitulo.Size = new Size(255, 45);
            lbTitulo.TabIndex = 0;
            lbTitulo.Text = "Redefinir Senha";
            lbTitulo.Click += label1_Click;
            // 
            // lbDescricao
            // 
            lbDescricao.AutoSize = true;
            lbDescricao.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbDescricao.ForeColor = Color.FromArgb(126, 99, 92);
            lbDescricao.Location = new Point(55, 120);
            lbDescricao.Name = "lbDescricao";
            lbDescricao.Size = new Size(454, 20);
            lbDescricao.TabIndex = 3;
            lbDescricao.Text = "Atualize sua senha com segurança para continuar usando o sistema";
            // 
            // FrmRedefinirSenha
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(246, 239, 237);
            ClientSize = new Size(1664, 911);
            Controls.Add(PanelJanela);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FrmRedefinirSenha";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "RedefinirSenha";
            PanelJanela.ResumeLayout(false);
            PanelJanela.PerformLayout();
            PanelDireito.ResumeLayout(false);
            PanelDireito.PerformLayout();
            PanelRegras.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)PicLogo).EndInit();
            PanelEsquerdo.ResumeLayout(false);
            PanelEsquerdo.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel PanelJanela;
        private Panel PanelEsquerdo;
        private Label lbTitulo;
        private Panel PanelDireito;
        private Button btnAtualizarSenha;
        private Label lbConfirmarSenha;
        private TextBox txtConfirmarSenha;
        private TextBox txtNovaSenha;
        private Label lbNovaSenha;
        private Label lbDescricao;
        private PictureBox PicLogo;
        private Label lbRegra2;
        private Label lbRegra1;
        private Label lbRegrasTitulo;
        private Label lbRegra3;
        private Panel PanelRegras;
        private Label PicBola1;
        private Label label5;
        private Label PicBola2;
        private TextBox txtCpf;
        private Label label1;
        private Label label2;
        private Label lblSair;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}