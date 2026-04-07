namespace PrimeiraTela
{
    partial class TelaLogin
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblLogin = new Label();
            panel3 = new Panel();
            btnEsqueciSenha = new Button();
            btnAcessar = new Button();
            txtSenha = new TextBox();
            lblSenha = new Label();
            txtcpf = new TextBox();
            lblCPF = new Label();
            lblDescricao = new Label();
            lblTitulo = new Label();
            panel4 = new Panel();
            pictureBox1 = new PictureBox();
            panel1 = new Panel();
            lblSair = new Label();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // lblLogin
            // 
            lblLogin.BackColor = Color.FromArgb(249, 245, 243);
            lblLogin.Font = new Font("Arial", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblLogin.ForeColor = Color.FromArgb(94, 74, 68);
            lblLogin.Location = new Point(55, 26);
            lblLogin.Name = "lblLogin";
            lblLogin.Size = new Size(113, 44);
            lblLogin.TabIndex = 2;
            lblLogin.Text = "Login";
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(235, 221, 218);
            panel3.Controls.Add(btnEsqueciSenha);
            panel3.Controls.Add(btnAcessar);
            panel3.Controls.Add(txtSenha);
            panel3.Controls.Add(lblSenha);
            panel3.Controls.Add(txtcpf);
            panel3.Controls.Add(lblCPF);
            panel3.Controls.Add(lblDescricao);
            panel3.Controls.Add(lblTitulo);
            panel3.Location = new Point(55, 90);
            panel3.Name = "panel3";
            panel3.Size = new Size(725, 615);
            panel3.TabIndex = 3;
            // 
            // btnEsqueciSenha
            // 
            btnEsqueciSenha.BackColor = Color.White;
            btnEsqueciSenha.FlatAppearance.BorderColor = Color.FromArgb(221, 201, 194);
            btnEsqueciSenha.FlatStyle = FlatStyle.Flat;
            btnEsqueciSenha.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnEsqueciSenha.ForeColor = Color.FromArgb(201, 142, 124);
            btnEsqueciSenha.Location = new Point(45, 487);
            btnEsqueciSenha.Name = "btnEsqueciSenha";
            btnEsqueciSenha.Size = new Size(485, 46);
            btnEsqueciSenha.TabIndex = 8;
            btnEsqueciSenha.Text = "Esqueci minha senha";
            btnEsqueciSenha.UseVisualStyleBackColor = false;
            btnEsqueciSenha.Click += btnEsqueciSenha_Click;
            // 
            // btnAcessar
            // 
            btnAcessar.BackColor = Color.FromArgb(201, 142, 124);
            btnAcessar.FlatAppearance.BorderSize = 0;
            btnAcessar.FlatStyle = FlatStyle.Flat;
            btnAcessar.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAcessar.ForeColor = Color.White;
            btnAcessar.Location = new Point(45, 430);
            btnAcessar.Name = "btnAcessar";
            btnAcessar.Size = new Size(485, 52);
            btnAcessar.TabIndex = 7;
            btnAcessar.Text = "Acessar";
            btnAcessar.UseVisualStyleBackColor = false;
            btnAcessar.Click += btnAcessar_Click;
            // 
            // txtSenha
            // 
            txtSenha.BorderStyle = BorderStyle.None;
            txtSenha.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSenha.ForeColor = Color.FromArgb(126, 99, 92);
            txtSenha.Location = new Point(55, 314);
            txtSenha.Name = "txtSenha";
            txtSenha.Size = new Size(430, 22);
            txtSenha.TabIndex = 6;
            txtSenha.UseSystemPasswordChar = true;
            txtSenha.Click += txtSenha_TextChanged;
            txtSenha.TextChanged += txtSenha_TextChanged;
            // 
            // lblSenha
            // 
            lblSenha.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSenha.ForeColor = Color.FromArgb(185, 120, 103);
            lblSenha.Location = new Point(55, 283);
            lblSenha.Name = "lblSenha";
            lblSenha.Size = new Size(100, 28);
            lblSenha.TabIndex = 5;
            lblSenha.Text = "Senha:";
            // 
            // txtcpf
            // 
            txtcpf.BorderStyle = BorderStyle.None;
            txtcpf.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtcpf.ForeColor = Color.FromArgb(126, 99, 92);
            txtcpf.Location = new Point(55, 207);
            txtcpf.Name = "txtcpf";
            txtcpf.Size = new Size(430, 22);
            txtcpf.TabIndex = 4;
            txtcpf.Click += txtcpf_TextChanged;
            txtcpf.TextChanged += txtcpf_TextChanged;
            // 
            // lblCPF
            // 
            lblCPF.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCPF.ForeColor = Color.FromArgb(185, 120, 103);
            lblCPF.Location = new Point(55, 176);
            lblCPF.Name = "lblCPF";
            lblCPF.Size = new Size(100, 28);
            lblCPF.TabIndex = 2;
            lblCPF.Text = "CPF:";
            // 
            // lblDescricao
            // 
            lblDescricao.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblDescricao.ForeColor = Color.FromArgb(126, 99, 92);
            lblDescricao.Location = new Point(55, 120);
            lblDescricao.Name = "lblDescricao";
            lblDescricao.Size = new Size(520, 42);
            lblDescricao.TabIndex = 1;
            lblDescricao.Text = "Digite seus dados no campo abaixo para acessar o sistema:";
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI Black", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.FromArgb(201, 137, 120);
            lblTitulo.Location = new Point(55, 58);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(323, 45);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Realize o seu login:";
            // 
            // panel4
            // 
            panel4.BackColor = Color.FromArgb(244, 240, 239);
            panel4.Controls.Add(pictureBox1);
            panel4.Location = new Point(780, 90);
            panel4.Name = "panel4";
            panel4.Size = new Size(725, 615);
            panel4.TabIndex = 4;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = Properties.Resources.LOGO__1__removebg_preview;
            pictureBox1.Location = new Point(219, 120);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(346, 319);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(249, 245, 243);
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(lblSair);
            panel1.Controls.Add(panel4);
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(lblLogin);
            panel1.Location = new Point(49, 86);
            panel1.Name = "panel1";
            panel1.Size = new Size(1553, 730);
            panel1.TabIndex = 0;
            // 
            // lblSair
            // 
            lblSair.AutoSize = true;
            lblSair.Font = new Font("Segoe UI", 26.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSair.ForeColor = Color.FromArgb(201, 137, 120);
            lblSair.Location = new Point(1505, 0);
            lblSair.Name = "lblSair";
            lblSair.Size = new Size(43, 47);
            lblSair.TabIndex = 1;
            lblSair.Text = "X";
            lblSair.Click += lblSair_Click;
            // 
            // TelaLogin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(246, 239, 237);
            ClientSize = new Size(1664, 911);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            Name = "TelaLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Login";
            Load += TelaLogin_Load;
            Click += txtcpf_TextChanged;
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Label lblLogin;
        private Panel panel3;
        private Button btnEsqueciSenha;
        private Button btnAcessar;
        private Label lblSenha;
        private TextBox txtcpf;
        private Label lblCPF;
        private Label lblDescricao;
        private Label lblTitulo;
        private Panel panel4;
        private PictureBox pictureBox1;
        private Panel panel1;
        private Label lblSair;
        private TextBox txtSenha;
    }
}
