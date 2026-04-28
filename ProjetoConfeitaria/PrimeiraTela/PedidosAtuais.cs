using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace PrimeiraTela
{
    public partial class PedidosAtuais : Form
    {
        private bool carregandoGrid = false;

        public PedidosAtuais()
        {
            InitializeComponent();
        }

        private void PedidosAtuais_Load(object sender, EventArgs e)
        {
            dgvPedidos.AutoGenerateColumns = false;
            dgvPedidos.AllowUserToAddRows = false;
            dgvPedidos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPedidos.MultiSelect = false;

            if (dgvPedidos.Columns["btacao"] is DataGridViewButtonColumn btn)
            {
                btn.Text = "Concluir";
                btn.UseColumnTextForButtonValue = true;
            }

            if (dgvPedidos.Columns["colstatus"] is DataGridViewComboBoxColumn status)
            {
                status.Items.Clear();
                status.Items.Add("Agendado");
                status.Items.Add("Em produção");
                status.Items.Add("Atrasado");
            }

            dgvPedidos.Columns["colcliente"].DataPropertyName = "NomeCliente";
            dgvPedidos.Columns["colpedido"].DataPropertyName = "Produto";
            dgvPedidos.Columns["colentrega"].DataPropertyName = "DataHoraEntrega";
            dgvPedidos.Columns["colvalor"].DataPropertyName = "Valor";
            dgvPedidos.Columns["colstatus"].DataPropertyName = "Status";

            dgvPedidos.Columns["colvalor"].DefaultCellStyle.Format = "C2";

            dgvPedidos.CellClick -= dgvPedidos_CellClick;
            dgvPedidos.CellClick += dgvPedidos_CellClick;

            dgvPedidos.CellValueChanged -= dgvPedidos_CellValueChanged;
            dgvPedidos.CellValueChanged += dgvPedidos_CellValueChanged;

            dgvPedidos.CurrentCellDirtyStateChanged -= dgvPedidos_CurrentCellDirtyStateChanged;
            dgvPedidos.CurrentCellDirtyStateChanged += dgvPedidos_CurrentCellDirtyStateChanged;

            dgvPedidos.DataError -= dgvPedidos_DataError;
            dgvPedidos.DataError += dgvPedidos_DataError;

            btnbuscarpedido.Click -= btnbuscarpedido_Click;
            btnbuscarpedido.Click += btnbuscarpedido_Click;

            btnhoje.Click -= btnhoje_Click;
            btnhoje.Click += btnhoje_Click;

            btnproducao.Click -= btnproducao_Click;
            btnproducao.Click += btnproducao_Click;

            btnagendados.Click -= btnagendados_Click;
            btnagendados.Click += btnagendados_Click;

            btnatrasado.Click -= btnatrasado_Click;
            btnatrasado.Click += btnatrasado_Click;

            txtbuscar.Enter -= txtbuscar_Enter;
            txtbuscar.Enter += txtbuscar_Enter;

            txtbuscar.Leave -= txtbuscar_Leave;
            txtbuscar.Leave += txtbuscar_Leave;

            txtbuscar.KeyDown -= txtbuscar_KeyDown;
            txtbuscar.KeyDown += txtbuscar_KeyDown;

            CarregarPedidos();
        }

        private void CarregarPedidos(string busca = "", string filtroStatus = "", bool somenteHoje = false)
        {
            carregandoGrid = true;

            conexao conexao = new conexao();

            using (MySqlConnection con = conexao.Conectar())
            {
                try
                {
                    con.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = con;

                    string sql = @"
                        SELECT
                            p.id_pedido,
                            p.NomeCliente,
                            p.TelefoneCliente,
                            GROUP_CONCAT(
                                CONCAT(
                                    CASE
                                        WHEN ip.nome_produto IS NULL OR ip.nome_produto = ''
                                            THEN pr.NomeProduto
                                        ELSE ip.nome_produto
                                    END,
                                    ' (',
                                    ip.Quantidade,
                                    'x)'
                                )
                                SEPARATOR ', '
                            ) AS Produto,
                            SUM(ip.Quantidade) AS Quantidade,
                            p.ValorTotal AS Valor,
                            p.DataHoraEntrega,
                            CASE
                                WHEN p.Status IS NULL OR p.Status = '' OR p.Status = 'Aberto'
                                    THEN 'Agendado'
                                ELSE p.Status
                            END AS Status
                        FROM pedidos p
                        INNER JOIN itens_pedido ip ON p.id_pedido = ip.id_pedido
                        INNER JOIN produtos pr ON ip.id_produto = pr.id_produto
                        WHERE
                            (
                                p.Status IS NULL
                                OR p.Status = ''
                                OR p.Status = 'Aberto'
                                OR p.Status = 'Agendado'
                                OR p.Status = 'Em produção'
                                OR p.Status = 'Atrasado'
                            )
                            AND p.Status <> 'Concluído'
                    ";

                    if (!string.IsNullOrWhiteSpace(busca) && busca != "Buscar pedido..")
                    {
                        sql += @"
                            AND (
                                p.NomeCliente LIKE @busca
                                OR p.TelefoneCliente LIKE @busca
                                OR ip.nome_produto LIKE @busca
                                OR pr.NomeProduto LIKE @busca
                                OR p.DataHoraEntrega LIKE @busca
                            )
                        ";

                        cmd.Parameters.AddWithValue("@busca", "%" + busca.Trim() + "%");
                    }

                    if (!string.IsNullOrWhiteSpace(filtroStatus))
                    {
                        if (filtroStatus == "Agendado")
                        {
                            sql += @"
                                AND (
                                    p.Status IS NULL
                                    OR p.Status = ''
                                    OR p.Status = 'Aberto'
                                    OR p.Status = 'Agendado'
                                )
                            ";
                        }
                        else
                        {
                            sql += " AND p.Status = @status ";
                            cmd.Parameters.AddWithValue("@status", filtroStatus);
                        }
                    }

                    if (somenteHoje)
                    {
                        sql += @"
                            AND DATE(
                                COALESCE(
                                    STR_TO_DATE(p.DataHoraEntrega, '%d/%m/%Y %H:%i'),
                                    STR_TO_DATE(p.DataHoraEntrega, '%d/%m/%Y - %H:%i')
                                )
                            ) = CURDATE()
                        ";
                    }

                    sql += @"
                        GROUP BY
                            p.id_pedido,
                            p.NomeCliente,
                            p.TelefoneCliente,
                            p.ValorTotal,
                            p.DataHoraEntrega,
                            p.Status
                        ORDER BY
                            COALESCE(
                                STR_TO_DATE(p.DataHoraEntrega, '%d/%m/%Y %H:%i'),
                                STR_TO_DATE(p.DataHoraEntrega, '%d/%m/%Y - %H:%i')
                            ) ASC;
                    ";

                    cmd.CommandText = sql;

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvPedidos.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar pedidos atuais: " + ex.Message);
                }
                finally
                {
                    carregandoGrid = false;
                }
            }
        }

        private void dgvPedidos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (dgvPedidos.Columns[e.ColumnIndex].Name != "btacao")
                return;

            DataRowView linha = dgvPedidos.Rows[e.RowIndex].DataBoundItem as DataRowView;

            if (linha == null)
            {
                MessageBox.Show("Não foi possível obter os dados do pedido.");
                return;
            }

            int idPedido = Convert.ToInt32(linha["id_pedido"]);

            DialogResult resposta = MessageBox.Show(
                "Deseja concluir este pedido e enviar para o histórico?",
                "Confirmar conclusão",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (resposta != DialogResult.Yes)
                return;

            ConcluirPedido(idPedido);
        }

        private void ConcluirPedido(int idPedido)
        {
            conexao conexao = new conexao();

            using (MySqlConnection con = conexao.Conectar())
            {
                con.Open();

                MySqlTransaction transacao = con.BeginTransaction();

                try
                {
                    string inserirHistorico = @"
                        INSERT INTO historico
                        (
                            NomeCliente,
                            TelefoneCliente,
                            DataHoraEntrega,
                            Produto,
                            Quantidade,
                            Valor
                        )
                        SELECT
                            p.NomeCliente,
                            p.TelefoneCliente,
                            p.DataHoraEntrega,
                            CASE
                                WHEN ip.nome_produto IS NULL OR ip.nome_produto = ''
                                    THEN pr.NomeProduto
                                ELSE ip.nome_produto
                            END AS Produto,
                            ip.Quantidade,
                            ip.ValorItem
                        FROM pedidos p
                        INNER JOIN itens_pedido ip ON p.id_pedido = ip.id_pedido
                        INNER JOIN produtos pr ON ip.id_produto = pr.id_produto
                        WHERE p.id_pedido = @id_pedido
                          AND p.Status <> 'Concluído';
                    ";

                    using (MySqlCommand cmdHistorico = new MySqlCommand(inserirHistorico, con, transacao))
                    {
                        cmdHistorico.Parameters.AddWithValue("@id_pedido", idPedido);
                        cmdHistorico.ExecuteNonQuery();
                    }

                    string atualizarPedido = @"
                        UPDATE pedidos
                        SET Status = 'Concluído'
                        WHERE id_pedido = @id_pedido;
                    ";

                    using (MySqlCommand cmdAtualizar = new MySqlCommand(atualizarPedido, con, transacao))
                    {
                        cmdAtualizar.Parameters.AddWithValue("@id_pedido", idPedido);
                        cmdAtualizar.ExecuteNonQuery();
                    }

                    transacao.Commit();

                    MessageBox.Show("Pedido concluído e enviado para o histórico com sucesso.");

                    CarregarPedidos();
                }
                catch (Exception ex)
                {
                    transacao.Rollback();
                    MessageBox.Show("Erro ao concluir pedido: " + ex.Message);
                }
            }
        }

        private void dgvPedidos_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvPedidos.IsCurrentCellDirty)
            {
                dgvPedidos.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dgvPedidos_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (carregandoGrid)
                return;

            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (dgvPedidos.Columns[e.ColumnIndex].Name != "colstatus")
                return;

            DataRowView linha = dgvPedidos.Rows[e.RowIndex].DataBoundItem as DataRowView;

            if (linha == null)
                return;

            int idPedido = Convert.ToInt32(linha["id_pedido"]);
            string novoStatus = dgvPedidos.Rows[e.RowIndex].Cells["colstatus"].Value?.ToString();

            if (string.IsNullOrWhiteSpace(novoStatus))
                return;

            AtualizarStatusPedido(idPedido, novoStatus);
        }

        private void AtualizarStatusPedido(int idPedido, string novoStatus)
        {
            conexao conexao = new conexao();

            using (MySqlConnection con = conexao.Conectar())
            {
                try
                {
                    con.Open();

                    string sql = @"
                        UPDATE pedidos
                        SET Status = @status
                        WHERE id_pedido = @id_pedido;
                    ";

                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@status", novoStatus);
                        cmd.Parameters.AddWithValue("@id_pedido", idPedido);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao atualizar status do pedido: " + ex.Message);
                }
            }
        }

        private void dgvPedidos_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void btnbuscarpedido_Click(object sender, EventArgs e)
        {
            CarregarPedidos(txtbuscar.Text.Trim());
        }

        private void btnhoje_Click(object sender, EventArgs e)
        {
            CarregarPedidos("", "", true);
        }

        private void btnproducao_Click(object sender, EventArgs e)
        {
            CarregarPedidos("", "Em produção", false);
        }

        private void btnagendados_Click(object sender, EventArgs e)
        {
            CarregarPedidos("", "Agendado", false);
        }

        private void btnatrasado_Click(object sender, EventArgs e)
        {
            CarregarPedidos("", "Atrasado", false);
        }

        private void txtbuscar_Enter(object sender, EventArgs e)
        {
            if (txtbuscar.Text == "Buscar pedido..")
            {
                txtbuscar.Clear();
            }
        }

        private void txtbuscar_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtbuscar.Text))
            {
                txtbuscar.Text = "Buscar pedido..";
            }
        }

        private void txtbuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CarregarPedidos(txtbuscar.Text.Trim());
                e.SuppressKeyPress = true;
            }
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}