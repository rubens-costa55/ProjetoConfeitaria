using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace PrimeiraTela
{
    public partial class FrmHistoricoPedidos : Form
    {
        private string filtroAtual = "Todos";
        private DataTable dadosParaPdf = new DataTable();
        private int linhaAtualPdf = 0;

        public FrmHistoricoPedidos()
        {
            InitializeComponent();
        }

        private void FrmHistoricoPedidos_Load(object sender, EventArgs e)
        {
            MoverJanela.Ativar(this);
            ConfigurarGrid();
            ConfigurarFiltros();
            ConfigurarBusca();
            ConfigurarEventos();

            CarregarHistorico();
        }

        private void ConfigurarGrid()
        {
            dgvPedidos.AutoGenerateColumns = false;
            dgvPedidos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPedidos.MultiSelect = false;
            dgvPedidos.AllowUserToAddRows = false;
            dgvPedidos.AllowUserToDeleteRows = false;
            dgvPedidos.AllowUserToResizeRows = false;
            dgvPedidos.AllowUserToResizeColumns = false;
            dgvPedidos.ReadOnly = true;
            dgvPedidos.RowHeadersVisible = false;
            dgvPedidos.EnableHeadersVisualStyles = false;
            dgvPedidos.BackgroundColor = Color.FromArgb(252, 250, 249);
            dgvPedidos.GridColor = Color.FromArgb(231, 211, 204);
            dgvPedidos.BorderStyle = BorderStyle.None;

            dgvPedidos.Columns["colcliente"].DataPropertyName = "NomeCliente";
            dgvPedidos.Columns["colpedido"].DataPropertyName = "Produto";
            dgvPedidos.Columns["colentrega"].DataPropertyName = "DataHoraEntrega";
            dgvPedidos.Columns["colvalor"].DataPropertyName = "Valor";

            dgvPedidos.Columns["colvalor"].DefaultCellStyle.Format = "C2";

            Color fundoLinha = Color.FromArgb(252, 250, 249);
            Color fundoSelecionado = Color.FromArgb(243, 232, 228);
            Color texto = Color.FromArgb(111, 84, 75);
            Color fundoCabecalho = Color.FromArgb(239, 229, 226);
            Color textoCabecalho = Color.FromArgb(95, 75, 69);

            dgvPedidos.DefaultCellStyle.BackColor = fundoLinha;
            dgvPedidos.DefaultCellStyle.ForeColor = texto;
            dgvPedidos.DefaultCellStyle.SelectionBackColor = fundoSelecionado;
            dgvPedidos.DefaultCellStyle.SelectionForeColor = texto;

            dgvPedidos.RowsDefaultCellStyle.BackColor = fundoLinha;
            dgvPedidos.RowsDefaultCellStyle.ForeColor = texto;
            dgvPedidos.RowsDefaultCellStyle.SelectionBackColor = fundoSelecionado;
            dgvPedidos.RowsDefaultCellStyle.SelectionForeColor = texto;

            dgvPedidos.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
            dgvPedidos.AlternatingRowsDefaultCellStyle.ForeColor = texto;
            dgvPedidos.AlternatingRowsDefaultCellStyle.SelectionBackColor = fundoSelecionado;
            dgvPedidos.AlternatingRowsDefaultCellStyle.SelectionForeColor = texto;

            dgvPedidos.ColumnHeadersDefaultCellStyle.BackColor = fundoCabecalho;
            dgvPedidos.ColumnHeadersDefaultCellStyle.ForeColor = textoCabecalho;
            dgvPedidos.ColumnHeadersDefaultCellStyle.SelectionBackColor = fundoCabecalho;
            dgvPedidos.ColumnHeadersDefaultCellStyle.SelectionForeColor = textoCabecalho;
            dgvPedidos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular);

            dgvPedidos.RowHeadersDefaultCellStyle.BackColor = fundoLinha;
            dgvPedidos.RowHeadersDefaultCellStyle.ForeColor = texto;
            dgvPedidos.RowHeadersDefaultCellStyle.SelectionBackColor = fundoSelecionado;
            dgvPedidos.RowHeadersDefaultCellStyle.SelectionForeColor = texto;

            foreach (DataGridViewColumn coluna in dgvPedidos.Columns)
            {
                coluna.ReadOnly = true;
                coluna.SortMode = DataGridViewColumnSortMode.Automatic;
            }

            dgvPedidos.ClearSelection();
            dgvPedidos.CurrentCell = null;
        }

        private void ConfigurarFiltros()
        {
            cbFiltrosHistorico.SelectedIndexChanged -= cbFiltrosHistorico_SelectedIndexChanged;
            cbFiltrosHistorico.SelectedIndexChanged -= cbFiltrosHistorico_SelectedIndexChanged_1;

            cbFiltrosHistorico.Items.Clear();
            cbFiltrosHistorico.Items.Add("Filtros...");
            cbFiltrosHistorico.Items.Add("Todos");
            cbFiltrosHistorico.Items.Add("Últimos 30 dias");
            cbFiltrosHistorico.Items.Add("Este mês");
            cbFiltrosHistorico.Items.Add("Maior valor");

            cbFiltrosHistorico.SelectedIndex = 0;

            cbFiltrosHistorico.SelectedIndexChanged += cbFiltrosHistorico_SelectedIndexChanged;
        }

        private void ConfigurarBusca()
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox1.Text = "Buscar pedido..";
                textBox1.ForeColor = Color.FromArgb(191, 167, 157);
            }
        }

        private void ConfigurarEventos()
        {
            btnPesquisarHistorico.Click -= btnPesquisarHistorico_Click;
            btnPesquisarHistorico.Click += btnPesquisarHistorico_Click;

            button1.Click -= button1_Click;
            button1.Click += button1_Click;

            btnremover.Click -= btnremover_Click;
            btnremover.Click += btnremover_Click;

            textBox1.Enter -= textBox1_Enter;
            textBox1.Enter += textBox1_Enter;

            textBox1.Leave -= textBox1_Leave;
            textBox1.Leave += textBox1_Leave;

            textBox1.KeyDown -= textBox1_KeyDown;
            textBox1.KeyDown += textBox1_KeyDown;

            dgvPedidos.CellContentClick -= dgvPedidos_CellContentClick;
            dgvPedidos.CellContentClick += dgvPedidos_CellContentClick;

            button2.Click -= button2_Click;
            button2.Click += button2_Click;

            button3.Click -= button3_Click;
            button3.Click += button3_Click;
        }

        private string ObterTextoBusca()
        {
            string busca = textBox1.Text.Trim();

            if (busca == "Buscar pedido.." || busca == "Buscar pedido.")
                return "";

            return busca;
        }

        private void CarregarHistorico()
        {
            string busca = ObterTextoBusca();

            conexao conexao = new conexao();

            using (MySqlConnection con = conexao.Conectar())
            {
                try
                {
                    con.Open();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = con;

                    string sql = MontarSqlHistorico(busca, cmd);
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
                    MessageBox.Show("Erro ao carregar histórico: " + ex.Message);
                }
            }
        }

        private string MontarSqlHistorico(string busca, MySqlCommand cmd)
        {
            string where = " WHERE 1 = 1 ";

            if (!string.IsNullOrWhiteSpace(busca))
            {
                where += @"
                    AND (
                        h.NomeCliente LIKE @busca
                        OR h.TelefoneCliente LIKE @busca
                        OR h.Produto LIKE @busca
                        OR h.DataHoraEntrega LIKE @busca
                        OR CAST(h.Quantidade AS CHAR) LIKE @busca
                        OR CAST(h.Valor AS CHAR) LIKE @busca
                    )
                ";

                cmd.Parameters.AddWithValue("@busca", "%" + busca + "%");
            }

            if (filtroAtual == "Ultimos30Dias")
            {
                where += @"
                    AND DATE(
                        COALESCE(
                            STR_TO_DATE(h.DataHoraEntrega, '%d/%m/%Y %H:%i'),
                            STR_TO_DATE(h.DataHoraEntrega, '%d/%m/%Y - %H:%i'),
                            STR_TO_DATE(h.DataHoraEntrega, '%d/%m/%Y %Hh'),
                            STR_TO_DATE(h.DataHoraEntrega, '%d/%m/%Y - %Hh')
                        )
                    ) >= DATE_SUB(CURDATE(), INTERVAL 30 DAY)
                ";
            }

            if (filtroAtual == "EsteMes")
            {
                where += @"
                    AND MONTH(
                        COALESCE(
                            STR_TO_DATE(h.DataHoraEntrega, '%d/%m/%Y %H:%i'),
                            STR_TO_DATE(h.DataHoraEntrega, '%d/%m/%Y - %H:%i'),
                            STR_TO_DATE(h.DataHoraEntrega, '%d/%m/%Y %Hh'),
                            STR_TO_DATE(h.DataHoraEntrega, '%d/%m/%Y - %Hh')
                        )
                    ) = MONTH(CURDATE())
                    AND YEAR(
                        COALESCE(
                            STR_TO_DATE(h.DataHoraEntrega, '%d/%m/%Y %H:%i'),
                            STR_TO_DATE(h.DataHoraEntrega, '%d/%m/%Y - %H:%i'),
                            STR_TO_DATE(h.DataHoraEntrega, '%d/%m/%Y %Hh'),
                            STR_TO_DATE(h.DataHoraEntrega, '%d/%m/%Y - %Hh')
                        )
                    ) = YEAR(CURDATE())
                ";
            }

            if (filtroAtual == "ClienteRecorrente")
            {
                return @"
                    SELECT
                        0 AS id_historico,
                        h.NomeCliente,
                        h.TelefoneCliente,
                        CONCAT(COUNT(DISTINCT h.DataHoraEntrega), ' pedidos no histórico') AS Produto,
                        MAX(h.DataHoraEntrega) AS DataHoraEntrega,
                        SUM(h.Valor) AS Valor
                    FROM historico h
                " + where + @"
                    GROUP BY
                        h.NomeCliente,
                        h.TelefoneCliente
                    HAVING COUNT(DISTINCT h.DataHoraEntrega) > 1
                    ORDER BY
                        COUNT(DISTINCT h.DataHoraEntrega) DESC,
                        SUM(h.Valor) DESC;
                ";
            }

            string orderBy = @"
                ORDER BY
                    COALESCE(
                        STR_TO_DATE(MAX(h.DataHoraEntrega), '%d/%m/%Y %H:%i'),
                        STR_TO_DATE(MAX(h.DataHoraEntrega), '%d/%m/%Y - %H:%i'),
                        STR_TO_DATE(MAX(h.DataHoraEntrega), '%d/%m/%Y %Hh'),
                        STR_TO_DATE(MAX(h.DataHoraEntrega), '%d/%m/%Y - %Hh')
                    ) DESC
            ";

            if (filtroAtual == "MaiorValor")
            {
                orderBy = " ORDER BY SUM(h.Valor) DESC ";
            }

            return @"
                SELECT
                    MIN(h.id_historico) AS id_historico,
                    h.NomeCliente,
                    h.TelefoneCliente,
                    GROUP_CONCAT(
                        CONCAT(h.Produto, ' (', h.Quantidade, 'x)')
                        SEPARATOR ', '
                    ) AS Produto,
                    h.DataHoraEntrega,
                    SUM(h.Valor) AS Valor
                FROM historico h
            " + where + @"
                GROUP BY
                    h.NomeCliente,
                    h.TelefoneCliente,
                    h.DataHoraEntrega
            " + orderBy + ";";
        }

        private void cbFiltrosHistorico_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFiltrosHistorico.SelectedItem == null)
                return;

            string filtroSelecionado = cbFiltrosHistorico.SelectedItem.ToString();

            if (filtroSelecionado == "Filtros...")
            {
                filtroAtual = "Todos";
                return;
            }

            switch (filtroSelecionado)
            {
                case "Todos":
                    filtroAtual = "Todos";
                    break;

                case "Últimos 30 dias":
                    filtroAtual = "Ultimos30Dias";
                    break;

                case "Este mês":
                    filtroAtual = "EsteMes";
                    break;

                case "Cliente recorrente":
                    filtroAtual = "ClienteRecorrente";
                    break;

                case "Maior valor":
                    filtroAtual = "MaiorValor";
                    break;

                default:
                    filtroAtual = "Todos";
                    break;
            }

            CarregarHistorico();
        }

        private void btnPesquisarHistorico_Click(object sender, EventArgs e)
        {
            CarregarHistorico();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "Buscar pedido.." || textBox1.Text == "Buscar pedido.")
            {
                textBox1.Clear();
                textBox1.ForeColor = Color.FromArgb(111, 84, 75);
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox1.Text = "Buscar pedido..";
                textBox1.ForeColor = Color.FromArgb(191, 167, 157);
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CarregarHistorico();
                e.SuppressKeyPress = true;
            }
        }

        private void btnremover_Click(object sender, EventArgs e)
        {
            if (dgvPedidos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um pedido para voltar aos pedidos atuais.");
                return;
            }

            if (filtroAtual == "ClienteRecorrente")
            {
                MessageBox.Show("No filtro Cliente recorrente não é possível voltar um pedido específico. Use o filtro Todos e selecione o pedido desejado.");
                return;
            }

            DataGridViewRow rowSelecionada = dgvPedidos.SelectedRows[0];

            DataRowView linha = rowSelecionada.DataBoundItem as DataRowView;

            if (linha == null)
            {
                MessageBox.Show("Não foi possível obter os dados selecionados.");
                return;
            }

            string nomeCliente = linha["NomeCliente"].ToString();
            string telefoneCliente = linha["TelefoneCliente"].ToString();
            string dataHoraEntrega = linha["DataHoraEntrega"].ToString();

            DialogResult resposta = MessageBox.Show(
                "Deseja voltar este pedido para Pedidos Atuais?",
                "Confirmar retorno",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (resposta != DialogResult.Yes)
                return;

            VoltarPedidoParaPedidosAtuais(nomeCliente, telefoneCliente, dataHoraEntrega);
            CarregarHistorico();
        }

        private void VoltarPedidoParaPedidosAtuais(string nomeCliente, string telefoneCliente, string dataHoraEntrega)
        {
            conexao conexao = new conexao();

            using (MySqlConnection con = conexao.Conectar())
            {
                con.Open();

                MySqlTransaction transacao = con.BeginTransaction();

                try
                {
                    string novoStatus = CalcularStatusRetorno(dataHoraEntrega);

                    string sqlAtualizarPedido = @"
                        UPDATE pedidos
                        SET Status = @status
                        WHERE NomeCliente = @nome
                        AND TelefoneCliente = @telefone
                        AND DataHoraEntrega = @dataHora
                        AND Status = 'Concluído';
                    ";

                    int pedidosAtualizados = 0;

                    using (MySqlCommand cmdAtualizar = new MySqlCommand(sqlAtualizarPedido, con, transacao))
                    {
                        cmdAtualizar.Parameters.AddWithValue("@status", novoStatus);
                        cmdAtualizar.Parameters.AddWithValue("@nome", nomeCliente);
                        cmdAtualizar.Parameters.AddWithValue("@telefone", telefoneCliente);
                        cmdAtualizar.Parameters.AddWithValue("@dataHora", dataHoraEntrega);

                        pedidosAtualizados = cmdAtualizar.ExecuteNonQuery();
                    }

                    if (pedidosAtualizados == 0)
                    {
                        transacao.Rollback();

                        MessageBox.Show(
                            "Não foi encontrado um pedido concluído correspondente para voltar aos Pedidos Atuais.",
                            "Pedido não encontrado",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );

                        return;
                    }

                    string sqlRemoverHistorico = @"
                        DELETE FROM historico
                        WHERE NomeCliente = @nome
                        AND TelefoneCliente = @telefone
                        AND DataHoraEntrega = @dataHora;
                    ";

                    using (MySqlCommand cmdRemover = new MySqlCommand(sqlRemoverHistorico, con, transacao))
                    {
                        cmdRemover.Parameters.AddWithValue("@nome", nomeCliente);
                        cmdRemover.Parameters.AddWithValue("@telefone", telefoneCliente);
                        cmdRemover.Parameters.AddWithValue("@dataHora", dataHoraEntrega);
                        cmdRemover.ExecuteNonQuery();
                    }

                    transacao.Commit();

                    MessageBox.Show("Pedido voltou para Pedidos Atuais com sucesso.");
                }
                catch (Exception ex)
                {
                    transacao.Rollback();
                    MessageBox.Show("Erro ao voltar pedido para Pedidos Atuais: " + ex.Message);
                }
            }
        }

        private string CalcularStatusRetorno(string dataHoraEntrega)
        {
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

            if (DateTime.TryParseExact(
                    dataHoraEntrega,
                    formatosPermitidos,
                    new CultureInfo("pt-BR"),
                    DateTimeStyles.None,
                    out dataValidada))
            {
                if (dataValidada < DateTime.Now)
                    return "Atrasado";

                return "Agendado";
            }

            return "Agendado";
        }

        private void RemoverPedidoHistorico(string nomeCliente, string telefoneCliente, string dataHoraEntrega)
        {
            conexao conexao = new conexao();

            using (MySqlConnection con = conexao.Conectar())
            {
                try
                {
                    con.Open();

                    string sql = @"
                        DELETE FROM historico
                        WHERE NomeCliente = @nome
                        AND TelefoneCliente = @telefone
                        AND DataHoraEntrega = @dataHora;
                    ";

                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@nome", nomeCliente);
                        cmd.Parameters.AddWithValue("@telefone", telefoneCliente);
                        cmd.Parameters.AddWithValue("@dataHora", dataHoraEntrega);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Pedido removido do histórico com sucesso.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao remover pedido do histórico: " + ex.Message);
                }
            }
        }

        private void RemoverHistoricoCliente(string nomeCliente, string telefoneCliente)
        {
            conexao conexao = new conexao();

            using (MySqlConnection con = conexao.Conectar())
            {
                try
                {
                    con.Open();

                    string sql = @"
                        DELETE FROM historico
                        WHERE NomeCliente = @nome
                        AND TelefoneCliente = @telefone;
                    ";

                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@nome", nomeCliente);
                        cmd.Parameters.AddWithValue("@telefone", telefoneCliente);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Histórico do cliente removido com sucesso.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao remover histórico do cliente: " + ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ExportarRelatorio();
        }

        private void ExportarRelatorio()
        {
            if (dgvPedidos.Rows.Count == 0)
            {
                MessageBox.Show("Não há dados para exportar.");
                return;
            }

            SaveFileDialog salvar = new SaveFileDialog();
            salvar.Title = "Exportar relatório";
            salvar.Filter = "Excel CSV (*.csv)|*.csv|PDF (*.pdf)|*.pdf";
            salvar.FileName = "historico_pedidos";

            if (salvar.ShowDialog() != DialogResult.OK)
                return;

            string extensao = Path.GetExtension(salvar.FileName).ToLower();

            if (extensao == ".pdf")
            {
                ExportarPdf(salvar.FileName);
            }
            else
            {
                ExportarExcelCsv(salvar.FileName);
            }
        }

        private void ExportarExcelCsv(string caminho)
        {
            try
            {
                StringBuilder csv = new StringBuilder();

                csv.AppendLine("Cliente;Telefone;Pedido;Entrega;Valor");

                foreach (DataGridViewRow row in dgvPedidos.Rows)
                {
                    if (row.IsNewRow)
                        continue;

                    DataRowView linha = row.DataBoundItem as DataRowView;

                    string cliente = TratarCsv(row.Cells["colcliente"].Value);
                    string telefone = "";

                    if (linha != null && linha.Row.Table.Columns.Contains("TelefoneCliente"))
                        telefone = TratarCsv(linha["TelefoneCliente"]);

                    string pedido = TratarCsv(row.Cells["colpedido"].Value);
                    string entrega = TratarCsv(row.Cells["colentrega"].Value);
                    string valor = TratarCsv(row.Cells["colvalor"].Value);

                    csv.AppendLine($"{cliente};{telefone};{pedido};{entrega};{valor}");
                }

                File.WriteAllText(caminho, csv.ToString(), new UTF8Encoding(true));

                MessageBox.Show("Relatório exportado para Excel com sucesso.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao exportar para Excel: " + ex.Message);
            }
        }

        private string TratarCsv(object valor)
        {
            string texto = valor?.ToString() ?? "";
            texto = texto.Replace("\"", "\"\"");
            return "\"" + texto + "\"";
        }

        private void ExportarPdf(string caminho)
        {
            try
            {
                bool temMicrosoftPrintPdf = PrinterSettings.InstalledPrinters
                    .Cast<string>()
                    .Any(p => p.Equals("Microsoft Print to PDF", StringComparison.OrdinalIgnoreCase));

                if (!temMicrosoftPrintPdf)
                {
                    MessageBox.Show("A impressora 'Microsoft Print to PDF' não foi encontrada no Windows.");
                    return;
                }

                dadosParaPdf = dgvPedidos.DataSource as DataTable;

                if (dadosParaPdf == null || dadosParaPdf.Rows.Count == 0)
                {
                    MessageBox.Show("Não há dados para exportar em PDF.");
                    return;
                }

                linhaAtualPdf = 0;

                PrintDocument documento = new PrintDocument();
                documento.DocumentName = "Histórico de Pedidos";
                documento.DefaultPageSettings.Landscape = true;
                documento.PrinterSettings.PrinterName = "Microsoft Print to PDF";
                documento.PrinterSettings.PrintToFile = true;
                documento.PrinterSettings.PrintFileName = caminho;

                documento.PrintPage += Documento_PrintPage;
                documento.Print();

                MessageBox.Show("Relatório exportado para PDF com sucesso.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao exportar para PDF: " + ex.Message);
            }
        }

        private void Documento_PrintPage(object sender, PrintPageEventArgs e)
        {
            Font fonteTitulo = new Font("Segoe UI", 16, FontStyle.Bold);
            Font fonteCabecalho = new Font("Segoe UI", 9, FontStyle.Bold);
            Font fonteLinha = new Font("Segoe UI", 8, FontStyle.Regular);

            Brush pincel = Brushes.Black;
            Pen caneta = Pens.Black;

            float x = e.MarginBounds.Left;
            float y = e.MarginBounds.Top;

            e.Graphics.DrawString("Histórico de Pedidos", fonteTitulo, pincel, x, y);
            y += 35;

            e.Graphics.DrawString("Gerado em: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"), fonteLinha, pincel, x, y);
            y += 35;

            float larguraCliente = 150;
            float larguraTelefone = 110;
            float larguraPedido = 360;
            float larguraEntrega = 140;
            float larguraValor = 100;
            float alturaLinha = 45;

            e.Graphics.DrawRectangle(caneta, x, y, larguraCliente, alturaLinha);
            e.Graphics.DrawRectangle(caneta, x + larguraCliente, y, larguraTelefone, alturaLinha);
            e.Graphics.DrawRectangle(caneta, x + larguraCliente + larguraTelefone, y, larguraPedido, alturaLinha);
            e.Graphics.DrawRectangle(caneta, x + larguraCliente + larguraTelefone + larguraPedido, y, larguraEntrega, alturaLinha);
            e.Graphics.DrawRectangle(caneta, x + larguraCliente + larguraTelefone + larguraPedido + larguraEntrega, y, larguraValor, alturaLinha);

            e.Graphics.DrawString("Cliente", fonteCabecalho, pincel, x + 5, y + 12);
            e.Graphics.DrawString("Telefone", fonteCabecalho, pincel, x + larguraCliente + 5, y + 12);
            e.Graphics.DrawString("Pedido", fonteCabecalho, pincel, x + larguraCliente + larguraTelefone + 5, y + 12);
            e.Graphics.DrawString("Entrega", fonteCabecalho, pincel, x + larguraCliente + larguraTelefone + larguraPedido + 5, y + 12);
            e.Graphics.DrawString("Valor", fonteCabecalho, pincel, x + larguraCliente + larguraTelefone + larguraPedido + larguraEntrega + 5, y + 12);

            y += alturaLinha;

            while (linhaAtualPdf < dadosParaPdf.Rows.Count)
            {
                if (y + alturaLinha > e.MarginBounds.Bottom)
                {
                    e.HasMorePages = true;
                    return;
                }

                DataRow row = dadosParaPdf.Rows[linhaAtualPdf];

                string cliente = row["NomeCliente"].ToString();
                string telefone = row["TelefoneCliente"].ToString();
                string pedido = row["Produto"].ToString();
                string entrega = row["DataHoraEntrega"].ToString();
                string valor = Convert.ToDecimal(row["Valor"]).ToString("C2", new CultureInfo("pt-BR"));

                e.Graphics.DrawRectangle(caneta, x, y, larguraCliente, alturaLinha);
                e.Graphics.DrawRectangle(caneta, x + larguraCliente, y, larguraTelefone, alturaLinha);
                e.Graphics.DrawRectangle(caneta, x + larguraCliente + larguraTelefone, y, larguraPedido, alturaLinha);
                e.Graphics.DrawRectangle(caneta, x + larguraCliente + larguraTelefone + larguraPedido, y, larguraEntrega, alturaLinha);
                e.Graphics.DrawRectangle(caneta, x + larguraCliente + larguraTelefone + larguraPedido + larguraEntrega, y, larguraValor, alturaLinha);

                e.Graphics.DrawString(cliente, fonteLinha, pincel, new RectangleF(x + 5, y + 5, larguraCliente - 10, alturaLinha - 10));
                e.Graphics.DrawString(telefone, fonteLinha, pincel, new RectangleF(x + larguraCliente + 5, y + 5, larguraTelefone - 10, alturaLinha - 10));
                e.Graphics.DrawString(pedido, fonteLinha, pincel, new RectangleF(x + larguraCliente + larguraTelefone + 5, y + 5, larguraPedido - 10, alturaLinha - 10));
                e.Graphics.DrawString(entrega, fonteLinha, pincel, new RectangleF(x + larguraCliente + larguraTelefone + larguraPedido + 5, y + 5, larguraEntrega - 10, alturaLinha - 10));
                e.Graphics.DrawString(valor, fonteLinha, pincel, new RectangleF(x + larguraCliente + larguraTelefone + larguraPedido + larguraEntrega + 5, y + 5, larguraValor - 10, alturaLinha - 10));

                y += alturaLinha;
                linhaAtualPdf++;
            }

            e.HasMorePages = false;
            linhaAtualPdf = 0;
        }

        private void btnMenuPrincipal_Click(object sender, EventArgs e)
        {
            MenuPrincipal menu = new MenuPrincipal();
            menu.Show();
            this.Hide();
        }

        private void btnPedidosAtuais_Click(object sender, EventArgs e)
        {
            PedidosAtuais tela = new PedidosAtuais();
            tela.Show();
            this.Hide();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            TelaLogin login = new TelaLogin();
            login.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NovoAgendamento tela = new NovoAgendamento();
            tela.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CadastroProdutos tela = new CadastroProdutos();
            tela.Show();
            this.Hide();
        }

        private void dgvPedidos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cbFiltrosHistorico_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }
    }
}