using Movies_app.Forms;
using Newtonsoft.Json;
using Microsoft.Data.Sqlite;
using System.Net;

namespace Movies_app
{
    public partial class FrmMain : Form
    {
        #region Atributos/Métodos gerais

        public static bool shouldClose = false;
        public static int Userid = -1;
        public static string Username = string.Empty;

        private string actualPage = "home";

        private FormWindowState? lastWindowState = null;

        private Resources.Movies.Storage filmesStorage = new();
        private Resources.Tv_Shows.Storage seriesStorage = new();

        private Resources.Movies.Genres.Rootobject filmesGenres = new();
        private Resources.Tv_Shows.Genres.Rootobject seriesGenres = new();

        private Resources.Data_Source.Crud c = new();

        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Shown(object sender, EventArgs e)
        {
            Hide();

            string text;

            try
            {
                text = File.ReadAllText("Resources/configuration.config").Split("\n")[0].Split(":")[1];
            }

            catch
            {
                text = string.Empty;
            }

            if (string.IsNullOrEmpty(text))
            {
                new FrmEnterKey().ShowDialog();
            }

            if (FrmMain.shouldClose)
            {
                Close();
                return;
            }

            Resources.Movies.Configuration.apiKey = File.ReadAllText("Resources/configuration.config").Split("\n")[0].Split(":")[1];
            Resources.Tv_Shows.Configuration.apiKey = File.ReadAllText("Resources/configuration.config").Split("\n")[0].Split(":")[1];
            
            loadConfig();

            try
            {
                string url = string.Format("https://api.themoviedb.org/3/genre/movie/list?api_key={0}&language={1}", Resources.Movies.Configuration.apiKey, Resources.Movies.Configuration.language);

                using (WebClient web = new())
                {
                    var json = web.DownloadString(url);
                    this.filmesGenres = JsonConvert.DeserializeObject<Resources.Movies.Genres.Rootobject>(json);

                    if (this.filmesGenres == null)
                    {
                        throw new Exception();
                    }
                }

                url = string.Format("https://api.themoviedb.org/3/genre/tv/list?api_key={0}&language={1}", Resources.Movies.Configuration.apiKey, Resources.Movies.Configuration.language);

                using (WebClient web = new())
                {
                    var json = web.DownloadString(url);
                    this.seriesGenres = JsonConvert.DeserializeObject<Resources.Tv_Shows.Genres.Rootobject>(json);

                    if (this.seriesGenres == null)
                    {
                        throw new Exception();
                    }
                }
            }

            catch
            {
                MessageBox.Show("Erro de conexão!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            new FrmLogin().ShowDialog();

            if (FrmMain.Userid == -1)
            {
                Close();
                return;
            }

            mudaTela(string.Empty, "home");
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void FrmMain_Resize(object sender, EventArgs e)
        {
            if (this.lastWindowState == null || this.lastWindowState == FormWindowState.Minimized || WindowState == FormWindowState.Minimized)
            {
                this.lastWindowState = WindowState;
                return;
            }

            if (WindowState != this.lastWindowState)
            {
                switch (this.actualPage)
                {
                    case "home":
                        mudaTela(this.actualPage, this.actualPage);
                        break;

                    case "filme":
                        mudaTela(this.actualPage, this.actualPage, (-1).ToString());
                        break;

                    case "filmes-buscar":
                        mudaTela(this.actualPage, this.actualPage, string.Empty, "resize");
                        break;

                    case "filme-popular":
                        mudaTela(this.actualPage, this.actualPage, "resize");
                        break;

                    case "filme-upcoming":
                        mudaTela(this.actualPage, this.actualPage, "resize");
                        break;

                    case "filme-nowplaying":
                        mudaTela(this.actualPage, this.actualPage, "resize");
                        break;

                    case "filme-genre":
                        mudaTela(this.actualPage, this.actualPage, this.filmesGenreId.ToString());
                        break;

                    case "filme-similar":
                        mudaTela(this.actualPage, this.actualPage, this.filmeSimilarId.ToString());
                        break;

                    case "serie":
                        mudaTela(this.actualPage, this.actualPage, (-1).ToString());
                        break;

                    case "serie-buscar":
                        mudaTela(this.actualPage, this.actualPage, string.Empty, "resize");
                        break;

                    case "serie-popular":
                        mudaTela(this.actualPage, this.actualPage, "resize");
                        break;

                    case "serie-genre":
                        mudaTela(this.actualPage, this.actualPage, this.serieGenreId.ToString());
                        break;

                    case "serie-similar":
                        mudaTela(this.actualPage, this.actualPage, this.serieSimilarId.ToString());
                        break;

                    case "fav-filme":
                        mudaTela(this.actualPage, this.actualPage);
                        break;

                    case "fav-serie":
                        mudaTela(this.actualPage, this.actualPage);
                        break;

                    case "config":
                        mudaTela(this.actualPage, this.actualPage);
                        break;
                }

                pnlSidebar.Height = pnlConteudo.Height;
                this.lastWindowState = WindowState;
            }
        }

        /// <summary>
        /// args[0] = Página atual, args[1] = Nova página, [...] Outros itens
        /// </summary>
        /// <param name="args"></param>
        private void mudaTela(params string[] args)
        {
            showLoading();

            switch (args[0])
            {
                case "home":
                    hideHome();
                    break;

                case "filme":
                    hideFilme();
                    break;

                case "filmes-buscar":
                    hideFilmesBuscar();
                    break;

                case "filme-popular":
                    hideFilmePopular();
                    break;

                case "filme-upcoming":
                    hideFilmeUpcoming();
                    break;

                case "filme-nowplaying":
                    hideFilmeNowPlaying();
                    break;

                case "filme-genre":
                    hideFilmeGenre();
                    break;

                case "filme-similar":
                    hideFilmeSimilar();
                    break;

                case "serie":
                    hideSerie();
                    break;

                case "serie-buscar":
                    hideSerieBuscar();
                    break;

                case "serie-popular":
                    hideSeriePopular();
                    break;

                case "serie-genre":
                    hideSerieGenre();
                    break;

                case "serie-similar":
                    hideSerieSimilar();
                    break;

                case "fav-filme":
                    hideFavoritoFilme();
                    break;

                case "fav-serie":
                    hideFavoritoSerie();
                    break;

                case "config":
                    hideConfig();
                    break;

                default:
                    break;
            }

            switch (args[1])
            {
                case "home":
                    setHome();
                    showHome();
                    resetPreviousCache();
                    break;

                case "filme":
                    setFilme(int.Parse(args[2]));
                    showFilme();
                    break;

                case "filmes-buscar":
                    if (args[3].Equals("newPage"))
                    {
                        resetFilmesBuscar();
                    }

                    else
                    {
                        resetFilmesBuscar(args);
                    }

                    setFilmesBuscar(args[2]);
                    showFilmesBuscar();
                    resetPreviousCache();
                    break;

                case "filme-popular":
                    if (args[2].Equals("newPage"))
                    {
                        resetFilmePopular();
                    }

                    setFilmePopular();
                    showFilmePopular();
                    resetPreviousCache();
                    break;

                case "filme-upcoming":
                    if (args[2].Equals("newPage"))
                    {
                        resetFilmeUpcoming();
                    }

                    setFilmeUpcoming();
                    showFilmeUpcoming();
                    resetPreviousCache();
                    break;

                case "filme-nowplaying":
                    if (args[2].Equals("newPage"))
                    {
                        resetFilmeNowPlaying();
                    }

                    setFilmeNowPlaying();
                    showFilmeNowPlaying();
                    resetPreviousCache();
                    break;

                case "filme-genre":
                    setFilmeGenre(int.Parse(args[2]));
                    showFilmeGenre();
                    break;

                case "filme-similar":
                    setFilmeSimilar(int.Parse(args[2]));
                    showFilmeSimilar();
                    break;

                case "serie":
                    setSerie(int.Parse(args[2]));
                    showSerie();
                    break;

                case "serie-buscar":
                    if (args[3].Equals("newPage"))
                    {
                        resetSerieBuscar();
                    }

                    else
                    {
                        resetSerieBuscar(args);
                    }

                    setSerieBuscar(args[2]);
                    showSerieBuscar();
                    resetPreviousCache();
                    break;

                case "serie-popular":
                    if (args[2].Equals("newPage"))
                    {
                        resetSeriePopular();
                    }

                    setSeriePopular();
                    showSeriePopular();
                    resetPreviousCache();
                    break;

                case "serie-genre":
                    setSerieGenre(int.Parse(args[2]));
                    showSerieGenre();
                    break;

                case "serie-similar":
                    setSerieSimilar(int.Parse(args[2]));
                    showSerieSimilar();
                    break;

                case "fav-filme":
                    setFavoritoFilme();
                    showFavoritoFilme();
                    break;

                case "fav-serie":
                    setFavoritoSerie();
                    showFavoritoSerie();
                    break;

                case "config":
                    setConfig();
                    showConfig();
                    break;
            }

            hideLoading();
        }

        private void resetPreviousCache()
        {
            this.filmePreviousPage.Clear();
            this.filmeGenrePreviousPage.Clear();
            this.filmeSimilarPreviousPage.Clear();
            this.seriePreviousPage.Clear();
            this.serieGenrePreviousPage.Clear();
            this.serieSimilarPreviousPage.Clear();
        }

        public static SqliteConnection getConnection()
        {
            var caminhoBanco = "Resources/Data Source/movies_app.db";
            return new SqliteConnection("Data Source=" + caminhoBanco);
        }

        #endregion

        #region Atributos/Métodos da Sidebar

        private bool isSidebarExpand = false;
        private bool isFilmesExpand = false;
        private bool isSeriesExpand = false;
        private bool isFavExpand = false;
        private readonly int sidebarSelectedColor = 45;

        private void btnMenu_MouseClick(object sender, MouseEventArgs e)
        {
            timerSidebar.Start();
        }

        private void btnHome_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.isSidebarExpand)
            {
                timerSidebar.Start();
            }

            if (this.isFilmesExpand)
            {
                timerSidebarFilmes.Start();
            }

            if (this.isSeriesExpand)
            {
                timerSidebarSeries.Start();
            }

            if (this.isFavExpand)
            {
                timerSidebarFavoritos.Start();
            }

            if (this.actualPage == "home")
            {
                return;
            }

            mudaTela(this.actualPage, "home");
        }

        private void btnFilmes_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.isSeriesExpand)
            {
                timerSidebarSeries.Start();
            }

            if (this.isFavExpand)
            {
                timerSidebarFavoritos.Start();
            }

            timerSidebarFilmes.Start();
        }

        private void btnFilmesBuscar_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.isSidebarExpand)
            {
                timerSidebar.Start();
            }

            if (this.isSeriesExpand)
            {
                timerSidebarSeries.Start();
            }

            if (this.isFavExpand)
            {
                timerSidebarFavoritos.Start();
            }

            if (this.actualPage == "filmes-buscar")
            {
                return;
            }

            mudaTela(this.actualPage, "filmes-buscar", string.Empty, "newPage");
        }

        private void btnFilmesPopular_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.isSidebarExpand)
            {
                timerSidebar.Start();
            }

            if (this.isSeriesExpand)
            {
                timerSidebarSeries.Start();
            }

            if (this.isFavExpand)
            {
                timerSidebarFavoritos.Start();
            }

            if (this.actualPage == "filme-popular")
            {
                return;
            }

            mudaTela(this.actualPage, "filme-popular", "newPage");
        }

        private void btnFilmesEmBreve_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.isSidebarExpand)
            {
                timerSidebar.Start();
            }

            if (this.isSeriesExpand)
            {
                timerSidebarSeries.Start();
            }

            if (this.isFavExpand)
            {
                timerSidebarFavoritos.Start();
            }

            if (this.actualPage == "filme-upcoming")
            {
                return;
            }

            mudaTela(this.actualPage, "filme-upcoming", "newPage");
        }

        private void btnFilmesEstreia_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.isSidebarExpand)
            {
                timerSidebar.Start();
            }

            if (this.isSeriesExpand)
            {
                timerSidebarSeries.Start();
            }

            if (this.isFavExpand)
            {
                timerSidebarFavoritos.Start();
            }

            if (this.actualPage == "filme-nowplaying")
            {
                return;
            }

            mudaTela(this.actualPage, "filme-nowplaying", "newPage");
        }

        private void btnSeries_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.isFilmesExpand)
            {
                timerSidebarFilmes.Start();
            }

            if (this.isFavExpand)
            {
                timerSidebarFavoritos.Start();
            }

            timerSidebarSeries.Start();
        }

        private void btnSeriesBuscar_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.isSidebarExpand)
            {
                timerSidebar.Start();
            }

            if (this.isFilmesExpand)
            {
                timerSidebarFilmes.Start();
            }

            if (this.isFavExpand)
            {
                timerSidebarFavoritos.Start();
            }

            if (this.actualPage == "serie-buscar")
            {
                return;
            }

            mudaTela(this.actualPage, "serie-buscar", string.Empty, "newPage");
        }

        private void btnSeriesPopular_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.isSidebarExpand)
            {
                timerSidebar.Start();
            }

            if (this.isFilmesExpand)
            {
                timerSidebarFilmes.Start();
            }

            if (this.isFavExpand)
            {
                timerSidebarFavoritos.Start();
            }

            if (this.actualPage == "serie-popular")
            {
                return;
            }

            mudaTela(this.actualPage, "serie-popular", "newPage");
        }

        private void btnFavoritos_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.isFilmesExpand)
            {
                timerSidebarFilmes.Start();
            }

            if (this.isSeriesExpand)
            {
                timerSidebarSeries.Start();
            }

            timerSidebarFavoritos.Start();
        }

        private void btnFavFilmes_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.isSidebarExpand)
            {
                timerSidebar.Start();
            }

            if (this.isFilmesExpand)
            {
                timerSidebarFilmes.Start();
            }

            if (this.isSeriesExpand)
            {
                timerSidebarSeries.Start();
            }

            if (this.actualPage == "fav-filme")
            {
                return;
            }

            this.favoritoFilmePage = 1;
            mudaTela(this.actualPage, "fav-filme");
        }

        private void btnFavSeries_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.isSidebarExpand)
            {
                timerSidebar.Start();
            }

            if (this.isFilmesExpand)
            {
                timerSidebarFilmes.Start();
            }

            if (this.isSeriesExpand)
            {
                timerSidebarSeries.Start();
            }

            if (this.actualPage == "fav-serie")
            {
                return;
            }

            this.favoritoSeriePage = 1;
            mudaTela(this.actualPage, "fav-serie");
        }

        private void btnConfig_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.isSidebarExpand)
            {
                timerSidebar.Start();
            }

            if (this.isFilmesExpand)
            {
                timerSidebarFilmes.Start();
            }

            if (this.isSeriesExpand)
            {
                timerSidebarSeries.Start();
            }

            if (this.isFavExpand)
            {
                timerSidebarSeries.Start();
            }

            if (this.actualPage == "config")
            {
                return;
            }

            mudaTela(this.actualPage, "config");
        }

        private void btnSair_MouseClick(object sender, MouseEventArgs e)
        {
            if (MessageBox.Show("Deseja realmente sair?", "Logout", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) != DialogResult.OK)
            {
                return;
            }

            Hide();

            FrmMain.Userid = -1;
            FrmMain.Username = string.Empty;

            new FrmLogin().ShowDialog();

            if (FrmMain.Userid == -1)
            {
                Close();
                return;
            }

            mudaTela(this.actualPage, "home");
            Show();
        }

        private void timerSidebar_Tick(object sender, EventArgs e)
        {
            if (this.isSidebarExpand)
            {
                pnlSidebar.Width -= 10;

                if (pnlSidebar.Width - 10 <= pnlSidebar.MinimumSize.Width)
                {
                    pnlSidebar.Width = pnlSidebar.MinimumSize.Width;
                    this.isSidebarExpand = false;
                    timerSidebar.Stop();
                }
            }

            else
            {
                pnlSidebar.Width += 10;

                if (pnlSidebar.Width + 10 >= pnlSidebar.MaximumSize.Width)
                {
                    pnlSidebar.Width = pnlSidebar.MaximumSize.Width;
                    this.isSidebarExpand = true;
                    timerSidebar.Stop();
                }
            }
        }

        private void timerSidebarFilmes_Tick(object sender, EventArgs e)
        {
            if (this.isFilmesExpand)
            {
                flpFilmes.Height -= 10;

                if (flpFilmes.Height - 10 <= flpFilmes.MinimumSize.Height)
                {
                    flpFilmes.Height = flpFilmes.MinimumSize.Height;
                    this.isFilmesExpand = false;
                    timerSidebarFilmes.Stop();
                }
            }

            else
            {
                flpFilmes.Height += 10;

                if (flpFilmes.Height + 10 >= flpFilmes.MaximumSize.Height)
                {
                    flpFilmes.Height = flpFilmes.MaximumSize.Height;
                    this.isFilmesExpand = true;
                    timerSidebarFilmes.Stop();
                }
            }
        }

        private void timerSidebarSeries_Tick(object sender, EventArgs e)
        {
            if (this.isSeriesExpand)
            {
                flpSeries.Height -= 10;

                if (flpSeries.Height - 10 <= flpSeries.MinimumSize.Height)
                {
                    flpSeries.Height = flpSeries.MinimumSize.Height;
                    this.isSeriesExpand = false;
                    timerSidebarSeries.Stop();
                }
            }

            else
            {
                flpSeries.Height += 10;

                if (flpSeries.Height + 10 >= flpSeries.MaximumSize.Height)
                {
                    flpSeries.Height = flpSeries.MaximumSize.Height;
                    this.isSeriesExpand = true;
                    timerSidebarSeries.Stop();
                }
            }
        }

        private void timerSidebarFavoritos_Tick(object sender, EventArgs e)
        {
            if (this.isFavExpand)
            {
                flpFavoritos.Height -= 10;

                if (flpFavoritos.Height - 10 <= flpFavoritos.MinimumSize.Height)
                {
                    flpFavoritos.Height = flpFavoritos.MinimumSize.Height;
                    this.isFavExpand = false;
                    timerSidebarFavoritos.Stop();
                }
            }

            else
            {
                flpFavoritos.Height += 10;

                if (flpFavoritos.Height + 10 >= flpFavoritos.MaximumSize.Height)
                {
                    flpFavoritos.Height = flpFavoritos.MaximumSize.Height;
                    this.isFavExpand = true;
                    timerSidebarFavoritos.Stop();
                }
            }
        }

        #endregion

        #region Atributos/Métodos tela Home

        private void showHome()
        {
            pnlHome.Dock = DockStyle.Fill;
            pnlHome.Visible = true;
            pnlHome.Enabled = true;
            btnHome.BackColor = Color.FromArgb(sidebarSelectedColor, sidebarSelectedColor, sidebarSelectedColor);
            this.actualPage = "home";
        }

        private void hideHome()
        {
            pnlHome.Dock = DockStyle.None;
            pnlHome.Visible = false;
            pnlHome.Enabled = false;
            btnHome.BackColor = Color.Transparent;
        }

        private void setHome()
        {
            #region Obtendo os filmes populares

            Resources.Movies.Objects.Popular.Rootobject? movies = null;

            if (!this.filmesStorage.Popular.ContainsKey(string.Format("{0}:{1}", 1, Resources.Movies.Configuration.region)))
            {
                movies = Resources.Movies.Recover.getPopular(Resources.Movies.Configuration.region, 1);

                if (movies != null)
                {
                    this.filmesStorage.Popular.Add(string.Format("{0}:{1}", 1, Resources.Movies.Configuration.region), movies);
                }
            }

            else
            {
                foreach (var item in this.filmesStorage.Popular)
                {
                    if (item.Key == string.Format("{0}:{1}", 1, Resources.Movies.Configuration.region))
                    {
                        movies = item.Value;
                        break;
                    }
                }
            }

            #endregion

            #region Obtendo as séries populares

            Resources.Tv_Shows.Objects.Popular.Rootobject? series = null;

            if (!this.seriesStorage.Popular.ContainsKey(1))
            {
                series = Resources.Tv_Shows.Recover.getPopular(1);

                if (series != null)
                {
                    this.seriesStorage.Popular.Add(1, series);
                }
            }

            else
            {
                foreach (KeyValuePair<int, Resources.Tv_Shows.Objects.Popular.Rootobject> p in this.seriesStorage.Popular)
                {
                    if (p.Key == 1)
                    {
                        series = p.Value;
                        break;
                    }
                }
            }

            #endregion

            #region Verificando os resultados obtidos

            if (movies == null || series == null || movies.total_results <= 0 || series.total_results <= 0)
            {
                MessageBox.Show("Erro de conexão", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            #endregion

            #region Guardando os ids obtidos

            int[] movieIds = new int[movies.results.Length];

            for (int i = 0; i < movieIds.Length; i++)
            {
                movieIds[i] = movies.results[i].id;
            }

            if (!this.filmesStorage.movieIds.Contains(new KeyValuePair<string, int[]>("popular", movieIds)))
            {
                if (this.filmesStorage.movieIds.ContainsKey("popular"))
                {
                    int[] aux = Array.Empty<int>(), ids = Array.Empty<int>(), mergedItens = Array.Empty<int>();

                    foreach (KeyValuePair<string, int[]> item in this.filmesStorage.movieIds)
                    {
                        if (item.Key.Equals("popular"))
                        {
                            aux = item.Value;
                            break;
                        }
                    }

                    for (int i = 0; i < aux.Length; i++)
                    {
                        for (int j = 0; j < movieIds.Length; j++)
                        {
                            if (aux[i] == movieIds[j])
                            {
                                mergedItens = mergedItens.Append(aux[i]).ToArray();
                                break;
                            }
                        }
                    }

                    bool canAdd;

                    foreach (int item in movieIds)
                    {
                        canAdd = true;

                        foreach (int key in mergedItens)
                        {
                            if (item == key)
                            {
                                canAdd = false;
                                break;
                            }
                        }

                        if (canAdd)
                        {
                            ids = ids.Append(item).ToArray();
                        }
                    }

                    foreach (int item in aux)
                    {
                        canAdd = true;

                        foreach (int key in mergedItens)
                        {
                            if (item == key)
                            {
                                canAdd = false;
                                break;
                            }
                        }

                        if (canAdd)
                        {
                            ids = ids.Append(item).ToArray();
                        }
                    }

                    ids = ids.Concat(mergedItens).ToArray();

                    this.filmesStorage.movieIds.Remove("popular");
                    this.filmesStorage.movieIds.Add("popular", ids);
                }

                else
                {
                    this.filmesStorage.movieIds.Add("popular", movieIds);
                }
            }

            #endregion

            #region Preenchendo o container de filmes

            flpHomeFilmesContainer.Controls.Clear();

            int size = (WindowState == FormWindowState.Minimized) ? 1124 : this.Size.Width;
            int maxWidth = size - pnlSidebar.MinimumSize.Width;
            int containerMargin = flpHomeFilmesContainer.Location.X - pnlSidebar.MinimumSize.Width;
            int containerWidth = maxWidth - containerMargin * 2;
            int larg = 105;
            int quant = (containerWidth - (containerWidth % larg)) / larg - 1;

            flpHomeFilmesContainer.Width = containerWidth;

            if (quant > movies.results.Length) quant = movies.results.Length;

            Button[] btnFilme = new Button[quant];

            for (int i = 0; i < quant; i++)
            {
                btnFilme[i] = new()
                {
                    BackgroundImageLayout = ImageLayout.Stretch,
                    FlatStyle = FlatStyle.Flat,
                    ForeColor = Color.White,
                    Margin = new Padding(0, 0, 10, 0),
                    Name = "btnHomeFilme_" + movies.results[i].id,
                    Size = new Size(102, 146),
                    TabIndex = 1,
                    TextAlign = ContentAlignment.MiddleCenter,
                    UseVisualStyleBackColor = true,
                    Location = new Point(i * larg, 0)
                };

                btnFilme[i].MouseClick += btnHomeFilme_MouseClick;
                btnFilme[i].FlatAppearance.BorderColor = Color.FromArgb(40, 40, 40);
                btnFilme[i].Cursor = Cursors.Hand;

                if (this.filmesStorage.PosterImage.ContainsKey(movies.results[i].id))
                {
                    foreach (KeyValuePair<int, Image> item in this.filmesStorage.PosterImage)
                    {
                        if (item.Key == movies.results[i].id)
                        {
                            btnFilme[i].BackgroundImage = item.Value;
                            break;
                        }
                    }
                }

                else
                {
                    var url = string.Format(Resources.Movies.Configuration.imageUrlBase, Resources.Movies.Configuration.imageUrlSize, movies.results[i].poster_path);

                    try
                    {
                        var request = WebRequest.Create(url);

                        using (var response = request.GetResponse())
                        using (var stream = response.GetResponseStream())
                        {
                            Image image = Bitmap.FromStream(stream);
                            btnFilme[i].BackgroundImage = image;

                            if (stream != null)
                            {
                                this.filmesStorage.PosterImage.Add(movies.results[i].id, image);
                            }
                        }
                    }

                    catch
                    {
                        btnFilme[i].Text = movies.results[i].title;
                    }
                }

                flpHomeFilmesContainer.Controls.Add(btnFilme[i]);
            }

            Button btnMoreFilmes = new()
            {
                BackColor = Color.Transparent,
                BackgroundImage = Properties.Resources.arrow,
                BackgroundImageLayout = ImageLayout.Stretch,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.Transparent,
                Location = new Point(btnFilme[quant - 1].Location.X + larg, 49),
                Margin = new Padding(20, 49, 0, 0),
                Name = "btnHomeFilmesMore",
                Padding = new Padding(30),
                Size = new Size(50, 50),
                TabIndex = 7,
                UseVisualStyleBackColor = false
            };

            btnMoreFilmes.FlatAppearance.MouseDownBackColor = Color.FromArgb(20, 20, 20);
            btnMoreFilmes.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 30, 30);
            btnMoreFilmes.FlatAppearance.BorderColor = Color.FromArgb(20, 20, 20);
            btnMoreFilmes.MouseClick += btnHomeFilmesMore_MouseClick;

            flpHomeFilmesContainer.Controls.Add(btnMoreFilmes);

            #endregion

            #region Preenchendo o banner/label(s) para filmes

            pbHomeBanner.Height = (int)((((WindowState == FormWindowState.Minimized) ? 1124 : this.Width) - pnlSidebar.MinimumSize.Width) / 1.777777776);

            var isMovie = Convert.ToBoolean(new Random().Next(0, 2));

            if (isMovie)
            {
                var whatMovie = new Random().Next(0, quant);

                if (this.filmesStorage.BannerImage.ContainsKey(movies.results[whatMovie].id))
                {
                    foreach (KeyValuePair<int, Image> item in this.filmesStorage.BannerImage)
                    {
                        if (item.Key == movies.results[whatMovie].id)
                        {
                            pbHomeBanner.BackgroundImage = item.Value;
                            break;
                        }
                    }
                }

                else
                {
                    var request = WebRequest.Create(string.Format(Resources.Movies.Configuration.imageUrlBase, Resources.Movies.Configuration.imageUrlSize, movies.results[whatMovie].backdrop_path));

                    try
                    {
                        using (var response = request.GetResponse())
                        using (var stream = response.GetResponseStream())
                        {
                            Image image = Bitmap.FromStream(stream);
                            pbHomeBanner.BackgroundImage = image;

                            if (stream != null)
                            {
                                this.filmesStorage.BannerImage.Add(movies.results[whatMovie].id, image);
                            }
                        }
                    }

                    catch
                    {
                        pbHomeBanner.BackgroundImage = null;
                    }
                }

                lblHomeTitle.Text = movies.results[whatMovie].title;
                lblHomeDescription.Text = movies.results[whatMovie].overview;
            }

            #endregion

            #region Preenchendo o container de séries

            flpHomeSeriesContainer.Controls.Clear();
            flpHomeSeriesContainer.Width = containerWidth;

            quant = (containerWidth - (containerWidth % larg)) / larg - 1; quant = (containerWidth - (containerWidth % larg)) / larg - 1;

            if (quant > series.results.Length)
            {
                quant = series.results.Length;
            }

            Button[] btnSerie = new Button[quant];

            for (int i = 0; i < quant; i++)
            {
                btnSerie[i] = new()
                {
                    BackgroundImageLayout = ImageLayout.Stretch,
                    FlatStyle = FlatStyle.Flat,
                    ForeColor = Color.White,
                    Margin = new Padding(0, 0, 10, 0),
                    Name = "btnHomeSerie_" + series.results[i].id,
                    Size = new Size(102, 146),
                    TabIndex = 1,
                    TextAlign = ContentAlignment.MiddleCenter,
                    UseVisualStyleBackColor = true,
                    Location = new Point(i * larg, 0)
                };

                btnSerie[i].Cursor = Cursors.Hand;
                btnSerie[i].FlatAppearance.BorderColor = Color.FromArgb(40, 40, 40);
                btnSerie[i].FlatAppearance.MouseDownBackColor = Color.FromArgb(100, 10, 10, 10);
                btnSerie[i].FlatAppearance.MouseOverBackColor = Color.FromArgb(100, 20, 20, 20);
                btnSerie[i].MouseClick += btnHomeSerie_MouseClick;

                if (this.seriesStorage.PosterImage.ContainsKey(series.results[i].id))
                {
                    foreach (KeyValuePair<int, Image> item in this.seriesStorage.PosterImage)
                    {
                        if (item.Key == series.results[i].id)
                        {
                            btnSerie[i].BackgroundImage = item.Value;
                            break;
                        }
                    }
                }

                else
                {
                    var url = string.Format(Resources.Tv_Shows.Configuration.imageUrlBase, Resources.Tv_Shows.Configuration.imageUrlSize, series.results[i].poster_path);

                    try
                    {
                        var request = WebRequest.Create(url);

                        using (var response = request.GetResponse())
                        using (var stream = response.GetResponseStream())
                        {
                            Image image = Bitmap.FromStream(stream);
                            btnSerie[i].BackgroundImage = image;

                            if (stream != null)
                            {
                                this.seriesStorage.PosterImage.Add(series.results[i].id, image);
                            }
                        }
                    }

                    catch
                    {
                        btnSerie[i].Text = series.results[i].name;
                    }
                }

                flpHomeSeriesContainer.Controls.Add(btnSerie[i]);
            }

            Button btnMoreSeries = new()
            {
                BackColor = Color.Transparent,
                BackgroundImage = Properties.Resources.arrow,
                BackgroundImageLayout = ImageLayout.Stretch,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.Transparent,
                Location = new Point(btnSerie[quant - 1].Location.X + larg, 49),
                Margin = new Padding(20, 49, 0, 0),
                Name = "btnHomeSerieMore",
                Padding = new Padding(30),
                Size = new Size(50, 50),
                TabIndex = 7,
                UseVisualStyleBackColor = false
            };

            btnMoreSeries.FlatAppearance.MouseDownBackColor = Color.FromArgb(20, 20, 20);
            btnMoreSeries.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 30, 30);
            btnMoreSeries.FlatAppearance.BorderColor = Color.FromArgb(20, 20, 20);
            btnMoreSeries.MouseClick += btnHomeSerieMore_MouseClick;

            flpHomeSeriesContainer.Controls.Add(btnMoreSeries);

            #endregion

            #region Preenchendo o banner/label(s) para séries

            if (!isMovie)
            {
                var whatSerie = new Random().Next(0, quant);

                if (this.seriesStorage.BannerImage.ContainsKey(series.results[whatSerie].id))
                {
                    foreach (KeyValuePair<int, Image> item in this.seriesStorage.BannerImage)
                    {
                        if (item.Key == series.results[whatSerie].id)
                        {
                            pbHomeBanner.BackgroundImage = item.Value;
                            break;
                        }
                    }
                }

                else
                {
                    var request = WebRequest.Create(string.Format(Resources.Tv_Shows.Configuration.imageUrlBase, Resources.Tv_Shows.Configuration.imageUrlSize, series.results[whatSerie].backdrop_path));

                    try
                    {
                        using (var response = request.GetResponse())
                        using (var stream = response.GetResponseStream())
                        {
                            Image image = Bitmap.FromStream(stream);
                            pbHomeBanner.BackgroundImage = image;

                            if (stream != null)
                            {
                                this.seriesStorage.BannerImage.Add(series.results[whatSerie].id, image);
                            }
                        }
                    }

                    catch
                    {
                        pbHomeBanner.BackgroundImage = null;
                    }
                }

                lblHomeTitle.Text = series.results[whatSerie].name;
                lblHomeDescription.Text = series.results[whatSerie].overview;
            }

            #endregion

            #region Posicionando os elementos corretamente

            lblHomeFilmePop.Location = new Point(lblHomeFilmePop.Location.X, pbHomeBanner.Height - 176);
            flpHomeFilmesContainer.Location = new Point(flpHomeFilmesContainer.Location.X, pbHomeBanner.Height - 148);

            lblHomeSeriePop.Location = new Point(lblHomeSeriePop.Location.X, pbHomeBanner.Location.Y + pbHomeBanner.Height + 40);
            flpHomeSeriesContainer.Location = new Point(flpHomeSeriesContainer.Location.X, pbHomeBanner.Location.Y + pbHomeBanner.Height + 70);
            pnlHomeSpacing.Location = new Point(pnlHomeSpacing.Location.X, pbHomeBanner.Location.Y + pbHomeBanner.Height + 215);

            #endregion

            pnlHome.VerticalScroll.Value = 0;
        }

        private void btnHomeFilme_MouseClick(object? sender, MouseEventArgs e)
        {
            int i = 0;

            while (true)
            {
                if (!this.filmePreviousPage.ContainsKey(this.actualPage + "_" + i))
                {
                    this.filmePreviousPage.Add(this.actualPage + "_" + i, Array.Empty<string>());
                    break;
                }

                i++;
            }

            mudaTela(this.actualPage, "filme", (sender as Button).Name.Split("_")[1]);
        }

        private void btnHomeSerie_MouseClick(object? sender, MouseEventArgs e)
        {
            int i = 0;

            while (true)
            {
                if (!this.seriePreviousPage.ContainsKey(this.actualPage + "_" + i))
                {
                    this.seriePreviousPage.Add(this.actualPage + "_" + i, Array.Empty<string>());
                    break;
                }

                i++;
            }

            mudaTela(this.actualPage, "serie", (sender as Button).Name.Split("_")[1]);
        }

        private void btnHomeFilmesMore_MouseClick(object? sender, MouseEventArgs e)
        {
            mudaTela(this.actualPage, "filme-popular", "newPage");
        }

        private void btnHomeSerieMore_MouseClick(object? sender, MouseEventArgs e)
        {
            mudaTela(this.actualPage, "serie-popular", "newPage");
        }

        #endregion

        #region Atributos/Métodos tela Filme

        /// <summary>
        /// string page, string[] args
        /// </summary>
        private Dictionary<string, string[]> filmePreviousPage = new();
        private int filmeId;

        private void showFilme()
        {
            pnlFilme.Dock = DockStyle.Fill;
            pnlFilme.Visible = true;
            pnlFilme.Enabled = true;
            this.actualPage = "filme";
        }

        private void hideFilme()
        {
            pnlFilme.Dock = DockStyle.None;
            pnlFilme.Visible = false;
            pnlFilme.Enabled = false;
        }

        private void setFilme(int id)
        {
            Resources.Movies.Objects.Movie.Rootobject? filme = null;

            lblFilmeTitle.Width = this.Width - lblFilmeTitle.Location.X - 48;
            lblFilmeDescricao.Width = lblFilmeTitle.Width;

            if (id == -1)
            {
                pbFilmeBanner.Height = (int)((this.Width - pnlSidebar.MinimumSize.Width) / 1.777777776);
                pnlFilme.VerticalScroll.Value = 0;
                return;
            }

            #region Obtendo os dados do filme

            if (this.filmesStorage.Movie.ContainsKey(id))
            {
                foreach (var item in this.filmesStorage.Movie)
                {
                    if (item.Key == id)
                    {
                        filme = item.Value;
                        break;
                    }
                }
            }

            else
            {
                filme = Resources.Movies.Recover.getMovie(id);

                if (filme != null)
                {
                    this.filmesStorage.Movie.Add(id, filme);
                }
            }

            #endregion

            #region Verificando os dados recebidos

            if (filme == null)
            {
                MessageBox.Show("Erro de conexão!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                hideFilme();
                setHome();
                showHome();
                return;
            }

            #endregion

            #region Preenchendo os campos de imagem

            pbFilmeBanner.Height = (int)((this.Width - pnlSidebar.MinimumSize.Width) / 1.777777776);
            pbFilmePoster.Controls.Clear();
            pbFilmeBanner.Controls.RemoveByKey("lblFilmeBanner");

            if (this.filmesStorage.PosterImage.ContainsKey(id))
            {
                foreach (var item in this.filmesStorage.PosterImage)
                {
                    if (item.Key == id)
                    {
                        pbFilmePoster.BackgroundImage = item.Value;
                        break;
                    }
                }
            }

            else
            {
                string url = string.Format(Resources.Movies.Configuration.imageUrlBase, Resources.Movies.Configuration.imageUrlSize, filme.poster_path);

                try
                {
                    var request = WebRequest.Create(url);

                    using (var response = request.GetResponse())
                    using (var stream = response.GetResponseStream())
                    {
                        Image img = Image.FromStream(stream);

                        if (img != null)
                        {
                            pbFilmePoster.BackgroundImage = img;
                            this.filmesStorage.PosterImage.Add(id, img);
                        }
                    }
                }

                catch
                {

                    pbFilmePoster.BackgroundImage = null;

                    pbFilmePoster.Controls.Add(new Label()
                    {
                        AutoSize = false,
                        Font = lblFilmeDescricao.Font,
                        ForeColor = Color.White,
                        Location = new Point(0, 0),
                        Name = "lblFilmePoster",
                        Size = pbFilmePoster.Size,
                        Text = filme.title,
                        TextAlign = ContentAlignment.MiddleCenter
                    });
                }
            }

            if (this.filmesStorage.BannerImage.ContainsKey(id))
            {
                foreach (var item in this.filmesStorage.BannerImage)
                {
                    if (item.Key == id)
                    {
                        pbFilmeBanner.BackgroundImage = item.Value;
                        break;
                    }
                }
            }

            else
            {
                string url = string.Format(Resources.Movies.Configuration.imageUrlBase, Resources.Movies.Configuration.imageUrlSize, filme.backdrop_path);

                try
                {
                    var request = WebRequest.Create(url);

                    using (var response = request.GetResponse())
                    using (var stream = response.GetResponseStream())
                    {
                        Image img = Image.FromStream(stream);

                        if (img != null)
                        {
                            pbFilmeBanner.BackgroundImage = img;
                            this.filmesStorage.BannerImage.Add(id, img);
                        }

                        else
                        {
                            throw new Exception();
                        }
                    }
                }

                catch
                {

                    pbFilmeBanner.BackgroundImage = null;

                    pbFilmeBanner.Controls.Add(new Label()
                    {
                        AutoSize = false,
                        Font = lblFilmeTitle.Font,
                        ForeColor = Color.White,
                        Location = new Point(pnlSidebar.MinimumSize.Width, 0),
                        Name = "lblFilmeBanner",
                        Size = new Size(this.Width - pnlSidebar.MinimumSize.Width, pbFilmeBanner.Height),
                        Text = filme.title,
                        TextAlign = ContentAlignment.MiddleCenter
                    });
                }
            }

            #endregion

            #region Preenchendo os campos de texto

            lblFilmeTitle.Text = filme.title;
            lblFilmeDescricao.Text = filme.overview;
            lblFilmeContentDataLancValue.Text = filme.release_date.Replace("-", "/");
            lblFilmeContentDuracaoValue.Text = (((filme.runtime - filme.runtime % 60) / 60 < 10) ? "0" + ((filme.runtime - filme.runtime % 60)) / 60 : (filme.runtime - filme.runtime % 60) / 60) + ":" + ((filme.runtime % 60 < 10) ? "0" + filme.runtime % 60 : filme.runtime % 60) + " horas";

            #endregion

            #region Preenchendo o grid de gêneros

            flpFilmeContentGenreGrid.Controls.Clear();

            Button[] card = new Button[filme.genres.Length];

            for (int i = 0; i < card.Length; i++)
            {
                card[i] = new()
                {
                    BackColor = Color.FromArgb(10, 10, 10),
                    FlatStyle = FlatStyle.Flat,
                    ForeColor = Color.White,
                    Location = new Point(130 * i, 0),
                    Margin = new Padding(0, 0, 10, 0),
                    Name = "btnFilmeContentGenreItem" + i,
                    Size = new Size(120, 24),
                    TabIndex = 0,
                    Text = filme.genres[i].name,
                    TextAlign = ContentAlignment.MiddleCenter
                };

                card[i].FlatAppearance.BorderSize = 1;
                card[i].FlatAppearance.BorderColor = Color.FromArgb(60, 60, 60);
                card[i].FlatAppearance.MouseDownBackColor = Color.FromArgb(30, 30, 30);
                card[i].FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 60, 60);
                card[i].MouseClick += btnFilmeContentGenreGrid_MouseClick;

                flpFilmeContentGenreGrid.Controls.Add(card[i]);
            }

            #endregion

            #region Ajustando o botão de favorito

            var result = c.Read(FrmMain.getConnection(), "filme", "filme_id", filme.id) ?? Array.Empty<object[]>();
            btnFilmeContentFav.Text = (result.Length > 0) ? "      Remover dos Favoritos" : "      Adicionar aos Favoritos";

            #endregion

            this.filmeId = id;
            pnlFilme.VerticalScroll.Value = 0;
        }

        private void btnFilmeContentGenreGrid_MouseClick(object? sender, MouseEventArgs e)
        {
            var b = sender as Button;

            foreach (var item in this.filmesGenres.genres)
            {
                if (item.name == b.Text)
                {
                    this.filmeGenrePage = 1;
                    mudaTela(this.actualPage, "filme-genre", item.id.ToString());

                    int i = 0;

                    while (true)
                    {
                        if (!this.filmeGenrePreviousPage.ContainsKey("filme_" + i))
                        {
                            this.filmeGenrePreviousPage.Add("filme_" + i, this.filmeId);
                            break;
                        }

                        i++;
                    }

                    break;
                }
            }
        }

        private void btnFilmePrevious_MouseClick(object sender, MouseEventArgs e)
        {
            string arg = string.Empty, arg1 = string.Empty;

            switch (this.filmePreviousPage.Last().Key.Split("_")[0])
            {
                case "home":
                    //Home don't need any arguments
                    break;

                case "filmes-buscar":
                    this.filmesBuscarPage = int.Parse(this.filmePreviousPage.Last().Value[0]);
                    arg = this.filmePreviousPage.Last().Value[1];
                    arg1 = "previous";
                    break;

                case "filme-popular":
                    this.filmePopularPage = int.Parse(this.filmePreviousPage.Last().Value[0]);
                    break;

                case "filme-nowplaying":
                    this.filmeNowPlayingPage = int.Parse(this.filmePreviousPage.Last().Value[0]);
                    break;

                case "filme-upcoming":
                    this.filmeUpcomingPage = int.Parse(this.filmePreviousPage.Last().Value[0]);
                    break;

                case "filme-genre":
                    this.filmeGenrePage = int.Parse(this.filmePreviousPage.Last().Value[0]);
                    arg = this.filmePreviousPage.Last().Value[1];
                    break;

                case "filme-similar":
                    this.filmeSimilarPage = int.Parse(this.filmePreviousPage.Last().Value[0]);
                    arg = this.filmePreviousPage.Last().Value[1];
                    break;

                case "fav-filme":
                    //Favorito-Filme don't need any arguments
                    break;
            }

            mudaTela(this.actualPage, this.filmePreviousPage.Last().Key.Split("_")[0], arg, arg1);

            if (this.filmePreviousPage.Count > 0)
            {
                this.filmePreviousPage.Remove(this.filmePreviousPage.Last().Key);
            }
        }

        private void btnFilmeContentFav_MouseClick(object sender, MouseEventArgs e)
        {
            var result = c.Read(FrmMain.getConnection(), "filme", "filme_id", this.filmeId) ?? Array.Empty<object[]>();

            if (result.Length > 0)
            {
                if (c.Delete(FrmMain.getConnection(), "filme", Convert.ToInt32(result[0][0]))) btnFilmeContentFav.Text = "      Adicionar aos Favoritos";
                else MessageBox.Show("Falha ao Remover dos Favoritos", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {
                if (c.Create(FrmMain.getConnection(), "filme", Convert.ToInt32(FrmMain.Userid), this.filmeId)) btnFilmeContentFav.Text = "      Remover dos Favoritos";
                else MessageBox.Show("Falha ao Adicionar aos Favoritos", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFilmeContentSimilar_MouseClick(object sender, MouseEventArgs e)
        {
            var i = 0;

            while (true)
            {
                if (!filmeSimilarPreviousPage.ContainsKey(this.actualPage + "_" + i))
                {
                    filmeSimilarPreviousPage.Add(this.actualPage + "_" + i, this.filmeId);
                    break;
                }

                i++;
            }

            this.filmeSimilarPage = 1;
            mudaTela(this.actualPage, "filme-similar", this.filmeId.ToString());
        }

        #endregion

        #region Atributos/Métodos tela Filmes-Buscar

        private Resources.Movies.Objects.Search.Rootobject? filmesBuscarMovie = null;
        private string filmesBuscarQry;
        private int filmesBuscarMaxPage;
        private int filmesBuscarPage = 1;

        private void showFilmesBuscar()
        {
            pnlFilmesBuscar.Dock = DockStyle.Fill;
            pnlFilmesBuscar.Visible = true;
            pnlFilmesBuscar.Enabled = true;
            btnFilmesBuscar.BackColor = Color.FromArgb(sidebarSelectedColor, sidebarSelectedColor, sidebarSelectedColor);
            this.actualPage = "filmes-buscar";
        }

        private void hideFilmesBuscar()
        {
            pnlFilmesBuscar.Dock = DockStyle.None;
            pnlFilmesBuscar.Visible = false;
            pnlFilmesBuscar.Enabled = false;
            btnFilmesBuscar.BackColor = Color.FromArgb(15, 15, 15);
        }

        private void resetFilmesBuscar()
        {
            flpFilmesBuscarFilmGrid.Controls.Clear();
            pnlFilmesBuscarFilmGrid.Height = 200;
            pnlFilmesBuscarPageSumary.Controls.Clear();
            tbFilmesBuscarQuery.Text = string.Empty;
            this.filmesBuscarMovie = null;
            this.filmesBuscarPage = 1;
        }

        private void resetFilmesBuscar(params string[] args)
        {
            tbFilmesBuscarQuery.Text = args[2];
        }

        private void setFilmesBuscar(string qry)
        {
            Resources.Movies.Objects.Search.Rootobject? movie = null;

            if (qry == string.Empty)
            {
                if (this.filmesBuscarMovie == null)
                {
                    return;
                }

                else
                {
                    movie = this.filmesBuscarMovie;
                }
            }

            #region Realizando a busca

            else
            {
                if (this.filmesStorage.Search.ContainsKey(qry + ":" + this.filmesBuscarPage))
                {
                    foreach (var item in this.filmesStorage.Search)
                    {
                        string query = item.Key.Split(":")[0];
                        int page = int.Parse(item.Key.Split(":")[1]);

                        if (page == this.filmesBuscarPage && qry == query)
                        {
                            movie = item.Value;
                            break;
                        }
                    }
                }

                else
                {
                    try
                    {
                        movie = Resources.Movies.Recover.getSearch(qry, this.filmesBuscarPage);

                        if (movie != null)
                        {
                            this.filmesStorage.Search.Add(qry + ":" + this.filmesBuscarPage, movie);
                        }

                        else
                        {
                            throw new WebException();
                        }
                    }

                    catch (Exception e)
                    {
                        if (e is WebException we)
                        {
                            MessageBox.Show(we.Status.ToString());
                        }

                        else
                        {
                            MessageBox.Show(e.Message);
                        }
                    }
                }
            }

            #endregion

            #region Verificando os resutados obtidos

            if (movie == null)
            {
                pnlFilmesBuscarFilmGrid.Controls.Clear();
                pnlFilmesBuscarPageSumary.Controls.Clear();
                return;
            }

            if (movie.total_results <= 0)
            {
                MessageBox.Show("Filme não encontrado", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            #endregion

            #region Preenchendo o grid com os resultados da busca

            int larg = 125 + 20;
            int alt = 183 + 20;
            int containerLarg = pnlConteudo.Width - (68 + 20);
            int quantPerRow = (containerLarg - (containerLarg % larg)) / larg;
            int quantRows = (movie.results.Length - (movie.results.Length % quantPerRow)) / quantPerRow + ((movie.results.Length % quantPerRow == 0) ? 0 : 1);

            flpFilmesBuscarFilmGrid.Controls.Clear();
            flpFilmesBuscarFilmGrid.Height = quantRows * alt + 100;
            pnlFilmesBuscarFilmGrid.Height = flpFilmesBuscarFilmGrid.Height;

            Button[] b = new Button[movie.results.Length];

            int index = 0;

            for (int i = 0; i < quantRows; i++)
            {
                for (int j = 0; j < quantPerRow; j++)
                {
                    if (index >= b.Length)
                    {
                        break;
                    }

                    b[index] = new()
                    {
                        BackgroundImageLayout = ImageLayout.Stretch,
                        FlatStyle = FlatStyle.Flat,
                        ForeColor = Color.White,
                        Location = new Point(128 + larg * j, 20 + alt * i),
                        Margin = new Padding(10),
                        Name = "btnFilmesBuscarGridMovie_" + movie.results[index].id,
                        Size = new Size(125, 183),
                        TabIndex = 0,
                        TextAlign = ContentAlignment.MiddleCenter,
                        UseVisualStyleBackColor = true
                    };

                    b[index].MouseClick += btnFilmesBuscarGridMovie_MouseClick;
                    b[index].FlatAppearance.BorderColor = Color.FromArgb(30, 30, 30);
                    b[index].Cursor = Cursors.Hand;

                    if (movie.results[index].poster_path == null)
                    {
                        b[index].Text = movie.results[index].title;
                    }

                    else
                    {
                        if (this.filmesStorage.PosterImage.ContainsKey(movie.results[index].id))
                        {
                            foreach (KeyValuePair<int, Image> item in this.filmesStorage.PosterImage)
                            {
                                if (item.Key == movie.results[index].id)
                                {
                                    b[index].BackgroundImage = item.Value;
                                    break;
                                }
                            }
                        }

                        else
                        {
                            var request = WebRequest.Create(string.Format(Resources.Movies.Configuration.imageUrlBase, Resources.Movies.Configuration.imageUrlSize, movie.results[index].poster_path));

                            using (var response = request.GetResponse())
                            using (var stream = response.GetResponseStream())
                            {
                                Image image = Bitmap.FromStream(stream);
                                b[index].BackgroundImage = image;

                                if (stream != null)
                                {
                                    this.filmesStorage.PosterImage.Add(movie.results[index].id, image);
                                }
                            }
                        }
                    }

                    flpFilmesBuscarFilmGrid.Controls.Add(b[index]);

                    index++;
                }
            }

            #endregion

            #region Criando os botões de navegação entre páginas

            pnlFilmesBuscarPageSumary.Controls.Clear();

            Button[] b1 = new Button[5];

            for (int i = 0; i < b1.Length; i++)
            {
                bool enabled = true;

                switch (i)
                {
                    case 0:
                        if (this.filmesBuscarPage == 1)
                        {
                            enabled = false;
                        }

                        break;

                    case 1:
                        if (this.filmesBuscarPage == 1)
                        {
                            enabled = false;
                        }

                        break;

                    case 3:
                        if (this.filmesBuscarPage == movie.total_pages)
                        {
                            enabled = false;
                        }

                        break;

                    case 4:
                        if (this.filmesBuscarPage == movie.total_pages)
                        {
                            enabled = false;
                        }

                        break;
                }

                b1[i] = new()
                {
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point),
                    ForeColor = Color.White,
                    Location = new Point(50 * i + ((this.Width - pnlSidebar.MinimumSize.Width - 250) / 2), 0),
                    Margin = new Padding(0, 0, 10, 0),
                    Name = "btnFilmesBuscarPageSumary" + i,
                    Size = new Size(40, 40),
                    TabIndex = 7,
                    UseVisualStyleBackColor = true,
                    Enabled = enabled,
                    BackgroundImageLayout = ImageLayout.Center,
                    Visible = true
                };

                switch (i)
                {
                    case 0:
                        b1[i].MouseClick += btnFilmesBuscarAllBack_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.all_back;
                        break;

                    case 1:
                        b1[i].MouseClick += btnFilmesBuscarBack_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.back;
                        break;

                    case 2:
                        b1[i].Text = this.filmesBuscarPage.ToString();
                        break;

                    case 3:
                        b1[i].MouseClick += btnFilmesBuscarForward_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.forward;
                        break;

                    case 4:
                        b1[i].MouseClick += btnFilmesBuscarAllForward_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.all_forward;
                        break;
                }

                b1[i].FlatAppearance.BorderColor = Color.FromArgb(60, 60, 60);
                b1[i].FlatAppearance.MouseDownBackColor = Color.FromArgb(30, 30, 30);
                b1[i].FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 60, 60);

                pnlFilmesBuscarPageSumary.Controls.Add(b1[i]);
            }

            #endregion

            this.filmesBuscarMovie = movie;
            this.filmesBuscarQry = qry;
            this.filmesBuscarMaxPage = movie.total_pages;
        }

        private void btnFilmesBuscarAllBack_MouseClick(object? sender, MouseEventArgs e)
        {
            this.filmesBuscarPage = 1;
            setFilmesBuscar(this.filmesBuscarQry);
            pnlFilmesBuscar.VerticalScroll.Value = 0;
        }

        private void btnFilmesBuscarBack_MouseClick(object? sender, MouseEventArgs e)
        {
            this.filmesBuscarPage -= 1;
            setFilmesBuscar(this.filmesBuscarQry);
            pnlFilmesBuscar.VerticalScroll.Value = 0;
        }

        private void btnFilmesBuscarAllForward_MouseClick(object? sender, MouseEventArgs e)
        {
            this.filmesBuscarPage = this.filmesBuscarMaxPage;
            setFilmesBuscar(this.filmesBuscarQry);
            pnlFilmesBuscar.VerticalScroll.Value = 0;
        }

        private void btnFilmesBuscarForward_MouseClick(object? sender, MouseEventArgs e)
        {
            this.filmesBuscarPage += 1;
            setFilmesBuscar(this.filmesBuscarQry);
            pnlFilmesBuscar.VerticalScroll.Value = 0;
        }

        private void btnFilmesBuscarSubmit_MouseClick(object sender, MouseEventArgs e)
        {
            if (tbFilmesBuscarQuery.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Insira um filme", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            setFilmesBuscar(tbFilmesBuscarQuery.Text);
            pnlFilmesBuscar.VerticalScroll.Value = 0;
        }

        private void btnFilmesBuscarGridMovie_MouseClick(object? sender, MouseEventArgs e)
        {
            var b = sender as Button;

            int i = 0;

            while (true)
            {
                if (!this.filmePreviousPage.ContainsKey(this.actualPage + "_" + i))
                {
                    this.filmePreviousPage.Add(this.actualPage + "_" + i, new string[] { this.filmesBuscarPage.ToString(), this.filmesBuscarQry });
                    break;
                }

                i++;
            }

            mudaTela(this.actualPage, "filme", b.Name.Split("_")[1]);
        }

        #endregion

        #region Atributos/Métodos tela Filme-Popular

        private int filmePopularPage = 1;
        private int filmePopularPageMax;

        private void showFilmePopular()
        {
            pnlFilmePopular.Dock = DockStyle.Fill;
            pnlFilmePopular.Enabled = true;
            pnlFilmePopular.Visible = true;
            btnFilmesPopular.BackColor = Color.FromArgb(sidebarSelectedColor, sidebarSelectedColor, sidebarSelectedColor);
            this.actualPage = "filme-popular";
        }

        private void hideFilmePopular()
        {
            pnlFilmePopular.Dock = DockStyle.None;
            pnlFilmePopular.Enabled = false;
            pnlFilmePopular.Visible = false;
            btnFilmesPopular.BackColor = Color.FromArgb(15, 15, 15);
        }

        private void setFilmePopular()
        {
            Resources.Movies.Objects.Popular.Rootobject? filme = null;

            #region Realizando a busca

            if (this.filmesStorage.Popular.ContainsKey(string.Format("{0}:{1}", this.filmePopularPage, Resources.Movies.Configuration.region)))
            {
                foreach (var item in this.filmesStorage.Popular)
                {
                    if (item.Key == string.Format("{0}:{1}", this.filmePopularPage, Resources.Movies.Configuration.region))
                    {
                        filme = item.Value;
                        break;
                    }
                }
            }

            else
            {
                try
                {
                    filme = Resources.Movies.Recover.getPopular(Resources.Movies.Configuration.region, this.filmePopularPage);

                    if (filme != null)
                    {
                        this.filmesStorage.Popular.Add(string.Format("{0}:{1}", this.filmePopularPage, Resources.Movies.Configuration.region), filme);
                    }
                }

                catch
                {
                    filme = null;
                }
            }

            #endregion

            #region Verificando os resultados recebidos

            if (filme == null || filme.total_results == 0)
            {
                MessageBox.Show("Erro de conexão!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                mudaTela(this.actualPage, "home");
                return;
            }

            #endregion

            #region Preenchendo o grid

            flpFilmePopularGrid.Controls.Clear();

            int quant = filme.results.Length;
            int larg = 125 + 40;
            int alt = 183 + 40;
            int containerLarg = this.Width - (68 + 20 + 113);
            int quantPerRow = (containerLarg - (containerLarg % larg)) / larg;
            int quantRows = (quant - (quant % quantPerRow)) / quantPerRow + ((quant % quantPerRow == 0) ? 0 : 1);

            flpFilmePopularGrid.Height = quantRows * alt + 40;

            Button[] b = new Button[quant];

            int index = 0;

            for (int i = 0; i < quantRows; i++)
            {
                for (int j = 0; j < quantPerRow; j++)
                {
                    if (index >= b.Length)
                    {
                        break;
                    }

                    b[index] = new()
                    {
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Cursor = Cursors.Hand,
                        FlatStyle = FlatStyle.Flat,
                        ForeColor = Color.White,
                        Location = new Point(113 + larg * j, 10 + alt * i),
                        Margin = new Padding(0, 20, 20, 0),
                        Name = "btnFilmePopularGridItem_" + filme.results[index].id,
                        Size = new Size(larg - 20, alt - 20),
                        TabIndex = 0,
                        TextAlign = ContentAlignment.MiddleCenter,
                        UseVisualStyleBackColor = true
                    };

                    b[index].FlatAppearance.BorderColor = Color.FromArgb(60, 60, 60);
                    b[index].FlatAppearance.MouseDownBackColor = Color.FromArgb(20, 20, 20);
                    b[index].FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 30, 30);
                    b[index].MouseClick += btnFilmePopularGridItem_MouseClick;

                    if (this.filmesStorage.PosterImage.ContainsKey(filme.results[index].id))
                    {
                        foreach (var item in this.filmesStorage.PosterImage)
                        {
                            if (item.Key == filme.results[index].id)
                            {
                                b[index].BackgroundImage = item.Value;
                                break;
                            }
                        }
                    }

                    else
                    {
                        string url = string.Format(Resources.Movies.Configuration.imageUrlBase, Resources.Movies.Configuration.imageUrlSize, filme.results[index].poster_path);

                        try
                        {
                            var request = WebRequest.Create(url);

                            using (var response = request.GetResponse())
                            using (var stream = response.GetResponseStream())
                            {
                                Image img = Image.FromStream(stream);

                                if (img != null)
                                {
                                    b[index].BackgroundImage = img;
                                    this.filmesStorage.PosterImage.Add(filme.results[index].id, img);
                                }

                                else
                                {
                                    throw new Exception();
                                }
                            }
                        }

                        catch
                        {
                            b[index].Text = filme.results[index].title;
                        }
                    }

                    flpFilmePopularGrid.Controls.Add(b[index]);
                    index++;
                }
            }

            #endregion

            #region Criando os botões de navegação entre páginas

            pnlFilmePopularPageSumary.Controls.Clear();

            Button[] b1 = new Button[5];

            for (int i = 0; i < b1.Length; i++)
            {
                bool enabled = true;

                switch (i)
                {
                    case 0:
                        if (this.filmePopularPage == 1)
                        {
                            enabled = false;
                        }

                        break;

                    case 1:
                        if (this.filmePopularPage == 1)
                        {
                            enabled = false;
                        }

                        break;

                    case 3:
                        if (this.filmePopularPage == ((filme.total_pages > 500) ? 500 : filme.total_pages))
                        {
                            enabled = false;
                        }

                        break;

                    case 4:
                        if (this.filmePopularPage == ((filme.total_pages > 500) ? 500 : filme.total_pages))
                        {
                            enabled = false;
                        }

                        break;
                }

                b1[i] = new()
                {
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point),
                    ForeColor = Color.White,
                    Location = new Point(50 * i + ((this.Width - pnlSidebar.MinimumSize.Width - 250) / 2), 0),
                    Margin = new Padding(0, 0, 10, 0),
                    Name = "btnFilmePopularPageSumary" + i,
                    Size = new Size(40, 40),
                    TabIndex = 7,
                    UseVisualStyleBackColor = true,
                    Enabled = enabled,
                    BackgroundImageLayout = ImageLayout.Center,
                    Visible = true
                };

                switch (i)
                {
                    case 0:
                        b1[i].MouseClick += btnFilmePopularAllBack_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.all_back;
                        break;

                    case 1:
                        b1[i].MouseClick += btnFilmePopularBack_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.back;
                        break;

                    case 2:
                        b1[i].Text = this.filmePopularPage.ToString();
                        break;

                    case 3:
                        b1[i].MouseClick += btnFilmePopularForward_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.forward;
                        break;

                    case 4:
                        b1[i].MouseClick += btnFilmePopularAllForward_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.all_forward;
                        break;
                }

                b1[i].FlatAppearance.BorderColor = Color.FromArgb(60, 60, 60);
                b1[i].FlatAppearance.MouseDownBackColor = Color.FromArgb(30, 30, 30);
                b1[i].FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 60, 60);

                pnlFilmePopularPageSumary.Controls.Add(b1[i]);
            }

            #endregion

            this.filmePopularPageMax = (filme.total_pages > 500) ? 500 : filme.total_pages;
            pnlFilmePopular.VerticalScroll.Value = 0;
        }

        private void resetFilmePopular()
        {
            this.filmePopularPage = 1;
            flpFilmePopularGrid.Height = 200;
        }

        private void btnFilmePopularGridItem_MouseClick(object? sender, MouseEventArgs e)
        {
            var b = sender as Button;

            int i = 0;

            while (true)
            {
                if (!this.filmePreviousPage.ContainsKey(this.actualPage + "_" + i))
                {
                    this.filmePreviousPage.Add(this.actualPage + "_" + i, new string[] { this.filmePopularPage.ToString() });
                    break;
                }
            }

            mudaTela(this.actualPage, "filme", b.Name.Split("_")[1]);
        }

        private void btnFilmePopularAllBack_MouseClick(object? sender, MouseEventArgs e)
        {
            this.filmePopularPage = 1;
            setFilmePopular();
        }

        private void btnFilmePopularBack_MouseClick(object? sender, MouseEventArgs e)
        {
            this.filmePopularPage--;
            setFilmePopular();
        }

        private void btnFilmePopularForward_MouseClick(object? sender, MouseEventArgs e)
        {
            this.filmePopularPage++;
            setFilmePopular();
        }

        private void btnFilmePopularAllForward_MouseClick(object? sender, MouseEventArgs e)
        {
            this.filmePopularPage = this.filmePopularPageMax;
            setFilmePopular();
        }

        #endregion

        #region Atributos/Métodos tela Filme-Upcoming

        private int filmeUpcomingPage = 1;
        private int filmeUpcomingPageMax;

        private void showFilmeUpcoming()
        {
            pnlFilmeUpcoming.Dock = DockStyle.Fill;
            pnlFilmeUpcoming.Enabled = true;
            pnlFilmeUpcoming.Visible = true;
            this.actualPage = "filme-upcoming";
            btnFilmesEmBreve.BackColor = Color.FromArgb(this.sidebarSelectedColor, this.sidebarSelectedColor, this.sidebarSelectedColor);
        }

        private void hideFilmeUpcoming()
        {
            pnlFilmeUpcoming.Dock = DockStyle.None;
            pnlFilmeUpcoming.Enabled = false;
            pnlFilmeUpcoming.Visible = false;
            btnFilmesEmBreve.BackColor = Color.FromArgb(15, 15, 15);
        }

        private void setFilmeUpcoming()
        {
            Resources.Movies.Objects.Upcoming.Rootobject? filme = null;

            #region Realizando a busca

            if (this.filmesStorage.Upcoming.ContainsKey(string.Format("{0}:{1}", this.filmeUpcomingPage, Resources.Movies.Configuration.region)))
            {
                foreach (var item in this.filmesStorage.Upcoming)
                {
                    if (item.Key == string.Format("{0}:{1}", this.filmeUpcomingPage, Resources.Movies.Configuration.region))
                    {
                        filme = item.Value;
                        break;
                    }
                }
            }

            else
            {
                try
                {
                    filme = Resources.Movies.Recover.getUpcoming(Resources.Movies.Configuration.region, this.filmeUpcomingPage);

                    if (filme != null)
                    {
                        this.filmesStorage.Upcoming.Add(string.Format("{0}:{1}", this.filmeUpcomingPage, Resources.Movies.Configuration.region), filme);
                    }
                }

                catch
                {
                    filme = null;
                }
            }

            #endregion

            #region Verificando os resultados recebidos

            if (filme == null || filme.total_results == 0)
            {
                MessageBox.Show("Erro de conexão!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                mudaTela(this.actualPage, "home");
                return;
            }

            #endregion

            #region Preenchendo o grid

            flpFilmeUpcomingGrid.Controls.Clear();

            int quant = filme.results.Length;
            int larg = 125 + 40;
            int alt = 183 + 40;
            int containerLarg = this.Width - (68 + 20 + 113);
            int quantPerRow = (containerLarg - (containerLarg % larg)) / larg;
            int quantRows = (quant - (quant % quantPerRow)) / quantPerRow + ((quant % quantPerRow == 0) ? 0 : 1);

            flpFilmeUpcomingGrid.Height = quantRows * alt + 40;

            Button[] b = new Button[quant];

            int index = 0;

            for (int i = 0; i < quantRows; i++)
            {
                for (int j = 0; j < quantPerRow; j++)
                {
                    if (index >= b.Length)
                    {
                        break;
                    }

                    b[index] = new()
                    {
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Cursor = Cursors.Hand,
                        FlatStyle = FlatStyle.Flat,
                        ForeColor = Color.White,
                        Location = new Point(113 + larg * j, 10 + alt * i),
                        Margin = new Padding(0, 20, 20, 0),
                        Name = "btnFilmeUpcomingGridItem_" + filme.results[index].id,
                        Size = new Size(larg - 20, alt - 20),
                        TabIndex = 0,
                        TextAlign = ContentAlignment.MiddleCenter,
                        UseVisualStyleBackColor = true
                    };

                    b[index].FlatAppearance.BorderColor = Color.FromArgb(60, 60, 60);
                    b[index].FlatAppearance.MouseDownBackColor = Color.FromArgb(20, 20, 20);
                    b[index].FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 30, 30);
                    b[index].MouseClick += btnFilmeUpcomingGridItem_MouseClick;

                    if (this.filmesStorage.PosterImage.ContainsKey(filme.results[index].id))
                    {
                        foreach (var item in this.filmesStorage.PosterImage)
                        {
                            if (item.Key == filme.results[index].id)
                            {
                                b[index].BackgroundImage = item.Value;
                                break;
                            }
                        }
                    }

                    else
                    {
                        string url = string.Format(Resources.Movies.Configuration.imageUrlBase, Resources.Movies.Configuration.imageUrlSize, filme.results[index].poster_path);

                        try
                        {
                            var request = WebRequest.Create(url);

                            using (var response = request.GetResponse())
                            using (var stream = response.GetResponseStream())
                            {
                                Image img = Image.FromStream(stream);

                                if (img != null)
                                {
                                    b[index].BackgroundImage = img;
                                    this.filmesStorage.PosterImage.Add(filme.results[index].id, img);
                                }

                                else
                                {
                                    throw new Exception();
                                }
                            }
                        }

                        catch
                        {
                            b[index].Text = filme.results[index].title;
                        }
                    }

                    flpFilmeUpcomingGrid.Controls.Add(b[index]);
                    index++;
                }
            }

            #endregion

            #region Criando os botões de navegação entre páginas

            pnlFilmeUpcomingPageSumary.Controls.Clear();

            Button[] b1 = new Button[5];

            for (int i = 0; i < b1.Length; i++)
            {
                bool enabled = true;

                switch (i)
                {
                    case 0:
                        if (this.filmeUpcomingPage == 1)
                        {
                            enabled = false;
                        }

                        break;

                    case 1:
                        if (this.filmeUpcomingPage == 1)
                        {
                            enabled = false;
                        }

                        break;

                    case 3:
                        if (this.filmeUpcomingPage == ((filme.total_pages > 500) ? 500 : filme.total_pages))
                        {
                            enabled = false;
                        }

                        break;

                    case 4:
                        if (this.filmeUpcomingPage == ((filme.total_pages > 500) ? 500 : filme.total_pages))
                        {
                            enabled = false;
                        }

                        break;
                }

                b1[i] = new()
                {
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point),
                    ForeColor = Color.White,
                    Location = new Point(50 * i + ((this.Width - pnlSidebar.MinimumSize.Width - 250) / 2), 0),
                    Margin = new Padding(0, 0, 10, 0),
                    Name = "btnFilmeUpcomingPageSumary" + i,
                    Size = new Size(40, 40),
                    TabIndex = 7,
                    UseVisualStyleBackColor = true,
                    Enabled = enabled,
                    BackgroundImageLayout = ImageLayout.Center,
                    Visible = true
                };

                switch (i)
                {
                    case 0:
                        b1[i].MouseClick += btnFilmeUpcomingAllBack_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.all_back;
                        break;

                    case 1:
                        b1[i].MouseClick += btnFilmeUpcomingBack_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.back;
                        break;

                    case 2:
                        b1[i].Text = this.filmeUpcomingPage.ToString();
                        break;

                    case 3:
                        b1[i].MouseClick += btnFilmeUpcomingForward_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.forward;
                        break;

                    case 4:
                        b1[i].MouseClick += btnFilmeUpcomingAllForward_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.all_forward;
                        break;
                }

                b1[i].FlatAppearance.BorderColor = Color.FromArgb(60, 60, 60);
                b1[i].FlatAppearance.MouseDownBackColor = Color.FromArgb(30, 30, 30);
                b1[i].FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 60, 60);

                pnlFilmeUpcomingPageSumary.Controls.Add(b1[i]);
            }

            #endregion

            this.filmeUpcomingPageMax = (filme.total_pages > 500) ? 500 : filme.total_pages;
            pnlFilmeUpcoming.VerticalScroll.Value = 0;
        }

        private void resetFilmeUpcoming()
        {
            this.filmeUpcomingPage = 1;
            flpFilmeUpcomingGrid.Height = 200;
        }

        private void btnFilmeUpcomingGridItem_MouseClick(object? sender, MouseEventArgs e)
        {
            var b = sender as Button;

            int i = 0;

            while (true)
            {
                if (!this.filmePreviousPage.ContainsKey(this.actualPage + "_" + i))
                {
                    this.filmePreviousPage.Add(this.actualPage + "_" + i, new string[] { this.filmeUpcomingPage.ToString() });
                    break;
                }
            }

            mudaTela(this.actualPage, "filme", b.Name.Split("_")[1]);
        }

        private void btnFilmeUpcomingAllBack_MouseClick(object? sender, MouseEventArgs e)
        {
            this.filmeUpcomingPage = 1;
            setFilmeUpcoming();
        }

        private void btnFilmeUpcomingBack_MouseClick(object? sender, MouseEventArgs e)
        {
            this.filmeUpcomingPage--;
            setFilmeUpcoming();
        }

        private void btnFilmeUpcomingForward_MouseClick(object? sender, MouseEventArgs e)
        {
            this.filmeUpcomingPage++;
            setFilmeUpcoming();
        }

        private void btnFilmeUpcomingAllForward_MouseClick(object? sender, MouseEventArgs e)
        {
            this.filmeUpcomingPage = this.filmeUpcomingPageMax;
            setFilmeUpcoming();
        }

        #endregion

        #region Atributos/Métodos tela Filme-NowPlaying

        private int filmeNowPlayingPage = 1;
        private int filmeNowPlayingPageMax;

        private void showFilmeNowPlaying()
        {
            pnlFilmeNowPlaying.Dock = DockStyle.Fill;
            pnlFilmeNowPlaying.Enabled = true;
            pnlFilmeNowPlaying.Visible = true;
            this.actualPage = "filme-nowplaying";
            btnFilmesEstreia.BackColor = Color.FromArgb(this.sidebarSelectedColor, this.sidebarSelectedColor, this.sidebarSelectedColor);
        }

        private void hideFilmeNowPlaying()
        {
            pnlFilmeNowPlaying.Dock = DockStyle.None;
            pnlFilmeNowPlaying.Enabled = false;
            pnlFilmeNowPlaying.Visible = false;
            btnFilmesEstreia.BackColor = Color.FromArgb(15, 15, 15);
        }

        private void setFilmeNowPlaying()
        {
            Resources.Movies.Objects.NowPlaying.Rootobject? filme = null;

            #region Realizando a busca

            if (this.filmesStorage.NowPlaying.ContainsKey(string.Format("{0}:{1}", this.filmeNowPlayingPage, Resources.Movies.Configuration.region)))
            {
                foreach (var item in this.filmesStorage.NowPlaying)
                {
                    if (item.Key == string.Format("{0}:{1}", this.filmeNowPlayingPage, Resources.Movies.Configuration.region))
                    {
                        filme = item.Value;
                        break;
                    }
                }
            }

            else
            {
                try
                {
                    filme = Resources.Movies.Recover.getNowPlaying(Resources.Movies.Configuration.region, this.filmeNowPlayingPage);

                    if (filme != null)
                    {
                        this.filmesStorage.NowPlaying.Add(string.Format("{0}:{1}", this.filmeNowPlayingPage, Resources.Movies.Configuration.region), filme);
                    }
                }

                catch
                {
                    filme = null;
                }
            }

            #endregion

            #region Verificando os resultados recebidos

            if (filme == null || filme.total_results == 0)
            {
                MessageBox.Show("Erro de conexão!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                mudaTela(this.actualPage, "home");
                return;
            }

            #endregion

            #region Preenchendo o grid

            flpFilmeNowPlayingGrid.Controls.Clear();

            int quant = filme.results.Length;
            int larg = 125 + 40;
            int alt = 183 + 40;
            int containerLarg = this.Width - (68 + 20 + 113);
            int quantPerRow = (containerLarg - (containerLarg % larg)) / larg;
            int quantRows = (quant - (quant % quantPerRow)) / quantPerRow + ((quant % quantPerRow == 0) ? 0 : 1);

            flpFilmeNowPlayingGrid.Height = quantRows * alt + 40;

            Button[] b = new Button[quant];

            int index = 0;

            for (int i = 0; i < quantRows; i++)
            {
                for (int j = 0; j < quantPerRow; j++)
                {
                    if (index >= b.Length)
                    {
                        break;
                    }

                    b[index] = new()
                    {
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Cursor = Cursors.Hand,
                        FlatStyle = FlatStyle.Flat,
                        ForeColor = Color.White,
                        Location = new Point(113 + larg * j, 10 + alt * i),
                        Margin = new Padding(0, 20, 20, 0),
                        Name = "btnFilmeNowPlayingGridItem_" + filme.results[index].id,
                        Size = new Size(larg - 20, alt - 20),
                        TabIndex = 0,
                        TextAlign = ContentAlignment.MiddleCenter,
                        UseVisualStyleBackColor = true
                    };

                    b[index].FlatAppearance.BorderColor = Color.FromArgb(60, 60, 60);
                    b[index].FlatAppearance.MouseDownBackColor = Color.FromArgb(20, 20, 20);
                    b[index].FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 30, 30);
                    b[index].MouseClick += btnFilmeNowPlayingGridItem_MouseClick;

                    if (this.filmesStorage.PosterImage.ContainsKey(filme.results[index].id))
                    {
                        foreach (var item in this.filmesStorage.PosterImage)
                        {
                            if (item.Key == filme.results[index].id)
                            {
                                b[index].BackgroundImage = item.Value;
                                break;
                            }
                        }
                    }

                    else
                    {
                        string url = string.Format(Resources.Movies.Configuration.imageUrlBase, Resources.Movies.Configuration.imageUrlSize, filme.results[index].poster_path);

                        try
                        {
                            var request = WebRequest.Create(url);

                            using (var response = request.GetResponse())
                            using (var stream = response.GetResponseStream())
                            {
                                Image img = Image.FromStream(stream);

                                if (img != null)
                                {
                                    b[index].BackgroundImage = img;
                                    this.filmesStorage.PosterImage.Add(filme.results[index].id, img);
                                }

                                else
                                {
                                    throw new Exception();
                                }
                            }
                        }

                        catch
                        {
                            b[index].Text = filme.results[index].title;
                        }
                    }

                    flpFilmeNowPlayingGrid.Controls.Add(b[index]);
                    index++;
                }
            }

            #endregion

            #region Criando os botões de navegação entre páginas

            pnlFilmeNowPlayingPageSumary.Controls.Clear();

            Button[] b1 = new Button[5];

            for (int i = 0; i < b1.Length; i++)
            {
                bool enabled = true;

                switch (i)
                {
                    case 0:
                        if (this.filmeNowPlayingPage == 1)
                        {
                            enabled = false;
                        }

                        break;

                    case 1:
                        if (this.filmeNowPlayingPage == 1)
                        {
                            enabled = false;
                        }

                        break;

                    case 3:
                        if (this.filmeNowPlayingPage == ((filme.total_pages > 500) ? 500 : filme.total_pages))
                        {
                            enabled = false;
                        }

                        break;

                    case 4:
                        if (this.filmeNowPlayingPage == ((filme.total_pages > 500) ? 500 : filme.total_pages))
                        {
                            enabled = false;
                        }

                        break;
                }

                b1[i] = new()
                {
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point),
                    ForeColor = Color.White,
                    Location = new Point(50 * i + ((this.Width - pnlSidebar.MinimumSize.Width - 250) / 2), 0),
                    Margin = new Padding(0, 0, 10, 0),
                    Name = "btnFilmeNowPlayingPageSumary" + i,
                    Size = new Size(40, 40),
                    TabIndex = 7,
                    UseVisualStyleBackColor = true,
                    Enabled = enabled,
                    BackgroundImageLayout = ImageLayout.Center,
                    Visible = true
                };

                switch (i)
                {
                    case 0:
                        b1[i].MouseClick += btnFilmeNowPlayingAllBack_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.all_back;
                        break;

                    case 1:
                        b1[i].MouseClick += btnFilmeNowPlayingBack_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.back;
                        break;

                    case 2:
                        b1[i].Text = this.filmeNowPlayingPage.ToString();
                        break;

                    case 3:
                        b1[i].MouseClick += btnFilmeNowPlayingForward_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.forward;
                        break;

                    case 4:
                        b1[i].MouseClick += btnFilmeNowPlayingAllForward_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.all_forward;
                        break;
                }

                b1[i].FlatAppearance.BorderColor = Color.FromArgb(60, 60, 60);
                b1[i].FlatAppearance.MouseDownBackColor = Color.FromArgb(30, 30, 30);
                b1[i].FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 60, 60);

                pnlFilmeNowPlayingPageSumary.Controls.Add(b1[i]);
            }

            #endregion

            this.filmeNowPlayingPageMax = (filme.total_pages > 500) ? 500 : filme.total_pages;
            pnlFilmeNowPlaying.VerticalScroll.Value = 0;
        }

        private void resetFilmeNowPlaying()
        {
            this.filmeNowPlayingPage = 1;
            flpFilmeNowPlayingGrid.Height = 200;
        }

        private void btnFilmeNowPlayingGridItem_MouseClick(object? sender, MouseEventArgs e)
        {
            var b = sender as Button;

            int i = 0;

            while (true)
            {
                if (!this.filmePreviousPage.ContainsKey(this.actualPage + "_" + i))
                {
                    this.filmePreviousPage.Add(this.actualPage + "_" + i, new string[] { this.filmeNowPlayingPage.ToString() });
                    break;
                }
            }

            mudaTela(this.actualPage, "filme", b.Name.Split("_")[1]);
        }

        private void btnFilmeNowPlayingAllBack_MouseClick(object? sender, MouseEventArgs e)
        {
            this.filmeNowPlayingPage = 1;
            setFilmeNowPlaying();
        }

        private void btnFilmeNowPlayingBack_MouseClick(object? sender, MouseEventArgs e)
        {
            this.filmeNowPlayingPage--;
            setFilmeNowPlaying();
        }

        private void btnFilmeNowPlayingForward_MouseClick(object? sender, MouseEventArgs e)
        {
            this.filmeNowPlayingPage++;
            setFilmeNowPlaying();
        }

        private void btnFilmeNowPlayingAllForward_MouseClick(object? sender, MouseEventArgs e)
        {
            this.filmeNowPlayingPage = this.filmeNowPlayingPageMax;
            setFilmeNowPlaying();
        }

        #endregion

        #region Atributos/Métodos tela Filme-Genre

        private int filmeGenrePage = 1;
        private int filmesGenreId;
        private int filmesGenrePageMax;

        /// <summary>
        /// string page, int movie_id
        /// </summary>
        private Dictionary<string, int> filmeGenrePreviousPage = new();

        private void showFilmeGenre()
        {
            pnlFilmeGenre.Dock = DockStyle.Fill;
            pnlFilmeGenre.Enabled = true;
            pnlFilmeGenre.Visible = true;
            this.actualPage = "filme-genre";
        }

        private void setFilmeGenre(int id)
        {
            Resources.Movies.Objects.Genre.Rootobject? filmes = null;

            #region Obtendo os resultados da busca

            if (this.filmesStorage.Genre.ContainsKey(this.filmeGenrePage + ":" + id))
            {
                foreach (var item in this.filmesStorage.Genre)
                {
                    if (item.Key == (this.filmeGenrePage + ":" + id))
                    {
                        filmes = item.Value;
                        break;
                    }
                }
            }

            else
            {
                try
                {
                    filmes = Resources.Movies.Recover.getGenre(this.filmeGenrePage, id);

                    if (filmes == null || filmes.total_results == 0)
                    {
                        throw new Exception();
                    }

                    else
                    {
                        this.filmesStorage.Genre.Add(this.filmeGenrePage + ":" + id, filmes);
                    }
                }

                catch
                {
                    MessageBox.Show("Erro de conexão!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            #endregion

            #region Verificando os resultados obtidos

            if (filmes == null || filmes.total_results == 0)
            {
                MessageBox.Show("Erro de conexão!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            #endregion

            #region Preenchendo o título

            foreach (var item in this.filmesGenres.genres)
            {
                if (item.id == id)
                {
                    lblFilmeGenreTitle.Text = "Mais filmes do gênero " + item.name.ToLower();
                }
            }

            #endregion

            #region Preenchendo o grid

            flpFilmeGenreGrid.Controls.Clear();

            int quant = filmes.results.Length;
            int larg = 125 + 40;
            int alt = 183 + 40;
            int containerLarg = this.Width - (68 + 20 + 113);
            int quantPerRow = (containerLarg - (containerLarg % larg)) / larg;
            int quantRows = (quant - (quant % quantPerRow)) / quantPerRow + ((quant % quantPerRow == 0) ? 0 : 1);

            flpFilmeGenreGrid.Height = quantRows * alt + 40;

            Button[] b = new Button[quant];

            int index = 0;

            for (int i = 0; i < quantRows; i++)
            {
                for (int j = 0; j < quantPerRow; j++)
                {
                    if (index >= b.Length)
                    {
                        break;
                    }

                    b[index] = new()
                    {
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Cursor = Cursors.Hand,
                        FlatStyle = FlatStyle.Flat,
                        ForeColor = Color.White,
                        Location = new Point(113 + larg * j, 10 + alt * i),
                        Margin = new Padding(0, 20, 20, 0),
                        Name = "btnFilmeGenreGridItem_" + filmes.results[index].id,
                        Size = new Size(larg - 20, alt - 20),
                        TabIndex = 0,
                        TextAlign = ContentAlignment.MiddleCenter,
                        UseVisualStyleBackColor = true
                    };

                    b[index].FlatAppearance.BorderColor = Color.FromArgb(60, 60, 60);
                    b[index].FlatAppearance.MouseDownBackColor = Color.FromArgb(20, 20, 20);
                    b[index].FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 30, 30);
                    b[index].MouseClick += btnFilmeGenreGridItem_MouseClick;

                    if (this.filmesStorage.PosterImage.ContainsKey(filmes.results[index].id))
                    {
                        foreach (var item in this.filmesStorage.PosterImage)
                        {
                            if (item.Key == filmes.results[index].id)
                            {
                                b[index].BackgroundImage = item.Value;
                                break;
                            }
                        }
                    }

                    else
                    {
                        string url = string.Format(Resources.Movies.Configuration.imageUrlBase, Resources.Movies.Configuration.imageUrlSize, filmes.results[index].poster_path);

                        try
                        {
                            var request = WebRequest.Create(url);

                            using (var response = request.GetResponse())
                            using (var stream = response.GetResponseStream())
                            {
                                Image img = Image.FromStream(stream);

                                if (img != null)
                                {
                                    b[index].BackgroundImage = img;
                                    this.filmesStorage.PosterImage.Add(filmes.results[index].id, img);
                                }

                                else
                                {
                                    throw new Exception();
                                }
                            }
                        }

                        catch
                        {
                            b[index].Text = filmes.results[index].title;
                        }
                    }

                    flpFilmeGenreGrid.Controls.Add(b[index]);
                    index++;
                }
            }

            #endregion

            #region Criando os botões de navegação entre páginas

            pnlFilmeGenrePageSumary.Controls.Clear();

            Button[] b1 = new Button[5];

            for (int i = 0; i < b1.Length; i++)
            {
                bool enabled = true;

                switch (i)
                {
                    case 0:
                        if (this.filmeGenrePage == 1)
                        {
                            enabled = false;
                        }

                        break;

                    case 1:
                        if (this.filmeGenrePage == 1)
                        {
                            enabled = false;
                        }

                        break;

                    case 3:
                        if (this.filmeGenrePage == ((filmes.total_pages > 500) ? 500 : filmes.total_pages))
                        {
                            enabled = false;
                        }

                        break;

                    case 4:
                        if (this.filmeGenrePage == ((filmes.total_pages > 500) ? 500 : filmes.total_pages))
                        {
                            enabled = false;
                        }

                        break;
                }

                b1[i] = new()
                {
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point),
                    ForeColor = Color.White,
                    Location = new Point(50 * i + ((this.Width - pnlSidebar.MinimumSize.Width - 250) / 2), 0),
                    Margin = new Padding(0, 0, 10, 0),
                    Name = "btnFilmeGenrePageSumary" + i,
                    Size = new Size(40, 40),
                    TabIndex = 7,
                    UseVisualStyleBackColor = true,
                    Enabled = enabled,
                    BackgroundImageLayout = ImageLayout.Center,
                    Visible = true
                };

                switch (i)
                {
                    case 0:
                        b1[i].MouseClick += btnFilmeGenreAllBack_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.all_back;
                        break;

                    case 1:
                        b1[i].MouseClick += btnFilmeGenreBack_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.back;
                        break;

                    case 2:
                        b1[i].Text = this.filmeGenrePage.ToString();
                        break;

                    case 3:
                        b1[i].MouseClick += btnFilmeGenreForward_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.forward;
                        break;

                    case 4:
                        b1[i].MouseClick += btnFilmeGenreAllForward_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.all_forward;
                        break;
                }

                b1[i].FlatAppearance.BorderColor = Color.FromArgb(60, 60, 60);
                b1[i].FlatAppearance.MouseDownBackColor = Color.FromArgb(30, 30, 30);
                b1[i].FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 60, 60);


                pnlFilmeGenrePageSumary.Controls.Add(b1[i]);
            }

            #endregion

            this.filmesGenreId = id;
            this.filmesGenrePageMax = ((filmes.total_pages > 500) ? 500 : filmes.total_pages);
            pnlFilmeGenre.VerticalScroll.Value = 0;
        }

        private void hideFilmeGenre()
        {
            pnlFilmeGenre.Dock = DockStyle.None;
            pnlFilmeGenre.Enabled = false;
            pnlFilmeGenre.Visible = false;
        }

        private void btnFilmeGenreGridItem_MouseClick(object? sender, MouseEventArgs e)
        {
            var b = sender as Button;

            int i = 0;

            while (true)
            {
                if (!this.filmePreviousPage.ContainsKey(this.actualPage + "_" + i))
                {
                    this.filmePreviousPage.Add(this.actualPage + "_" + i, new string[] { this.filmeGenrePage.ToString(), this.filmesGenreId.ToString() });
                    break;
                }

                i++;
            }

            mudaTela(this.actualPage, "filme", b.Name.Split("_")[1]);
        }

        private void btnFilmeGenreAllBack_MouseClick(object? sender, MouseEventArgs e)
        {
            this.filmeGenrePage = 1;
            setFilmeGenre(this.filmesGenreId);
        }

        private void btnFilmeGenreBack_MouseClick(object? sender, MouseEventArgs e)
        {
            this.filmeGenrePage--;
            setFilmeGenre(this.filmesGenreId);
        }

        private void btnFilmeGenreForward_MouseClick(object? sender, MouseEventArgs e)
        {
            this.filmeGenrePage++;
            setFilmeGenre(this.filmesGenreId);
        }

        private void btnFilmeGenreAllForward_MouseClick(object? sender, MouseEventArgs e)
        {
            this.filmeGenrePage = this.filmesGenrePageMax;
            setFilmeGenre(this.filmesGenreId);
        }

        private void btnFilmeGenrePrevious_MouseClick(object sender, MouseEventArgs e)
        {
            mudaTela(this.actualPage, "filme", this.filmeGenrePreviousPage.Last().Value.ToString());
            this.filmeGenrePreviousPage.Remove(this.filmeGenrePreviousPage.Last().Key);
        }

        #endregion

        #region Atributos/Métodos tela Filme-Similar

        private int filmeSimilarPage = 1;
        private int filmeSimilarId;
        private int filmeSimilarPageMax;

        /// <summary>
        /// string page, int movie_id
        /// </summary>
        private Dictionary<string, int> filmeSimilarPreviousPage = new();

        private void showFilmeSimilar()
        {
            pnlFilmeSimilar.Dock = DockStyle.Fill;
            pnlFilmeSimilar.Enabled = true;
            pnlFilmeSimilar.Visible = true;
            this.actualPage = "filme-similar";
        }

        private void hideFilmeSimilar()
        {
            pnlFilmeSimilar.Dock = DockStyle.None;
            pnlFilmeSimilar.Enabled = false;
            pnlFilmeSimilar.Visible = false;
        }

        private void setFilmeSimilar(int id)
        {
            Resources.Movies.Objects.Similar.Rootobject? filmes = null;

            #region Obtendo os resultados da busca

            if (this.filmesStorage.Similar.ContainsKey(this.filmeSimilarPage + ":" + id))
            {
                foreach (var item in this.filmesStorage.Similar)
                {
                    if (item.Key == (this.filmeSimilarPage + ":" + id))
                    {
                        filmes = item.Value;
                        break;
                    }
                }
            }

            else
            {
                try
                {
                    filmes = Resources.Movies.Recover.getSimilar(id, this.filmeSimilarPage);

                    if (filmes == null || filmes.total_results == 0)
                    {
                        throw new Exception();
                    }

                    else
                    {
                        this.filmesStorage.Similar.Add(this.filmeSimilarPage + ":" + id, filmes);
                    }
                }

                catch
                {
                    MessageBox.Show("Erro de conexão!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    mudaTela(this.actualPage, "home");
                    return;
                }
            }

            #endregion

            #region Verificando os resultados obtidos

            if (filmes == null || filmes.total_results == 0)
            {
                MessageBox.Show("Erro de conexão!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                mudaTela(this.actualPage, "home");
                return;
            }

            #endregion

            #region Preenchendo o título

            foreach (var item in this.filmesStorage.Movie)
            {
                if (item.Key == id)
                {
                    lblFilmeSimilarTitle.Text = "Filmes Similares a " + item.Value.title;
                }
            }

            #endregion

            #region Preenchendo o grid

            flpFilmeSimilarGrid.Controls.Clear();

            int quant = filmes.results.Length;
            int larg = 125 + 40;
            int alt = 183 + 40;
            int containerLarg = this.Width - (68 + 20 + 113);
            int quantPerRow = (containerLarg - (containerLarg % larg)) / larg;
            int quantRows = (quant - (quant % quantPerRow)) / quantPerRow + ((quant % quantPerRow == 0) ? 0 : 1);

            flpFilmeSimilarGrid.Height = quantRows * alt + 40;

            Button[] b = new Button[quant];

            int index = 0;

            for (int i = 0; i < quantRows; i++)
            {
                for (int j = 0; j < quantPerRow; j++)
                {
                    if (index >= b.Length)
                    {
                        break;
                    }

                    b[index] = new()
                    {
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Cursor = Cursors.Hand,
                        FlatStyle = FlatStyle.Flat,
                        ForeColor = Color.White,
                        Location = new Point(113 + larg * j, 10 + alt * i),
                        Margin = new Padding(0, 20, 20, 0),
                        Name = "btnFilmeSimilarGridItem_" + filmes.results[index].id,
                        Size = new Size(larg - 20, alt - 20),
                        TabIndex = 0,
                        TextAlign = ContentAlignment.MiddleCenter,
                        UseVisualStyleBackColor = true
                    };

                    b[index].FlatAppearance.BorderColor = Color.FromArgb(60, 60, 60);
                    b[index].FlatAppearance.MouseDownBackColor = Color.FromArgb(20, 20, 20);
                    b[index].FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 30, 30);
                    b[index].MouseClick += btnFilmeSimilarGridItem_MouseClick;

                    if (this.filmesStorage.PosterImage.ContainsKey(filmes.results[index].id))
                    {
                        foreach (var item in this.filmesStorage.PosterImage)
                        {
                            if (item.Key == filmes.results[index].id)
                            {
                                b[index].BackgroundImage = item.Value;
                                break;
                            }
                        }
                    }

                    else
                    {
                        string url = string.Format(Resources.Movies.Configuration.imageUrlBase, Resources.Movies.Configuration.imageUrlSize, filmes.results[index].poster_path);

                        try
                        {
                            var request = WebRequest.Create(url);

                            using (var response = request.GetResponse())
                            using (var stream = response.GetResponseStream())
                            {
                                Image img = Image.FromStream(stream);

                                if (img != null)
                                {
                                    b[index].BackgroundImage = img;
                                    this.filmesStorage.PosterImage.Add(filmes.results[index].id, img);
                                }

                                else
                                {
                                    throw new Exception();
                                }
                            }
                        }

                        catch
                        {
                            b[index].Text = filmes.results[index].title;
                        }
                    }

                    flpFilmeSimilarGrid.Controls.Add(b[index]);
                    index++;
                }
            }

            #endregion

            #region Criando os botões de navegação entre páginas

            pnlFilmeSimilarPageSumary.Controls.Clear();

            Button[] b1 = new Button[5];

            for (int i = 0; i < b1.Length; i++)
            {
                bool enabled = true;

                switch (i)
                {
                    case 0:
                        if (this.filmeSimilarPage == 1)
                        {
                            enabled = false;
                        }

                        break;

                    case 1:
                        if (this.filmeSimilarPage == 1)
                        {
                            enabled = false;
                        }

                        break;

                    case 3:
                        if (this.filmeSimilarPage == ((filmes.total_pages > 500) ? 500 : filmes.total_pages))
                        {
                            enabled = false;
                        }

                        break;

                    case 4:
                        if (this.filmeSimilarPage == ((filmes.total_pages > 500) ? 500 : filmes.total_pages))
                        {
                            enabled = false;
                        }

                        break;
                }

                b1[i] = new()
                {
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point),
                    ForeColor = Color.White,
                    Location = new Point(50 * i + ((this.Width - pnlSidebar.MinimumSize.Width - 250) / 2), 0),
                    Margin = new Padding(0, 0, 10, 0),
                    Name = "btnFilmeSimilarPageSumary" + i,
                    Size = new Size(40, 40),
                    TabIndex = 7,
                    UseVisualStyleBackColor = true,
                    Enabled = enabled,
                    BackgroundImageLayout = ImageLayout.Center,
                    Visible = true
                };

                switch (i)
                {
                    case 0:
                        b1[i].MouseClick += btnFilmeSimilarAllBack_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.all_back;
                        break;

                    case 1:
                        b1[i].MouseClick += btnFilmeSimilarBack_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.back;
                        break;

                    case 2:
                        b1[i].Text = this.filmeSimilarPage.ToString();
                        break;

                    case 3:
                        b1[i].MouseClick += btnFilmeSimilarForward_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.forward;
                        break;

                    case 4:
                        b1[i].MouseClick += btnFilmeSimilarAllForward_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.all_forward;
                        break;
                }

                b1[i].FlatAppearance.BorderColor = Color.FromArgb(60, 60, 60);
                b1[i].FlatAppearance.MouseDownBackColor = Color.FromArgb(30, 30, 30);
                b1[i].FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 60, 60);

                pnlFilmeSimilarPageSumary.Controls.Add(b1[i]);
            }

            #endregion

            this.filmeSimilarId = id;
            this.filmeSimilarPageMax = ((filmes.total_pages > 500) ? 500 : filmes.total_pages);
            pnlFilmeSimilar.VerticalScroll.Value = 0;
        }

        private void btnFilmeSimilarGridItem_MouseClick(object? sender, MouseEventArgs e)
        {
            var b = sender as Button;

            int i = 0;

            while (true)
            {
                if (!this.filmePreviousPage.ContainsKey(this.actualPage + "_" + i))
                {
                    this.filmePreviousPage.Add(this.actualPage + "_" + i, new string[] { this.filmeSimilarPage.ToString(), this.filmeSimilarId.ToString() });
                    break;
                }

                i++;
            }

            mudaTela(this.actualPage, "filme", b.Name.Split("_")[1]);
        }

        private void btnFilmeSimilarAllBack_MouseClick(object? sender, MouseEventArgs e)
        {
            this.filmeSimilarPage = 1;
            setFilmeSimilar(this.filmeSimilarId);
        }

        private void btnFilmeSimilarBack_MouseClick(object? sender, MouseEventArgs e)
        {
            this.filmeSimilarPage--;
            setFilmeSimilar(this.filmeSimilarId);
        }

        private void btnFilmeSimilarForward_MouseClick(object? sender, MouseEventArgs e)
        {
            this.filmeSimilarPage++;
            setFilmeSimilar(this.filmeSimilarId);
        }

        private void btnFilmeSimilarAllForward_MouseClick(object? sender, MouseEventArgs e)
        {
            this.filmeSimilarPage = this.filmeSimilarPageMax;
            setFilmeSimilar(this.filmeSimilarId);
        }

        private void btnFilmeSimilarPrevious_MouseClick(object sender, MouseEventArgs e)
        {
            mudaTela(this.actualPage, "filme", this.filmeSimilarPreviousPage.Last().Value.ToString());
            this.filmeSimilarPreviousPage.Remove(this.filmeSimilarPreviousPage.Last().Key);
        }

        #endregion

        #region Atributos/Métodos tela Serie

        /// <summary>
        /// string page, string[] args
        /// </summary>
        private Dictionary<string, string[]> seriePreviousPage = new();
        private int serieId;

        private void showSerie()
        {
            pnlSerie.Dock = DockStyle.Fill;
            pnlSerie.Enabled = true;
            pnlSerie.Visible = true;
            this.actualPage = "serie";
        }

        private void hideSerie()
        {
            pnlSerie.Dock = DockStyle.None;
            pnlSerie.Enabled = false;
            pnlSerie.Visible = false;
        }

        private void setSerie(int id)
        {
            Resources.Tv_Shows.Objects.TvShow.Rootobject? serie = null;

            lblSerieTitle.Width = this.Width - lblSerieTitle.Location.X - 48;
            lblSerieDescricao.Width = lblSerieTitle.Width;

            if (id == -1)
            {
                pbSerieBanner.Height = (int)((this.Width - pnlSidebar.MinimumSize.Width) / 1.777777776);
                pnlSerie.VerticalScroll.Value = 0;
                return;
            }

            #region Obtendo os dados da série

            if (this.seriesStorage.TvShow.ContainsKey(id))
            {
                foreach (var item in this.seriesStorage.TvShow)
                {
                    if (item.Key == id)
                    {
                        serie = item.Value;
                        break;
                    }
                }
            }

            else
            {
                serie = Resources.Tv_Shows.Recover.getSerie(id);

                if (serie != null)
                {
                    this.seriesStorage.TvShow.Add(id, serie);
                }
            }

            #endregion

            #region Verificando os dados recebidos

            if (serie == null)
            {
                MessageBox.Show("Erro de conexão!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                mudaTela(this.actualPage, "home");
                return;
            }

            #endregion

            #region Preenchendo os campos de imagem

            pbSerieBanner.Height = (int)((this.Width - pnlSidebar.MinimumSize.Width) / 1.777777776);
            pbSeriePoster.Controls.Clear();
            pbSerieBanner.Controls.RemoveByKey("lblSerieBanner");

            if (this.seriesStorage.PosterImage.ContainsKey(id))
            {
                foreach (var item in this.seriesStorage.PosterImage)
                {
                    if (item.Key == id)
                    {
                        pbSeriePoster.BackgroundImage = item.Value;
                        break;
                    }
                }
            }

            else
            {
                string url = string.Format(Resources.Tv_Shows.Configuration.imageUrlBase, Resources.Tv_Shows.Configuration.imageUrlSize, serie.poster_path);

                try
                {
                    var request = WebRequest.Create(url);

                    using (var response = request.GetResponse())
                    using (var stream = response.GetResponseStream())
                    {
                        Image img = Image.FromStream(stream);

                        if (img != null)
                        {
                            pbSeriePoster.BackgroundImage = img;
                            this.seriesStorage.PosterImage.Add(id, img);
                        }

                        else
                        {
                            throw new Exception();
                        }
                    }
                }

                catch
                {
                    pbSeriePoster.BackgroundImage = null;

                    pbSeriePoster.Controls.Add(new Label()
                    {
                        AutoSize = false,
                        Font = lblSerieDescricao.Font,
                        ForeColor = Color.White,
                        Location = new Point(0, 0),
                        Name = "lblSeriePoster",
                        Size = pbSeriePoster.Size,
                        Text = serie.name,
                        TextAlign = ContentAlignment.MiddleCenter
                    });
                }
            }

            if (this.seriesStorage.BannerImage.ContainsKey(id))
            {
                foreach (var item in this.seriesStorage.BannerImage)
                {
                    if (item.Key == id)
                    {
                        pbSerieBanner.BackgroundImage = item.Value;
                        break;
                    }
                }
            }

            else
            {
                string url = string.Format(Resources.Tv_Shows.Configuration.imageUrlBase, Resources.Tv_Shows.Configuration.imageUrlSize, serie.backdrop_path);

                try
                {
                    var request = WebRequest.Create(url);

                    using (var response = request.GetResponse())
                    using (var stream = response.GetResponseStream())
                    {
                        Image img = Image.FromStream(stream);

                        if (img != null)
                        {
                            pbSerieBanner.BackgroundImage = img;
                            this.seriesStorage.BannerImage.Add(id, img);
                        }

                        else
                        {
                            throw new Exception();
                        }
                    }
                }

                catch
                {
                    pbSerieBanner.BackgroundImage = null;

                    pbSerieBanner.Controls.Add(new Label()
                    {
                        AutoSize = false,
                        Font = lblSerieTitle.Font,
                        ForeColor = Color.White,
                        Location = new Point(pnlSidebar.MinimumSize.Width, 0),
                        Name = "lblSerieBanner",
                        Size = new Size(this.Width - pnlSidebar.MinimumSize.Width, pbSerieBanner.Height),
                        Text = serie.name,
                        TextAlign = ContentAlignment.MiddleCenter
                    });
                }
            }

            #endregion

            #region Preenchendo os campos de texto

            lblSerieTitle.Text = serie.name;
            lblSerieDescricao.Text = serie.overview;
            var date = serie.first_air_date.Split("-") ?? new string[] { "--", "--", "--" };
            lblSerieContentDataLancValue.Text = date[1] + "/" + date[0] + "/" + date[2];
            lblSerieContentNumEpsValue.Text = serie.number_of_episodes.ToString();
            lblSerieContentNumTempValue.Text = serie.number_of_seasons.ToString();

            try
            {
                lblSerieContentDuracaoValue.Text = serie.episode_run_time[0].ToString() + " minutos";
            }

            catch
            {
                lblSerieContentDuracaoValue.Text = "-- minutos";
            }

            #endregion

            #region Preenchendo o grid de gêneros

            flpSerieContentGenreGrid.Controls.Clear();

            Button[] card = new Button[serie.genres.Length];

            for (int i = 0; i < card.Length; i++)
            {
                card[i] = new()
                {
                    BackColor = Color.FromArgb(10, 10, 10),
                    FlatStyle = FlatStyle.Flat,
                    ForeColor = Color.White,
                    Location = new Point(130 * i, 0),
                    Margin = new Padding(0, 0, 10, 0),
                    Name = "btnSerieContentGenreItem" + i,
                    Size = new Size(120, 24),
                    TabIndex = 0,
                    Text = serie.genres[i].name,
                    TextAlign = ContentAlignment.MiddleCenter
                };

                card[i].FlatAppearance.BorderSize = 1;
                card[i].FlatAppearance.BorderColor = Color.FromArgb(60, 60, 60);
                card[i].FlatAppearance.MouseDownBackColor = Color.FromArgb(30, 30, 30);
                card[i].FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 60, 60);
                card[i].MouseClick += btnSerieContentGenreGrid_MouseClick;

                flpSerieContentGenreGrid.Controls.Add(card[i]);
            }

            #endregion

            #region Ajustando o botão de favorito

            var result = c.Read(FrmMain.getConnection(), "serie", "serie_id", serie.id) ?? Array.Empty<object[]>();
            btnSerieContentFav.Text = (result.Length > 0) ? "      Remover dos Favoritos" : "      Adicionar aos Favoritos";

            #endregion

            this.serieId = serie.id;
            pnlSerie.VerticalScroll.Value = 0;
        }

        private void btnSerieContentGenreGrid_MouseClick(object? sender, MouseEventArgs e)
        {
            var b = sender as Button;

            int i = 0;

            foreach (var item in this.seriesGenres.genres)
            {
                if (item.name == b.Text)
                {
                    while (true)
                    {
                        if (!this.serieGenrePreviousPage.ContainsKey(this.actualPage + "_" + i))
                        {
                            this.serieGenrePreviousPage.Add(this.actualPage + "_" + i, this.serieId);
                            break;
                        }

                        i++;
                    }

                    this.serieGenrePage = 1;
                    mudaTela(this.actualPage, "serie-genre", item.id.ToString());
                    break;
                }
            }
        }

        private void btnSeriePrevious_MouseClick(object sender, MouseEventArgs e)
        {
            string arg = string.Empty, arg1 = string.Empty;

            switch (this.seriePreviousPage.Last().Key.Split("_")[0])
            {
                case "home":
                    //Home don't need any arguments
                    break;

                case "serie-buscar":
                    this.serieBuscarPage = int.Parse(this.seriePreviousPage.Last().Value[0]);
                    arg = this.seriePreviousPage.Last().Value[1];
                    arg1 = "previous";
                    break;

                case "serie-popular":
                    this.seriePopularPage = int.Parse(this.seriePreviousPage.Last().Value[0]);
                    arg = "Previous";
                    break;

                case "serie-genre":
                    this.serieGenrePage = int.Parse(this.seriePreviousPage.Last().Value[0]);
                    arg = this.seriePreviousPage.Last().Value[1];
                    break;

                case "serie-similar":
                    this.serieSimilarPage = int.Parse(this.seriePreviousPage.Last().Value[0]);
                    arg = this.seriePreviousPage.Last().Value[1];
                    break;

                case "fav-serie":
                    //Favorito-Serie don't need any arguments
                    break;
            }

            mudaTela(this.actualPage, this.seriePreviousPage.Last().Key.Split("_")[0], arg, arg1);

            if (this.seriePreviousPage.Count > 0)
            {
                this.seriePreviousPage.Remove(this.seriePreviousPage.Last().Key);
            }
        }

        private void btnSerieContentFav_MouseClick(object sender, MouseEventArgs e)
        {

            var result = c.Read(FrmMain.getConnection(), "serie", "serie_id", this.serieId) ?? Array.Empty<object[]>();

            if (result.Length > 0)
            {
                if (c.Delete(FrmMain.getConnection(), "serie", Convert.ToInt32(result[0][0]))) btnSerieContentFav.Text = "      Adicionar aos Favoritos";
                else MessageBox.Show("Falha ao Remover dos Favoritos", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {
                if (c.Create(FrmMain.getConnection(), "serie", Convert.ToInt32(FrmMain.Userid), this.serieId)) btnSerieContentFav.Text = "      Remover dos Favoritos";
                else MessageBox.Show("Falha ao Adicionar aos Favoritos", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSerieContentSimilar_MouseClick(object sender, MouseEventArgs e)
        {
            var i = 0;

            while (true)
            {
                if (!serieSimilarPreviousPage.ContainsKey(this.actualPage + "_" + i))
                {
                    serieSimilarPreviousPage.Add(this.actualPage + "_" + i, this.serieId);
                    break;
                }

                i++;
            }

            this.serieSimilarPage = 1;
            mudaTela(this.actualPage, "serie-similar", this.serieId.ToString());

        }

        private void btnSerieContentHomepage_MouseClick(object sender, MouseEventArgs e)
        {
            Resources.Tv_Shows.Objects.TvShow.Rootobject? serie = null;

            try
            {
                foreach (var item in this.seriesStorage.TvShow)
                {
                    if (item.Key == this.serieId)
                    {
                        serie = item.Value;
                        break;
                    }
                }

                System.Diagnostics.Process.Start("explorer", (serie.homepage ?? "https://www.google.com/search?q=" + serie.name));
            }

            catch
            {
                MessageBox.Show("Erro ao carregar a página!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Atributos/Métodos tela Series-Buscar

        private Resources.Tv_Shows.Objects.Search.Rootobject? serieBuscarShow = null;
        private string serieBuscarQry;
        private int serieBuscarMaxPage;
        private int serieBuscarPage = 1;

        private void showSerieBuscar()
        {
            pnlBuscarSerie.Dock = DockStyle.Fill;
            pnlBuscarSerie.Visible = true;
            pnlBuscarSerie.Enabled = true;
            this.actualPage = "serie-buscar";
            btnSeriesBuscar.BackColor = Color.FromArgb(sidebarSelectedColor, sidebarSelectedColor, sidebarSelectedColor);
        }

        private void hideSerieBuscar()
        {
            pnlBuscarSerie.Dock = DockStyle.None;
            pnlBuscarSerie.Visible = false;
            pnlBuscarSerie.Enabled = false;
            btnSeriesBuscar.BackColor = Color.FromArgb(15, 15, 15);
        }

        private void resetSerieBuscar()
        {
            this.serieBuscarShow = null;
            this.serieBuscarPage = 1;
            flpBuscarSerieGrid.Controls.Clear();
            pnlBuscarSerieGrid.Height = 200;
            pnlBuscarSerieGrid.VerticalScroll.Value = 0;
            pnlBuscarSeriePageSumary.Controls.Clear();
            tbBuscarSerieQuery.Text = string.Empty;
        }

        private void resetSerieBuscar(params string[] args)
        {
            tbBuscarSerieQuery.Text = args[2];
        }

        private void setSerieBuscar(string qry)
        {
            Resources.Tv_Shows.Objects.Search.Rootobject? serie = null;

            if (qry == string.Empty)
            {
                if (this.serieBuscarShow == null)
                {
                    return;
                }

                else
                {
                    serie = this.serieBuscarShow;
                }
            }

            #region Realizando a busca

            else
            {
                if (this.seriesStorage.Search.ContainsKey(qry + ":" + this.serieBuscarPage))
                {
                    foreach (KeyValuePair<string, Resources.Tv_Shows.Objects.Search.Rootobject> item in this.seriesStorage.Search)
                    {
                        string query = item.Key.Split(":")[0];
                        int page = int.Parse(item.Key.Split(":")[1]);

                        if (page == this.serieBuscarPage && qry == query)
                        {
                            serie = item.Value;
                            break;
                        }
                    }
                }

                else
                {
                    try
                    {
                        serie = Resources.Tv_Shows.Recover.getSearch(qry, this.serieBuscarPage);

                        if (serie != null)
                        {
                            this.seriesStorage.Search.Add(qry + ":" + this.serieBuscarPage, serie);
                        }

                        else
                        {
                            throw new WebException();
                        }
                    }

                    catch (Exception e)
                    {
                        if (e is WebException we)
                        {
                            MessageBox.Show(we.Status.ToString());
                        }

                        else
                        {
                            MessageBox.Show(e.Message);
                        }
                    }
                }
            }

            #endregion

            #region Verificando os resultados da busca

            if (serie == null)
            {
                pnlBuscarSerieGrid.Controls.Clear();
                pnlBuscarSeriePageSumary.Controls.Clear();
                return;
            }

            if (serie.total_results <= 0)
            {
                MessageBox.Show("Série não encontrada", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            #endregion

            #region Preenchendo o grid com os resultados da busca

            int larg = 125 + 20;
            int alt = 183 + 20;
            int containerLarg = this.Width - (68 + 20);
            int quantPerRow = (containerLarg - (containerLarg % larg)) / larg;
            int quantRows = (serie.results.Length - (serie.results.Length % quantPerRow)) / quantPerRow + ((serie.results.Length % quantPerRow == 0) ? 0 : 1);

            flpBuscarSerieGrid.Controls.Clear();
            pnlBuscarSerieGrid.Height = quantRows * alt + 60;

            Button[] b = new Button[serie.results.Length];

            int index = 0;

            for (int i = 0; i < quantRows; i++)
            {
                for (int j = 0; j < quantPerRow; j++)
                {
                    if (index >= b.Length)
                    {
                        break;
                    }

                    b[index] = new()
                    {
                        BackgroundImageLayout = ImageLayout.Stretch,
                        FlatStyle = FlatStyle.Flat,
                        Margin = new Padding(10),
                        Name = "btnBuscarSerieGridShow_" + serie.results[index].id,
                        Size = new Size(125, 183),
                        TabIndex = 0,
                        UseVisualStyleBackColor = true,
                        Location = new Point(128 + larg * j, 20 + alt * i),
                        ForeColor = Color.White
                    };

                    b[index].MouseClick += btnSerieBuscarGridShow_MouseClick;
                    b[index].FlatAppearance.BorderColor = Color.FromArgb(30, 30, 30);
                    b[index].Cursor = Cursors.Hand;

                    if (serie.results[index].poster_path == null)
                    {
                        b[index].Text = serie.results[i].name;
                    }

                    else
                    {
                        if (this.seriesStorage.PosterImage.ContainsKey(serie.results[index].id))
                        {
                            foreach (KeyValuePair<int, Image> item in this.seriesStorage.PosterImage)
                            {
                                if (item.Key == serie.results[index].id)
                                {
                                    b[index].BackgroundImage = item.Value;
                                    break;
                                }
                            }
                        }

                        else
                        {
                            var request = WebRequest.Create(string.Format(Resources.Tv_Shows.Configuration.imageUrlBase, Resources.Tv_Shows.Configuration.imageUrlSize, serie.results[index].poster_path));

                            using (var response = request.GetResponse())
                            using (var stream = response.GetResponseStream())
                            {
                                Image image = Bitmap.FromStream(stream);
                                b[index].BackgroundImage = image;

                                if (stream != null)
                                {
                                    this.seriesStorage.PosterImage.Add(serie.results[index].id, image);
                                }
                            }
                        }
                    }

                    flpBuscarSerieGrid.Controls.Add(b[index]);

                    index++;
                }
            }

            #endregion

            #region Criando os botões de navegação entre páginas

            pnlBuscarSeriePageSumary.Controls.Clear();

            Button[] b1 = new Button[5];

            for (int i = 0; i < b1.Length; i++)
            {
                bool enabled = true;

                switch (i)
                {
                    case 0:
                        if (this.serieBuscarPage == 1)
                        {
                            enabled = false;
                        }

                        break;

                    case 1:
                        if (this.serieBuscarPage == 1)
                        {
                            enabled = false;
                        }

                        break;

                    case 3:
                        if (this.serieBuscarPage == serie.total_pages)
                        {
                            enabled = false;
                        }

                        break;

                    case 4:
                        if (this.serieBuscarPage == serie.total_pages)
                        {
                            enabled = false;
                        }

                        break;
                }

                b1[i] = new()
                {
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point),
                    ForeColor = Color.White,
                    Location = new Point(50 * i + ((this.Width - pnlSidebar.MinimumSize.Width - 250) / 2), 0),
                    Margin = new Padding(0, 0, 10, 0),
                    Name = "btnBuscarSeriePageSumary" + i,
                    Size = new Size(40, 40),
                    TabIndex = 7,
                    UseVisualStyleBackColor = true,
                    Enabled = enabled,
                    BackgroundImageLayout = ImageLayout.Center,
                    Visible = true
                };

                switch (i)
                {
                    case 0:
                        b1[i].MouseClick += btnSerieBuscarAllBack_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.all_back;
                        break;

                    case 1:
                        b1[i].MouseClick += btnSerieBuscarBack_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.back;
                        break;

                    case 2:
                        b1[i].Text = this.serieBuscarPage.ToString();
                        break;

                    case 3:
                        b1[i].MouseClick += btnSerieBuscarForward_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.forward;
                        break;

                    case 4:
                        b1[i].MouseClick += btnSerieBuscarAllForward_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.all_forward;
                        break;
                }

                b1[i].FlatAppearance.BorderColor = Color.FromArgb(60, 60, 60);
                b1[i].FlatAppearance.MouseDownBackColor = Color.FromArgb(30, 30, 30);
                b1[i].FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 60, 60);

                pnlBuscarSeriePageSumary.Controls.Add(b1[i]);
            }

            #endregion

            this.serieBuscarShow = serie;
            this.serieBuscarQry = qry;
            this.serieBuscarMaxPage = serie.total_pages;
            pnlBuscarSerie.VerticalScroll.Value = 0;
        }

        private void btnSerieBuscarAllBack_MouseClick(object? sender, MouseEventArgs e)
        {
            this.serieBuscarPage = 1;
            setSerieBuscar(this.serieBuscarQry);
            pnlBuscarSerie.VerticalScroll.Value = 0;
        }

        private void btnSerieBuscarBack_MouseClick(object? sender, MouseEventArgs e)
        {
            this.serieBuscarPage -= 1;
            setSerieBuscar(this.serieBuscarQry);
            pnlBuscarSerie.VerticalScroll.Value = 0;
        }

        private void btnSerieBuscarAllForward_MouseClick(object? sender, MouseEventArgs e)
        {
            this.serieBuscarPage = this.serieBuscarMaxPage;
            setSerieBuscar(this.serieBuscarQry);
            pnlBuscarSerie.VerticalScroll.Value = 0;
        }

        private void btnSerieBuscarForward_MouseClick(object? sender, MouseEventArgs e)
        {
            this.serieBuscarPage += 1;
            setSerieBuscar(this.serieBuscarQry);
            pnlBuscarSerie.VerticalScroll.Value = 0;
        }

        private void btnSerieBuscarGridShow_MouseClick(object? sender, MouseEventArgs e)
        {
            var b = sender as Button;

            int i = 0;

            while (true)
            {
                if (!this.seriePreviousPage.ContainsKey(this.actualPage + "_" + i))
                {
                    this.seriePreviousPage.Add(this.actualPage + "_" + i, new string[] { this.serieBuscarPage.ToString(), this.serieBuscarQry });
                    break;
                }

                i++;
            }

            mudaTela(this.actualPage, "serie", b.Name.Split("_")[1]);
        }

        private void btnBuscarSerieSubmit_MouseClick(object sender, MouseEventArgs e)
        {
            if (tbBuscarSerieQuery.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Insira uma Série!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            setSerieBuscar(tbBuscarSerieQuery.Text);
            pnlBuscarSerie.VerticalScroll.Value = 0;
        }

        #endregion

        #region Atributos/Métodos tela Serie-Popular

        private int seriePopularPage = 1;
        private int seriePopularPageMax;

        private void showSeriePopular()
        {
            pnlSeriePopular.Dock = DockStyle.Fill;
            pnlSeriePopular.Enabled = true;
            pnlSeriePopular.Visible = true;
            this.actualPage = "serie-popular";
            btnSeriesPopular.BackColor = Color.FromArgb(sidebarSelectedColor, sidebarSelectedColor, sidebarSelectedColor);
        }

        private void hideSeriePopular()
        {
            pnlSeriePopular.Dock = DockStyle.None;
            pnlSeriePopular.Enabled = false;
            pnlSeriePopular.Visible = false;
            btnSeriesPopular.BackColor = Color.FromArgb(15, 15, 15);
        }

        private void setSeriePopular()
        {
            Resources.Tv_Shows.Objects.Popular.Rootobject? serie = null;

            #region Realizando a busca

            if (this.seriesStorage.Popular.ContainsKey(this.seriePopularPage))
            {
                foreach (var item in this.seriesStorage.Popular)
                {
                    if (item.Key == this.seriePopularPage)
                    {
                        serie = item.Value;
                        break;
                    }
                }
            }

            else
            {
                try
                {
                    serie = Resources.Tv_Shows.Recover.getPopular(this.seriePopularPage);

                    if (serie != null)
                    {
                        this.seriesStorage.Popular.Add(this.seriePopularPage, serie);
                    }
                }

                catch
                {
                    serie = null;
                }
            }

            #endregion

            #region Verificando os resultados recebidos

            if (serie == null || serie.total_results == 0)
            {
                MessageBox.Show("Erro de conexão!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                mudaTela(this.actualPage, "home");
                return;
            }

            #endregion

            #region Preenchendo o grid

            flpSeriePopularGrid.Controls.Clear();

            int quant = serie.results.Length;
            int larg = 125 + 40;
            int alt = 183 + 40;
            int containerLarg = this.Width - (68 + 20 + 113);
            int quantPerRow = (containerLarg - (containerLarg % larg)) / larg;
            int quantRows = (quant - (quant % quantPerRow)) / quantPerRow + ((quant % quantPerRow == 0) ? 0 : 1);

            flpSeriePopularGrid.Height = quantRows * alt + 40;

            Button[] b = new Button[quant];

            int index = 0;

            for (int i = 0; i < quantRows; i++)
            {
                for (int j = 0; j < quantPerRow; j++)
                {
                    if (index >= b.Length)
                    {
                        break;
                    }

                    b[index] = new()
                    {
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Cursor = Cursors.Hand,
                        FlatStyle = FlatStyle.Flat,
                        ForeColor = Color.White,
                        Location = new Point(113 + larg * j, 10 + alt * i),
                        Margin = new Padding(0, 20, 20, 0),
                        Name = "btnSeriePopularGridItem_" + serie.results[index].id,
                        Size = new Size(larg - 20, alt - 20),
                        TabIndex = 0,
                        TextAlign = ContentAlignment.MiddleCenter,
                        UseVisualStyleBackColor = true
                    };

                    b[index].FlatAppearance.BorderColor = Color.FromArgb(60, 60, 60);
                    b[index].FlatAppearance.MouseDownBackColor = Color.FromArgb(20, 20, 20);
                    b[index].FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 30, 30);
                    b[index].MouseClick += btnSeriePopularGridItem_MouseClick;

                    if (this.seriesStorage.PosterImage.ContainsKey(serie.results[index].id))
                    {
                        foreach (var item in this.seriesStorage.PosterImage)
                        {
                            if (item.Key == serie.results[index].id)
                            {
                                b[index].BackgroundImage = item.Value;
                                break;
                            }
                        }
                    }

                    else
                    {
                        string url = string.Format(Resources.Tv_Shows.Configuration.imageUrlBase, Resources.Tv_Shows.Configuration.imageUrlSize, serie.results[index].poster_path);

                        try
                        {
                            var request = WebRequest.Create(url);

                            using (var response = request.GetResponse())
                            using (var stream = response.GetResponseStream())
                            {
                                Image img = Image.FromStream(stream);

                                if (img != null)
                                {
                                    b[index].BackgroundImage = img;
                                    this.seriesStorage.PosterImage.Add(serie.results[index].id, img);
                                }

                                else
                                {
                                    throw new Exception();
                                }
                            }
                        }

                        catch
                        {
                            b[index].Text = serie.results[index].name;
                        }
                    }

                    flpSeriePopularGrid.Controls.Add(b[index]);
                    index++;
                }
            }

            #endregion

            #region Criando os botões de navegação entre páginas

            pnlSeriePopularPageSumary.Controls.Clear();

            Button[] b1 = new Button[5];

            for (int i = 0; i < b1.Length; i++)
            {
                bool enabled = true;

                switch (i)
                {
                    case 0:
                        if (this.seriePopularPage == 1)
                        {
                            enabled = false;
                        }

                        break;

                    case 1:
                        if (this.seriePopularPage == 1)
                        {
                            enabled = false;
                        }

                        break;

                    case 3:
                        if (this.seriePopularPage == ((serie.total_pages > 500) ? 500 : serie.total_pages))
                        {
                            enabled = false;
                        }

                        break;

                    case 4:
                        if (this.seriePopularPage == ((serie.total_pages > 500) ? 500 : serie.total_pages))
                        {
                            enabled = false;
                        }

                        break;
                }

                b1[i] = new()
                {
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point),
                    ForeColor = Color.White,
                    Location = new Point(50 * i + ((this.Width - pnlSidebar.MinimumSize.Width - 250) / 2), 0),
                    Margin = new Padding(0, 0, 10, 0),
                    Name = "btnSeriePopularPageSumary" + i,
                    Size = new Size(40, 40),
                    TabIndex = 7,
                    UseVisualStyleBackColor = true,
                    Enabled = enabled,
                    BackgroundImageLayout = ImageLayout.Center,
                    Visible = true
                };

                switch (i)
                {
                    case 0:
                        b1[i].MouseClick += btnSeriePopularAllBack_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.all_back;
                        break;

                    case 1:
                        b1[i].MouseClick += btnSeriePopularBack_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.back;
                        break;

                    case 2:
                        b1[i].Text = this.seriePopularPage.ToString();
                        break;

                    case 3:
                        b1[i].MouseClick += btnSeriePopularForward_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.forward;
                        break;

                    case 4:
                        b1[i].MouseClick += btnSeriePopularAllForward_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.all_forward;
                        break;
                }

                b1[i].FlatAppearance.BorderColor = Color.FromArgb(60, 60, 60);
                b1[i].FlatAppearance.MouseDownBackColor = Color.FromArgb(30, 30, 30);
                b1[i].FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 60, 60);

                pnlSeriePopularPageSumary.Controls.Add(b1[i]);
            }

            #endregion

            this.seriePopularPageMax = (serie.total_pages > 500) ? 500 : serie.total_pages;
            pnlSeriePopular.VerticalScroll.Value = 0;
        }

        private void resetSeriePopular()
        {
            this.seriePopularPage = 1;
            flpSeriePopularGrid.Height = 200;
        }

        private void btnSeriePopularGridItem_MouseClick(object? sender, MouseEventArgs e)
        {
            var b = sender as Button;

            int i = 0;

            while (true)
            {
                if (!this.seriePreviousPage.ContainsKey(this.actualPage + "_" + i))
                {
                    this.seriePreviousPage.Add(this.actualPage + "_" + i, new string[] { this.seriePopularPage.ToString() });
                    break;
                }

                i++;
            }

            mudaTela(this.actualPage, "serie", b.Name.Split("_")[1]);
        }

        private void btnSeriePopularAllBack_MouseClick(object? sender, MouseEventArgs e)
        {
            this.seriePopularPage = 1;
            setSeriePopular();
        }

        private void btnSeriePopularBack_MouseClick(object? sender, MouseEventArgs e)
        {
            this.seriePopularPage--;
            setSeriePopular();
        }

        private void btnSeriePopularForward_MouseClick(object? sender, MouseEventArgs e)
        {
            this.seriePopularPage++;
            setSeriePopular();
        }

        private void btnSeriePopularAllForward_MouseClick(object? sender, MouseEventArgs e)
        {
            this.seriePopularPage = this.seriePopularPageMax;
            setSeriePopular();
        }

        #endregion

        #region Atributos/Métodos tela Serie-Genre

        private int serieGenrePage = 1;
        private int serieGenreId;
        private int serieGenrePageMax;

        /// <summary>
        /// string page, int tv_show_id
        /// </summary>
        private Dictionary<string, int> serieGenrePreviousPage = new();

        private void showSerieGenre()
        {
            pnlSerieGenre.Dock = DockStyle.Fill;
            pnlSerieGenre.Enabled = true;
            pnlSerieGenre.Visible = true;
            this.actualPage = "serie-genre";
        }

        private void setSerieGenre(int id)
        {
            Resources.Tv_Shows.Objects.Genre.Rootobject? series = null;

            #region Obtendo os resultados da busca

            if (this.seriesStorage.Genre.ContainsKey(this.serieGenrePage + ":" + id))
            {
                foreach (var item in this.seriesStorage.Genre)
                {
                    if (item.Key == (this.serieGenrePage + ":" + id))
                    {
                        series = item.Value;
                        break;
                    }
                }
            }

            else
            {
                try
                {
                    series = Resources.Tv_Shows.Recover.getGenre(this.serieGenrePage, id);

                    if (series == null || series.total_results == 0)
                    {
                        throw new Exception();
                    }

                    else
                    {
                        this.seriesStorage.Genre.Add(this.serieGenrePage + ":" + id, series);
                    }
                }

                catch
                {
                    MessageBox.Show("Erro de conexão!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            #endregion

            #region Verificando os resultados obtidos

            if (series == null || series.total_results == 0)
            {
                MessageBox.Show("Erro de conexão!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            #endregion

            #region Preenchendo o título

            foreach (var item in this.seriesGenres.genres)
            {
                if (item.id == id)
                {
                    lblSerieGenreTitle.Text = "Mais séries do gênero " + item.name.ToLower();
                }
            }

            #endregion

            #region Preenchendo o grid

            flpSerieGenreGrid.Controls.Clear();

            int quant = series.results.Length;
            int larg = 125 + 40;
            int alt = 183 + 40;
            int containerLarg = this.Width - (68 + 20 + 113);
            int quantPerRow = (containerLarg - (containerLarg % larg)) / larg;
            int quantRows = (quant - (quant % quantPerRow)) / quantPerRow + ((quant % quantPerRow == 0) ? 0 : 1);

            flpSerieGenreGrid.Height = quantRows * alt + 40;

            Button[] b = new Button[quant];

            int index = 0;

            for (int i = 0; i < quantRows; i++)
            {
                for (int j = 0; j < quantPerRow; j++)
                {
                    if (index >= b.Length)
                    {
                        break;
                    }

                    b[index] = new()
                    {
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Cursor = Cursors.Hand,
                        FlatStyle = FlatStyle.Flat,
                        ForeColor = Color.White,
                        Location = new Point(113 + larg * j, 10 + alt * i),
                        Margin = new Padding(0, 20, 20, 0),
                        Name = "btnSerieGenreGridItem_" + series.results[index].id,
                        Size = new Size(larg - 20, alt - 20),
                        TabIndex = 0,
                        TextAlign = ContentAlignment.MiddleCenter,
                        UseVisualStyleBackColor = true
                    };

                    b[index].FlatAppearance.BorderColor = Color.FromArgb(60, 60, 60);
                    b[index].FlatAppearance.MouseDownBackColor = Color.FromArgb(20, 20, 20);
                    b[index].FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 30, 30);
                    b[index].MouseClick += btnSerieGenreGridItem_MouseClick;

                    if (this.seriesStorage.PosterImage.ContainsKey(series.results[index].id))
                    {
                        foreach (var item in this.seriesStorage.PosterImage)
                        {
                            if (item.Key == series.results[index].id)
                            {
                                b[index].BackgroundImage = item.Value;
                                break;
                            }
                        }
                    }

                    else
                    {
                        string url = string.Format(Resources.Tv_Shows.Configuration.imageUrlBase, Resources.Tv_Shows.Configuration.imageUrlSize, series.results[index].poster_path);

                        try
                        {
                            var request = WebRequest.Create(url);

                            using (var response = request.GetResponse())
                            using (var stream = response.GetResponseStream())
                            {
                                Image img = Image.FromStream(stream);

                                if (img != null)
                                {
                                    b[index].BackgroundImage = img;
                                    this.seriesStorage.PosterImage.Add(series.results[index].id, img);
                                }

                                else
                                {
                                    throw new Exception();
                                }
                            }
                        }

                        catch
                        {
                            b[index].Text = series.results[index].name;
                        }
                    }

                    flpSerieGenreGrid.Controls.Add(b[index]);
                    index++;
                }
            }

            #endregion

            #region Criando os botões de navegação entre páginas

            pnlSerieGenrePageSumary.Controls.Clear();

            Button[] b1 = new Button[5];

            for (int i = 0; i < b1.Length; i++)
            {
                bool enabled = true;

                switch (i)
                {
                    case 0:
                        if (this.serieGenrePage == 1)
                        {
                            enabled = false;
                        }

                        break;

                    case 1:
                        if (this.serieGenrePage == 1)
                        {
                            enabled = false;
                        }

                        break;

                    case 3:
                        if (this.serieGenrePage == ((series.total_pages > 500) ? 500 : series.total_pages))
                        {
                            enabled = false;
                        }

                        break;

                    case 4:
                        if (this.serieGenrePage == ((series.total_pages > 500) ? 500 : series.total_pages))
                        {
                            enabled = false;
                        }

                        break;
                }

                b1[i] = new()
                {
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point),
                    ForeColor = Color.White,
                    Location = new Point(50 * i + ((this.Width - pnlSidebar.MinimumSize.Width - 250) / 2), 0),
                    Margin = new Padding(0, 0, 10, 0),
                    Name = "btnSerieGenrePageSumary" + i,
                    Size = new Size(40, 40),
                    TabIndex = 7,
                    UseVisualStyleBackColor = true,
                    Enabled = enabled,
                    BackgroundImageLayout = ImageLayout.Center,
                    Visible = true
                };

                switch (i)
                {
                    case 0:
                        b1[i].MouseClick += btnSerieGenreAllBack_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.all_back;
                        break;

                    case 1:
                        b1[i].MouseClick += btnSerieGenreBack_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.back;
                        break;

                    case 2:
                        b1[i].Text = this.serieGenrePage.ToString();
                        break;

                    case 3:
                        b1[i].MouseClick += btnSerieGenreForward_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.forward;
                        break;

                    case 4:
                        b1[i].MouseClick += btnSerieGenreAllForward_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.all_forward;
                        break;
                }

                b1[i].FlatAppearance.BorderColor = Color.FromArgb(60, 60, 60);
                b1[i].FlatAppearance.MouseDownBackColor = Color.FromArgb(30, 30, 30);
                b1[i].FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 60, 60);


                pnlSerieGenrePageSumary.Controls.Add(b1[i]);
            }

            #endregion

            this.serieGenreId = id;
            this.serieGenrePageMax = ((series.total_pages > 500) ? 500 : series.total_pages);
            pnlSerieGenre.VerticalScroll.Value = 0;
        }

        private void hideSerieGenre()
        {
            pnlSerieGenre.Dock = DockStyle.None;
            pnlSerieGenre.Enabled = false;
            pnlSerieGenre.Visible = false;
        }

        private void btnSerieGenreGridItem_MouseClick(object? sender, MouseEventArgs e)
        {
            var b = sender as Button;

            int i = 0;

            while (true)
            {
                if (!this.seriePreviousPage.ContainsKey(this.actualPage + "_" + i))
                {
                    this.seriePreviousPage.Add(this.actualPage + "_" + i, new string[] { this.serieGenrePage.ToString(), this.serieGenreId.ToString() });
                    break;
                }

                i++;
            }

            mudaTela(this.actualPage, "serie", b.Name.Split("_")[1]);
        }

        private void btnSerieGenreAllBack_MouseClick(object? sender, MouseEventArgs e)
        {
            this.serieGenrePage = 1;
            setSerieGenre(this.serieGenreId);
        }

        private void btnSerieGenreBack_MouseClick(object? sender, MouseEventArgs e)
        {
            this.serieGenrePage--;
            setSerieGenre(this.serieGenreId);
        }

        private void btnSerieGenreForward_MouseClick(object? sender, MouseEventArgs e)
        {
            this.serieGenrePage++;
            setSerieGenre(this.serieGenreId);
        }

        private void btnSerieGenreAllForward_MouseClick(object? sender, MouseEventArgs e)
        {
            this.serieGenrePage = this.serieGenrePageMax;
            setSerieGenre(this.serieGenreId);
        }

        private void btnSerieGenrePrevious_MouseClick(object sender, MouseEventArgs e)
        {
            mudaTela(this.actualPage, "serie", this.serieGenrePreviousPage.Last().Value.ToString());
            this.serieGenrePreviousPage.Remove(this.serieGenrePreviousPage.Last().Key);
        }

        #endregion

        #region Atributos/Métodos tela Serie-Similar

        private int serieSimilarPage = 1;
        private int serieSimilarId;
        private int serieSimilarPageMax;

        /// <summary>
        /// string page, int serie_id
        /// </summary>
        private Dictionary<string, int> serieSimilarPreviousPage = new();

        private void showSerieSimilar()
        {
            pnlSerieSimilar.Dock = DockStyle.Fill;
            pnlSerieSimilar.Enabled = true;
            pnlSerieSimilar.Visible = true;
            this.actualPage = "serie-similar";
        }

        private void hideSerieSimilar()
        {
            pnlSerieSimilar.Dock = DockStyle.None;
            pnlSerieSimilar.Enabled = false;
            pnlSerieSimilar.Visible = false;
        }

        private void setSerieSimilar(int id)
        {
            Resources.Tv_Shows.Objects.Similar.Rootobject? series = null;

            #region Obtendo os resultados da busca

            if (this.seriesStorage.Similar.ContainsKey(this.serieSimilarPage + ":" + id))
            {
                foreach (var item in this.seriesStorage.Similar)
                {
                    if (item.Key == (this.serieSimilarPage + ":" + id))
                    {
                        series = item.Value;
                        break;
                    }
                }
            }

            else
            {
                try
                {
                    series = Resources.Tv_Shows.Recover.getSimilar(id, this.serieSimilarPage);

                    if (series == null || series.total_results == 0)
                    {
                        throw new Exception();
                    }

                    else
                    {
                        this.seriesStorage.Similar.Add(this.serieSimilarPage + ":" + id, series);
                    }
                }

                catch
                {
                    MessageBox.Show("Erro de conexão!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    mudaTela(this.actualPage, "home");
                    return;
                }
            }

            #endregion

            #region Verificando os resultados obtidos

            if (series == null || series.total_results == 0)
            {
                MessageBox.Show("Erro de conexão!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                mudaTela(this.actualPage, "home");
                return;
            }

            #endregion

            #region Preenchendo o título

            foreach (var item in this.seriesStorage.TvShow)
            {
                if (item.Key == id)
                {
                    lblSerieSimilarTitle.Text = "Séries Similares a " + item.Value.name;
                }
            }

            #endregion

            #region Preenchendo o grid

            flpSerieSimilarGrid.Controls.Clear();

            int quant = series.results.Length;
            int larg = 125 + 40;
            int alt = 183 + 40;
            int containerLarg = this.Width - (68 + 20 + 113);
            int quantPerRow = (containerLarg - (containerLarg % larg)) / larg;
            int quantRows = (quant - (quant % quantPerRow)) / quantPerRow + ((quant % quantPerRow == 0) ? 0 : 1);

            flpSerieSimilarGrid.Height = quantRows * alt + 40;

            Button[] b = new Button[quant];

            int index = 0;

            for (int i = 0; i < quantRows; i++)
            {
                for (int j = 0; j < quantPerRow; j++)
                {
                    if (index >= b.Length)
                    {
                        break;
                    }

                    b[index] = new()
                    {
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Cursor = Cursors.Hand,
                        FlatStyle = FlatStyle.Flat,
                        ForeColor = Color.White,
                        Location = new Point(113 + larg * j, 10 + alt * i),
                        Margin = new Padding(0, 20, 20, 0),
                        Name = "btnSerieSimilarGridItem_" + series.results[index].id,
                        Size = new Size(larg - 20, alt - 20),
                        TabIndex = 0,
                        TextAlign = ContentAlignment.MiddleCenter,
                        UseVisualStyleBackColor = true
                    };

                    b[index].FlatAppearance.BorderColor = Color.FromArgb(60, 60, 60);
                    b[index].FlatAppearance.MouseDownBackColor = Color.FromArgb(20, 20, 20);
                    b[index].FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 30, 30);
                    b[index].MouseClick += btnSerieSimilarGridItem_MouseClick;

                    if (this.seriesStorage.PosterImage.ContainsKey(series.results[index].id))
                    {
                        foreach (var item in this.seriesStorage.PosterImage)
                        {
                            if (item.Key == series.results[index].id)
                            {
                                b[index].BackgroundImage = item.Value;
                                break;
                            }
                        }
                    }

                    else
                    {
                        string url = string.Format(Resources.Tv_Shows.Configuration.imageUrlBase, Resources.Tv_Shows.Configuration.imageUrlSize, series.results[index].poster_path);

                        try
                        {
                            var request = WebRequest.Create(url);

                            using (var response = request.GetResponse())
                            using (var stream = response.GetResponseStream())
                            {
                                Image img = Image.FromStream(stream);

                                if (img != null)
                                {
                                    b[index].BackgroundImage = img;
                                    this.seriesStorage.PosterImage.Add(series.results[index].id, img);
                                }

                                else
                                {
                                    throw new Exception();
                                }
                            }
                        }

                        catch
                        {
                            b[index].Text = series.results[index].name;
                        }
                    }

                    flpSerieSimilarGrid.Controls.Add(b[index]);
                    index++;
                }
            }

            #endregion

            #region Criando os botões de navegação entre páginas

            pnlSerieSimilarPageSumary.Controls.Clear();

            Button[] b1 = new Button[5];

            for (int i = 0; i < b1.Length; i++)
            {
                bool enabled = true;

                switch (i)
                {
                    case 0:
                        if (this.serieSimilarPage == 1)
                        {
                            enabled = false;
                        }

                        break;

                    case 1:
                        if (this.serieSimilarPage == 1)
                        {
                            enabled = false;
                        }

                        break;

                    case 3:
                        if (this.serieSimilarPage == ((series.total_pages > 500) ? 500 : series.total_pages))
                        {
                            enabled = false;
                        }

                        break;

                    case 4:
                        if (this.serieSimilarPage == ((series.total_pages > 500) ? 500 : series.total_pages))
                        {
                            enabled = false;
                        }

                        break;
                }

                b1[i] = new()
                {
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point),
                    ForeColor = Color.White,
                    Location = new Point(50 * i + ((this.Width - pnlSidebar.MinimumSize.Width - 250) / 2), 0),
                    Margin = new Padding(0, 0, 10, 0),
                    Name = "btnSerieSimilarPageSumary" + i,
                    Size = new Size(40, 40),
                    TabIndex = 7,
                    UseVisualStyleBackColor = true,
                    Enabled = enabled,
                    BackgroundImageLayout = ImageLayout.Center,
                    Visible = true
                };

                switch (i)
                {
                    case 0:
                        b1[i].MouseClick += btnSerieSimilarAllBack_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.all_back;
                        break;

                    case 1:
                        b1[i].MouseClick += btnSerieSimilarBack_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.back;
                        break;

                    case 2:
                        b1[i].Text = this.serieSimilarPage.ToString();
                        break;

                    case 3:
                        b1[i].MouseClick += btnSerieSimilarForward_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.forward;
                        break;

                    case 4:
                        b1[i].MouseClick += btnSerieSimilarAllForward_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.all_forward;
                        break;
                }

                b1[i].FlatAppearance.BorderColor = Color.FromArgb(60, 60, 60);
                b1[i].FlatAppearance.MouseDownBackColor = Color.FromArgb(30, 30, 30);
                b1[i].FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 60, 60);

                pnlSerieSimilarPageSumary.Controls.Add(b1[i]);
            }

            #endregion

            this.serieSimilarId = id;
            this.serieSimilarPageMax = ((series.total_pages > 500) ? 500 : series.total_pages);
            pnlSerieSimilar.VerticalScroll.Value = 0;
        }

        private void btnSerieSimilarGridItem_MouseClick(object? sender, MouseEventArgs e)
        {
            var b = sender as Button;

            int i = 0;

            while (true)
            {
                if (!this.seriePreviousPage.ContainsKey(this.actualPage + "_" + i))
                {
                    this.seriePreviousPage.Add(this.actualPage + "_" + i, new string[] { this.serieSimilarPage.ToString(), this.serieSimilarId.ToString() });
                    break;
                }

                i++;
            }

            mudaTela(this.actualPage, "serie", b.Name.Split("_")[1]);
        }

        private void btnSerieSimilarAllBack_MouseClick(object? sender, MouseEventArgs e)
        {
            this.serieSimilarPage = 1;
            setSerieSimilar(this.serieSimilarId);
        }

        private void btnSerieSimilarBack_MouseClick(object? sender, MouseEventArgs e)
        {
            this.serieSimilarPage--;
            setSerieSimilar(this.serieSimilarId);
        }

        private void btnSerieSimilarForward_MouseClick(object? sender, MouseEventArgs e)
        {
            this.serieSimilarPage++;
            setSerieSimilar(this.serieSimilarId);
        }

        private void btnSerieSimilarAllForward_MouseClick(object? sender, MouseEventArgs e)
        {
            this.serieSimilarPage = this.serieSimilarPageMax;
            setSerieSimilar(this.serieSimilarId);
        }

        private void btnSerieSimilarPrevious_MouseClick(object sender, MouseEventArgs e)
        {
            mudaTela(this.actualPage, "serie", this.serieSimilarPreviousPage.Last().Value.ToString());
            this.serieSimilarPreviousPage.Remove(this.serieSimilarPreviousPage.Last().Key);
        }

        #endregion

        #region Atributos/Métodos tela Favorito-Filme

        private int favoritoFilmePage = 1;
        private int favoritoFilmePageMax;

        private void showFavoritoFilme()
        {
            pnlFavoritoFilme.Dock = DockStyle.Fill;
            pnlFavoritoFilme.Enabled = true;
            pnlFavoritoFilme.Visible = true;
            btnFavFilmes.BackColor = Color.FromArgb(sidebarSelectedColor, sidebarSelectedColor, sidebarSelectedColor);
            this.actualPage = "fav-filme";
        }

        private void hideFavoritoFilme()
        {
            pnlFavoritoFilme.Dock = DockStyle.None;
            pnlFavoritoFilme.Enabled = false;
            pnlFavoritoFilme.Visible = false;
            btnFavFilmes.BackColor = Color.FromArgb(15, 15, 15);
        }

        private void setFavoritoFilme()
        {
            Resources.Movies.Objects.Movie.Rootobject[] filme = Array.Empty<Resources.Movies.Objects.Movie.Rootobject>();

            #region Recuperando os dados solicitados

            var result = c.Read(FrmMain.getConnection(), "filme", "usuario", FrmMain.Userid) ?? Array.Empty<object[]>();

            if (result.Length > 0)
            {
                foreach (var item in result)
                {
                    if (this.filmesStorage.Movie.ContainsKey(Convert.ToInt32(item[2])))
                    {
                        foreach (var i in this.filmesStorage.Movie)
                        {
                            if (i.Key == Convert.ToInt32(item[2]))
                            {
                                filme = filme.Append(i.Value).ToArray();
                                break;
                            }
                        }
                    }

                    else
                    {
                        try
                        {
                            var movie = Resources.Movies.Recover.getMovie(Convert.ToInt32(item[2])) ?? throw new Exception();
                            this.filmesStorage.Movie.Add(Convert.ToInt32(item[2]), movie);
                            filme = filme.Append(movie).ToArray();
                        }

                        catch
                        {
                            MessageBox.Show("Falha ao Carregar os Favoritos!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            mudaTela(this.actualPage, "home");
                            return;
                        }
                    }
                }
            }

            #endregion

            #region Preenchendo o grid

            flpFavoritoFilmeGrid.Controls.Clear();

            int quant = (filme.Length - (this.favoritoFilmePage - 1) * 20 > 20) ? 20 : filme.Length - (this.favoritoFilmePage - 1) * 20;
            int larg = 125 + 40;
            int alt = 183 + 40;
            int containerLarg = this.Width - (68 + 20 + 113);
            int quantPerRow = (containerLarg - (containerLarg % larg)) / larg;
            int quantRows = (quant - (quant % quantPerRow)) / quantPerRow + ((quant % quantPerRow == 0) ? 0 : 1);

            flpFavoritoFilmeGrid.Height = (quant > 0) ? quantRows * alt + 40 : this.Height - (pnlFavoritoFilmeTitle.Height + pnlFavoritoFilmePageSumary.Height);

            Button[] b = new Button[quant];

            int index = 0;

            for (int i = 0; i < quantRows; i++)
            {
                for (int j = 0; j < quantPerRow; j++)
                {
                    if (index >= b.Length)
                    {
                        break;
                    }

                    b[index] = new()
                    {
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Cursor = Cursors.Hand,
                        FlatStyle = FlatStyle.Flat,
                        ForeColor = Color.White,
                        Location = new Point(113 + larg * j, 10 + alt * i),
                        Margin = new Padding(0, 20, 20, 0),
                        Name = "btnFavoritoFilmeGridItem_" + filme[(this.favoritoFilmePage - 1) * 20 + index].id,
                        Size = new Size(larg - 20, alt - 20),
                        TabIndex = 0,
                        TextAlign = ContentAlignment.MiddleCenter,
                        UseVisualStyleBackColor = true
                    };

                    b[index].FlatAppearance.BorderColor = Color.FromArgb(60, 60, 60);
                    b[index].FlatAppearance.MouseDownBackColor = Color.FromArgb(20, 20, 20);
                    b[index].FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 30, 30);
                    b[index].MouseClick += btnFavoritoFilmeGridItem_MouseClick;

                    if (this.filmesStorage.PosterImage.ContainsKey(filme[(this.favoritoFilmePage - 1) * 20 + index].id))
                    {
                        foreach (var item in this.filmesStorage.PosterImage)
                        {
                            if (item.Key == filme[(this.favoritoFilmePage - 1) * 20 + index].id)
                            {
                                b[index].BackgroundImage = item.Value;
                                break;
                            }
                        }
                    }

                    else
                    {
                        string url = string.Format(Resources.Movies.Configuration.imageUrlBase, Resources.Movies.Configuration.imageUrlSize, filme[(this.favoritoFilmePage - 1) * 20 + index].poster_path);

                        try
                        {
                            var request = WebRequest.Create(url);

                            using (var response = request.GetResponse())
                            using (var stream = response.GetResponseStream())
                            {
                                Image img = Image.FromStream(stream);

                                if (img != null)
                                {
                                    b[index].BackgroundImage = img;
                                    this.filmesStorage.PosterImage.Add(filme[(this.favoritoFilmePage - 1) * 20 + index].id, img);
                                }

                                else
                                {
                                    throw new Exception();
                                }
                            }
                        }

                        catch
                        {
                            b[index].Text = filme[(this.favoritoFilmePage - 1) * 20 + index].title;
                        }
                    }

                    flpFavoritoFilmeGrid.Controls.Add(b[index]);
                    index++;
                }
            }

            if (b.Length == 0)
            {
                flpFavoritoFilmeGrid.Controls.Add(new Label()
                {
                    AutoSize = false,
                    Font = lblFilmeTitle.Font,
                    ForeColor = Color.White,
                    Location = new Point(0, 0),
                    Name = "lblFavoritoFilmeError",
                    Size = new Size(containerLarg, flpFavoritoFilmeGrid.Height),
                    Text = "Você não Possui Favoritos",
                    TextAlign = ContentAlignment.MiddleCenter
                });
            }

            #endregion

            #region Criando os botões de navegação entre páginas

            pnlFavoritoFilmePageSumary.Controls.Clear();

            Button[] b1 = new Button[(filme.Length > 20) ? 5 : 0];

            for (int i = 0; i < b1.Length; i++)
            {
                bool enabled = true;

                switch (i)
                {
                    case 0:
                        if (this.favoritoFilmePage == 1)
                        {
                            enabled = false;
                        }

                        break;

                    case 1:
                        if (this.favoritoFilmePage == 1)
                        {
                            enabled = false;
                        }

                        break;

                    case 3:
                        if (this.favoritoFilmePage == filme.Length / 20 + ((filme.Length % 20 > 0) ? 1 : 0))
                        {
                            enabled = false;
                        }

                        break;

                    case 4:
                        if (this.favoritoFilmePage == filme.Length / 20 + ((filme.Length % 20 > 0) ? 1 : 0))
                        {
                            enabled = false;
                        }

                        break;
                }

                b1[i] = new()
                {
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point),
                    ForeColor = Color.White,
                    Location = new Point(50 * i + ((this.Width - pnlSidebar.MinimumSize.Width - 250) / 2), 0),
                    Margin = new Padding(0, 0, 10, 0),
                    Name = "btnFavoritoFilmePageSumary" + i,
                    Size = new Size(40, 40),
                    TabIndex = 7,
                    UseVisualStyleBackColor = true,
                    Enabled = enabled,
                    BackgroundImageLayout = ImageLayout.Center,
                    Visible = true
                };

                switch (i)
                {
                    case 0:
                        b1[i].MouseClick += btnFavoritoFilmeAllBack_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.all_back;
                        break;

                    case 1:
                        b1[i].MouseClick += btnFavoritoFilmeBack_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.back;
                        break;

                    case 2:
                        b1[i].Text = this.favoritoFilmePage.ToString();
                        break;

                    case 3:
                        b1[i].MouseClick += btnFavoritoFilmeForward_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.forward;
                        break;

                    case 4:
                        b1[i].MouseClick += btnFavoritoFilmeAllForward_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.all_forward;
                        break;
                }

                b1[i].FlatAppearance.BorderColor = Color.FromArgb(60, 60, 60);
                b1[i].FlatAppearance.MouseDownBackColor = Color.FromArgb(30, 30, 30);
                b1[i].FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 60, 60);

                pnlFavoritoFilmePageSumary.Controls.Add(b1[i]);
            }

            #endregion

            this.favoritoFilmePageMax = filme.Length / 20 + ((filme.Length % 20 > 0) ? 1 : 0);
            pnlFavoritoFilme.VerticalScroll.Value = 0;
        }

        private void btnFavoritoFilmeGridItem_MouseClick(object? sender, MouseEventArgs e)
        {
            var b = sender as Button;

            int i = 0;

            while (true)
            {
                if (!this.filmePreviousPage.ContainsKey(this.actualPage + "_" + i))
                {
                    this.filmePreviousPage.Add(this.actualPage + "_" + i, new string[] { this.filmePopularPage.ToString() });
                    break;
                }
            }

            mudaTela(this.actualPage, "filme", b.Name.Split("_")[1]);
        }

        private void btnFavoritoFilmeAllBack_MouseClick(object? sender, MouseEventArgs e)
        {
            this.favoritoFilmePage = 1;
            setFavoritoFilme();
        }

        private void btnFavoritoFilmeBack_MouseClick(object? sender, MouseEventArgs e)
        {
            this.favoritoFilmePage--;
            setFavoritoFilme();
        }

        private void btnFavoritoFilmeForward_MouseClick(object? sender, MouseEventArgs e)
        {
            this.favoritoFilmePage++;
            setFavoritoFilme();
        }

        private void btnFavoritoFilmeAllForward_MouseClick(object? sender, MouseEventArgs e)
        {
            this.favoritoFilmePage = this.favoritoFilmePageMax;
            setFavoritoFilme();
        }

        #endregion

        #region Atributos/Métodos tela Favorito-Serie

        private int favoritoSeriePage = 1;
        private int favoritoSeriePageMax;

        private void showFavoritoSerie()
        {
            pnlFavoritoSerie.Dock = DockStyle.Fill;
            pnlFavoritoSerie.Enabled = true;
            pnlFavoritoSerie.Visible = true;
            btnFavSeries.BackColor = Color.FromArgb(sidebarSelectedColor, sidebarSelectedColor, sidebarSelectedColor);
            this.actualPage = "fav-serie";
        }

        private void hideFavoritoSerie()
        {
            pnlFavoritoSerie.Dock = DockStyle.None;
            pnlFavoritoSerie.Enabled = false;
            pnlFavoritoSerie.Visible = false;
            btnFavSeries.BackColor = Color.FromArgb(15, 15, 15);
        }

        private void setFavoritoSerie()
        {
            Resources.Tv_Shows.Objects.TvShow.Rootobject[] serie = Array.Empty<Resources.Tv_Shows.Objects.TvShow.Rootobject>();

            #region Recuperando os dados solicitados

            var result = c.Read(FrmMain.getConnection(), "serie", "usuario", FrmMain.Userid) ?? Array.Empty<object[]>();

            if (result.Length > 0)
            {
                foreach (var item in result)
                {
                    if (this.seriesStorage.TvShow.ContainsKey(Convert.ToInt32(item[2])))
                    {
                        foreach (var i in this.seriesStorage.TvShow)
                        {
                            if (i.Key == Convert.ToInt32(item[2]))
                            {
                                serie = serie.Append(i.Value).ToArray();
                                break;
                            }
                        }
                    }

                    else
                    {
                        try
                        {
                            var movie = Resources.Tv_Shows.Recover.getSerie(Convert.ToInt32(item[2])) ?? throw new Exception();
                            this.seriesStorage.TvShow.Add(Convert.ToInt32(item[2]), movie);
                            serie = serie.Append(movie).ToArray();
                        }

                        catch
                        {
                            MessageBox.Show("Falha ao Carregar os Favoritos!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            mudaTela(this.actualPage, "home");
                            return;
                        }
                    }
                }
            }

            #endregion

            #region Preenchendo o grid

            flpFavoritoSerieGrid.Controls.Clear();

            int quant = (serie.Length - (this.favoritoSeriePage - 1) * 20 > 20) ? 20 : serie.Length - (this.favoritoSeriePage - 1) * 20;
            int larg = 125 + 40;
            int alt = 183 + 40;
            int containerLarg = this.Width - (68 + 20 + 113);
            int quantPerRow = (containerLarg - (containerLarg % larg)) / larg;
            int quantRows = (quant - (quant % quantPerRow)) / quantPerRow + ((quant % quantPerRow == 0) ? 0 : 1);

            flpFavoritoSerieGrid.Height = (quant > 0) ? quantRows * alt + 40 : this.Height - (pnlFavoritoSerieTitle.Height + pnlFavoritoSeriePageSumary.Height);

            Button[] b = new Button[quant];

            int index = 0;

            for (int i = 0; i < quantRows; i++)
            {
                for (int j = 0; j < quantPerRow; j++)
                {
                    if (index >= b.Length)
                    {
                        break;
                    }

                    b[index] = new()
                    {
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Cursor = Cursors.Hand,
                        FlatStyle = FlatStyle.Flat,
                        ForeColor = Color.White,
                        Location = new Point(113 + larg * j, 10 + alt * i),
                        Margin = new Padding(0, 20, 20, 0),
                        Name = "btnFavoritoSerieGridItem_" + serie[(this.favoritoSeriePage - 1) * 20 + index].id,
                        Size = new Size(larg - 20, alt - 20),
                        TabIndex = 0,
                        TextAlign = ContentAlignment.MiddleCenter,
                        UseVisualStyleBackColor = true
                    };

                    b[index].FlatAppearance.BorderColor = Color.FromArgb(60, 60, 60);
                    b[index].FlatAppearance.MouseDownBackColor = Color.FromArgb(20, 20, 20);
                    b[index].FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 30, 30);
                    b[index].MouseClick += btnFavoritoSerieGridItem_MouseClick;

                    if (this.seriesStorage.PosterImage.ContainsKey(serie[(this.favoritoSeriePage - 1) * 20 + index].id))
                    {
                        foreach (var item in this.seriesStorage.PosterImage)
                        {
                            if (item.Key == serie[(this.favoritoSeriePage - 1) * 20 + index].id)
                            {
                                b[index].BackgroundImage = item.Value;
                                break;
                            }
                        }
                    }

                    else
                    {
                        string url = string.Format(Resources.Tv_Shows.Configuration.imageUrlBase, Resources.Tv_Shows.Configuration.imageUrlSize, serie[(this.favoritoSeriePage - 1) * 20 + index].poster_path);

                        try
                        {
                            var request = WebRequest.Create(url);

                            using (var response = request.GetResponse())
                            using (var stream = response.GetResponseStream())
                            {
                                Image img = Image.FromStream(stream);

                                if (img != null)
                                {
                                    b[index].BackgroundImage = img;
                                    this.seriesStorage.PosterImage.Add(serie[(this.favoritoSeriePage - 1) * 20 + index].id, img);
                                }

                                else
                                {
                                    throw new Exception();
                                }
                            }
                        }

                        catch
                        {
                            b[index].Text = serie[(this.favoritoSeriePage - 1) * 20 + index].name;
                        }
                    }

                    flpFavoritoSerieGrid.Controls.Add(b[index]);
                    index++;
                }
            }

            if (b.Length == 0)
            {
                flpFavoritoSerieGrid.Controls.Add(new Label()
                {
                    AutoSize = false,
                    Font = lblSerieTitle.Font,
                    ForeColor = Color.White,
                    Location = new Point(0, 0),
                    Name = "lblFavoritoSerieError",
                    Size = new Size(containerLarg, flpFavoritoSerieGrid.Height),
                    Text = "Você não Possui Favoritos",
                    TextAlign = ContentAlignment.MiddleCenter
                });
            }

            #endregion

            #region Criando os botões de navegação entre páginas

            pnlFavoritoSeriePageSumary.Controls.Clear();

            Button[] b1 = new Button[(serie.Length > 20) ? 5 : 0];

            for (int i = 0; i < b1.Length; i++)
            {
                bool enabled = true;

                switch (i)
                {
                    case 0:
                        if (this.favoritoSeriePage == 1)
                        {
                            enabled = false;
                        }

                        break;

                    case 1:
                        if (this.favoritoSeriePage == 1)
                        {
                            enabled = false;
                        }

                        break;

                    case 3:
                        if (this.favoritoSeriePage == ((serie.Length / 20 > 500) ? 500 : serie.Length / 20))
                        {
                            enabled = false;
                        }

                        break;

                    case 4:
                        if (this.favoritoSeriePage == ((serie.Length / 20 > 500) ? 500 : serie.Length / 20))
                        {
                            enabled = false;
                        }

                        break;
                }

                b1[i] = new()
                {
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point),
                    ForeColor = Color.White,
                    Location = new Point(50 * i + ((this.Width - pnlSidebar.MinimumSize.Width - 250) / 2), 0),
                    Margin = new Padding(0, 0, 10, 0),
                    Name = "btnFavoritoSeriePageSumary" + i,
                    Size = new Size(40, 40),
                    TabIndex = 7,
                    UseVisualStyleBackColor = true,
                    Enabled = enabled,
                    BackgroundImageLayout = ImageLayout.Center,
                    Visible = true
                };

                switch (i)
                {
                    case 0:
                        b1[i].MouseClick += btnFavoritoSerieAllBack_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.all_back;
                        break;

                    case 1:
                        b1[i].MouseClick += btnFavoritoSerieBack_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.back;
                        break;

                    case 2:
                        b1[i].Text = this.favoritoSeriePage.ToString();
                        break;

                    case 3:
                        b1[i].MouseClick += btnFavoritoSerieForward_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.forward;
                        break;

                    case 4:
                        b1[i].MouseClick += btnFavoritoSerieAllForward_MouseClick;
                        b1[i].BackgroundImage = Properties.Resources.all_forward;
                        break;
                }

                b1[i].FlatAppearance.BorderColor = Color.FromArgb(60, 60, 60);
                b1[i].FlatAppearance.MouseDownBackColor = Color.FromArgb(30, 30, 30);
                b1[i].FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 60, 60);

                pnlFavoritoSeriePageSumary.Controls.Add(b1[i]);
            }

            #endregion

            this.favoritoSeriePageMax = (serie.Length / 20 > 500) ? 500 : serie.Length / 20;
            pnlFavoritoSerie.VerticalScroll.Value = 0;
        }

        private void btnFavoritoSerieGridItem_MouseClick(object? sender, MouseEventArgs e)
        {
            var b = sender as Button;

            int i = 0;

            while (true)
            {
                if (!this.seriePreviousPage.ContainsKey(this.actualPage + "_" + i))
                {
                    this.seriePreviousPage.Add(this.actualPage + "_" + i, new string[] { this.seriePopularPage.ToString() });
                    break;
                }
            }

            mudaTela(this.actualPage, "serie", b.Name.Split("_")[1]);
        }

        private void btnFavoritoSerieAllBack_MouseClick(object? sender, MouseEventArgs e)
        {
            this.favoritoSeriePage = 1;
            setFavoritoSerie();
        }

        private void btnFavoritoSerieBack_MouseClick(object? sender, MouseEventArgs e)
        {
            this.favoritoSeriePage--;
            setFavoritoSerie();
        }

        private void btnFavoritoSerieForward_MouseClick(object? sender, MouseEventArgs e)
        {
            this.favoritoSeriePage++;
            setFavoritoSerie();
        }

        private void btnFavoritoSerieAllForward_MouseClick(object? sender, MouseEventArgs e)
        {
            this.favoritoSeriePage = this.favoritoSeriePageMax;
            setFavoritoSerie();
        }

        #endregion

        #region Atributos/Métodos tela Loading

        private void showLoading()
        {
            pnlLoading.Dock = DockStyle.Fill;
            pnlLoading.Enabled = true;
            pnlLoading.Visible = true;
            timerLoading.Start();
        }

        private void hideLoading()
        {
            pnlLoading.Dock = DockStyle.None;
            pnlLoading.Visible = false;
            pnlLoading.Enabled = false;
            lblLoading.Text = "Carregando";
            timerLoading.Stop();
        }

        private void timerLoading_Tick(object sender, EventArgs e)
        {
            if (lblLoading.Text != "Carregando...")
            {
                lblLoading.Text += ".";
            }

            else
            {
                lblLoading.Text = "Carregando";
            }
        }

        #endregion

        #region Atributos/Métodos tela Config

        private void showConfig()
        {
            pnlConfig.Dock = DockStyle.Fill;
            pnlConfig.Enabled = true;
            pnlConfig.Visible = true;
            this.actualPage = "config";
        }

        private void hideConfig()
        {
            pnlConfig.Dock = DockStyle.None;
            pnlConfig.Enabled = false;
            pnlConfig.Visible = false;
        }

        private void setConfig()
        {
            string text = File.ReadAllText("Resources/configuration.config");

            string filme = text.Split('\n')[1];
            string serie = text.Split('\n')[2];

            cbConfigGeralFilme.SelectedIndex = cbConfigGeralFilme.FindString(filme.Split(":")[1]);
            cbConfigGeralSerie.SelectedIndex = cbConfigGeralSerie.FindString(serie.Split(":")[1]);

            tbConfigUsuarioNome.Text = FrmMain.Username;
        }

        private void loadConfig()
        {
            string file;

            try
            {
                file = File.ReadAllText("Resources/configuration.config");
                string film = file.Split("\n")[1];
                string serie = file.Split("\n")[2];

                Resources.Movies.Configuration.region = film.Split(":")[1];
                Resources.Tv_Shows.Configuration.region = serie.Split(":")[1];
            }

            catch
            {
                return;
            }
        }

        private void btnConfigUsuarioExcluir_MouseClick(object sender, MouseEventArgs e)
        {
            if (MessageBox.Show("Deseja Excluir o Usuário?", "Excluir", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) != DialogResult.OK)
            {
                return;
            }

            if (c.Delete(FrmMain.getConnection(), "usuario", FrmMain.Userid))
            {
                MessageBox.Show("Excluído com Sucesso", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                var obj = c.Read(FrmMain.getConnection(), "filme", "usuario", FrmMain.Userid) ?? Array.Empty<object[]>();

                foreach (var item in obj)
                {
                    c.Delete(FrmMain.getConnection(), "filme", Convert.ToInt32(item[0]));
                }

                obj = c.Read(FrmMain.getConnection(), "serie", "usuario", FrmMain.Userid) ?? Array.Empty<object[]>();

                foreach (var item in obj)
                {
                    c.Delete(FrmMain.getConnection(), "serie", Convert.ToInt32(item[0]));
                }

                Hide();

                FrmMain.Userid = -1;
                FrmMain.Username = string.Empty;

                new FrmLogin().ShowDialog();

                if (FrmMain.Userid == -1)
                {
                    Close();
                    return;
                }

                mudaTela(this.actualPage, "home");
                Show();
            }

            else
            {
                MessageBox.Show("Falha ao Excluir", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnConfigUsuarioSalvar_MouseClick(object sender, MouseEventArgs e)
        {
            string senha = tbConfigUsuarioSenha.Text;

            senha = Convert.ToBase64String(Resources.Criptography.Encripty(senha, Resources.Criptography.GetKey(), Resources.Criptography.GetIv()));

            if (c.Update(FrmMain.getConnection(), "usuario", FrmMain.Userid, "senha", senha))
            {
                MessageBox.Show("Atualizado com sucesso", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else
            {
                MessageBox.Show("Erro ao atualizar", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnConfigGeralSalvar_MouseClick(object sender, MouseEventArgs e)
        {
            string filmeRegion = Convert.ToString(cbConfigGeralFilme.SelectedItem) ?? string.Empty;
            string serieRegion = Convert.ToString(cbConfigGeralSerie.SelectedItem) ?? string.Empty;

            saveConfig(filmeRegion, serieRegion);
        }

        private void saveConfig(string f, string s)
        {
            try
            {
                if (string.IsNullOrEmpty(f) || string.IsNullOrEmpty(s))
                {
                    throw new Exception();
                }

                string text = $"apikey:{Resources.Movies.Configuration.apiKey}\nregion-film:{f}\nregion-serie:{s}";
                File.WriteAllText("Resources/configuration.config", text);

                loadConfig();
                MessageBox.Show("Configurações Salvas!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            catch
            {
                MessageBox.Show("Falha ao Salvar", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}