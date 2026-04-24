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
    public partial class CadastroProdutos : Form
    {
        public CadastroProdutos()
        {
            InitializeComponent();
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
            this.Close();
        }
    }
}
