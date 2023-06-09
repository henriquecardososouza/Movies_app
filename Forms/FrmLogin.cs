using Movies_app.Resources;

namespace Movies_app.Forms
{
    public partial class FrmLogin : Form
    {
        private Resources.Data_Source.Crud c = new();

        public FrmLogin()
        {
            InitializeComponent();
        }

        private void btnEntrar_MouseClick(object sender, MouseEventArgs e)
        {
            string usuario = tbUsuario.Text;
            string senha = tbSenha.Text;

            if (usuario == string.Empty)
            {
                MessageBox.Show("Preencha o Campo de Usuário!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (senha == string.Empty)
            {
                MessageBox.Show("Preencha o Campo de Senha!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                senha = Convert.ToBase64String(Criptography.Encripty(senha, Criptography.GetKey(), Criptography.GetIv()));
                var obj = c.Read(FrmMain.getConnection(), "usuario", "nome", usuario, "senha", senha) ?? throw new Exception("usuario incorreto");

                if (obj.Length > 1) throw new Exception();

                FrmMain.Userid = Convert.ToInt32(obj[0][0]);
                FrmMain.Username = Convert.ToString(obj[0][1]);

                this.Close();
            }

            catch (Exception ex)
            {
                if (ex.Message.Equals("usuario incorreto"))
                {
                    MessageBox.Show("Usuário ou senha incorretos!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("Erro de conexão!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblCadastrar_MouseClick(object sender, MouseEventArgs e)
        {
            this.Hide();
            tbUsuario.Text = string.Empty;
            tbSenha.Text = string.Empty;

            new FrmCadastrar().ShowDialog();

            if (FrmMain.Userid != -1)
            {
                this.Close();
            }

            else
            {
                this.Show();
            }
        }

        private void lblCadastrar_MouseEnter(object sender, EventArgs e)
        {
            lblCadastrar.Font = new Font(lblCadastrar.Font, FontStyle.Underline);
        }

        private void lblCadastrar_MouseLeave(object sender, EventArgs e)
        {
            lblCadastrar.Font = new Font(lblCadastrar.Font, FontStyle.Regular);
        }

        private void tbUsuario_Enter(object sender, EventArgs e)
        {
            pnlUserLine.BackColor = Color.White;
        }

        private void tbUsuario_Leave(object sender, EventArgs e)
        {
            pnlUserLine.BackColor = Color.FromArgb(50, 50, 50);
        }

        private void tbSenha_Enter(object sender, EventArgs e)
        {
            pnlSenhaLine.BackColor = Color.White;
        }

        private void tbSenha_Leave(object sender, EventArgs e)
        {
            pnlSenhaLine.BackColor = Color.FromArgb(50, 50, 50);
        }
    }
}
