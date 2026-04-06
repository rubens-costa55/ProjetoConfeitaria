using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
