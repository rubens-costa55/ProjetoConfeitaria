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
using Org.BouncyCastle.Tls;

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
            lbClienteResumo.Text = txtNomeCliente.Text;

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


            conexao conect = new conexao();
            MySqlConnection conn = conect.Conectar();

            try
            {
                conn.Open();
                string sql = "INSERT INTO clientes (NomeCliente, DataeHoraEntrega, Produto, Quantidade, Valor) " +
                 "VALUES (@NomeCliente, @DataeHoraEntrega, @Produto, @Quantidade, @Valor)";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@NomeCliente", txtNomeCliente.Text);
                cmd.Parameters.AddWithValue("@DataeHoraEntrega", txtDataeHora.Text);
                cmd.Parameters.AddWithValue("@Produto", cbProduto.Text);
                cmd.Parameters.AddWithValue("@Quantidade", txtQuantidade.Text);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Pedido cadastrado com sucesso.");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar pedido: " + ex.Message);

            }
        }

        private void txtNomeCliente_Click(object sender, EventArgs e)
        {
            txtNomeCliente.Clear();
        }

        private void txtProduto_Click(object sender, EventArgs e)
        {

        }

        private void txtQuantidade_Click(object sender, EventArgs e)
        {
            txtQuantidade.Clear();
        }

        private void txtValor_Click(object sender, EventArgs e)
        {

        }

        private void txtDataeHora_Click(object sender, EventArgs e)
        {
            txtDataeHora.Clear();
        }

        private void btnHistoricoNA_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void lbClienteResumo_Click(object sender, EventArgs e)
        {

        }

        private void txtQuantidade_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtValor_TextChanged(object sender, EventArgs e)
        {


        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {

        }
    }
}
