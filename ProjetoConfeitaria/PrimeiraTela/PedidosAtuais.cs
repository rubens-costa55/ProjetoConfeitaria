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
        string conexao = "server=localhost;user=root;pwd =; database = sistemaconfeitaria";
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
                    MySqlConnection conf = new MySqlConnection(conexao);
                    try
                    {
                        conf.Open();
                        string sql = "SELECT NomeCliente,Produto,Quantidade,Valor,DataeHoraEntrega FROM agendamento;";
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
                    dgvPedidos.Columns["colstatus"].DataPropertyName = "Status";



                }

        private void btnMenuPrincipal_Click(object sender, EventArgs e)
        {
            new MenuPrincipal().Show();
        }

        private void btnNovoAgendamento_Click(object sender, EventArgs e)
        {
            new NovoAgendamento().Show();
        }

        private void btnHistorico_Click(object sender, EventArgs e)
        {
            new FrmHistoricoPedidos().Show();
        }

        private void btnsair_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
