namespace Movies_app.Resources.Movies
{
    public static class Configuration
    {
        /// <summary>
        /// 0 = key, 1 = query, 2 = lang, 3 = page
        /// </summary>
        public const string searchUrlBase = "https://api.themoviedb.org/3/search/movie?api_key={0}&query={1}&language={2}&include_adult=false&page={3}";

        /// <summary>
        /// 0 = movie id, 1 = key, 2 = lang
        /// </summary>
        public const string urlBase = "https://api.themoviedb.org/3/movie/{0}?api_key={1}&language={2}";

        /// <summary>
        /// 0 = key, 1 = lang, 2 = region, 3 = page
        /// </summary>
        public const string popularUrlBase = "https://api.themoviedb.org/3/movie/popular?api_key={0}&language={1}&region={2}&page={3}";

        /// <summary>
        /// 0 = key, 1 = lang, 2 = region, 3 = page
        /// </summary>
        public const string upcomingUrlBase = "https://api.themoviedb.org/3/movie/upcoming?api_key={0}&language={1}&region={2}&page={3}";

        /// <summary>
        /// 0 = key, 1 = lang, 2 = region, 3 = page
        /// </summary>
        public const string nowPlayingUrlBase = "https://api.themoviedb.org/3/movie/now_playing?api_key={0}&language={1}&region={2}&page={3}";

        /// <summary>
        /// 0 = movie id, 1 = key, 2 = lang, 3 = page
        /// </summary>
        public const string similarUrlBase = "https://api.themoviedb.org/3/movie/{0}/similar?api_key={1}&language={2}&page={3}";

        /// <summary>
        /// 0 = key, 1 = lang, 2 = page, 3 = genre_id
        /// </summary>
        public const string genreUrlBase = "https://api.themoviedb.org/3/discover/movie?api_key={0}&language={1}&sort_by=popularity.desc&include_adult=false&include_video=false&page={2}&with_genres={3}";

        /// <summary>
        /// 0 = size, 1 = image
        /// </summary>
        public const string imageUrlBase = "https://image.tmdb.org/t/p/{0}{1}";

        public const string imageUrlSize = "original";

        public static string apiKey { get; set; } = "";
        public static string language { get; set; } = "pt-BR";
        public static string region { get; set; } = "BR";
    }
}
