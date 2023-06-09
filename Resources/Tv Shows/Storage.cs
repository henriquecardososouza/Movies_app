namespace Movies_app.Resources.Tv_Shows
{
    public class Storage
    {
        /// <summary>
        /// int page, Popular
        /// </summary>
        public Dictionary<int, Objects.Popular.Rootobject> Popular = new();

        /// <summary>
        /// string (int page : int id), Similar
        /// </summary>
        public Dictionary<string, Objects.Similar.Rootobject> Similar = new();

        /// <summary>
        /// string (string query : int page), Search
        /// </summary>
        public Dictionary<string, Objects.Search.Rootobject> Search = new();

        /// <summary>
        /// int Serie_id, TvShow
        /// </summary>
        public Dictionary<int, Objects.TvShow.Rootobject> TvShow = new();

        /// <summary>
        /// string (int page : int id), Genre
        /// </summary>
        public Dictionary<string, Objects.Genre.Rootobject> Genre = new();

        /// <summary>
        /// int Serie_id, PosterImage
        /// </summary>
        public Dictionary<int, Image> PosterImage = new();

        /// <summary>
        /// int Serie_id, BannerImage
        /// </summary>
        public Dictionary<int, Image> BannerImage = new();

        /// <summary>
        /// string data_type, int[] ids
        /// </summary>
        public Dictionary<string, int[]> serieIds = new();
    }
}
