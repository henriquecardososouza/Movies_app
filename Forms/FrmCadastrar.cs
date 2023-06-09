using Movies_app.Resources;

namespace Movies_app.Forms
{
    public partial class FrmCadastrar : Form
    {
        private Resources.Data_Source.Crud c = new();

        public FrmCadastrar()
        {
            InitializeComponent();
        }

        private void btnCadastrar_MouseClick(object sender, MouseEventArgs e)
        {
            string user = tbUsuario.Text;
            string senha = tbSenha.Text;


            if (user == string.Empty)
            {
                MessageBox.Show("Preencha o Campo de Usuário!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if (senha == string.Empty)
            {
                MessageBox.Show("Preencha o Campo de Usuário!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var result = c.Read(FrmMain.getConnection(), "usuario", "nome", user);

                if (result == null)
                {
                    senha = Convert.ToBase64String(Criptography.Encripty(senha, Criptography.GetKey(), Criptography.GetIv()));

                    if (c.Create(FrmMain.getConnection(), "usuario", user, senha))
                    {
                        result = c.Read(FrmMain.getConnection(), "usuario", "nome", user) ?? throw new Exception();
                        FrmMain.Userid = Convert.ToInt32(result[0][0]);
                        FrmMain.Username = user;
                        this.Close();
                        return;
                    }

                    else
                    {
                        throw new Exception();
                    }
                }

                else
                {
                    throw new Exception("ja cadastrado");
                }
            }

            catch (Exception ex)
            {
                if (ex.Message.Equals("ja cadastrado"))
                {
                    MessageBox.Show("Usuário já cadastrado!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                else
                {
                    MessageBox.Show("Erro de conexão!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
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
