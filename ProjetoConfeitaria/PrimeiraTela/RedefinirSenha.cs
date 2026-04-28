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
using MySql.Data.MySqlClient;
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

            if (txtCpf.Text == "" || txtNovaSenha.Text == "" || txtConfirmarSenha.Text == "")
            {
                MessageBox.Show("Preencha todos os campos.");
                return;
            }

            if (txtNovaSenha.Text != txtConfirmarSenha.Text)
            {
                MessageBox.Show("As senhas não coincidem.");
                return;
            }

            if (txtNovaSenha.Text.Length < 8)
            {
                MessageBox.Show("A senha deve ter no mínimo 8 caracteres.");
                return;
            }

            if (!Regex.IsMatch(txtNovaSenha.Text, @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&]).{8,}$"))
            {
                MessageBox.Show("A senha deve conter letra, número e caractere especial.");
                return;
            }

            try
            {
                conexao conexao = new conexao();
                using (MySqlConnection con = conexao.Conectar())
                {
                    con.Open();

                    string sql = "UPDATE login SET senha = @senha WHERE cpf = @cpf";
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@cpf", txtCpf.Text);
                    cmd.Parameters.AddWithValue("@senha", txtNovaSenha.Text);

                    int linhas = cmd.ExecuteNonQuery();

                    if (linhas > 0)
                    {
                        MessageBox.Show("Senha atualizada com sucesso!");
                        TelaLogin tela = new TelaLogin();
                        tela.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("CPF não encontrado.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }
        }


        

        private void lbRegrasTitulo_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void txtCpf_Click(object sender, EventArgs e)
        {

        }

        private void txtNovaSenha_Click(object sender, EventArgs e)
        {
            txtNovaSenha.Clear();
        }

        private void txtConfirmarSenha_Click(object sender, EventArgs e)
        {
            txtConfirmarSenha.Clear();
        }

        private void lblSair_Click(object sender, EventArgs e)
        {

            this.Close();
        }

        private void txtCpf_Enter(object sender, EventArgs e)
        {
            txtCpf.Clear();
        }

        private void btnVerSenha_Click(object sender, EventArgs e)
        {

        }

        private void btnVer_Click(object sender, EventArgs e)
        {


        }
    }

}
