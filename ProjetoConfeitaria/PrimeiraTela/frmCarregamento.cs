using System;
using System.Windows.Forms;

namespace PrimeiraTela
{
    public partial class frmCarregamento : Form
    {
        private int progresso = 0;
        private int pontos = 0;

        public frmCarregamento()
        {
            InitializeComponent();
        }

        private void frmCarregamento_Load(object sender, EventArgs e)
        {
            progresso = 0;
            pontos = 0;

            panel2.Width = 0;
            lbcarregando.Text = "Carregando sistema";

            tCarregar.Tick -= tCarregar_Tick;
            tCarregar.Tick += tCarregar_Tick;

            tPontos.Tick -= tPontos_Tick;
            tPontos.Tick += tPontos_Tick;

            tCarregar.Start();
            tPontos.Start();
        }

        private void tCarregar_Tick(object sender, EventArgs e)
        {
            progresso += 2;

            int larguraMaxima = panelbarrafundo.Width;
            int novaLargura = (larguraMaxima * progresso) / 100;

            if (novaLargura <= larguraMaxima)
            {
                panel2.Width = novaLargura;
            }

            if (progresso >= 100)
            {
                tCarregar.Stop();
                tPontos.Stop();

                AbrirTelaLogin();
            }
        }

        private void tPontos_Tick(object sender, EventArgs e)
        {
            pontos++;

            if (pontos > 3)
                pontos = 0;

            lbcarregando.Text = "Carregando sistema" + new string('.', pontos);
        }

        private void AbrirTelaLogin()
        {
            TelaLogin login = new TelaLogin();

            login.Show();
            this.Hide();
        }
    }
}
