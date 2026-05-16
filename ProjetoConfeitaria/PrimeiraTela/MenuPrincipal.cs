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
    public partial class MenuPrincipal : Form
    {
        public MenuPrincipal()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void btnNovoAgendamento_Click(object sender, EventArgs e)
        {
            new NovoAgendamento().Show();
            this.Hide();

        }


        private void btnPedidosAtuais_Click(object sender, EventArgs e)
        {

            new PedidosAtuais().Show();
            this.Hide();
        }

        private void btnMenuPrincipal_Click(object sender, EventArgs e)
        {
            new MenuPrincipal().Show();
            this.Hide();
        }


        private void btnHistórico_Click(object sender, EventArgs e)
        {
            new FrmHistoricoPedidos().Show();
            this.Hide();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {

        }

        private void btnFormularioAgendamento_Click(object sender, EventArgs e)
        {
            new NovoAgendamento().Show();
            this.Hide();
        }

        private void btnConsultarHistorico_Click(object sender, EventArgs e)
        {
            new FrmHistoricoPedidos().Show();
            this.Hide();
        }

        private void btnListPedidosAtuais_Click(object sender, EventArgs e)
        {
            new PedidosAtuais().Show();
            this.Hide();
        }

        private void TituloPrincipal_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void btnMenuPrincipal1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            TelaLogin login = new TelaLogin();
            login.Show();
            this.Hide();

        }

        private void btnAbrirCadastroProdutos_Click(object sender, EventArgs e)
        {
            CadastroProdutos telaprodutos = new CadastroProdutos();
            telaprodutos.Show();
            this.Hide();
        }

        private void MenuPrincipal_Load(object sender, EventArgs e)
        {
            MoverJanela.Ativar(this);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CadastroProdutos telaprodutos = new CadastroProdutos();
            telaprodutos.Show();
            this.Hide();
        }
    }
}

