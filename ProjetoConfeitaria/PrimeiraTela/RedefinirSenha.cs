using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
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
            if (txtCpf.Text == "00000000000")
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
                else if (!Regex.IsMatch(txtNovaSenha.Text, @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&]).{8,}$"))
                {

                    MessageBox.Show("A senha deve ter pelo menos 8 caracteres,1 letra,1 número e caractere especial");
                }
                else
                {
                    MessageBox.Show("Senha atualizada com sucesso!");
                    TelaLogin tela = new TelaLogin();
                    tela.Show();
                    this.Hide();
                }

            }

            else MessageBox.Show("CPF incorreto"); return;
        }

        private void lbRegrasTitulo_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void txtCpf_Click(object sender, EventArgs e)
        {
            txtCpf.Clear();
        }

        private void txtNovaSenha_Click(object sender, EventArgs e)
        {
            txtNovaSenha.Clear();
        }

        private void txtConfirmarSenha_Click(object sender, EventArgs e)
        {
            txtConfirmarSenha.Clear();
        }
    }

}
