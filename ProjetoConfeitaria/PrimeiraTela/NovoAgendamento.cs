using MySql.Data.MySqlClient;
using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrimeiraTela
{
    public partial class NovoAgendamento : Form
    {
        public NovoAgendamento()
        {
            InitializeComponent();
            
            List<string> listaCategorias = new List<string>();

            conexao conect = new conexao();
            MySqlConnection conn = conect.Conectar();

            try
            {
                conn.Open();

                string sqlcategorias = "SELECT nome_categoria FROM categorias";
                MySqlCommand cmd = new MySqlCommand(sqlcategorias, conn);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    listaCategorias.Add(reader["nome_categoria"].ToString());
                }

                reader.Close();

                cbCategoriaAgendamento.DataSource = listaCategorias;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar categorias: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            List<string> listaProdutos = new List<string>();

            conexao conect = new conexao();
            MySqlConnection conn = conect.Conectar();

            try
            {
                conn.Open();

                string categoriaSelecionada = cbCategoriaAgendamento.SelectedItem.ToString();

                string sqlprodutos = @"
                            SELECT p.NomeProduto
                            FROM produtos p
                            INNER JOIN categorias c ON p.id_categoria = c.id_categoria
                            WHERE c.nome_categoria = @categoria";

                MySqlCommand cmd = new MySqlCommand(sqlprodutos, conn);
                cmd.Parameters.AddWithValue("@categoria", categoriaSelecionada);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    listaProdutos.Add(reader["NomeProduto"].ToString());
                }

                reader.Close();

                cbProdutoAgendamento.DataSource = null;
                cbProdutoAgendamento.DataSource = listaProdutos;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar produtos: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

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
            conexao conect = new conexao();
            MySqlConnection conn = conect.Conectar();

            try
            {
                conn.Open();

                string totalStr = lbValorTotal.Text.Replace("R$", "").Trim();

                decimal total = decimal.Parse(
                    totalStr,
                    new CultureInfo("pt-BR")
                );

                string sqlPedido = @"
                    INSERT INTO pedidos
                    (NomeCliente, TelefoneCliente, DataHoraEntrega, ValorTotal, Status)
                    VALUES
                    (@nome, @telefone, @dataEntrega, @total, 'Aberto');
                    SELECT LAST_INSERT_ID();";

                MySqlCommand cmdPedido = new MySqlCommand(sqlPedido, conn);

                cmdPedido.Parameters.AddWithValue("@nome", txtNomeCliente.Text);
                cmdPedido.Parameters.AddWithValue("@telefone", txtTelefone.Text);
                cmdPedido.Parameters.AddWithValue("@dataEntrega", txtDataeHora.Text);
                cmdPedido.Parameters.AddWithValue("@total", total);

                int idPedido = Convert.ToInt32(cmdPedido.ExecuteScalar());

                // INSERIR ITENS DO PEDIDO
                foreach (DataGridViewRow row in dgvCarrinho.Rows)
                {
                    string nomeProduto = row.Cells["ColProduto"].Value.ToString();
                    int quantidade = Convert.ToInt32(row.Cells["ColQuantidade"].Value);
                    decimal valorUnit = Convert.ToDecimal(row.Cells["ColValor"].Value)/quantidade;
                    decimal subtotal = Convert.ToDecimal(row.Cells["ColValor"].Value);

                    // Buscar ID do produto
                    string sqlIdProduto = "SELECT id_produto FROM produtos WHERE NomeProduto = @produto";
                    MySqlCommand cmdIdProd = new MySqlCommand(sqlIdProduto, conn);
                    cmdIdProd.Parameters.AddWithValue("@produto", nomeProduto);
                    int idProduto = Convert.ToInt32(cmdIdProd.ExecuteScalar());

                    string sqlItem = @"
                        INSERT INTO itens_pedido
                        (id_pedido, id_produto, Quantidade, ValorUnitario, ValorItem)
                        VALUES
                        (@pedido, @produto, @qtd, @unit, @item);";

                    MySqlCommand cmdItem = new MySqlCommand(sqlItem, conn);
                    cmdItem.Parameters.AddWithValue("@pedido", idPedido);
                    cmdItem.Parameters.AddWithValue("@produto", idProduto);
                    cmdItem.Parameters.AddWithValue("@qtd", quantidade);
                    cmdItem.Parameters.AddWithValue("@unit", valorUnit);
                    cmdItem.Parameters.AddWithValue("@item", subtotal);

                    cmdItem.ExecuteNonQuery();
                }

                MessageBox.Show("Pedido salvo com sucesso!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar pedido: " + ex.Message);
            }
            finally
            {
                conn.Close();
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
            string produto = cbProdutoAgendamento.SelectedItem.ToString();
            int quantidade = int.Parse(txtQuantidade.Text);
            decimal valorUnitario = 0;

            conexao conect = new conexao();
            MySqlConnection conn = conect.Conectar();

            try
            {

                conn.Open();

                string sql = "SELECT PrecoProduto FROM produtos WHERE NomeProduto = @produto";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@produto", produto);

                object resultado = cmd.ExecuteScalar();
                if (resultado != null)
                {
                    valorUnitario = Convert.ToDecimal(resultado);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao buscar preço: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            decimal subtotal = quantidade * valorUnitario;
            
            dgvCarrinho.Rows.Add(produto, quantidade, subtotal);

            decimal total = 0;

            foreach (DataGridViewRow row in dgvCarrinho.Rows)
            {
                total += Convert.ToDecimal(row.Cells["ColValor"].Value);
            }

            lbValorTotal.Text = $"{total:C2}";
        }

        private void dgvCarrinho_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
