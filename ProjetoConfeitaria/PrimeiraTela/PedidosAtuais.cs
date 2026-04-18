using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using MySql.Data.MySqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrimeiraTela
{
    public partial class PedidosAtuais : Form
    {
        
        public PedidosAtuais()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        private void PedidosAtuais_Load(object sender, EventArgs e)
        {
            dgvPedidos.AutoGenerateColumns = false;
            if (dgvPedidos.Columns["btacao"] is DataGridViewButtonColumn btn)
            {
                btn.Text = "Concluir";
                btn.UseColumnTextForButtonValue = true;
            }

            CarregarPedidos();
        }

        private void CarregarPedidos()
        {
            conexao conexao = new conexao();
            MySqlConnection con = conexao.Conectar();

            try
            {
                con.Open();
                
                string sql = "SELECT id_cliente,NomeCliente,Produto,Quantidade,Valor,DataeHoraEntrega FROM clientes;";
                MySqlDataAdapter cmd = new MySqlDataAdapter(sql, con);
                //datatable: tabela virtual
                DataTable dt = new DataTable();
                cmd.Fill(dt);

                dgvPedidos.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar pedidos: " + ex.Message);
            }

            dgvPedidos.Columns["colcliente"].DataPropertyName = "NomeCliente";
            dgvPedidos.Columns["colpedido"].DataPropertyName = "Produto";
            dgvPedidos.Columns["colvalor"].DataPropertyName = "Valor";
            dgvPedidos.Columns["colentrega"].DataPropertyName = "DataeHoraEntrega";
            dgvPedidos.Columns["colstatus"].DataPropertyName = "Status";

        }   

        private void btnMenuPrincipal_Click(object sender, EventArgs e)
        {
            new MenuPrincipal().Show();
            this.Hide();
        }

        private void btnNovoAgendamento_Click(object sender, EventArgs e)
        {
            new NovoAgendamento().Show();
            this.Hide();
        }

        private void btnHistorico_Click(object sender, EventArgs e)
        {
            new FrmHistoricoPedidos().Show();
            this.Hide();
        }

        private void btnsair_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void dgvPedidos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (dgvPedidos.Columns[e.ColumnIndex].Name != "btacao")
                return;

            DataRowView linha = dgvPedidos.Rows[e.RowIndex].DataBoundItem as DataRowView;

            if (linha == null)
            {
                MessageBox.Show("Não foi possível obter os dados da linha.");
                return;
            }

            int idCliente = Convert.ToInt32(linha["id_cliente"]);

            DialogResult resposta = MessageBox.Show(
                "Deseja concluir este pedido?",
                "Confirmar",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resposta != DialogResult.Yes)
                return;

            conexao conexao = new conexao();
            MySqlConnection con = conexao.Conectar();

            try
            {
                con.Open();
                MySqlTransaction transacao = con.BeginTransaction();

                string inserirHistorico = @"
            INSERT INTO historico
            (NomeCliente, DataeHoraEntrega, Produto, Quantidade, Valor)
            SELECT NomeCliente, DataeHoraEntrega, Produto, Quantidade, Valor
            FROM clientes
            WHERE id_cliente = @id_cliente";

                using (MySqlCommand cmdInsert = new MySqlCommand(inserirHistorico, con, transacao))
                {
                    cmdInsert.Parameters.AddWithValue("@id_cliente", idCliente);
                    cmdInsert.ExecuteNonQuery();
                }

                string excluirCliente = "DELETE FROM clientes WHERE id_cliente = @id_cliente";

                using (MySqlCommand cmdDelete = new MySqlCommand(excluirCliente, con, transacao))
                {
                    cmdDelete.Parameters.AddWithValue("@id_cliente", idCliente);
                    cmdDelete.ExecuteNonQuery();
                }

                transacao.Commit();
                MessageBox.Show("Pedido concluído com sucesso.");
                CarregarPedidos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao concluir pedido: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

        }
    }
}
