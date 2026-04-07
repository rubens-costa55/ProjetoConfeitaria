namespace PrimeiraTela
{
    public partial class TelaLogin : Form
    {
        public TelaLogin()
        {
            InitializeComponent();
        }

        private void TelaLogin_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void lblSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEsqueciSenha_Click(object sender, EventArgs e)
        {
            FrmRedefinirSenha tela = new FrmRedefinirSenha();
            tela.Show();
        }

        private void btnAcessar_Click(object sender, EventArgs e)
        {
            MenuPrincipal tela = new MenuPrincipal();
            tela.Show();
        }

        private void txtcpf_TextChanged(object sender, EventArgs e)
        {
            

        }

        private void txtSenha_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void txtcpf_click(object sender, EventArgs e)
        {
            
        }
    }
}
