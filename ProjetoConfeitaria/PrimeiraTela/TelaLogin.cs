using MySql.Data.MySqlClient;
using System.Drawing.Drawing2D;


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
            ArredondarBotao(btnEsqueciSenha);
            GraphicsPath path = new GraphicsPath();
            int radius = 20;

            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(btnAcessar.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(btnAcessar.Width - radius, btnAcessar.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, btnAcessar.Height - radius, radius, radius, 90, 90);
            path.CloseAllFigures();

            btnAcessar.Region = new Region(path);
        }





        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void lblSair_Click(object sender, EventArgs e)
        {

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

        private void btnAcessar_MouseEnter(object sender, EventArgs e)
        {
            btnAcessar.BackColor = Color.FromArgb(200, 120, 100);
        }

        private void btnAcessar_MouseLeave(object sender, EventArgs e)
        {
            btnAcessar.BackColor = Color.FromArgb(190, 140, 120);
        }

        private void btnAcessar_MouseDown(object sender, MouseEventArgs e)
        {
            btnAcessar.BackColor = Color.FromArgb(160, 100, 90);
        }

        private void btnAcessar_MouseUp(object sender, MouseEventArgs e)
        {
            btnAcessar.BackColor = Color.FromArgb(200, 120, 100);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ArredondarBotao_Load(object sender, EventArgs e)
        {


        }

        private void ArredondarBotao(Button botao)
        {
            GraphicsPath path = new GraphicsPath();
            int raio = 20;

            path.AddArc(0, 0, raio, raio, 180, 90);
            path.AddArc(botao.Width - raio, 0, raio, raio, 270, 90);
            path.AddArc(botao.Width - raio, botao.Height - raio, raio, raio, 0, 90);
            path.AddArc(0, botao.Height - raio, raio, raio, 90, 90);

            path.CloseAllFigures();
            botao.Region = new Region(path);
        }

        private void lblSair_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }



}
