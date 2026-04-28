using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace PrimeiraTela
{
    public partial class CadastroProdutos : Form
    {
        private int idProdutoSelecionado = 0;

        public CadastroProdutos()
        {
            InitializeComponent();

            this.Load += CadastroProdutos_Load;
        }

        private void CadastroProdutos_Load(object sender, EventArgs e)
        {
            ConfigurarGrid();
            ConfigurarEventos();
            CarregarCategoriasCadastro();
            CarregarCategoriasFiltro();
            CarregarProdutos();
        }

        private void ConfigurarEventos()
        {
            button1.Click -= button1_Click;
            button1.Click += button1_Click;

            btnfiltrarprodutos.Click -= btnfiltrarprodutos_Click;
            btnfiltrarprodutos.Click += btnfiltrarprodutos_Click;

            btnremover.Click -= btnremover_Click;
            btnremover.Click += btnremover_Click;

            dgvprodutos.CellDoubleClick -= dgvprodutos_CellDoubleClick;
            dgvprodutos.CellDoubleClick += dgvprodutos_CellDoubleClick;

            txtpesquisarprod.KeyDown -= txtpesquisarprod_KeyDown;
            txtpesquisarprod.KeyDown += txtpesquisarprod_KeyDown;

            cbFiltroCategoria.SelectedIndexChanged -= cbFiltroCategoria_SelectedIndexChanged;
            cbFiltroCategoria.SelectedIndexChanged += cbFiltroCategoria_SelectedIndexChanged;
        }

        private void ConfigurarGrid()
        {
            dgvprodutos.AutoGenerateColumns = false;
            dgvprodutos.Columns.Clear();

            dgvprodutos.AllowUserToAddRows = false;
            dgvprodutos.AllowUserToDeleteRows = false;
            dgvprodutos.ReadOnly = true;
            dgvprodutos.MultiSelect = false;
            dgvprodutos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvprodutos.RowHeadersVisible = false;
            dgvprodutos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            DataGridViewTextBoxColumn colId = new DataGridViewTextBoxColumn();
            colId.Name = "id_produto";
            colId.HeaderText = "ID";
            colId.DataPropertyName = "id_produto";
            colId.Visible = false;
            dgvprodutos.Columns.Add(colId);

            DataGridViewTextBoxColumn colNome = new DataGridViewTextBoxColumn();
            colNome.Name = "NomeProduto";
            colNome.HeaderText = "Produto";
            colNome.DataPropertyName = "NomeProduto";
            colNome.FillWeight = 45;
            dgvprodutos.Columns.Add(colNome);

            DataGridViewTextBoxColumn colPreco = new DataGridViewTextBoxColumn();
            colPreco.Name = "PrecoProduto";
            colPreco.HeaderText = "Preço";
            colPreco.DataPropertyName = "PrecoProduto";
            colPreco.DefaultCellStyle.Format = "C2";
            colPreco.FillWeight = 20;
            dgvprodutos.Columns.Add(colPreco);

            DataGridViewTextBoxColumn colCategoria = new DataGridViewTextBoxColumn();
            colCategoria.Name = "nome_categoria";
            colCategoria.HeaderText = "Categoria";
            colCategoria.DataPropertyName = "nome_categoria";
            colCategoria.FillWeight = 35;
            dgvprodutos.Columns.Add(colCategoria);
        }

        private void CarregarCategoriasCadastro()
        {
            conexao conexao = new conexao();

            using (MySqlConnection con = conexao.Conectar())
            {
                try
                {
                    con.Open();

                    string sql = @"
                        SELECT id_categoria, nome_categoria
                        FROM categorias
                        ORDER BY nome_categoria;
                    ";

                    MySqlDataAdapter adapter = new MySqlDataAdapter(sql, con);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    cbCategoria.DataSource = dt;
                    cbCategoria.DisplayMember = "nome_categoria";
                    cbCategoria.ValueMember = "id_categoria";
                    cbCategoria.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar categorias: " + ex.Message);
                }
            }
        }

        private void CarregarCategoriasFiltro()
        {
            conexao conexao = new conexao();

            using (MySqlConnection con = conexao.Conectar())
            {
                try
                {
                    con.Open();

                    string sql = @"
                        SELECT id_categoria, nome_categoria
                        FROM categorias
                        ORDER BY nome_categoria;
                    ";

                    MySqlDataAdapter adapter = new MySqlDataAdapter(sql, con);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    DataRow linhaTodos = dt.NewRow();
                    linhaTodos["id_categoria"] = 0;
                    linhaTodos["nome_categoria"] = "Todas as categorias";
                    dt.Rows.InsertAt(linhaTodos, 0);

                    cbFiltroCategoria.DataSource = dt;
                    cbFiltroCategoria.DisplayMember = "nome_categoria";
                    cbFiltroCategoria.ValueMember = "id_categoria";
                    cbFiltroCategoria.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar filtro de categorias: " + ex.Message);
                }
            }
        }

        private void CarregarProdutos(string busca = "", int idCategoria = 0)
        {
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
                            c.nome_categoria
                        FROM produtos p
                        INNER JOIN categorias c ON p.id_categoria = c.id_categoria
                        WHERE 1 = 1
                    ";

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = con;

                    if (!string.IsNullOrWhiteSpace(busca))
                    {
                        sql += @"
                            AND (
                                p.NomeProduto LIKE @busca
                                OR c.nome_categoria LIKE @busca
                            )
                        ";

                        cmd.Parameters.AddWithValue("@busca", "%" + busca.Trim() + "%");
                    }

                    if (idCategoria > 0)
                    {
                        sql += " AND p.id_categoria = @id_categoria ";
                        cmd.Parameters.AddWithValue("@id_categoria", idCategoria);
                    }

                    sql += " ORDER BY p.NomeProduto; ";

                    cmd.CommandText = sql;

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvprodutos.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar produtos: " + ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SalvarProduto();
        }

        private void SalvarProduto()
        {
            string nomeProduto = textBox1.Text.Trim();
            string precoTexto = textBox2.Text.Trim();

            if (string.IsNullOrWhiteSpace(nomeProduto))
            {
                MessageBox.Show("Digite o nome do produto.");
                textBox1.Focus();
                return;
            }

            if (cbCategoria.SelectedValue == null || cbCategoria.SelectedIndex < 0)
            {
                MessageBox.Show("Selecione uma categoria.");
                cbCategoria.Focus();
                return;
            }

            decimal precoProduto;

            if (!decimal.TryParse(precoTexto, NumberStyles.Any, new CultureInfo("pt-BR"), out precoProduto))
            {
                MessageBox.Show("Digite um preço válido. Exemplo: 45,00");
                textBox2.Focus();
                return;
            }

            if (precoProduto <= 0)
            {
                MessageBox.Show("O preço precisa ser maior que zero.");
                textBox2.Focus();
                return;
            }

            int idCategoria = Convert.ToInt32(cbCategoria.SelectedValue);

            conexao conexao = new conexao();

            using (MySqlConnection con = conexao.Conectar())
            {
                try
                {
                    con.Open();

                    if (idProdutoSelecionado == 0)
                    {
                        string sqlInsert = @"
                            INSERT INTO produtos
                            (
                                NomeProduto,
                                PrecoProduto,
                                id_categoria
                            )
                            VALUES
                            (
                                @nome,
                                @preco,
                                @categoria
                            );
                        ";

                        using (MySqlCommand cmd = new MySqlCommand(sqlInsert, con))
                        {
                            cmd.Parameters.AddWithValue("@nome", nomeProduto);
                            cmd.Parameters.AddWithValue("@preco", precoProduto);
                            cmd.Parameters.AddWithValue("@categoria", idCategoria);
                            cmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Produto cadastrado com sucesso.");
                    }
                    else
                    {
                        string sqlUpdate = @"
                            UPDATE produtos
                            SET
                                NomeProduto = @nome,
                                PrecoProduto = @preco,
                                id_categoria = @categoria
                            WHERE id_produto = @id_produto;
                        ";

                        using (MySqlCommand cmd = new MySqlCommand(sqlUpdate, con))
                        {
                            cmd.Parameters.AddWithValue("@nome", nomeProduto);
                            cmd.Parameters.AddWithValue("@preco", precoProduto);
                            cmd.Parameters.AddWithValue("@categoria", idCategoria);
                            cmd.Parameters.AddWithValue("@id_produto", idProdutoSelecionado);
                            cmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Produto atualizado com sucesso.");
                    }

                    LimparCampos();
                    CarregarProdutos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao salvar produto: " + ex.Message);
                }
            }
        }

        private void btnfiltrarprodutos_Click(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        private void cbFiltroCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            AplicarFiltros();
        }

        private void txtpesquisarprod_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                AplicarFiltros();
                e.SuppressKeyPress = true;
            }
        }

        private void AplicarFiltros()
        {
            int idCategoria = 0;

            if (cbFiltroCategoria.SelectedValue != null)
            {
                int.TryParse(cbFiltroCategoria.SelectedValue.ToString(), out idCategoria);
            }

            string busca = txtpesquisarprod.Text.Trim();

            CarregarProdutos(busca, idCategoria);
        }

        private void dgvprodutos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvprodutos.Rows[e.RowIndex];

            idProdutoSelecionado = Convert.ToInt32(row.Cells["id_produto"].Value);
            textBox1.Text = row.Cells["NomeProduto"].Value.ToString();

            decimal preco = Convert.ToDecimal(row.Cells["PrecoProduto"].Value);
            textBox2.Text = preco.ToString("N2", new CultureInfo("pt-BR"));

            SelecionarCategoriaDoProduto(idProdutoSelecionado);

            button1.Text = "Atualizar Produto";
        }

        private void SelecionarCategoriaDoProduto(int idProduto)
        {
            conexao conexao = new conexao();

            using (MySqlConnection con = conexao.Conectar())
            {
                try
                {
                    con.Open();

                    string sql = @"
                        SELECT id_categoria
                        FROM produtos
                        WHERE id_produto = @id_produto;
                    ";

                    using (MySqlCommand cmd = new MySqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@id_produto", idProduto);

                        object resultado = cmd.ExecuteScalar();

                        if (resultado != null)
                        {
                            cbCategoria.SelectedValue = Convert.ToInt32(resultado);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao selecionar categoria do produto: " + ex.Message);
                }
            }
        }

        private void btnremover_Click(object sender, EventArgs e)
        {
            if (dgvprodutos.CurrentRow == null)
            {
                MessageBox.Show("Selecione um produto para remover.");
                return;
            }

            int idProduto = Convert.ToInt32(dgvprodutos.CurrentRow.Cells["id_produto"].Value);
            string nomeProduto = dgvprodutos.CurrentRow.Cells["NomeProduto"].Value.ToString();

            DialogResult resposta = MessageBox.Show(
                "Deseja remover o produto '" + nomeProduto + "'?",
                "Confirmar remoção",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (resposta != DialogResult.Yes)
                return;

            conexao conexao = new conexao();

            using (MySqlConnection con = conexao.Conectar())
            {
                try
                {
                    con.Open();

                    string verificarUso = @"
                        SELECT COUNT(*)
                        FROM itens_pedido
                        WHERE id_produto = @id_produto;
                    ";

                    using (MySqlCommand cmdVerificar = new MySqlCommand(verificarUso, con))
                    {
                        cmdVerificar.Parameters.AddWithValue("@id_produto", idProduto);

                        int totalUso = Convert.ToInt32(cmdVerificar.ExecuteScalar());

                        if (totalUso > 0)
                        {
                            MessageBox.Show("Este produto já está vinculado a pedidos e não pode ser removido.");
                            return;
                        }
                    }

                    string sqlDelete = "DELETE FROM produtos WHERE id_produto = @id_produto;";

                    using (MySqlCommand cmd = new MySqlCommand(sqlDelete, con))
                    {
                        cmd.Parameters.AddWithValue("@id_produto", idProduto);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Produto removido com sucesso.");

                    LimparCampos();
                    CarregarProdutos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao remover produto: " + ex.Message);
                }
            }
        }

        private void LimparCampos()
        {
            idProdutoSelecionado = 0;
            textBox1.Clear();
            textBox2.Text = "0,00";
            cbCategoria.SelectedIndex = -1;
            button1.Text = "Salvar Produto";
            textBox1.Focus();
        }

        private void btnMenuNA_Click(object sender, EventArgs e)
        {
            MenuPrincipal telamenu = new MenuPrincipal();
            telamenu.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NovoAgendamento telaagendamento = new NovoAgendamento();
            telaagendamento.Show();
            this.Hide();
        }

        private void btnPedidosAtuaisNA_Click(object sender, EventArgs e)
        {
            PedidosAtuais telapedidos = new PedidosAtuais();
            telapedidos.Show();
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
            Application.Exit();
        }
    }
}