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
    }
}
