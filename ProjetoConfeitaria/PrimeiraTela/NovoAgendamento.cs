using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace PrimeiraTela
{
    public partial class NovoAgendamento : Form
    {
        public NovoAgendamento()
        {
            InitializeComponent();
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

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void btnMenuNA_Click(object sender, EventArgs e)
        {
            MenuPrincipal telaprincipal = new MenuPrincipal();
            telaprincipal.Show();
            this.Hide();
        }

        private void btnPedidosAtuaisNA_Click(object sender, EventArgs e)
        {
            PedidosAtuais telaPedidosAtuais = new PedidosAtuais();
            telaPedidosAtuais.Show();
            this.Hide();
        }

        private void btnSairNA_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnSalvarNA_Click(object sender, EventArgs e)
        {
            lbClienteResumo.Text = txtNomeCliente.Text;
            lbentregaResumo.Text = txtDataeHora.Text;
            lbValorResumo.Text = txtValor.Text;

            /*string conexao = "server=localhost; user=root; password=; database=confeitaria";
            MySqlConnection conn = new MySqlConnection(conexao);

            string sql = "INSERT INTO agendamentos (nomecliente, dataehora, produto, quantidade, valor) " +
             "VALUES (@nome, @dataehora, @produto, @quantidade, @valor)";

            MySqlCommand cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@nome", txtNomeCliente.Text);
            cmd.Parameters.AddWithValue("@dataehora", txtDataeHora.Text);
            cmd.Parameters.AddWithValue("@produto", txtProduto.Text);
            cmd.Parameters.AddWithValue("@quantidade", txtQuantidade.Text);
            cmd.Parameters.AddWithValue("@valor", txtValor.Text);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            */
        }
    }
}
