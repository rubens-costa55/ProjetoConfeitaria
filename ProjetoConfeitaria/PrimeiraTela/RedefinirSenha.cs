using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrimeiraTela
{
    public partial class FrmRedefinirSenha : Form
    {
        public FrmRedefinirSenha()
        {
            InitializeComponent();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void btnAtualizarSenha_Click(object sender, EventArgs e)
        {
            if (txtNovaSenha.Text == "" || txtConfirmarSenha.Text == "")
            {
                MessageBox.Show("Preencha os dois campos.");
                return;
            }
            else if (txtNovaSenha.Text != txtConfirmarSenha.Text)
            {
                MessageBox.Show("As senhas não coincidem.");
                return;
            }
            else if (txtNovaSenha.Text.Length < 8 || txtConfirmarSenha.Text.Length < 8)
            {
               
                MessageBox.Show("A senha está com menos de 8 digitos");
                return;
            }
            else
            {
                MessageBox.Show("Senha atualizada com sucesso!");
                TelaLogin tela = new TelaLogin();
                tela.Show();
                this.Hide();
            }
        }
        
    }
    
}
