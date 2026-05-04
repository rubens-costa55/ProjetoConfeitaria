using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PrimeiraTela
{
    public partial class NovoAgendamento : Form
    {
        private bool carregandoCategorias = false;

        private const string PlaceholderNome = "Ex.: Ana Paula Oliveira";
        private const string PlaceholderTelefone = "Digite o telefone";
        private const string PlaceholderDataHora = "15/04/2026 - 16:30";

        private class ItemCarrinho
        {
            public int IdProduto { get; set; }
            public string NomeProduto { get; set; }
            public int Quantidade { get; set; }
            public decimal ValorUnitario { get; set; }
            public decimal ValorItem { get; set; }
        }

        public NovoAgendamento()
        {
            InitializeComponent();

            ConfigurarCarrinho();
            CarregarCategorias();
            LimparPedidoCompleto();
        }

        private void ConfigurarCarrinho()
        {
            dgvCarrinho.ReadOnly = true;
            dgvCarrinho.EditMode = DataGridViewEditMode.EditProgrammatically;

            dgvCarrinho.AllowUserToAddRows = false;
            dgvCarrinho.AllowUserToDeleteRows = false;
            dgvCarrinho.AllowUserToOrderColumns = false;
            dgvCarrinho.AllowUserToResizeColumns = false;
            dgvCarrinho.AllowUserToResizeRows = false;

            dgvCarrinho.MultiSelect = false;
            dgvCarrinho.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCarrinho.RowHeadersVisible = false;
            dgvCarrinho.EnableHeadersVisualStyles = false;

            foreach (DataGridViewColumn coluna in dgvCarrinho.Columns)
            {
                coluna.ReadOnly = true;
                coluna.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            if (dgvCarrinho.Columns["ColValor"] != null)
            {
                dgvCarrinho.Columns["ColValor"].DefaultCellStyle.Format = "N2";
            }

            Color fundo = Color.FromArgb(252, 250, 249);
            Color texto = Color.FromArgb(126, 99, 92);
            Color cabecalho = Color.FromArgb(239, 229, 226);
            Color textoCabecalho = Color.FromArgb(95, 75, 69);

            dgvCarrinho.BackgroundColor = fundo;

            dgvCarrinho.DefaultCellStyle.BackColor = fundo;
            dgvCarrinho.DefaultCellStyle.ForeColor = texto;
            dgvCarrinho.DefaultCellStyle.SelectionBackColor = fundo;
            dgvCarrinho.DefaultCellStyle.SelectionForeColor = texto;

            dgvCarrinho.RowsDefaultCellStyle.BackColor = fundo;
            dgvCarrinho.RowsDefaultCellStyle.ForeColor = texto;
            dgvCarrinho.RowsDefaultCellStyle.SelectionBackColor = fundo;
            dgvCarrinho.RowsDefaultCellStyle.SelectionForeColor = texto;

            dgvCarrinho.AlternatingRowsDefaultCellStyle.BackColor = fundo;
            dgvCarrinho.AlternatingRowsDefaultCellStyle.ForeColor = texto;
            dgvCarrinho.AlternatingRowsDefaultCellStyle.SelectionBackColor = fundo;
            dgvCarrinho.AlternatingRowsDefaultCellStyle.SelectionForeColor = texto;

            dgvCarrinho.ColumnHeadersDefaultCellStyle.BackColor = cabecalho;
            dgvCarrinho.ColumnHeadersDefaultCellStyle.ForeColor = textoCabecalho;
            dgvCarrinho.ColumnHeadersDefaultCellStyle.SelectionBackColor = cabecalho;
            dgvCarrinho.ColumnHeadersDefaultCellStyle.SelectionForeColor = textoCabecalho;

            dgvCarrinho.ClearSelection();

            if (dgvCarrinho.Rows.Count > 0)
            {
                dgvCarrinho.CurrentCell = null;
            }
        }

        private void CarregarCategorias()
        {
            carregandoCategorias = true;

            conexao conect = new conexao();

            using (MySqlConnection conn = conect.Conectar())
            {
                try
                {
                    conn.Open();

                    string sql = @"
                        SELECT
                            id_categoria,
                            nome_categoria
                        FROM categorias
                        ORDER BY nome_categoria;
                    ";

                    MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    cbCategoriaAgendamento.DataSource = dt;
                    cbCategoriaAgendamento.DisplayMember = "nome_categoria";
                    cbCategoriaAgendamento.ValueMember = "id_categoria";
                    cbCategoriaAgendamento.SelectedIndex = -1;

                    cbProdutoAgendamento.DataSource = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar categorias: " + ex.Message);
                }
                finally
                {
                    carregandoCategorias = false;
                }
            }
        }

        private void CarregarProdutosPorCategoria(int idCategoria)
        {
            conexao conect = new conexao();

            using (MySqlConnection conn = conect.Conectar())
            {
                try
                {
                    conn.Open();

                    string sql = @"
                        SELECT
                            id_produto,
                            NomeProduto,
                            PrecoProduto
                        FROM produtos
                        WHERE id_categoria = @id_categoria
                        ORDER BY NomeProduto;
                    ";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id_categoria", idCategoria);

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    cbProdutoAgendamento.DataSource = dt;
                    cbProdutoAgendamento.DisplayMember = "NomeProduto";
                    cbProdutoAgendamento.ValueMember = "id_produto";
                    cbProdutoAgendamento.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar produtos: " + ex.Message);
                }
            }
        }

        private bool CampoVazio(TextBox campo, string placeholder)
        {
            return string.IsNullOrWhiteSpace(campo.Text) || campo.Text.Trim() == placeholder;
        }

        private bool ValidarNomeCliente()
        {
            if (CampoVazio(txtNomeCliente, PlaceholderNome))
            {
                MessageBox.Show(
                    "(Nome do Cliente vazio)",
                    "Campo obrigatório",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                txtNomeCliente.Focus();
                return false;
            }

            string nome = Regex.Replace(txtNomeCliente.Text.Trim(), @"\s+", " ");

            if (!Regex.IsMatch(nome, @"^[A-Za-zÀ-ÖØ-öø-ÿ\s]+$"))
            {
                MessageBox.Show(
                    "Nome do Cliente inválido.\n\nExemplo de preenchimento:\nAna Paula Oliveira\n\nUse apenas letras e espaços.",
                    "Campo preenchido incorretamente",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                txtNomeCliente.Focus();
                return false;
            }

            txtNomeCliente.Text = nome;
            return true;
        }

        private bool ValidarTelefoneCliente()
        {
            if (CampoVazio(txtTelefone, PlaceholderTelefone))
            {
                MessageBox.Show(
                    "(Telefone do Cliente vazio)",
                    "Campo obrigatório",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                txtTelefone.Focus();
                return false;
            }

            string telefoneDigitado = txtTelefone.Text.Trim();
            string apenasNumeros = Regex.Replace(telefoneDigitado, @"\D", "");

            if (apenasNumeros.Length != 10 && apenasNumeros.Length != 11)
            {
                MessageBox.Show(
                    "Telefone do Cliente inválido.\n\nExemplo de preenchimento:\n(11) 99999-9999\n\nDigite o DDD e o número do telefone.",
                    "Campo preenchido incorretamente",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                txtTelefone.Focus();
                return false;
            }

            txtTelefone.Text = FormatarTelefone(apenasNumeros);
            return true;
        }

        private string FormatarTelefone(string numeros)
        {
            if (numeros.Length == 11)
            {
                return "(" + numeros.Substring(0, 2) + ") " +
                       numeros.Substring(2, 5) + "-" +
                       numeros.Substring(7, 4);
            }

            return "(" + numeros.Substring(0, 2) + ") " +
                   numeros.Substring(2, 4) + "-" +
                   numeros.Substring(6, 4);
        }

        private bool ValidarDataHoraEntrega(out DateTime dataEntrega)
        {
            dataEntrega = DateTime.MinValue;

            if (string.IsNullOrWhiteSpace(txtDataeHora.Text))
            {
                MessageBox.Show(
                    "(Data e Hora da entrega vazio)",
                    "Campo obrigatório",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                txtDataeHora.Focus();
                return false;
            }

            string entrega = txtDataeHora.Text.Trim();

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
                    out dataEntrega))
            {
                MessageBox.Show(
                    "Data e Hora da entrega inválida.\n\nExemplo de preenchimento:\n15/04/2026 - 16:30",
                    "Campo preenchido incorretamente",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                txtDataeHora.Focus();
                return false;
            }

            txtDataeHora.Text = dataEntrega.ToString("dd/MM/yyyy - HH:mm");
            return true;
        }

        private bool ValidarQuantidade(out int quantidade)
        {
            quantidade = 0;

            if (string.IsNullOrWhiteSpace(txtQuantidade.Text))
            {
                MessageBox.Show(
                    "(Quantidade vazio)",
                    "Campo obrigatório",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                txtQuantidade.Focus();
                return false;
            }

            if (!int.TryParse(txtQuantidade.Text.Trim(), out quantidade))
            {
                MessageBox.Show(
                    "Quantidade inválida.\n\nExemplo de preenchimento:\n2\n\nDigite apenas números inteiros.",
                    "Campo preenchido incorretamente",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                txtQuantidade.Clear();
                txtQuantidade.Focus();
                return false;
            }

            if (quantidade <= 0)
            {
                MessageBox.Show(
                    "Quantidade inválida.\n\nExemplo de preenchimento:\n2\n\nA quantidade precisa ser maior que zero.",
                    "Campo preenchido incorretamente",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                txtQuantidade.Clear();
                txtQuantidade.Focus();
                return false;
            }

            return true;
        }

        private bool ValidarCamposCentrais()
        {
            if (!ValidarNomeCliente())
                return false;

            if (!ValidarTelefoneCliente())
                return false;

            DateTime dataEntrega;

            if (!ValidarDataHoraEntrega(out dataEntrega))
                return false;

            return true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (carregandoCategorias)
                return;

            if (sender != cbCategoriaAgendamento)
                return;

            if (cbCategoriaAgendamento.SelectedValue == null)
            {
                cbProdutoAgendamento.DataSource = null;
                return;
            }

            int idCategoria;

            if (!int.TryParse(cbCategoriaAgendamento.SelectedValue.ToString(), out idCategoria))
                return;

            if (idCategoria <= 0)
                return;

            CarregarProdutosPorCategoria(idCategoria);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (!ValidarCamposCentrais())
                return;

            if (cbCategoriaAgendamento.SelectedValue == null || cbCategoriaAgendamento.SelectedIndex < 0)
            {
                MessageBox.Show(
                    "Selecione uma categoria.\n\nExemplo: Bento Cakes",
                    "Campo obrigatório",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                cbCategoriaAgendamento.Focus();
                return;
            }

            if (cbProdutoAgendamento.SelectedItem == null || cbProdutoAgendamento.SelectedIndex < 0)
            {
                MessageBox.Show(
                    "Selecione um produto.\n\nExemplo: Bento Cake Chocolate",
                    "Campo obrigatório",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                cbProdutoAgendamento.Focus();
                return;
            }

            int quantidade;

            if (!ValidarQuantidade(out quantidade))
                return;

            DataRowView linhaProduto = cbProdutoAgendamento.SelectedItem as DataRowView;

            if (linhaProduto == null)
            {
                MessageBox.Show("Não foi possível obter o produto selecionado.");
                return;
            }

            int idProduto = Convert.ToInt32(linhaProduto["id_produto"]);
            string nomeProduto = linhaProduto["NomeProduto"].ToString();
            decimal valorUnitario = Convert.ToDecimal(linhaProduto["PrecoProduto"]);
            decimal subtotal = quantidade * valorUnitario;

            ItemCarrinho item = new ItemCarrinho();
            item.IdProduto = idProduto;
            item.NomeProduto = nomeProduto;
            item.Quantidade = quantidade;
            item.ValorUnitario = valorUnitario;
            item.ValorItem = subtotal;

            int indiceLinha = dgvCarrinho.Rows.Add(nomeProduto, quantidade, subtotal);
            dgvCarrinho.Rows[indiceLinha].Tag = item;

            AtualizarTotal();

            txtQuantidade.Clear();
            cbProdutoAgendamento.SelectedIndex = -1;

            dgvCarrinho.ClearSelection();

            if (dgvCarrinho.Rows.Count > 0)
            {
                dgvCarrinho.CurrentCell = null;
            }
        }

        private void btnSalvarNA_Click(object sender, EventArgs e)
        {
            if (!ValidarNomeCliente())
                return;

            if (!ValidarTelefoneCliente())
                return;

            DateTime dataEntrega;

            if (!ValidarDataHoraEntrega(out dataEntrega))
                return;

            if (dgvCarrinho.Rows.Count == 0)
            {
                MessageBox.Show(
                    "Inclua pelo menos um item no carrinho antes de salvar o pedido.\n\nExemplo:\nProduto: Bento Cake Chocolate\nQuantidade: 2",
                    "Carrinho vazio",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            decimal total = CalcularTotalCarrinho();

            conexao conect = new conexao();

            using (MySqlConnection conn = conect.Conectar())
            {
                conn.Open();

                MySqlTransaction transacao = conn.BeginTransaction();

                try
                {
                    string status = dataEntrega < DateTime.Now ? "Atrasado" : "Aberto";

                    string sqlPedido = @"
                        INSERT INTO pedidos
                        (
                            NomeCliente,
                            TelefoneCliente,
                            DataHoraEntrega,
                            ValorTotal,
                            Status
                        )
                        VALUES
                        (
                            @nome,
                            @telefone,
                            @dataEntrega,
                            @total,
                            @status
                        );
                        SELECT LAST_INSERT_ID();
                    ";

                    int idPedido;

                    using (MySqlCommand cmdPedido = new MySqlCommand(sqlPedido, conn, transacao))
                    {
                        cmdPedido.Parameters.AddWithValue("@nome", txtNomeCliente.Text.Trim());
                        cmdPedido.Parameters.AddWithValue("@telefone", txtTelefone.Text.Trim());
                        cmdPedido.Parameters.AddWithValue("@dataEntrega", dataEntrega.ToString("dd/MM/yyyy - HH:mm"));
                        cmdPedido.Parameters.AddWithValue("@total", total);
                        cmdPedido.Parameters.AddWithValue("@status", status);

                        idPedido = Convert.ToInt32(cmdPedido.ExecuteScalar());
                    }

                    string sqlItem = @"
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

                    foreach (DataGridViewRow row in dgvCarrinho.Rows)
                    {
                        if (row.IsNewRow)
                            continue;

                        ItemCarrinho item = row.Tag as ItemCarrinho;

                        if (item == null)
                        {
                            string nomeProdutoFallback = row.Cells["ColProduto"].Value.ToString();
                            int quantidadeFallback = Convert.ToInt32(row.Cells["ColQuantidade"].Value);
                            decimal subtotalFallback = Convert.ToDecimal(row.Cells["ColValor"].Value);
                            decimal valorUnitarioFallback = subtotalFallback / quantidadeFallback;

                            item = new ItemCarrinho();
                            item.IdProduto = BuscarIdProdutoPorNome(nomeProdutoFallback, conn, transacao);
                            item.NomeProduto = nomeProdutoFallback;
                            item.Quantidade = quantidadeFallback;
                            item.ValorUnitario = valorUnitarioFallback;
                            item.ValorItem = subtotalFallback;
                        }

                        using (MySqlCommand cmdItem = new MySqlCommand(sqlItem, conn, transacao))
                        {
                            cmdItem.Parameters.AddWithValue("@id_pedido", idPedido);
                            cmdItem.Parameters.AddWithValue("@id_produto", item.IdProduto);
                            cmdItem.Parameters.AddWithValue("@nome_produto", item.NomeProduto);
                            cmdItem.Parameters.AddWithValue("@quantidade", item.Quantidade);
                            cmdItem.Parameters.AddWithValue("@valor_unitario", item.ValorUnitario);
                            cmdItem.Parameters.AddWithValue("@valor_item", item.ValorItem);
                            cmdItem.ExecuteNonQuery();
                        }
                    }

                    transacao.Commit();

                    MessageBox.Show(
                        "Pedido salvo com sucesso!",
                        "Sucesso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    LimparPedidoCompleto();
                }
                catch (Exception ex)
                {
                    transacao.Rollback();
                    MessageBox.Show("Erro ao salvar pedido: " + ex.Message);
                }
            }
        }

        private int BuscarIdProdutoPorNome(string nomeProduto, MySqlConnection conn, MySqlTransaction transacao)
        {
            string sql = @"
                SELECT id_produto
                FROM produtos
                WHERE NomeProduto = @produto
                LIMIT 1;
            ";

            using (MySqlCommand cmd = new MySqlCommand(sql, conn, transacao))
            {
                cmd.Parameters.AddWithValue("@produto", nomeProduto);

                object resultado = cmd.ExecuteScalar();

                if (resultado == null)
                    throw new Exception("Produto não encontrado: " + nomeProduto);

                return Convert.ToInt32(resultado);
            }
        }

        private void RemoverItem_Click(object sender, EventArgs e)
        {
            if (dgvCarrinho.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Selecione um item do carrinho para remover.",
                    "Item não selecionado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            dgvCarrinho.Rows.RemoveAt(dgvCarrinho.SelectedRows[0].Index);

            AtualizarTotal();

            dgvCarrinho.ClearSelection();

            if (dgvCarrinho.Rows.Count > 0)
            {
                dgvCarrinho.CurrentCell = null;
            }
        }

        private decimal CalcularTotalCarrinho()
        {
            decimal total = 0;

            foreach (DataGridViewRow row in dgvCarrinho.Rows)
            {
                if (row.IsNewRow)
                    continue;

                if (row.Cells["ColValor"].Value != null)
                {
                    total += Convert.ToDecimal(row.Cells["ColValor"].Value);
                }
            }

            return total;
        }

        private void AtualizarTotal()
        {
            decimal total = CalcularTotalCarrinho();

            lbValorTotal.Text = total.ToString("C2", new CultureInfo("pt-BR"));
        }

        private void LimparPedidoCompleto()
        {
            txtNomeCliente.Text = PlaceholderNome;
            txtTelefone.Text = PlaceholderTelefone;
            txtDataeHora.Text = PlaceholderDataHora;
            txtQuantidade.Clear();

            dgvCarrinho.Rows.Clear();
            lbValorTotal.Text = 0.ToString("C2", new CultureInfo("pt-BR"));

            if (cbCategoriaAgendamento.DataSource != null)
                cbCategoriaAgendamento.SelectedIndex = -1;

            cbProdutoAgendamento.DataSource = null;

            dgvCarrinho.ClearSelection();

            if (dgvCarrinho.Rows.Count > 0)
            {
                dgvCarrinho.CurrentCell = null;
            }

            txtNomeCliente.Focus();
        }

        private void txtNomeCliente_Click(object sender, EventArgs e)
        {
            if (txtNomeCliente.Text == PlaceholderNome)
                txtNomeCliente.Clear();
        }

        private void txtTelefone_Click(object sender, EventArgs e)
        {
            if (txtTelefone.Text == PlaceholderTelefone)
                txtTelefone.Clear();
        }

        private void txtDataeHora_Click(object sender, EventArgs e)
        {
            txtDataeHora.SelectAll();
        }

        private void txtQuantidade_Click(object sender, EventArgs e)
        {
            txtQuantidade.SelectAll();
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

        private void btnHistoricoNA_Click(object sender, EventArgs e)
        {
            FrmHistoricoPedidos telahistorico = new FrmHistoricoPedidos();
            telahistorico.Show();
            this.Hide();
        }

        private void btnSairNA_Click(object sender, EventArgs e)
        {
            TelaLogin login = new TelaLogin();
            login.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CadastroProdutos telacadastro = new CadastroProdutos();
            telacadastro.Show();
            this.Hide();
        }

        private void dgvCarrinho_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtTelefone_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDataeHora_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtQuantidade_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void txtProduto_Click(object sender, EventArgs e)
        {

        }

        private void txtValor_Click(object sender, EventArgs e)
        {

        }

        private void txtValor_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void lbClienteResumo_Click(object sender, EventArgs e)
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
    }
}