namespace Movies_app.Resources.Movies
{
    public class Storage
    {
        /// <summary>
        /// string (int page : string region), Popular
        /// </summary>
        public Dictionary<string, Objects.Popular.Rootobject> Popular = new();

        /// <summary>
        /// string (int id : int page), Similar
        /// </summary>
        public Dictionary<string, Objects.Similar.Rootobject> Similar = new();

        /// <summary>
        /// string (string query : int page), Search
        /// </summary>
        public Dictionary<string, Objects.Search.Rootobject> Search = new();

        /// <summary>
        /// string (int page : string region), NowPlaying
        /// </summary>
        public Dictionary<string, Objects.NowPlaying.Rootobject> NowPlaying = new();

        /// <summary>
        /// string (int page : string region), Upcoming
        /// </summary>
        public Dictionary<string, Objects.Upcoming.Rootobject> Upcoming = new();

        /// <summary>
        /// int id, Movie
        /// </summary>
        public Dictionary<int, Objects.Movie.Rootobject> Movie = new();

        /// <summary>
        /// string (int page : int id), Genre
        /// </summary>
        public Dictionary<string, Objects.Genre.Rootobject> Genre = new();

        /// <summary>
        /// int Filme_id, PosterImage
        /// </summary>
        public Dictionary<int, Image> PosterImage = new();

        /// <summary>
        /// int Filme_id, BannerImage
        /// </summary>
        public Dictionary<int, Image> BannerImage = new();

        /// <summary>
        /// string data_type, int[] ids
        /// </summary>
        public Dictionary<string, int[]> movieIds = new();
    }
}
