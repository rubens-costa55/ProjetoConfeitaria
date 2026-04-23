namespace PrimeiraTela
{
    partial class CadastroProdutos
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
            panel1 = new Panel();
            panel2 = new Panel();
            label1 = new Label();
            label2 = new Label();
            panel3 = new Panel();
            label3 = new Label();
            textBox1 = new TextBox();
            label4 = new Label();
            textBox2 = new TextBox();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(239, 229, 226);
            panel1.Location = new Point(30, 90);
            panel1.Name = "panel1";
            panel1.Size = new Size(260, 720);
            panel1.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(232, 174, 184);
            panel2.Controls.Add(label1);
            panel2.Location = new Point(350, 100);
            panel2.Name = "panel2";
            panel2.Size = new Size(500, 70);
            panel2.TabIndex = 1;
            // 
            // label1
            // 
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(500, 70);
            label1.TabIndex = 0;
            label1.Text = "CADASTRO DE PRODUTOS";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.FromArgb(142, 111, 101);
            label2.Location = new Point(350, 188);
            label2.Name = "label2";
            label2.Size = new Size(700, 35);
            label2.TabIndex = 2;
            label2.Text = "Cadastre os produtos que poderão ser utilizados nos novos pedidos.";
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(252, 250, 249);
            panel3.BorderStyle = BorderStyle.FixedSingle;
            panel3.Controls.Add(textBox2);
            panel3.Controls.Add(label4);
            panel3.Controls.Add(textBox1);
            panel3.Controls.Add(label3);
            panel3.Location = new Point(350, 250);
            panel3.Name = "panel3";
            panel3.Size = new Size(760, 500);
            panel3.TabIndex = 3;
            // 
            // label3
            // 
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.FromArgb(201, 142, 124);
            label3.Location = new Point(40, 40);
            label3.Name = "label3";
            label3.Size = new Size(200, 30);
            label3.TabIndex = 0;
            label3.Text = "Nome do Produto:";
            // 
            // textBox1
            // 
            textBox1.ForeColor = Color.FromArgb(111, 84, 75);
            textBox1.Location = new Point(40, 73);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(650, 23);
            textBox1.TabIndex = 1;
            // 
            // label4
            // 
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.FromArgb(201, 142, 124);
            label4.Location = new Point(40, 138);
            label4.Name = "label4";
            label4.Size = new Size(200, 30);
            label4.TabIndex = 2;
            label4.Text = "Preço unitário:";
            // 
            // textBox2
            // 
            textBox2.ForeColor = Color.FromArgb(111, 84, 75);
            textBox2.Location = new Point(40, 171);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(147, 23);
            textBox2.TabIndex = 3;
            // 
            // CadastroProdutos
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(247, 242, 241);
            ClientSize = new Size(1584, 861);
            Controls.Add(panel3);
            Controls.Add(label2);
            Controls.Add(panel2);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "CadastroProdutos";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Cadastro Produtos";
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private Label label1;
        private Label label2;
        private Panel panel3;
        private Label label3;
        private TextBox textBox2;
        private Label label4;
        private TextBox textBox1;
    }
}