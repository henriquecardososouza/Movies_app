using System.Net;
using Newtonsoft.Json;
using Movies_app.Resources.Movies.Objects;
using Movies_app.Resources.Movies;

namespace Movies_app.Forms
{
    public partial class FrmEnterKey : Form
    {
        private bool closeNormal = false;

        public FrmEnterKey()
        {
            InitializeComponent();
        }

        private void lblLink_MouseClick(object sender, MouseEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer", "https://www.themoviedb.org/settings/api");
        }

        private void btnVerificar_MouseClick(object sender, MouseEventArgs e)
        {
            using (WebClient web = new())
            {
                string url;

                try
                {
                    url = string.Format(Configuration.searchUrlBase, tbChave.Text, "Jack Reacher", Configuration.language, 1);
                    var json = web.DownloadString(url);

                    Search.Rootobject? s = JsonConvert.DeserializeObject<Search.Rootobject>(json) ?? throw new Exception();

                    MessageBox.Show("Chave verificada!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.closeNormal = true;

                    string? region;

                    try
                    {
                        region = File.ReadAllText("Resources/configuration.config");

                        if (string.IsNullOrEmpty(region))
                        {
                            throw new Exception();
                        }
                    }

                    catch
                    {
                        region = null;
                    }

                    if (!WriteConfigurationFile(tbChave.Text, region)) throw new Exception();

                    Close();
                }

                catch (Exception ex)
                {
                    if (ex is WebException we)
                    {
                        if (we.Status == WebExceptionStatus.ProtocolError)
                        {
                            MessageBox.Show("Chave Incorreta!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        else if (we.Status == WebExceptionStatus.NameResolutionFailure)
                        {
                            MessageBox.Show("Erro de Conexão!\nCertifique-se que você está\nconectado a internet", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    MessageBox.Show("Chave não Verificada!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool WriteConfigurationFile(string chave, string? region)
        {
            string text;

            if (region == null)
            {
                text = $"apikey:{chave}\nregion-film:BR\nregion-serie:BR";
            }

            else
            {
                string regionFilm = region.Split("\n")[1].Split(":")[1];
                string regionSerie = region.Split("\n")[2].Split(":")[1];

                text = $"apikey:{chave}\nregion-film:{regionFilm}\nregion-serie:{regionSerie}";
            }

            try
            {
                if (!Directory.Exists("Resources"))
                {
                    Directory.CreateDirectory("Resources");
                }

                File.WriteAllText("Resources/configuration.config", text);

                return true;
            }

            catch (Exception e)
            {
                return false;
            }
        }

        private void FrmEnterKey_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!this.closeNormal)
            {
                FrmMain.shouldClose = true;
            }
        }

        private void lblLink_MouseEnter(object sender, EventArgs e)
        {
            lblLink.ForeColor = Color.Blue;
            lblLink.Font = new Font(lblLink.Font, FontStyle.Underline);
        }

        private void lblLink_MouseLeave(object sender, EventArgs e)
        {
            lblLink.ForeColor = Color.White;
            lblLink.Font = new Font(lblLink.Font, FontStyle.Regular);
        }
    }
}
