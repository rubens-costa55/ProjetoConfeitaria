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
            label1 = new Label();
            panel3 = new Panel();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            txtcpf = new TextBox();
            label5 = new Label();
            textBox1 = new TextBox();
            button1 = new Button();
            button2 = new Button();
            panel4 = new Panel();
            pictureBox1 = new PictureBox();
            panel1 = new Panel();
            panel2 = new Panel();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.BackColor = Color.FromArgb(238, 231, 228);
            label1.Font = new Font("Segoe UI Black", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.FromArgb(94, 74, 68);
            label1.Location = new Point(58, 12);
            label1.Name = "label1";
            label1.Size = new Size(120, 28);
            label1.TabIndex = 2;
            label1.Text = "Login";
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(235, 221, 218);
            panel3.Controls.Add(button2);
            panel3.Controls.Add(button1);
            panel3.Controls.Add(textBox1);
            panel3.Controls.Add(label5);
            panel3.Controls.Add(txtcpf);
            panel3.Controls.Add(label4);
            panel3.Controls.Add(label3);
            panel3.Controls.Add(label2);
            panel3.Location = new Point(55, 90);
            panel3.Name = "panel3";
            panel3.Size = new Size(725, 615);
            panel3.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Black", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.FromArgb(201, 137, 120);
            label2.Location = new Point(55, 58);
            label2.Name = "label2";
            label2.Size = new Size(323, 45);
            label2.TabIndex = 0;
            label2.Text = "Realize o seu login:";
            // 
            // label3
            // 
            label3.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.FromArgb(126, 99, 92);
            label3.Location = new Point(55, 120);
            label3.Name = "label3";
            label3.Size = new Size(520, 42);
            label3.TabIndex = 1;
            label3.Text = "Digite seus dados no campo abaixo para acessar o sistema:";
            // 
            // label4
            // 
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.FromArgb(185, 120, 103);
            label4.Location = new Point(55, 176);
            label4.Name = "label4";
            label4.Size = new Size(100, 28);
            label4.TabIndex = 2;
            label4.Text = "CPF:";
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
            txtcpf.Text = "000.000.000-00";
            // 
            // label5
            // 
            label5.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.ForeColor = Color.FromArgb(185, 120, 103);
            label5.Location = new Point(55, 283);
            label5.Name = "label5";
            label5.Size = new Size(100, 28);
            label5.TabIndex = 5;
            label5.Text = "Senha:";
            // 
            // textBox1
            // 
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox1.ForeColor = Color.FromArgb(126, 99, 92);
            textBox1.Location = new Point(55, 314);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(430, 22);
            textBox1.TabIndex = 6;
            textBox1.Text = "12345678";
            textBox1.UseSystemPasswordChar = true;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(201, 142, 124);
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.ForeColor = Color.White;
            button1.Location = new Point(45, 430);
            button1.Name = "button1";
            button1.Size = new Size(485, 52);
            button1.TabIndex = 7;
            button1.Text = "Acessar";
            button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            button2.BackColor = Color.White;
            button2.FlatAppearance.BorderColor = Color.FromArgb(221, 201, 194);
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button2.ForeColor = Color.FromArgb(201, 142, 124);
            button2.Location = new Point(45, 487);
            button2.Name = "button2";
            button2.Size = new Size(485, 46);
            button2.TabIndex = 8;
            button2.Text = "Esqueci minha senha";
            button2.UseVisualStyleBackColor = false;
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
            panel1.Controls.Add(panel4);
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(panel2);
            panel1.Location = new Point(35, 40);
            panel1.Name = "panel1";
            panel1.Size = new Size(1570, 845);
            panel1.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(238, 231, 228);
            panel2.Location = new Point(-1, -1);
            panel2.Name = "panel2";
            panel2.Size = new Size(1570, 52);
            panel2.TabIndex = 1;
            // 
            // TelaLogin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(246, 239, 237);
            ClientSize = new Size(1664, 911);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "TelaLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Login";
            Load += TelaLogin_Load;
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Label label1;
        private Panel panel3;
        private Button button2;
        private Button button1;
        private TextBox textBox1;
        private Label label5;
        private TextBox txtcpf;
        private Label label4;
        private Label label3;
        private Label label2;
        private Panel panel4;
        private PictureBox pictureBox1;
        private Panel panel1;
        private Panel panel2;
    }
}
