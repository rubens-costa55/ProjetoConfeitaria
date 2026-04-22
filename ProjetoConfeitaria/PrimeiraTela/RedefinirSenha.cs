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
            conexao conexao = new conexao();
            MySqlConnection con = conexao.Conectar();

            try
            {
                con.Open();
                string sql = "UPDATE login SET senha = @senha WHERE cpf = @cpf";
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@cpf", txtCpf.Text);
                cmd.Parameters.AddWithValue("@senha", txtNovaSenha);
                int linhas = cmd.ExecuteNonQuery();
                if (linhas > 0)
                {
                    MessageBox.Show("Senha atualizada!");
                    this.Close();

                }
                else
                {
                    MessageBox.Show("CPF incorreto");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("erro: " + ex.Message);
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
