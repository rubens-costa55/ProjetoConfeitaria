using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PrimeiraTela
{
    public partial class NovoAgendamento : Form
    {
        private bool carregandoCategorias = false;
        private bool restaurandoRascunho = false;

        private const string PlaceholderNome = "Ex.: Ana Paula Oliveira";
        private const string PlaceholderTelefone = "Digite o telefone";

        private class ItemCarrinho
        {
            public int IdProduto { get; set; }
            public string NomeProduto { get; set; }
            public int Quantidade { get; set; }
            public decimal ValorUnitario { get; set; }
            public decimal ValorItem { get; set; }
        }

        private class ItemOrcamentoPdf
        {
            public string Produto { get; set; }
            public int Quantidade { get; set; }
            public decimal ValorUnitario { get; set; }
            public decimal ValorTotal { get; set; }
        }

        private static bool rascunhoAtivo = false;
        private static string rascunhoNomeCliente = "";
        private static string rascunhoTelefone = "";
        private static string rascunhoQuantidade = "";
        private static DateTime rascunhoDataEntrega = DateTime.Today;
        private static DateTime rascunhoHoraEntrega = DateTime.Now;
        private static int rascunhoCategoriaId = 0;
        private static int rascunhoProdutoId = 0;
        private static List<ItemCarrinho> rascunhoItens = new List<ItemCarrinho>();

        private List<ItemOrcamentoPdf> itensOrcamentoPdf = new List<ItemOrcamentoPdf>();
        private int indiceItemOrcamentoPdf = 0;
        private string nomeClienteOrcamentoPdf = "";
        private string telefoneClienteOrcamentoPdf = "";
        private DateTime dataEntregaOrcamentoPdf = DateTime.Now;
        private decimal totalOrcamentoPdf = 0;

        public NovoAgendamento()
        {
            InitializeComponent();

            ConfigurarCarrinho();
            ConfigurarDataHora();
            ConfigurarBotaoOrcamento();
            CarregarCategorias();

            if (rascunhoAtivo)
            {
                RestaurarRascunhoTemporario();
            }
            else
            {
                LimparPedidoCompleto();
            }
        }

        private void ConfigurarDataHora()
        {
            dtpDataEntrega.Format = DateTimePickerFormat.Custom;
            dtpDataEntrega.CustomFormat = "dd/MM/yyyy";

            dtpHoraEntrega.Format = DateTimePickerFormat.Custom;
            dtpHoraEntrega.CustomFormat = "HH:mm";
            dtpHoraEntrega.ShowUpDown = true;
        }

        private void ConfigurarBotaoOrcamento()
        {
            btnExportarOrcamento.Click -= btnExportarOrcamento_Click;
            btnExportarOrcamento.Click += btnExportarOrcamento_Click;
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

            Color selecao = Color.FromArgb(232, 174, 184);
            Color textoSelecao = Color.White;

            dgvCarrinho.BackgroundColor = fundo;

            dgvCarrinho.DefaultCellStyle.BackColor = fundo;
            dgvCarrinho.DefaultCellStyle.ForeColor = texto;
            dgvCarrinho.DefaultCellStyle.SelectionBackColor = selecao;
            dgvCarrinho.DefaultCellStyle.SelectionForeColor = textoSelecao;

            dgvCarrinho.RowsDefaultCellStyle.BackColor = fundo;
            dgvCarrinho.RowsDefaultCellStyle.ForeColor = texto;
            dgvCarrinho.RowsDefaultCellStyle.SelectionBackColor = selecao;
            dgvCarrinho.RowsDefaultCellStyle.SelectionForeColor = textoSelecao;

            dgvCarrinho.AlternatingRowsDefaultCellStyle.BackColor = fundo;
            dgvCarrinho.AlternatingRowsDefaultCellStyle.ForeColor = texto;
            dgvCarrinho.AlternatingRowsDefaultCellStyle.SelectionBackColor = selecao;
            dgvCarrinho.AlternatingRowsDefaultCellStyle.SelectionForeColor = textoSelecao;

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
            DateTime data = dtpDataEntrega.Value.Date;
            TimeSpan hora = dtpHoraEntrega.Value.TimeOfDay;

            dataEntrega = data.Add(hora);

            if (dataEntrega < DateTime.Now)
            {
                DialogResult resposta = MessageBox.Show(
                    "A data e hora escolhida já passou.\n\nDeseja salvar esse pedido como atrasado?",
                    "Data e hora anterior ao momento atual",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (resposta != DialogResult.Yes)
                {
                    dtpDataEntrega.Focus();
                    return false;
                }
            }

            return true;
        }

        private DateTime ObterDataHoraEntregaSelecionada()
        {
            DateTime data = dtpDataEntrega.Value.Date;
            TimeSpan hora = dtpHoraEntrega.Value.TimeOfDay;

            return data.Add(hora);
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
            if (carregandoCategorias || restaurandoRascunho)
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
            dgvCarrinho.CurrentCell = null;

            SalvarRascunhoTemporario();
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
                    LimparRascunhoTemporario();
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
            dgvCarrinho.CurrentCell = null;

            SalvarRascunhoTemporario();
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
            txtQuantidade.Clear();

            dtpDataEntrega.Value = DateTime.Today;
            dtpHoraEntrega.Value = DateTime.Now;

            dgvCarrinho.Rows.Clear();
            lbValorTotal.Text = 0.ToString("C2", new CultureInfo("pt-BR"));

            if (cbCategoriaAgendamento.DataSource != null)
                cbCategoriaAgendamento.SelectedIndex = -1;

            cbProdutoAgendamento.DataSource = null;

            dgvCarrinho.ClearSelection();
            dgvCarrinho.CurrentCell = null;

            txtNomeCliente.Focus();
        }

        private bool TextoPreenchido(TextBox campo, string placeholder)
        {
            if (campo == null)
                return false;

            string texto = campo.Text.Trim();

            return !string.IsNullOrWhiteSpace(texto) && texto != placeholder;
        }

        private bool ExisteAlgumDadoParaRascunho()
        {
            if (TextoPreenchido(txtNomeCliente, PlaceholderNome))
                return true;

            if (TextoPreenchido(txtTelefone, PlaceholderTelefone))
                return true;

            if (!string.IsNullOrWhiteSpace(txtQuantidade.Text))
                return true;

            if (cbCategoriaAgendamento.SelectedValue != null && cbCategoriaAgendamento.SelectedIndex >= 0)
                return true;

            if (cbProdutoAgendamento.SelectedValue != null && cbProdutoAgendamento.SelectedIndex >= 0)
                return true;

            if (dgvCarrinho.Rows.Count > 0)
                return true;

            if (dtpDataEntrega.Value.Date != DateTime.Today)
                return true;

            if (Math.Abs((dtpHoraEntrega.Value.TimeOfDay - DateTime.Now.TimeOfDay).TotalMinutes) > 2)
                return true;

            return false;
        }

        private ItemCarrinho CopiarItem(ItemCarrinho item)
        {
            if (item == null)
                return null;

            return new ItemCarrinho
            {
                IdProduto = item.IdProduto,
                NomeProduto = item.NomeProduto,
                Quantidade = item.Quantidade,
                ValorUnitario = item.ValorUnitario,
                ValorItem = item.ValorItem
            };
        }

        private void SalvarRascunhoTemporario()
        {
            if (!ExisteAlgumDadoParaRascunho())
            {
                LimparRascunhoTemporario();
                return;
            }

            rascunhoAtivo = true;

            rascunhoNomeCliente = txtNomeCliente.Text;
            rascunhoTelefone = txtTelefone.Text;
            rascunhoQuantidade = txtQuantidade.Text;

            rascunhoDataEntrega = dtpDataEntrega.Value;
            rascunhoHoraEntrega = dtpHoraEntrega.Value;

            rascunhoCategoriaId = 0;
            rascunhoProdutoId = 0;

            if (cbCategoriaAgendamento.SelectedValue != null)
            {
                int.TryParse(cbCategoriaAgendamento.SelectedValue.ToString(), out rascunhoCategoriaId);
            }

            if (cbProdutoAgendamento.SelectedValue != null)
            {
                int.TryParse(cbProdutoAgendamento.SelectedValue.ToString(), out rascunhoProdutoId);
            }

            rascunhoItens.Clear();

            foreach (DataGridViewRow row in dgvCarrinho.Rows)
            {
                if (row.IsNewRow)
                    continue;

                ItemCarrinho item = row.Tag as ItemCarrinho;

                if (item != null)
                {
                    rascunhoItens.Add(CopiarItem(item));
                }
                else
                {
                    try
                    {
                        string nomeProduto = row.Cells["ColProduto"].Value?.ToString() ?? "";
                        int quantidade = Convert.ToInt32(row.Cells["ColQuantidade"].Value);
                        decimal valorItem = Convert.ToDecimal(row.Cells["ColValor"].Value);
                        decimal valorUnitario = quantidade > 0 ? valorItem / quantidade : 0;

                        rascunhoItens.Add(new ItemCarrinho
                        {
                            IdProduto = 0,
                            NomeProduto = nomeProduto,
                            Quantidade = quantidade,
                            ValorUnitario = valorUnitario,
                            ValorItem = valorItem
                        });
                    }
                    catch
                    {

                    }
                }
            }
        }

        private void RestaurarRascunhoTemporario()
        {
            restaurandoRascunho = true;

            try
            {
                txtNomeCliente.Text = string.IsNullOrWhiteSpace(rascunhoNomeCliente) ? PlaceholderNome : rascunhoNomeCliente;
                txtTelefone.Text = string.IsNullOrWhiteSpace(rascunhoTelefone) ? PlaceholderTelefone : rascunhoTelefone;
                txtQuantidade.Text = rascunhoQuantidade;

                dtpDataEntrega.Value = rascunhoDataEntrega;
                dtpHoraEntrega.Value = rascunhoHoraEntrega;

                if (rascunhoCategoriaId > 0 && cbCategoriaAgendamento.DataSource != null)
                {
                    try
                    {
                        cbCategoriaAgendamento.SelectedValue = rascunhoCategoriaId;
                        CarregarProdutosPorCategoria(rascunhoCategoriaId);
                    }
                    catch
                    {
                        cbCategoriaAgendamento.SelectedIndex = -1;
                    }
                }
                else
                {
                    cbCategoriaAgendamento.SelectedIndex = -1;
                    cbProdutoAgendamento.DataSource = null;
                }

                if (rascunhoProdutoId > 0 && cbProdutoAgendamento.DataSource != null)
                {
                    try
                    {
                        cbProdutoAgendamento.SelectedValue = rascunhoProdutoId;
                    }
                    catch
                    {
                        cbProdutoAgendamento.SelectedIndex = -1;
                    }
                }

                dgvCarrinho.Rows.Clear();

                foreach (ItemCarrinho itemSalvo in rascunhoItens)
                {
                    ItemCarrinho item = CopiarItem(itemSalvo);

                    if (item == null)
                        continue;

                    int indiceLinha = dgvCarrinho.Rows.Add(item.NomeProduto, item.Quantidade, item.ValorItem);
                    dgvCarrinho.Rows[indiceLinha].Tag = item;
                }

                AtualizarTotal();

                dgvCarrinho.ClearSelection();
                dgvCarrinho.CurrentCell = null;
            }
            finally
            {
                restaurandoRascunho = false;
            }
        }

        private void LimparRascunhoTemporario()
        {
            rascunhoAtivo = false;
            rascunhoNomeCliente = "";
            rascunhoTelefone = "";
            rascunhoQuantidade = "";
            rascunhoDataEntrega = DateTime.Today;
            rascunhoHoraEntrega = DateTime.Now;
            rascunhoCategoriaId = 0;
            rascunhoProdutoId = 0;
            rascunhoItens.Clear();
        }

        private void AbrirTelaMantendoRascunho(Form tela)
        {
            SalvarRascunhoTemporario();

            tela.Show();
            this.Hide();
        }

        private void btnExportarOrcamento_Click(object sender, EventArgs e)
        {
            ExportarOrcamentoPdf();
        }

        private void ExportarOrcamentoPdf()
        {
            if (!ValidarNomeCliente())
                return;

            if (!ValidarTelefoneCliente())
                return;

            if (dgvCarrinho.Rows.Count == 0)
            {
                MessageBox.Show(
                    "Inclua pelo menos um item no carrinho antes de gerar o orçamento.",
                    "Carrinho vazio",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            itensOrcamentoPdf = ObterItensCarrinhoParaOrcamento();

            if (itensOrcamentoPdf.Count == 0)
            {
                MessageBox.Show(
                    "Não foi possível obter os itens do carrinho para gerar o orçamento.",
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                return;
            }

            bool temMicrosoftPrintPdf = PrinterSettings.InstalledPrinters
                .Cast<string>()
                .Any(p => p.Equals("Microsoft Print to PDF", StringComparison.OrdinalIgnoreCase));

            if (!temMicrosoftPrintPdf)
            {
                MessageBox.Show(
                    "A impressora 'Microsoft Print to PDF' não foi encontrada no Windows.\n\n" +
                    "Ative esse recurso no Windows para exportar o orçamento em PDF.",
                    "PDF indisponível",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            SaveFileDialog salvar = new SaveFileDialog();
            salvar.Title = "Salvar orçamento em PDF";
            salvar.Filter = "PDF (*.pdf)|*.pdf";
            salvar.FileName = "orcamento_" + SanitizarNomeArquivo(txtNomeCliente.Text.Trim()) + ".pdf";

            if (salvar.ShowDialog() != DialogResult.OK)
                return;

            string caminho = salvar.FileName;

            if (Path.GetExtension(caminho).ToLower() != ".pdf")
            {
                caminho += ".pdf";
            }

            nomeClienteOrcamentoPdf = txtNomeCliente.Text.Trim();
            telefoneClienteOrcamentoPdf = txtTelefone.Text.Trim();
            dataEntregaOrcamentoPdf = ObterDataHoraEntregaSelecionada();
            totalOrcamentoPdf = itensOrcamentoPdf.Sum(i => i.ValorTotal);
            indiceItemOrcamentoPdf = 0;

            try
            {
                PrintDocument documento = new PrintDocument();
                documento.DocumentName = "Orçamento";
                documento.DefaultPageSettings.Landscape = false;
                documento.PrinterSettings.PrinterName = "Microsoft Print to PDF";
                documento.PrinterSettings.PrintToFile = true;
                documento.PrinterSettings.PrintFileName = caminho;
                documento.PrintController = new StandardPrintController();

                documento.PrintPage += DocumentoOrcamento_PrintPage;
                documento.Print();

                MessageBox.Show(
                    "Orçamento exportado com sucesso!",
                    "PDF gerado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao exportar orçamento em PDF: " + ex.Message);
            }
        }

        private List<ItemOrcamentoPdf> ObterItensCarrinhoParaOrcamento()
        {
            List<ItemOrcamentoPdf> itens = new List<ItemOrcamentoPdf>();

            foreach (DataGridViewRow row in dgvCarrinho.Rows)
            {
                if (row.IsNewRow)
                    continue;

                try
                {
                    string produto = row.Cells["ColProduto"].Value?.ToString() ?? "";
                    int quantidade = Convert.ToInt32(row.Cells["ColQuantidade"].Value);
                    decimal valorTotal = Convert.ToDecimal(row.Cells["ColValor"].Value);
                    decimal valorUnitario = quantidade > 0 ? valorTotal / quantidade : 0;

                    ItemCarrinho itemTag = row.Tag as ItemCarrinho;

                    if (itemTag != null)
                    {
                        produto = itemTag.NomeProduto;
                        quantidade = itemTag.Quantidade;
                        valorUnitario = itemTag.ValorUnitario;
                        valorTotal = itemTag.ValorItem;
                    }

                    itens.Add(new ItemOrcamentoPdf
                    {
                        Produto = produto,
                        Quantidade = quantidade,
                        ValorUnitario = valorUnitario,
                        ValorTotal = valorTotal
                    });
                }
                catch
                {

                }
            }

            return itens;
        }

        private string SanitizarNomeArquivo(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return "cliente";

            foreach (char caractere in Path.GetInvalidFileNameChars())
            {
                texto = texto.Replace(caractere.ToString(), "");
            }

            texto = texto.Trim().Replace(" ", "_");

            if (string.IsNullOrWhiteSpace(texto))
                return "cliente";

            return texto;
        }

        private void DocumentoOrcamento_PrintPage(object sender, PrintPageEventArgs e)
        {
            Color corRosa = Color.FromArgb(232, 174, 184);
            Color corRose = Color.FromArgb(201, 142, 124);
            Color corTexto = Color.FromArgb(95, 75, 69);
            Color corTextoClaro = Color.FromArgb(126, 99, 92);
            Color corFundoClaro = Color.FromArgb(252, 250, 249);
            Color corTabela = Color.FromArgb(239, 229, 226);
            Color corLinha = Color.FromArgb(228, 206, 199);

            using (Font fonteTitulo = new Font("Segoe UI", 24, FontStyle.Bold))
            using (Font fonteSubtitulo = new Font("Segoe UI", 11, FontStyle.Bold))
            using (Font fonteTexto = new Font("Segoe UI", 10, FontStyle.Regular))
            using (Font fonteTextoBold = new Font("Segoe UI", 10, FontStyle.Bold))
            using (Font fonteTabela = new Font("Segoe UI", 9, FontStyle.Regular))
            using (Font fonteTabelaBold = new Font("Segoe UI", 9, FontStyle.Bold))
            using (Font fonteTotal = new Font("Segoe UI", 13, FontStyle.Bold))
            using (SolidBrush brushRosa = new SolidBrush(corRosa))
            using (SolidBrush brushRose = new SolidBrush(corRose))
            using (SolidBrush brushTexto = new SolidBrush(corTexto))
            using (SolidBrush brushTextoClaro = new SolidBrush(corTextoClaro))
            using (SolidBrush brushBranco = new SolidBrush(Color.White))
            using (SolidBrush brushFundoClaro = new SolidBrush(corFundoClaro))
            using (SolidBrush brushTabela = new SolidBrush(corTabela))
            using (Pen penLinha = new Pen(corLinha, 1))
            {
                Graphics g = e.Graphics;
                g.Clear(Color.White);

                Rectangle margem = e.MarginBounds;

                int x = margem.Left;
                int y = margem.Top;
                int largura = margem.Width;

                g.FillRectangle(brushFundoClaro, x, y, largura, margem.Height);

                Rectangle cabecalho = new Rectangle(x, y, largura, 105);
                g.FillRectangle(brushRosa, cabecalho);

                try
                {
                    Image logo = Properties.Resources.LOGO__1__removebg_preview;
                    g.DrawImage(logo, x + 20, y + 12, 95, 75);
                }
                catch
                {

                }

                StringFormat centralizado = new StringFormat();
                centralizado.Alignment = StringAlignment.Center;
                centralizado.LineAlignment = StringAlignment.Center;

                g.DrawString(
                    "ORÇAMENTO",
                    fonteTitulo,
                    brushBranco,
                    new RectangleF(x, y + 18, largura, 45),
                    centralizado
                );

                g.DrawString(
                    "Thayara Polizel - Confeitaria Artesanal",
                    fonteSubtitulo,
                    brushBranco,
                    new RectangleF(x, y + 62, largura, 25),
                    centralizado
                );

                y += 130;

                g.DrawString("Dados do cliente", fonteSubtitulo, brushRose, x + 20, y);
                y += 28;

                g.DrawString("Cliente:", fonteTextoBold, brushTexto, x + 20, y);
                g.DrawString(nomeClienteOrcamentoPdf, fonteTexto, brushTextoClaro, x + 95, y);

                g.DrawString("Telefone:", fonteTextoBold, brushTexto, x + 350, y);
                g.DrawString(telefoneClienteOrcamentoPdf, fonteTexto, brushTextoClaro, x + 430, y);

                y += 25;

                g.DrawString("Entrega:", fonteTextoBold, brushTexto, x + 20, y);
                g.DrawString(dataEntregaOrcamentoPdf.ToString("dd/MM/yyyy - HH:mm"), fonteTexto, brushTextoClaro, x + 95, y);

                y += 45;

                int alturaCabecalhoTabela = 32;
                int alturaLinha = 34;

                int colProduto = 280;
                int colValorUnitario = 105;
                int colQuantidade = 75;
                int colTotal = largura - colProduto - colValorUnitario - colQuantidade - 40;

                int tabelaX = x + 20;
                int tabelaY = y;
                int tabelaLargura = largura - 40;

                g.FillRectangle(brushTabela, tabelaX, tabelaY, tabelaLargura, alturaCabecalhoTabela);

                g.DrawString("Produto", fonteTabelaBold, brushTexto, tabelaX + 8, tabelaY + 8);
                g.DrawString("Valor unit.", fonteTabelaBold, brushTexto, tabelaX + colProduto + 8, tabelaY + 8);
                g.DrawString("Qtd", fonteTabelaBold, brushTexto, tabelaX + colProduto + colValorUnitario + 8, tabelaY + 8);
                g.DrawString("Total", fonteTabelaBold, brushTexto, tabelaX + colProduto + colValorUnitario + colQuantidade + 8, tabelaY + 8);

                y += alturaCabecalhoTabela;

                while (indiceItemOrcamentoPdf < itensOrcamentoPdf.Count)
                {
                    if (y + alturaLinha + 95 > margem.Bottom)
                    {
                        e.HasMorePages = true;
                        return;
                    }

                    ItemOrcamentoPdf item = itensOrcamentoPdf[indiceItemOrcamentoPdf];

                    g.FillRectangle(Brushes.White, tabelaX, y, tabelaLargura, alturaLinha);

                    g.DrawString(
                        item.Produto,
                        fonteTabela,
                        brushTextoClaro,
                        new RectangleF(tabelaX + 8, y + 8, colProduto - 16, alturaLinha - 8)
                    );

                    g.DrawString(
                        item.ValorUnitario.ToString("C2", new CultureInfo("pt-BR")),
                        fonteTabela,
                        brushTextoClaro,
                        tabelaX + colProduto + 8,
                        y + 8
                    );

                    g.DrawString(
                        item.Quantidade.ToString(),
                        fonteTabela,
                        brushTextoClaro,
                        tabelaX + colProduto + colValorUnitario + 8,
                        y + 8
                    );

                    g.DrawString(
                        item.ValorTotal.ToString("C2", new CultureInfo("pt-BR")),
                        fonteTabela,
                        brushTextoClaro,
                        tabelaX + colProduto + colValorUnitario + colQuantidade + 8,
                        y + 8
                    );

                    g.DrawLine(penLinha, tabelaX, y + alturaLinha, tabelaX + tabelaLargura, y + alturaLinha);

                    y += alturaLinha;
                    indiceItemOrcamentoPdf++;
                }

                y += 35;

                Rectangle totalBox = new Rectangle(tabelaX + tabelaLargura - 250, y, 250, 50);
                g.FillRectangle(brushRose, totalBox);

                g.DrawString(
                    "Total do pedido",
                    fonteTextoBold,
                    brushBranco,
                    totalBox.X + 15,
                    totalBox.Y + 8
                );

                g.DrawString(
                    totalOrcamentoPdf.ToString("C2", new CultureInfo("pt-BR")),
                    fonteTotal,
                    brushBranco,
                    totalBox.X + 15,
                    totalBox.Y + 25
                );

                y += 85;

                g.DrawLine(penLinha, x + 20, margem.Bottom - 45, x + largura - 20, margem.Bottom - 45);

                g.DrawString(
                    "Orçamento gerado em " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                    fonteTexto,
                    brushTextoClaro,
                    x + 20,
                    margem.Bottom - 32
                );

                g.DrawString(
                    "Valores sujeitos à confirmação.",
                    fonteTexto,
                    brushTextoClaro,
                    x + largura - 230,
                    margem.Bottom - 32
                );

                e.HasMorePages = false;
                indiceItemOrcamentoPdf = 0;
            }
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

        private void txtQuantidade_Click(object sender, EventArgs e)
        {
            txtQuantidade.SelectAll();
        }

        private void btnMenuNA_Click(object sender, EventArgs e)
        {
            AbrirTelaMantendoRascunho(new MenuPrincipal());
        }

        private void btnPedidosAtuaisNA_Click(object sender, EventArgs e)
        {
            AbrirTelaMantendoRascunho(new PedidosAtuais());
        }

        private void btnHistoricoNA_Click(object sender, EventArgs e)
        {
            AbrirTelaMantendoRascunho(new FrmHistoricoPedidos());
        }

        private void btnSairNA_Click(object sender, EventArgs e)
        {
            AbrirTelaMantendoRascunho(new TelaLogin());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AbrirTelaMantendoRascunho(new CadastroProdutos());
        }

        private void dgvCarrinho_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtTelefone_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDataeHora_Click(object sender, EventArgs e)
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

        private void NovoAgendamento_Load(object sender, EventArgs e)
        {
            MoverJanela.Ativar(this);
        }

        private void dtpDataEntrega_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}