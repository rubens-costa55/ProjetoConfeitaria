using MySql.Data.MySqlClient;

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
            if (txtcpf.Text.Trim() == "" || txtSenha.Text.Trim() == "")
            {
                MessageBox.Show("Preencha o CPF e a senha.");
                return;
            }

            conexao conexao = new conexao();
            MySqlConnection con = conexao.Conectar();
            
            try
            {
                con.Open();
                string sql = "SELECT * FROM login WHERE cpf = @cpf AND senha = @senha";
                MySqlCommand cmd = new MySqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@cpf", txtcpf.Text.Trim());
                cmd.Parameters.AddWithValue("@senha", txtSenha.Text.Trim());

                MySqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    MenuPrincipal tela = new MenuPrincipal();
                    tela.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("CPF ou senha inválidos.");
                    txtSenha.Clear();
                    txtcpf.Clear();
                    txtcpf.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao fazer login: " + ex.Message);
            }

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
