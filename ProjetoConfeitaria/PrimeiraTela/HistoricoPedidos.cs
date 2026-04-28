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
    public partial class FrmHistoricoPedidos : Form
    {
        public FrmHistoricoPedidos()
        {
            InitializeComponent();
        }

        private void btnMenuPrincipal_Click(object sender, EventArgs e)
        {
            MenuPrincipal menu = new MenuPrincipal();
            menu.Show();
            this.Hide();
        }

        private void lblTagConcluido1_Click(object sender, EventArgs e)
        {

        }

        private void lblTagConcluido2_Click(object sender, EventArgs e)
        {

        }

        private void btnNovoAgendamento_Click(object sender, EventArgs e)
        {
            NovoAgendamento tela = new NovoAgendamento();
            tela.Show();
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

        private void btnClienteRecorrente_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void FrmHistoricoPedidos_Load(object sender, EventArgs e)
        {
            string conexao = "server=localhost;user=root;pwd =; database = sistemaconfeitaria";

            dgvPedidos.AutoGenerateColumns = false;
            MySqlConnection conf = new MySqlConnection(conexao);
            try
            {
                conf.Open();
                string sql = "SELECT NomeCliente, Produto, Quantidade, Valor, DataHoraEntrega FROM historico;";
                MySqlDataAdapter cmd = new MySqlDataAdapter(sql, conf);
                //datatable: tabela virtual
                DataTable dt = new DataTable();
                cmd.Fill(dt);

                dgvPedidos.DataSource = dt;
            }
            catch (Exception ex) { }



            dgvPedidos.Columns["colcliente"].DataPropertyName = "NomeCliente";
            dgvPedidos.Columns["colpedido"].DataPropertyName = "Produto";
            dgvPedidos.Columns["colvalor"].DataPropertyName = "Valor";
            dgvPedidos.Columns["colentrega"].DataPropertyName = "DataeHoraEntrega";
            
        }

        private void dgvPedidos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
