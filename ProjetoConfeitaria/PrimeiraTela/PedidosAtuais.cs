using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace PrimeiraTela
{
    public partial class PedidosAtuais : Form
    {
        private bool carregandoGrid = false;
        private bool carregandoCategoriaEdicao = false;

        private int idPedidoEdicao = 0;
        private List<ItemPedidoEdicao> itensEdicao = new List<ItemPedidoEdicao>();

        private class ItemPedidoEdicao
        {
            public int IdProduto { get; set; }
            public string NomeProduto { get; set; }
            public int Quantidade { get; set; }
            public decimal ValorUnitario { get; set; }
            public decimal ValorItem { get; set; }

            public override string ToString()
            {
                return NomeProduto + " - " + Quantidade + "x - " +
                       ValorItem.ToString("C2", new CultureInfo("pt-BR"));
            }
        }

        public PedidosAtuais()
        {
            InitializeComponent();
        }

        private void PedidosAtuais_Load(object sender, EventArgs e)
        {
            MoverJanela.Ativar(this);

            dgvPedidos.AutoGenerateColumns = false;
            dgvPedidos.AllowUserToAddRows = false;
            dgvPedidos.AllowUserToDeleteRows = false;
            dgvPedidos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPedidos.MultiSelect = false;

            dgvPedidos.ReadOnly = false;
            dgvPedidos.EditMode = DataGridViewEditMode.EditOnEnter;

            foreach (DataGridViewColumn coluna in dgvPedidos.Columns)
            {
                coluna.ReadOnly = true;
            }

            if (dgvPedidos.Columns["colstatus"] != null)
                dgvPedidos.Columns["colstatus"].ReadOnly = false;

            if (dgvPedidos.Columns["btacao"] != null)
                dgvPedidos.Columns["btacao"].ReadOnly = false;

            RemoverSelecaoAzulGrid();
            ConfigurarPainelEdicaoPedido();

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

        private void ConfigurarPainelEdicaoPedido()
        {
            panelEditarPedido.Visible = false;

            btnEditarDados.Click -= btnEditarDados_Click;
            btnEditarDados.Click += btnEditarDados_Click;

            btnCancelarEdicaoPedido.Click -= btnCancelarEdicaoPedido_Click;
            btnCancelarEdicaoPedido.Click += btnCancelarEdicaoPedido_Click;

            btnSalvarEdicaoPedido.Click -= btnSalvarEdicaoPedido_Click;
            btnSalvarEdicaoPedido.Click += btnSalvarEdicaoPedido_Click;

            btnIncluirItemEditar.Click -= btnIncluirItemEditar_Click;
            btnIncluirItemEditar.Click += btnIncluirItemEditar_Click;

            btnRemoverItemEditar.Click -= btnRemoverItemEditar_Click;
            btnRemoverItemEditar.Click += btnRemoverItemEditar_Click;

            cbProdutoEditar.SelectedIndexChanged -= cbProdutoEditar_SelectedIndexChanged;
            cbProdutoEditar.SelectedIndexChanged += cbProdutoEditar_SelectedIndexChanged;

            cbCategoriaEditar.SelectedIndexChanged -= cbCategoriaEditar_SelectedIndexChanged;
            cbCategoriaEditar.SelectedIndexChanged += cbCategoriaEditar_SelectedIndexChanged;
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

                    AtualizarPedidosAtrasadosAutomaticamente(con);

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
                            AND (
                                p.Status IS NULL
                                OR p.Status <> 'Concluído'
                            )
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
                                    STR_TO_DATE(p.DataHoraEntrega, '%d/%m/%Y - %H:%i'),
                                    STR_TO_DATE(p.DataHoraEntrega, '%d/%m/%Y %H:%i'),
                                    STR_TO_DATE(p.DataHoraEntrega, '%d/%m/%Y - %Hh'),
                                    STR_TO_DATE(p.DataHoraEntrega, '%d/%m/%Y %Hh'),
                                    STR_TO_DATE(p.DataHoraEntrega, '%d/%m/%Y - %Hh%i'),
                                    STR_TO_DATE(p.DataHoraEntrega, '%d/%m/%Y %Hh%i')
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
                                STR_TO_DATE(p.DataHoraEntrega, '%d/%m/%Y - %H:%i'),
                                STR_TO_DATE(p.DataHoraEntrega, '%d/%m/%Y %H:%i'),
                                STR_TO_DATE(p.DataHoraEntrega, '%d/%m/%Y - %Hh'),
                                STR_TO_DATE(p.DataHoraEntrega, '%d/%m/%Y %Hh'),
                                STR_TO_DATE(p.DataHoraEntrega, '%d/%m/%Y - %Hh%i'),
                                STR_TO_DATE(p.DataHoraEntrega, '%d/%m/%Y %Hh%i')
                            ) ASC;
                    ";

                    cmd.CommandText = sql;

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvPedidos.DataSource = dt;

                    dgvPedidos.ClearSelection();

                    if (dgvPedidos.Rows.Count > 0)
                    {
                        dgvPedidos.CurrentCell = null;
                    }
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

        private void AtualizarPedidosAtrasadosAutomaticamente(MySqlConnection con)
        {
            try
            {
                string sql = @"
                    UPDATE pedidos
                    SET Status = 'Atrasado'
                    WHERE
                        (
                            Status IS NULL
                            OR Status = ''
                            OR Status = 'Aberto'
                            OR Status = 'Agendado'
                            OR Status = 'Em produção'
                        )
                        AND (
                            Status IS NULL
                            OR Status <> 'Concluído'
                        )
                        AND COALESCE(
                            STR_TO_DATE(DataHoraEntrega, '%d/%m/%Y - %H:%i'),
                            STR_TO_DATE(DataHoraEntrega, '%d/%m/%Y %H:%i'),
                            STR_TO_DATE(DataHoraEntrega, '%d/%m/%Y - %Hh'),
                            STR_TO_DATE(DataHoraEntrega, '%d/%m/%Y %Hh'),
                            STR_TO_DATE(DataHoraEntrega, '%d/%m/%Y - %Hh%i'),
                            STR_TO_DATE(DataHoraEntrega, '%d/%m/%Y %Hh%i')
                        ) IS NOT NULL
                        AND COALESCE(
                            STR_TO_DATE(DataHoraEntrega, '%d/%m/%Y - %H:%i'),
                            STR_TO_DATE(DataHoraEntrega, '%d/%m/%Y %H:%i'),
                            STR_TO_DATE(DataHoraEntrega, '%d/%m/%Y - %Hh'),
                            STR_TO_DATE(DataHoraEntrega, '%d/%m/%Y %Hh'),
                            STR_TO_DATE(DataHoraEntrega, '%d/%m/%Y - %Hh%i'),
                            STR_TO_DATE(DataHoraEntrega, '%d/%m/%Y %Hh%i')
                        ) < NOW();
                ";

                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao atualizar pedidos atrasados automaticamente: " + ex.Message);
            }
        }

        private void dgvPedidos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            string nomeColuna = dgvPedidos.Columns[e.ColumnIndex].Name;

            if (nomeColuna == "colstatus")
            {
                dgvPedidos.Rows[e.RowIndex].Selected = true;
                dgvPedidos.CurrentCell = dgvPedidos.Rows[e.RowIndex].Cells[e.ColumnIndex];
                dgvPedidos.BeginEdit(true);

                ComboBox combo = dgvPedidos.EditingControl as ComboBox;

                if (combo != null)
                {
                    combo.DroppedDown = true;
                }

                return;
            }

            if (nomeColuna != "btacao")
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

        private void btnEditarDados_Click(object sender, EventArgs e)
        {
            if (dgvPedidos.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Selecione um pedido na lista para editar.",
                    "Pedido não selecionado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            DataGridViewRow rowSelecionada = dgvPedidos.SelectedRows[0];

            DataRowView linha = rowSelecionada.DataBoundItem as DataRowView;

            if (linha == null)
            {
                MessageBox.Show(
                    "Não foi possível obter os dados do pedido selecionado.",
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                return;
            }

            idPedidoEdicao = Convert.ToInt32(linha["id_pedido"]);

            CarregarCategoriasEdicao();
            LimparProdutosEdicao();

            CarregarDadosPedidoEdicao(idPedidoEdicao);
            CarregarItensPedidoEdicao(idPedidoEdicao);

            panelEditarPedido.Visible = true;
            panelEditarPedido.BringToFront();
        }

        private void CarregarDadosPedidoEdicao(int idPedido)
        {
            conexao conexao = new conexao();

            using (MySqlConnection con = conexao.Conectar())
            {
                try
                {
                    con.Open();

                    string sql = @"
                        SELECT
                            NomeCliente,
                            DataHoraEntrega
                        FROM pedidos
                        WHERE id_pedido = @id_pedido;
                    ";

                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@id_pedido", idPedido);

                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                txtClienteEditar.Text = dr["NomeCliente"].ToString();
                                txtEntregaEditar.Text = dr["DataHoraEntrega"].ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar dados do pedido: " + ex.Message);
                }
            }
        }

        private void CarregarCategoriasEdicao()
        {
            carregandoCategoriaEdicao = true;

            conexao conexao = new conexao();

            using (MySqlConnection con = conexao.Conectar())
            {
                try
                {
                    con.Open();

                    string sql = @"
                        SELECT
                            id_categoria,
                            nome_categoria
                        FROM categorias
                        ORDER BY nome_categoria;
                    ";

                    MySqlDataAdapter adapter = new MySqlDataAdapter(sql, con);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    DataRow linhaInicial = dt.NewRow();
                    linhaInicial["id_categoria"] = 0;
                    linhaInicial["nome_categoria"] = "Selecione a categoria";
                    dt.Rows.InsertAt(linhaInicial, 0);

                    cbCategoriaEditar.DataSource = dt;
                    cbCategoriaEditar.DisplayMember = "nome_categoria";
                    cbCategoriaEditar.ValueMember = "id_categoria";
                    cbCategoriaEditar.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar categorias: " + ex.Message);
                }
                finally
                {
                    carregandoCategoriaEdicao = false;
                }
            }
        }

        private void LimparProdutosEdicao()
        {
            cbProdutoEditar.DataSource = null;
            cbProdutoEditar.Items.Clear();
            cbProdutoEditar.Text = "";

            txtQuantidadeEditar.Text = "1";
            txtValorEditar.Text = "0,00";
        }

        private void CarregarProdutosEdicao(int idCategoria)
        {
            if (idCategoria <= 0)
            {
                LimparProdutosEdicao();
                return;
            }

            conexao conexao = new conexao();

            using (MySqlConnection con = conexao.Conectar())
            {
                try
                {
                    con.Open();

                    string sql = @"
                        SELECT
                            p.id_produto,
                            p.NomeProduto,
                            p.PrecoProduto,
                            p.id_categoria
                        FROM produtos p
                        INNER JOIN categorias c ON p.id_categoria = c.id_categoria
                        WHERE p.id_categoria = @id_categoria
                        ORDER BY p.NomeProduto;
                    ";

                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@id_categoria", idCategoria);

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    cbProdutoEditar.DataSource = dt;
                    cbProdutoEditar.DisplayMember = "NomeProduto";
                    cbProdutoEditar.ValueMember = "id_produto";
                    cbProdutoEditar.SelectedIndex = -1;

                    txtQuantidadeEditar.Text = "1";
                    txtValorEditar.Text = "0,00";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar produtos para edição: " + ex.Message);
                }
            }
        }

        private void cbCategoriaEditar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (carregandoCategoriaEdicao)
                return;

            if (cbCategoriaEditar.SelectedValue == null)
            {
                LimparProdutosEdicao();
                return;
            }

            int idCategoria = 0;

            int.TryParse(cbCategoriaEditar.SelectedValue.ToString(), out idCategoria);

            if (idCategoria <= 0)
            {
                LimparProdutosEdicao();
                return;
            }

            CarregarProdutosEdicao(idCategoria);
        }

        private void cbProdutoEditar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbProdutoEditar.SelectedItem == null)
                return;

            DataRowView linha = cbProdutoEditar.SelectedItem as DataRowView;

            if (linha == null)
                return;

            decimal preco = Convert.ToDecimal(linha["PrecoProduto"]);
            txtValorEditar.Text = preco.ToString("N2", new CultureInfo("pt-BR"));
        }

        private void CarregarItensPedidoEdicao(int idPedido)
        {
            itensEdicao.Clear();
            lstItensEditar.Items.Clear();

            conexao conexao = new conexao();

            using (MySqlConnection con = conexao.Conectar())
            {
                try
                {
                    con.Open();

                    string sql = @"
                        SELECT
                            ip.id_produto,
                            CASE
                                WHEN ip.nome_produto IS NULL OR ip.nome_produto = ''
                                    THEN pr.NomeProduto
                                ELSE ip.nome_produto
                            END AS NomeProduto,
                            ip.Quantidade,
                            ip.ValorUnitario,
                            ip.ValorItem
                        FROM itens_pedido ip
                        INNER JOIN produtos pr ON ip.id_produto = pr.id_produto
                        WHERE ip.id_pedido = @id_pedido
                        ORDER BY ip.id_item;
                    ";

                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@id_pedido", idPedido);

                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                ItemPedidoEdicao item = new ItemPedidoEdicao();

                                item.IdProduto = Convert.ToInt32(dr["id_produto"]);
                                item.NomeProduto = dr["NomeProduto"].ToString();
                                item.Quantidade = Convert.ToInt32(dr["Quantidade"]);
                                item.ValorUnitario = Convert.ToDecimal(dr["ValorUnitario"]);
                                item.ValorItem = Convert.ToDecimal(dr["ValorItem"]);

                                itensEdicao.Add(item);
                            }
                        }
                    }

                    AtualizarListaItensEdicao();
                    RecalcularTotalEdicao();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar itens do pedido: " + ex.Message);
                }
            }
        }

        private void btnIncluirItemEditar_Click(object sender, EventArgs e)
        {
            if (cbProdutoEditar.SelectedItem == null || cbProdutoEditar.SelectedIndex < 0)
            {
                MessageBox.Show("Selecione uma categoria e depois selecione um produto.");
                cbCategoriaEditar.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtQuantidadeEditar.Text))
            {
                MessageBox.Show("Preencha a quantidade.");
                txtQuantidadeEditar.Focus();
                return;
            }

            int quantidade;

            if (!int.TryParse(txtQuantidadeEditar.Text.Trim(), out quantidade))
            {
                MessageBox.Show("Digite uma quantidade válida.");
                txtQuantidadeEditar.Clear();
                txtQuantidadeEditar.Focus();
                return;
            }

            if (quantidade <= 0)
            {
                MessageBox.Show("A quantidade precisa ser maior que zero.");
                txtQuantidadeEditar.Focus();
                return;
            }

            decimal valorUnitario;

            if (!decimal.TryParse(txtValorEditar.Text.Trim(), NumberStyles.Any, new CultureInfo("pt-BR"), out valorUnitario))
            {
                MessageBox.Show("Digite um valor válido. Exemplo: 45,00");
                txtValorEditar.Focus();
                return;
            }

            if (valorUnitario <= 0)
            {
                MessageBox.Show("O valor precisa ser maior que zero.");
                txtValorEditar.Focus();
                return;
            }

            DataRowView linhaProduto = cbProdutoEditar.SelectedItem as DataRowView;

            if (linhaProduto == null)
            {
                MessageBox.Show("Não foi possível obter o produto selecionado.");
                return;
            }

            int idProduto = Convert.ToInt32(linhaProduto["id_produto"]);
            string nomeProduto = linhaProduto["NomeProduto"].ToString();
            decimal valorItem = quantidade * valorUnitario;

            ItemPedidoEdicao item = new ItemPedidoEdicao();

            item.IdProduto = idProduto;
            item.NomeProduto = nomeProduto;
            item.Quantidade = quantidade;
            item.ValorUnitario = valorUnitario;
            item.ValorItem = valorItem;

            itensEdicao.Add(item);

            AtualizarListaItensEdicao();
            RecalcularTotalEdicao();

            txtQuantidadeEditar.Text = "1";
            txtValorEditar.Text = "0,00";
            cbProdutoEditar.SelectedIndex = -1;
        }

        private void btnRemoverItemEditar_Click(object sender, EventArgs e)
        {
            if (lstItensEditar.SelectedIndex < 0)
            {
                MessageBox.Show("Selecione um item para remover.");
                return;
            }

            int indice = lstItensEditar.SelectedIndex;

            if (indice >= 0 && indice < itensEdicao.Count)
            {
                itensEdicao.RemoveAt(indice);
            }

            AtualizarListaItensEdicao();
            RecalcularTotalEdicao();
        }

        private void AtualizarListaItensEdicao()
        {
            lstItensEditar.Items.Clear();

            foreach (ItemPedidoEdicao item in itensEdicao)
            {
                lstItensEditar.Items.Add(item);
            }
        }

        private decimal RecalcularTotalEdicao()
        {
            decimal total = 0;

            foreach (ItemPedidoEdicao item in itensEdicao)
            {
                total += item.ValorItem;
            }

            lblValorTotalEditar.Text = "Total: " + total.ToString("C2", new CultureInfo("pt-BR"));

            return total;
        }

        private void btnCancelarEdicaoPedido_Click(object sender, EventArgs e)
        {
            panelEditarPedido.Visible = false;

            idPedidoEdicao = 0;
            itensEdicao.Clear();
            lstItensEditar.Items.Clear();

            txtClienteEditar.Clear();
            txtEntregaEditar.Clear();
            txtQuantidadeEditar.Text = "1";
            txtValorEditar.Text = "0,00";
            lblValorTotalEditar.Text = "Total: R$ 0,00";

            if (cbCategoriaEditar.DataSource != null)
                cbCategoriaEditar.SelectedIndex = 0;

            LimparProdutosEdicao();
        }

        private void btnSalvarEdicaoPedido_Click(object sender, EventArgs e)
        {
            if (idPedidoEdicao <= 0)
            {
                MessageBox.Show("Nenhum pedido foi selecionado para edição.");
                return;
            }

            string cliente = txtClienteEditar.Text.Trim();
            string entrega = txtEntregaEditar.Text.Trim();

            if (string.IsNullOrWhiteSpace(cliente))
            {
                MessageBox.Show("Preencha o nome do cliente.");
                txtClienteEditar.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(entrega))
            {
                MessageBox.Show("Preencha a data e hora da entrega.");
                txtEntregaEditar.Focus();
                return;
            }

            DateTime dataValidada;

            string[] formatosPermitidos =
            {
                "dd/MM/yyyy - HH:mm",
                "dd/MM/yyyy HH:mm",
                "dd/MM/yyyy - H:mm",
                "dd/MM/yyyy H:mm",
                "dd/MM/yyyy - HH'h'",
                "dd/MM/yyyy HH'h'",
                "dd/MM/yyyy - H'h'",
                "dd/MM/yyyy H'h'",
                "dd/MM/yyyy - HH'h'mm",
                "dd/MM/yyyy HH'h'mm"
            };

            if (!DateTime.TryParseExact(
                    entrega,
                    formatosPermitidos,
                    new CultureInfo("pt-BR"),
                    DateTimeStyles.None,
                    out dataValidada))
            {
                MessageBox.Show("Digite a data e hora corretamente. Exemplo: 15/04/2026 - 16:30");
                txtEntregaEditar.Focus();
                return;
            }

            if (itensEdicao.Count == 0)
            {
                MessageBox.Show("O pedido precisa ter pelo menos um item.");
                return;
            }

            decimal total = RecalcularTotalEdicao();

            conexao conexao = new conexao();

            using (MySqlConnection con = conexao.Conectar())
            {
                con.Open();

                MySqlTransaction transacao = con.BeginTransaction();

                try
                {
                    string novoStatus = "Agendado";

                    if (dataValidada < DateTime.Now)
                    {
                        novoStatus = "Atrasado";
                    }

                    string sqlPedido = @"
                        UPDATE pedidos
                        SET
                            NomeCliente = @cliente,
                            DataHoraEntrega = @entrega,
                            ValorTotal = @total,
                            Status = @status
                        WHERE id_pedido = @id_pedido;
                    ";

                    using (MySqlCommand cmdPedido = new MySqlCommand(sqlPedido, con, transacao))
                    {
                        cmdPedido.Parameters.AddWithValue("@cliente", cliente);
                        cmdPedido.Parameters.AddWithValue("@entrega", dataValidada.ToString("dd/MM/yyyy - HH:mm"));
                        cmdPedido.Parameters.AddWithValue("@total", total);
                        cmdPedido.Parameters.AddWithValue("@status", novoStatus);
                        cmdPedido.Parameters.AddWithValue("@id_pedido", idPedidoEdicao);
                        cmdPedido.ExecuteNonQuery();
                    }

                    string sqlExcluirItens = @"
                        DELETE FROM itens_pedido
                        WHERE id_pedido = @id_pedido;
                    ";

                    using (MySqlCommand cmdExcluir = new MySqlCommand(sqlExcluirItens, con, transacao))
                    {
                        cmdExcluir.Parameters.AddWithValue("@id_pedido", idPedidoEdicao);
                        cmdExcluir.ExecuteNonQuery();
                    }

                    string sqlInserirItem = @"
                        INSERT INTO itens_pedido
                        (
                            id_pedido,
                            id_produto,
                            nome_produto,
                            Quantidade,
                            ValorUnitario,
                            ValorItem
                        )
                        VALUES
                        (
                            @id_pedido,
                            @id_produto,
                            @nome_produto,
                            @quantidade,
                            @valor_unitario,
                            @valor_item
                        );
                    ";

                    foreach (ItemPedidoEdicao item in itensEdicao)
                    {
                        using (MySqlCommand cmdItem = new MySqlCommand(sqlInserirItem, con, transacao))
                        {
                            cmdItem.Parameters.AddWithValue("@id_pedido", idPedidoEdicao);
                            cmdItem.Parameters.AddWithValue("@id_produto", item.IdProduto);
                            cmdItem.Parameters.AddWithValue("@nome_produto", item.NomeProduto);
                            cmdItem.Parameters.AddWithValue("@quantidade", item.Quantidade);
                            cmdItem.Parameters.AddWithValue("@valor_unitario", item.ValorUnitario);
                            cmdItem.Parameters.AddWithValue("@valor_item", item.ValorItem);
                            cmdItem.ExecuteNonQuery();
                        }
                    }

                    transacao.Commit();

                    MessageBox.Show("Pedido atualizado com sucesso.");

                    panelEditarPedido.Visible = false;
                    idPedidoEdicao = 0;
                    itensEdicao.Clear();
                    lstItensEditar.Items.Clear();

                    CarregarPedidos();
                }
                catch (Exception ex)
                {
                    transacao.Rollback();
                    MessageBox.Show("Erro ao atualizar pedido: " + ex.Message);
                }
            }
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
                          AND (
                                p.Status IS NULL
                                OR p.Status <> 'Concluído'
                          );
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

                    CarregarPedidos();
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

        private void RemoverSelecaoAzulGrid()
        {
            dgvPedidos.EnableHeadersVisualStyles = false;

            dgvPedidos.DefaultCellStyle.SelectionBackColor = Color.FromArgb(243, 232, 228);
            dgvPedidos.DefaultCellStyle.SelectionForeColor = Color.FromArgb(111, 84, 75);

            dgvPedidos.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(243, 232, 228);
            dgvPedidos.RowsDefaultCellStyle.SelectionForeColor = Color.FromArgb(111, 84, 75);

            dgvPedidos.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(243, 232, 228);
            dgvPedidos.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.FromArgb(111, 84, 75);

            dgvPedidos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(239, 229, 226);
            dgvPedidos.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(111, 84, 75);

            dgvPedidos.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(239, 229, 226);
            dgvPedidos.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.FromArgb(111, 84, 75);

            dgvPedidos.RowHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(243, 232, 228);
            dgvPedidos.RowHeadersDefaultCellStyle.SelectionForeColor = Color.FromArgb(111, 84, 75);

            dgvPedidos.BackgroundColor = Color.White;
            dgvPedidos.GridColor = Color.FromArgb(228, 206, 199);
            dgvPedidos.BorderStyle = BorderStyle.None;
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
            TelaLogin login = new TelaLogin();
            login.Show();
            this.Hide();
        }

        private void btncadastroprodutos_Click(object sender, EventArgs e)
        {
            CadastroProdutos telaprodutos = new CadastroProdutos();
            telaprodutos.Show();
            this.Hide();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}